using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class TenantIdentityOptionsProvider : IOptions<IdentityOptions>
    {
        private CookieAuthenticationEvents cookieEvents;
        private IHttpContextAccessor httpContextAccessor;

        public TenantIdentityOptionsProvider(
            IHttpContextAccessor httpContextAccessor,
            CookieAuthenticationEvents cookieEvents)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cookieEvents = cookieEvents;
        }

        public IdentityOptions Value
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var tenant = context.GetTenant<SiteSettings>();

                var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                    ? PathString.Empty
                    : new PathString("/" + tenant.SiteFolderName);

                var identityOptions = new IdentityOptions();

                /*
                identityOptions.ClaimsIdentity = singletonOptions.ClaimsIdentity;
                identityOptions.Cookies = singletonOptions.Cookies;
                identityOptions.Lockout = singletonOptions.Lockout;
                identityOptions.Password = singletonOptions.Password;
                identityOptions.SecurityStampValidationInterval = singletonOptions.SecurityStampValidationInterval;
                identityOptions.SignIn = singletonOptions.SignIn;
                identityOptions.Tokens = singletonOptions.Tokens;
                identityOptions.User = singletonOptions.User;
                */

                Setup(identityOptions.Cookies.ExternalCookie, AuthenticationScheme.External, tenant);
                Setup(identityOptions.Cookies.TwoFactorRememberMeCookie, AuthenticationScheme.TwoFactorRememberMe, tenant);
                Setup(identityOptions.Cookies.TwoFactorUserIdCookie, AuthenticationScheme.TwoFactorUserId, tenant);
                Setup(identityOptions.Cookies.ApplicationCookie, AuthenticationScheme.Application, tenant);

                var application = identityOptions.Cookies.ApplicationCookie;

                application.AutomaticAuthenticate = true;
                application.AutomaticChallenge = true;
                application.CookiePath = "/" + tenant.SiteFolderName;

                return identityOptions;
            }
        }

        private void Setup(CookieAuthenticationOptions options, string scheme, SiteSettings tenant)
        {
            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
            options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.Events = cookieEvents;
        }
    }
}
