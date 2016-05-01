
using Microsoft.AspNet.Identity;
using cloudscribe.Core.Models;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Identity;
using Microsoft.AspNet.Http;

namespace cloudscribe.Core.Web.Components
{
    public class IdentityOptionsResolver : IIdentityOptionsResolver
    {
        public IdentityOptionsResolver(
            SiteSettings siteSettings)
        {
            this.siteSettings = siteSettings;
        }

        private SiteSettings siteSettings;

        public IdentityOptions ResolveOptions(IdentityOptions singletonOptions)
        {
            var identityOptions = new IdentityOptions();
            identityOptions.ClaimsIdentity = singletonOptions.ClaimsIdentity;
            identityOptions.Cookies = singletonOptions.Cookies;
            identityOptions.Lockout = singletonOptions.Lockout;
            identityOptions.Password = singletonOptions.Password;
            identityOptions.SecurityStampValidationInterval = singletonOptions.SecurityStampValidationInterval;
            identityOptions.SignIn = singletonOptions.SignIn;
            identityOptions.Tokens = singletonOptions.Tokens;
            identityOptions.User = singletonOptions.User;

            identityOptions.Cookies.ExternalCookie.CookieName = AuthenticationScheme.External + "-" + siteSettings.SiteFolderName;
            identityOptions.Cookies.ExternalCookie.AuthenticationScheme = AuthenticationScheme.External + "-" + siteSettings.SiteFolderName;

            identityOptions.Cookies.ExternalCookie.LoginPath = new PathString("/" + siteSettings.SiteFolderName + "/account/login");
            identityOptions.Cookies.ExternalCookie.LogoutPath = new PathString("/" + siteSettings.SiteFolderName + "/account/logoff");
            identityOptions.Cookies.ExternalCookie.AccessDeniedPath = new PathString("/" + siteSettings.SiteFolderName + "/forbidden");
            //identityOptions.Cookies.ExternalCookie.AutomaticChallenge = true;
            //identityOptions.Cookies.ExternalCookie.AutomaticAuthenticate = true;

            identityOptions.Cookies.TwoFactorRememberMeCookie.CookieName = AuthenticationScheme.TwoFactorRememberMe + "-" + siteSettings.SiteFolderName;
            identityOptions.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe + "-" + siteSettings.SiteFolderName;

            identityOptions.Cookies.TwoFactorUserIdCookie.CookieName = AuthenticationScheme.TwoFactorUserId + "-" + siteSettings.SiteFolderName;
            identityOptions.Cookies.TwoFactorUserIdCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId + "-" + siteSettings.SiteFolderName;

            identityOptions.Cookies.ApplicationCookie.CookieName = AuthenticationScheme.Application + "-" + siteSettings.SiteFolderName;
            identityOptions.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application + "-" + siteSettings.SiteFolderName;
            //identityOptions.Cookies.ApplicationCookie.AccessDeniedPath
            //identityOptions.Cookies.ApplicationCookie.AutomaticAuthenticate 
            //identityOptions.Cookies.ApplicationCookie.AutomaticChallenge
            //identityOptions.Cookies.ApplicationCookie.ClaimsIssuer
            //identityOptions.Cookies.ApplicationCookie.CookieDomain
            //identityOptions.Cookies.ApplicationCookie.CookiePath
            //identityOptions.Cookies.ApplicationCookie.DataProtectionProvider
            //identityOptions.Cookies.ApplicationCookie.LoginPath
            //identityOptions.Cookies.ApplicationCookie.LogoutPath
            //identityOptions.Cookies.ApplicationCookie.ReturnUrlParameter
            //identityOptions.Cookies.ApplicationCookie.SlidingExpiration
            if(siteSettings.SiteFolderName.Length > 0)
            {
                identityOptions.Cookies.ApplicationCookie.CookiePath = new PathString("/" + siteSettings.SiteFolderName);
                identityOptions.Cookies.ApplicationCookie.LoginPath = new PathString("/" + siteSettings.SiteFolderName + "/account/login");
                identityOptions.Cookies.ApplicationCookie.LogoutPath = new PathString("/" + siteSettings.SiteFolderName + "/account/logoff");
                //identityOptions.Cookies.ApplicationCookie.AccessDeniedPath
            }


            return identityOptions;
        }
    }
}
