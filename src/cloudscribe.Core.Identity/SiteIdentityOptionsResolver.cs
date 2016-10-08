// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-05-04
// Last Modified:		    2016-10-08
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Identity
{
    public class SiteIdentityOptionsResolver : IOptions<IdentityOptions>
    {
        private CookieAuthenticationEvents cookieEvents;
        private SiteAuthCookieValidator siteValidator;
        private IHttpContextAccessor httpContextAccessor;
        private MultiTenantOptions multiTenantOptions;
        private TokenOptions tokenOptions;

        public SiteIdentityOptionsResolver(
            IHttpContextAccessor httpContextAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<TokenOptions> tokenOptionsAccessor,
            SiteAuthCookieValidator siteValidator
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cookieEvents = new CookieAuthenticationEvents();
            this.siteValidator = siteValidator;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            tokenOptions = tokenOptionsAccessor.Value;
        }

        public IdentityOptions Value
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var tenant = context.GetTenant<SiteContext>();
                var identityOptions = new IdentityOptions();

                identityOptions.Tokens = tokenOptions;

                identityOptions.Password.RequiredLength = tenant.MinRequiredPasswordLength;
                identityOptions.Password.RequireNonAlphanumeric = true; //default
                identityOptions.Password.RequireLowercase = true; //default
                identityOptions.Password.RequireUppercase = true; //default
                identityOptions.Password.RequireDigit = true; // default

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = tenant.MaxInvalidPasswordAttempts;

                identityOptions.SignIn.RequireConfirmedEmail = tenant.RequireConfirmedEmail;
                // this is a dangerous setting -existing users including admin can't login if they don't have a phone
                // number configured and there is no way for them to add the needed number
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = tenant.RequireConfirmedPhone;

                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // default value
                
                SetupAppCookie(identityOptions.Cookies.ApplicationCookie, AuthenticationScheme.Application, tenant);
                SetupOtherCookies(identityOptions.Cookies.ExternalCookie, AuthenticationScheme.External, tenant);
                SetupOtherCookies(identityOptions.Cookies.TwoFactorRememberMeCookie, AuthenticationScheme.TwoFactorRememberMe, tenant);
                SetupOtherCookies(identityOptions.Cookies.TwoFactorUserIdCookie, AuthenticationScheme.TwoFactorUserId, tenant);
                
                return identityOptions;
            }
        }

        private void SetupAppCookie(CookieAuthenticationOptions options, string scheme, SiteContext tenant)
        {
            if(multiTenantOptions.UseRelatedSitesMode)
            {
                options.AuthenticationScheme = scheme;
                options.CookieName =scheme;
                options.CookiePath = "/";
            }
            else
            {
                //options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
                options.AuthenticationScheme = scheme;
                options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
                options.CookiePath = "/" + tenant.SiteFolderName;
                cookieEvents.OnValidatePrincipal = siteValidator.ValidatePrincipal;
            }
            
            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.AccessDeniedPath = tenantPathBase + "/account/accessdenied";

            options.Events = cookieEvents;

            options.AutomaticAuthenticate = true;
            options.AutomaticChallenge = true;
        }

        private void SetupOtherCookies(CookieAuthenticationOptions options, string scheme, SiteContext tenant)
        {
            if (multiTenantOptions.UseRelatedSitesMode)
            {
                options.AuthenticationScheme = scheme;
                options.CookieName = scheme;
                options.CookiePath = "/";
            }
            else
            {
                //options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
                options.AuthenticationScheme = scheme;
                options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
                options.CookiePath = "/" + tenant.SiteFolderName;
            }
            //options.AutomaticAuthenticate = false;
        }
    }
}
