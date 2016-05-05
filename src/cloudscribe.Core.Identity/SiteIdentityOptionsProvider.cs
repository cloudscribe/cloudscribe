// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-05-04
// Last Modified:		    2016-05-05
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.OptionsModel;

namespace cloudscribe.Core.Identity
{
    public class SiteIdentityOptionsProvider : IOptions<IdentityOptions>
    {
        private CookieAuthenticationEvents cookieEvents;
        private SiteAuthCookieValidator siteValidator;
        private IHttpContextAccessor httpContextAccessor;

        public SiteIdentityOptionsProvider(
            IHttpContextAccessor httpContextAccessor,
            CookieAuthenticationEvents cookieEvents,
            SiteAuthCookieValidator siteValidator
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cookieEvents = cookieEvents;
            this.siteValidator = siteValidator;
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

                cookieEvents.OnValidatePrincipal = siteValidator.ValidatePrincipal;

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

                Setup(identityOptions.Cookies.ApplicationCookie, AuthenticationScheme.Application, tenant);
                Setup(identityOptions.Cookies.ExternalCookie, AuthenticationScheme.External, tenant);
                Setup(identityOptions.Cookies.TwoFactorRememberMeCookie, AuthenticationScheme.TwoFactorRememberMe, tenant);
                Setup(identityOptions.Cookies.TwoFactorUserIdCookie, AuthenticationScheme.TwoFactorUserId, tenant);
                
                var application = identityOptions.Cookies.ApplicationCookie;

                application.AutomaticAuthenticate = true;
                application.AutomaticChallenge = true;
                
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
            options.CookiePath = "/" + tenant.SiteFolderName;
            options.Events = cookieEvents;
        }
    }
}
