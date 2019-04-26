//using cloudscribe.Core.Models;
//using IdentityModel.Client;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Protocols.OpenIdConnect;
//using System;
//using System.Collections.Concurrent;
//using System.Globalization;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

// 2019-04-26 commented out because this approach did not work though it is just like the sample mvc client with auto refresh
// see additiional comments below
// in my testing I was using a scenario where the IDServer app is its own oidc client, maybe it would work in a more conventional scenario.

// I've found an alternate way by storing the tokens in the db at login time then caching them in distributed cache
// and auto refreshing as needed so long as the refresh token has not expired.
// see HybridFlowHelper and IOidcHybridFlowHelper

//namespace cloudscribe.Core.Identity
//{
//    public class SiteOidcHybridFlowHelperAuthCookieEvents : CookieAuthenticationEvents, ISiteAuthCookieEvents
//    {
//        public SiteOidcHybridFlowHelperAuthCookieEvents(
//            OidcTokenEndpointService oidcTokenEndpointService,
//            IOptions<OidcTokenManagementOptions> tokenOptions,
//            ICaptureOidcTokens oidcHybridFlowHelper,
//            IOptions<MultiTenantOptions> multiTenantOptions,
//            ISystemClock clock,
//            ILogger<SiteOidcHybridFlowHelperAuthCookieEvents> logger
//            )
//        {
//            _oidcHybridFlowHelper = oidcHybridFlowHelper;
//            _oidcTokenEndpointService = oidcTokenEndpointService;
//            _tokenOptions = tokenOptions.Value;
//            _multiTenantOptions = multiTenantOptions.Value;
//            _clock = clock;
//            _log = logger;
//        }

//        private readonly ICaptureOidcTokens _oidcHybridFlowHelper;
//        private readonly OidcTokenEndpointService _oidcTokenEndpointService;
//        private readonly OidcTokenManagementOptions _tokenOptions;
//        private readonly MultiTenantOptions _multiTenantOptions;
//        private readonly ISystemClock _clock;
//        private readonly ILogger _log;

//        private static readonly ConcurrentDictionary<string, bool> _pendingRefreshTokenRequests =
//            new ConcurrentDictionary<string, bool>();

//        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
//        {
//            var tenant = context.HttpContext.GetTenant<SiteContext>();

//            if (tenant == null)
//            {
//                context.RejectPrincipal();
//            }

//            var siteGuidClaim = new Claim("SiteGuid", tenant.Id.ToString());

//            if(_multiTenantOptions.UseRelatedSitesMode || context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
//            {
//                if(
//                    !string.IsNullOrWhiteSpace(tenant.OidConnectAuthority) 
//                    && !string.IsNullOrWhiteSpace(tenant.OidConnectAppId)
//                    && !string.IsNullOrWhiteSpace(tenant.OidConnectAppSecret)
//                    )
//                {
//                    try
//                    {
//                        var response = await RefreshOidcTokenIfNeeded(context);
//                        if(response != null)
//                        {
//                            context.Properties.UpdateTokenValue("access_token", response.AccessToken);
//                            context.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

//                            var newExpiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
//                            context.Properties.UpdateTokenValue("expires_at", newExpiresAt.ToString("o", CultureInfo.InvariantCulture));

//                            // these attempts using the schema name all seem to cause an uncatchable error
//                            //var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();
//                            //await authService.SignInAsync(context.HttpContext, context.Scheme.Name, context.Principal, context.Properties);
//                            //await context.HttpContext.SignInAsync(context.Scheme.Name, context.Principal);
//                            //await context.HttpContext.SignInAsync(context.Scheme.Name, context.Principal, context.Properties);


//                            //this throws no errors and is just like the sample:
//                            //https://github.com/IdentityServer/IdentityServer4.Samples/blob/master/Clients/src/MvcHybridAutomaticRefresh/AutomaticTokenManagement/AutomaticTokenManagementCookieEvents.cs
//                            // but it does not seem to update the auth cookie, next request we still have the old tokens
//                            await context.HttpContext.SignInAsync(context.Principal, context.Properties);
//                        }
//                    }
//                    catch(Exception ex)
//                    {
//                        _log.LogError($"{ex.Message}:{ex.StackTrace}");
//                    }
                    
