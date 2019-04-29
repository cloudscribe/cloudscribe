using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class HybridFlowHelper : IOidcHybridFlowHelper
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
        

        private async Task<Dictionary<string,string>> GetOidcTokens(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) return null;
            if (!user.Identity.IsAuthenticated) return null;
            var userIdClaim = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return null;

            var cacheKey = "oidc-tokens-" + userIdClaim.Value;
            
            var json = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            var siteIdClaim = user.Claims.Where(x => x.Type == "SiteGuid").FirstOrDefault();

            if (siteIdClaim == null) return null;

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

        private async Task ClearCache(ClaimsPrincipal user)
        {
            if (user == null) return;
            var userIdClaim = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return;

            var cacheKey = "oidc-tokens-" + userIdClaim.Value;
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task<string> GetAccessToken(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tokens = await GetOidcTokens(user, cancellationToken);
            if(tokens == null)
            {
                _log.LogWarning($"Failed to get OIDC auth tokens for: {user.Identity.Name}, returning null access token");
                return null;
            }

            var accessToken = tokens["access_token"];
            var refreshToken = tokens["refresh_token"];
            var exp = tokens["expires_at"];

            if (string.IsNullOrEmpty(exp) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                _log.LogWarning($"Cache was not null but still failed to get OIDC auth tokens for: {user.Identity.Name}, returning null access token");
                return null;
            }

            var dtExpires = DateTimeOffset.Parse(exp, CultureInfo.InvariantCulture);
            var dtRefresh = dtExpires.Subtract(_tokenOptions.RefreshBeforeExpiration);

            if (dtRefresh < _clock.UtcNow)
            {
                var shouldRefresh = _pendingRefreshTokenRequests.TryAdd(refreshToken, true);
                if (shouldRefresh)
                {
                    try
                    {
                        var response = await _oidcTokenEndpointService.RefreshTokenAsync(refreshToken);

                        if (response.IsError)
                        {
                            _log.LogWarning("Error refreshing token: {error}", response.Error);
                            return null;
                        }

                        //set the token to return
                        accessToken = response.AccessToken;

                        var newAccessToken = new UserToken()
                        {
                            Name = "access_token",
                            Value = response.AccessToken,
                            UserId = user.GetUserIdAsGuid(),
                            SiteId = user.GetUserSiteIdAsGuid(),
                            LoginProvider = "OpenIdConnect"
                        };

                        await _userCommands.UpdateToken(newAccessToken);

                        var newRefreshToken = new UserToken()
                        {
                            Name = "refresh_token",
                            Value = response.RefreshToken,
                            UserId = user.GetUserIdAsGuid(),
                            SiteId = user.GetUserSiteIdAsGuid(),
                            LoginProvider = "OpenIdConnect"
                        };

                        await _userCommands.UpdateToken(newRefreshToken);

                        var newExpiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
                        var newExpiresAtToken = new UserToken()
                        {
                            Name = "expires_at",
                            Value = newExpiresAt.ToString("o", CultureInfo.InvariantCulture),
                            UserId = user.GetUserIdAsGuid(),
                            SiteId = user.GetUserSiteIdAsGuid(),
                            LoginProvider = "OpenIdConnect"
                        };


                        await _userCommands.UpdateToken(newExpiresAtToken);

                        await ClearCache(user);


                    }
                    finally
                    {
                        _pendingRefreshTokenRequests.TryRemove(refreshToken, out _);
                    }
                }
            }


            return accessToken;


        }


    }
}
