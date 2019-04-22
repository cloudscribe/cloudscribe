using cloudscribe.Core.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class HybridFlowHelper
    {
        public HybridFlowHelper(
            IUserQueries userQueries,
            IUserCommands userCommands,
            IDistributedCache distributedCache,
            OidcTokenEndpointService oidcTokenEndpointService,
            IOptions<OidcTokenManagementOptions> tokenOptions,
            ISystemClock clock,
            ILogger<HybridFlowHelper> logger
            )
        {
            _userQueries = userQueries;
            _userCommands = userCommands;
            _cache = distributedCache;
            _oidcTokenEndpointService = oidcTokenEndpointService;
            _tokenOptions = tokenOptions.Value;
            _clock = clock;
            _log = logger;
        }

        private readonly IUserQueries _userQueries;
        private readonly IUserCommands _userCommands;
        private readonly IDistributedCache _cache;
        private readonly OidcTokenEndpointService _oidcTokenEndpointService;
        private readonly OidcTokenManagementOptions _tokenOptions;
        private readonly ISystemClock _clock;
        private readonly ILogger _log;

        private static readonly ConcurrentDictionary<string, bool> _pendingRefreshTokenRequests =
            new ConcurrentDictionary<string, bool>();


        private async Task<TokenResponse> RefreshOidcTokenIfNeeded(IEnumerable<AuthenticationToken> tokens)
        {
            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
            {
                _log.LogWarning("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
                return null;
            }

            var expiresAt = tokens.SingleOrDefault(t => t.Name == "expires_at");
            if (expiresAt == null)
            {
                _log.LogWarning("No expires_at value found in cookie properties.");
                return null;
            }

            var dtExpires = DateTimeOffset.Parse(expiresAt.Value, CultureInfo.InvariantCulture);
            var dtRefresh = dtExpires.Subtract(_tokenOptions.RefreshBeforeExpiration);

            if (dtRefresh < _clock.UtcNow)
            {
                var shouldRefresh = _pendingRefreshTokenRequests.TryAdd(refreshToken.Value, true);
                if (shouldRefresh)
                {
                    try
                    {
                        var response = await _oidcTokenEndpointService.RefreshTokenAsync(refreshToken.Value);

                        if (response.IsError)
                        {
                            _log.LogWarning("Error refreshing token: {error}", response.Error);
                            _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);

                            return null;
                        }

                        _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);

                        return response;
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"{ex.Message}:{ex.StackTrace}");
                    }
                    finally
                    {
                        _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);
                    }
                }
            }

            return null;

        }

        private async Task<Dictionary<string,string>> GetOidcTokens(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) return null;
            var userIdClaim = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return null;

            var cacheKey = "oidc-tokens-" + userIdClaim.Value;

            //var result = await _distributedCache.GetAsync<List<AuthenticationToken>>(cacheKey);
            var json = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            var siteIdClaim = user.Claims.Where(x => x.Type == "SiteGuid").FirstOrDefault();

            if(userIdClaim.Value.Length == 36 && siteIdClaim.Value.Length == 36)
            {
                var siteId = new Guid(siteIdClaim.Value);
                var userId = new Guid(userIdClaim.Value);
                var tokens = await _userQueries.GetUserTokensByProvider(siteId, userId, "OpenIdConnect", cancellationToken);
                if(tokens != null && tokens.Count > 0)
                {
                    var result = new Dictionary<string, string>();
                    foreach(var t in tokens)
                    {
                        result.Add(t.Name, t.Value);
                    }


                    var serialized = JsonConvert.SerializeObject(result);

                    var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));

                    await _cache.SetStringAsync(cacheKey, serialized, options);

                    return result;

                }
            }
            
            return null;
        }


    }
}