//                }


//                await SecurityStampValidator.ValidatePrincipalAsync(context);
//                return;
//            }
//            else
//            {
//                _log.LogInformation("rejecting principal because it does not have siteguid");
//                context.RejectPrincipal();
//            }


            

//        }

//        private async Task<TokenResponse> RefreshOidcTokenIfNeeded(CookieValidatePrincipalContext context)
//        {
//            var tokens = context.Properties.GetTokens();
//            if (tokens == null || !tokens.Any())
//            {
//                _log.LogDebug("No tokens found in cookie properties. SaveTokens must be enabled for automatic token refresh.");
//                return null;
//            }


//            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
//            if (refreshToken == null)
//            {
//                _log.LogWarning("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
//                return null;
//            }

//            var expiresAt = tokens.SingleOrDefault(t => t.Name == "expires_at");
//            if (expiresAt == null)
//            {
//                _log.LogWarning("No expires_at value found in cookie properties.");
//                return null;
//            }

//            var dtExpires = DateTimeOffset.Parse(expiresAt.Value, CultureInfo.InvariantCulture);
//            var dtRefresh = dtExpires.Subtract(_tokenOptions.RefreshBeforeExpiration);

//            if (dtRefresh < _clock.UtcNow)
//            {
//                var shouldRefresh = _pendingRefreshTokenRequests.TryAdd(refreshToken.Value, true);
//                if (shouldRefresh)
//                {
//                    try
//                    {
//                        var response = await _oidcTokenEndpointService.RefreshTokenAsync(refreshToken.Value);

//                        if (response.IsError)
//                        {
//                            _log.LogWarning("Error refreshing token: {error}", response.Error);
//                            _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);

//                            return null;
//                        }
                        
//                        _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);

//                        return response;
//                    }
//                    catch(Exception ex)
//                    {
//                        _log.LogError($"{ex.Message}:{ex.StackTrace}");
//                    }
//                    finally
//                    {
//                        _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);
//                    }
//                }
//            }

//            return null;

//        }


//        public override async Task SigningOut(CookieSigningOutContext context)
//        {
//            if (_tokenOptions.RevokeRefreshTokenOnSignout == false) return;

//            var result = await context.HttpContext.AuthenticateAsync();

//            if (!result.Succeeded)
//            {
//                _log.LogDebug("Can't find cookie for default scheme. Might have been deleted already.");
//                return;
//            }

//            var tokens = result.Properties.GetTokens();
//            if (tokens == null || !tokens.Any())
//            {
//                _log.LogDebug("No tokens found in cookie properties. SaveTokens must be enabled for automatic token revocation.");
//                return;
//            }

//            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
//            if (refreshToken == null)
//            {
//                _log.LogWarning("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
//                return;
//            }

//            var response = await _oidcTokenEndpointService.RevokeTokenAsync(refreshToken.Value);

//            if (response.IsError)
//            {
//                _log.LogWarning("Error revoking token: {error}", response.Error);
//                return;
//            }
//        }


//        public override async Task SigningIn(CookieSigningInContext context)
//        {
//            var path = context.Request.Path.Value;
//            if(!string.IsNullOrEmpty(path) && path.Contains("/account/externallogincallback"))
//            {
//                var oidcTokens = await _oidcHybridFlowHelper.GetOidcTokens(context.Principal);
//                if(oidcTokens != null && oidcTokens.Count > 0)
//                {
//                    context.Properties.StoreTokens(oidcTokens.Where(x => 
//                        x.Name == OpenIdConnectParameterNames.AccessToken 
//                        || x.Name == OpenIdConnectParameterNames.RefreshToken
//                        || x.Name == "expires_at"
//                        )
//                        );

//                }
//                //var tokens = context.Properties.GetTokens();
//            }
            

//            await base.SigningIn(context);
//        }

//        public Type GetCookieAuthenticationEventsType()
//        {
//            return typeof(SiteOidcHybridFlowHelperAuthCookieEvents);
//        }
//    }
//}
