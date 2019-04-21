using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteAuthCookieEvents : CookieAuthenticationEvents, ISiteAuthCookieEvents
    {
        public SiteAuthCookieEvents(
            IOidcHybridFlowHelper oidcHybridFlowHelper,
            ILogger<SiteAuthCookieEvents> logger
            )
        {
            _oidcHybridFlowHelper = oidcHybridFlowHelper;
            _log = logger;
        }

        private readonly IOidcHybridFlowHelper _oidcHybridFlowHelper;
        private readonly ILogger _log;

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var tenant = context.HttpContext.GetTenant<SiteContext>();

            if (tenant == null)
            {
                context.RejectPrincipal();
            }

            var siteGuidClaim = new Claim("SiteGuid", tenant.Id.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                var optionsAccessor = context.HttpContext.RequestServices.GetRequiredService<IOptions<MultiTenantOptions>>();
                var options = optionsAccessor.Value;
                if (options.UseRelatedSitesMode == true)
                {
                    await SecurityStampValidator.ValidatePrincipalAsync(context);
                    return;
                }


                _log.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }
            else
            {
                await SecurityStampValidator.ValidatePrincipalAsync(context);
            }
        }

        public override async Task SigningIn(CookieSigningInContext context)
        {
            var path = context.Request.Path.Value;
            if(!string.IsNullOrEmpty(path) && path.Contains("/account/externallogincallback"))
            {
                var oidcTokens = await _oidcHybridFlowHelper.GetOidcTokens(context.Principal);
                if(oidcTokens != null && oidcTokens.Count > 0)
                {
                    context.Properties.StoreTokens(oidcTokens.Where(x => 
                        x.Name == OpenIdConnectParameterNames.AccessToken 
                        || x.Name == OpenIdConnectParameterNames.RefreshToken)
                        );

                }
                //var tokens = context.Properties.GetTokens();
            }
            

            await base.SigningIn(context);
        }

        public Type GetCookieAuthenticationEventsType()
        {
            return typeof(SiteAuthCookieEvents);
        }
    }
}
