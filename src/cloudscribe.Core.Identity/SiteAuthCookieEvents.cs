using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteAuthCookieEvents : CookieAuthenticationEvents, ISiteAuthCookieEvents
    {
        public SiteAuthCookieEvents(
             IOptions<MultiTenantOptions> multiTenantOptions,
             ILogger<SiteAuthCookieEvents> logger
            )
        {
            _multiTenantOptions = multiTenantOptions.Value;
            _log = logger;
        }

        private readonly MultiTenantOptions _multiTenantOptions;
        private readonly ILogger _log;

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (IsApiCall(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Task.CompletedTask;
            }

            return base.RedirectToAccessDenied(context);
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (IsApiCall(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }

            return base.RedirectToLogin(context);
        }

        private bool IsApiCall(RedirectContext<CookieAuthenticationOptions> context)
        {
            bool isApiCall = false;

            if (context.Request.Path.StartsWithSegments("/api"))
            {                
                isApiCall = true;
            }
            else {
                var tenant = context.HttpContext.GetTenant<SiteContext>();
                if (tenant != null && !string.IsNullOrWhiteSpace(tenant.SiteFolderName))
                {
                    if (context.Request.Path.StartsWithSegments("/" + tenant.SiteFolderName + "/api"))
                    {             
                        isApiCall = true;
                    }
                }
            }            

            return isApiCall;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var tenant = context.HttpContext.GetTenant<SiteContext>();

            if (tenant == null)
            {
                context.RejectPrincipal();
            }

            var siteGuidClaim = new Claim("SiteGuid", tenant.Id.ToString());

            if (_multiTenantOptions.UseRelatedSitesMode || _multiTenantOptions.RootUserCanSignInToTenants || context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                
                await SecurityStampValidator.ValidatePrincipalAsync(context);
                return;
            }
            else
            {
                _log.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }
            

        }

        public override async Task SigningOut(CookieSigningOutContext context)
        {
            //any persisted oidc tokens should be cleared when signing out
            try
            {
                var userId = context.HttpContext.User.GetUserIdAsGuid();
                var siteId = context.HttpContext.User.GetUserSiteIdAsGuid();
                var commands = context.HttpContext.RequestServices.GetRequiredService<IUserCommands>();
                await commands.DeleteTokensByProvider(siteId, userId, "OpenIdConnect");
            }
            catch { }
            

            await base.SigningOut(context);
        }
               
        public Type GetCookieAuthenticationEventsType()
        {
            return typeof(SiteAuthCookieEvents);
        }
    }
}
