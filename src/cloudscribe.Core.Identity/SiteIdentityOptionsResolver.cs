// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-05-04
// Last Modified:		    2016-05-18
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Identity
{
    public class SiteIdentityOptionsResolver : IOptions<IdentityOptions>
    {
        private CookieAuthenticationEvents cookieEvents;
        private SiteAuthCookieValidator siteValidator;
        private IHttpContextAccessor httpContextAccessor;

        public SiteIdentityOptionsResolver(
            IHttpContextAccessor httpContextAccessor,
           // CookieAuthenticationEvents cookieEvents,
            SiteAuthCookieValidator siteValidator
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cookieEvents = new CookieAuthenticationEvents();
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
                
                // TODO: I'm not sure newing this up here is agood idea
                // are we missing any default configuration thast would normally be set for identity?
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

                SetupAppCookie(identityOptions.Cookies.ApplicationCookie, AuthenticationScheme.Application, tenant);
                SetupOtherCookies(identityOptions.Cookies.ExternalCookie, AuthenticationScheme.External, tenant);
                SetupOtherCookies(identityOptions.Cookies.TwoFactorRememberMeCookie, AuthenticationScheme.TwoFactorRememberMe, tenant);
                SetupOtherCookies(identityOptions.Cookies.TwoFactorUserIdCookie, AuthenticationScheme.TwoFactorUserId, tenant);
                
                return identityOptions;
            }
        }

        private void SetupAppCookie(CookieAuthenticationOptions options, string scheme, SiteSettings tenant)
        {
            options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
            options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
            options.CookiePath = "/" + tenant.SiteFolderName;

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";

            cookieEvents.OnValidatePrincipal = siteValidator.ValidatePrincipal;
            options.Events = cookieEvents;

            options.AutomaticAuthenticate = true;
            options.AutomaticChallenge = true;
        }

        private void SetupOtherCookies(CookieAuthenticationOptions options, string scheme, SiteSettings tenant)
        {
            //var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
            //    ? PathString.Empty
            //    : new PathString("/" + tenant.SiteFolderName);

            options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
            options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
            options.CookiePath = "/" + tenant.SiteFolderName;
            
        }
    }
}
