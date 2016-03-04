// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2016-03-04
// 

using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Twitter;

using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Localization;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Routing;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Messaging;
using cloudscribe.Web.Navigation;
using cloudscribe.Core.Identity;
//using cloudscribe.Core.Identity.OAuth;

namespace example.WebApp
{
    /// <summary>
    /// Setup application configuration for cloudscribe core
    /// 
    /// this file is part of cloudscribe.Core.Integration nuget package
    /// there are only a few lines of cloudscribe specific code in main Startup.cs and those reference methods in this file
    /// you are allowed to modify this file if needed but beware that if you upgrade the nuget package it would overwrite this file
    /// so you should probably make a copy of your changes somewhere first so you could restore them after upgrading
    /// </summary>
    public static class CloudscribeCoreApplicationBuilderExtensions
    {


        /// <summary>
        /// application configuration for cloudscribe core
        /// here is where we will need to do some magic for mutli tenants by folder if configured for that as by default
        /// we would also plug in any custom OWIN middleware components here
        /// things that would historically be implemented as HttpModules would now be implemented as OWIN middleware components
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCloudscribeCore(
            this IApplicationBuilder app,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IConfigurationRoot Configuration)
        {
            app.UseMultitenancy<SiteSettings>();

            // https://github.com/saaskit/saaskit/blob/master/src/SaasKit.Multitenancy/MultitenancyApplicationBuilderExtensions.cs
            // should custom builder extension use Microsoft.AspNet.Builder as their own namespace?
            // I can see the merits but wondered is that conventional
            app.UsePerTenant<SiteSettings>((ctx, builder) =>
            {

                var identityOptions = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
                if (identityOptions == null) { throw new ArgumentException("failed to get identity options"); }
                if (identityOptions.Cookies.ApplicationCookie == null) { throw new ArgumentException("failed to get identity application cookie options"); }

                var shouldUseFolder = (
                (multiTenantOptions.Value.Mode == MultiTenantMode.FolderName)
                && (!multiTenantOptions.Value.UseRelatedSitesMode)
                && (ctx.Tenant.SiteFolderName.Length > 0)
                );


                if (shouldUseFolder)
                {    
                    identityOptions.Cookies.ExternalCookie.CookieName = AuthenticationScheme.External + "-" + ctx.Tenant.SiteFolderName;
                    identityOptions.Cookies.ExternalCookie.AuthenticationScheme = AuthenticationScheme.External + "-" + ctx.Tenant.SiteFolderName;
                    
                    identityOptions.Cookies.ExternalCookie.LoginPath = new PathString("/" + ctx.Tenant.SiteFolderName + "/account/login");
                    identityOptions.Cookies.ExternalCookie.LogoutPath = new PathString("/" + ctx.Tenant.SiteFolderName + "/account/logoff");
                    identityOptions.Cookies.ExternalCookie.AccessDeniedPath = new PathString("/" + ctx.Tenant.SiteFolderName + "/forbidden");
                    //identityOptions.Cookies.ExternalCookie.AutomaticChallenge = true;
                    //identityOptions.Cookies.ExternalCookie.AutomaticAuthenticate = true;

                    identityOptions.Cookies.TwoFactorRememberMeCookie.CookieName = AuthenticationScheme.TwoFactorRememberMe + "-" + ctx.Tenant.SiteFolderName;
                    identityOptions.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe + "-" + ctx.Tenant.SiteFolderName;

                    identityOptions.Cookies.TwoFactorUserIdCookie.CookieName = AuthenticationScheme.TwoFactorUserId + "-" + ctx.Tenant.SiteFolderName;
                    identityOptions.Cookies.TwoFactorUserIdCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId + "-" + ctx.Tenant.SiteFolderName;
                    
                    identityOptions.Cookies.ApplicationCookie.CookieName = AuthenticationScheme.Application + "-" + ctx.Tenant.SiteFolderName;
                    identityOptions.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application + "-" + ctx.Tenant.SiteFolderName;

                }
                else
                {
                    
                    identityOptions.Cookies.ExternalCookie.CookieName = AuthenticationScheme.External;
                    identityOptions.Cookies.ExternalCookie.AuthenticationScheme = AuthenticationScheme.External;

                    identityOptions.Cookies.TwoFactorRememberMeCookie.CookieName = AuthenticationScheme.TwoFactorRememberMe;
                    identityOptions.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe;

                    identityOptions.Cookies.TwoFactorUserIdCookie.CookieName = AuthenticationScheme.TwoFactorUserId;
                    identityOptions.Cookies.TwoFactorUserIdCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId;

                    identityOptions.Cookies.ApplicationCookie.CookieName = AuthenticationScheme.Application;
                    identityOptions.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application;
                }

                //var cookieEvents = app.ApplicationServices.GetService<SiteCookieAuthenticationEvents>();
                //identityOptions.Cookies.ExternalCookie.Events = cookieEvents;
                //identityOptions.Cookies.TwoFactorRememberMeCookie.Events = cookieEvents;
                //identityOptions.Cookies.TwoFactorUserIdCookie.Events = cookieEvents;
                //identityOptions.Cookies.ApplicationCookie.Events = cookieEvents;

                builder.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
                builder.UseCookieAuthentication(identityOptions.Cookies.TwoFactorRememberMeCookie);
                builder.UseCookieAuthentication(identityOptions.Cookies.TwoFactorUserIdCookie);
                builder.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);

                //builder.UseCookieAuthentication(options =>
                //{

                //    options.LoginPath = new PathString("/account/login");
                //    options.AccessDeniedPath = new PathString("/account/forbidden");
                //    options.AutomaticAuthenticate = true;
                //    options.AutomaticChallenge = true;
                //});

                // TODO: will this require a restart if the options are updated in the ui?
                // no just need to clear the tenant cache after updating the settings
                if (!string.IsNullOrEmpty(ctx.Tenant.GoogleClientId))
                {
                    builder.UseGoogleAuthentication(options =>
                    {
                        options.AuthenticationScheme = "Google";
                        options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;

                        options.ClientId = ctx.Tenant.GoogleClientId;
                        options.ClientSecret = ctx.Tenant.GoogleClientSecret;

                        if (shouldUseFolder)
                        {
                            options.CallbackPath = "/" + ctx.Tenant.SiteFolderName + "/signin-google";
                        }

                    });
                }

                if (!string.IsNullOrEmpty(ctx.Tenant.FacebookAppId))
                {
                    builder.UseFacebookAuthentication(options =>
                    {
                        options.AuthenticationScheme = "Facebook";
                        options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;
                        options.AppId = ctx.Tenant.FacebookAppId;
                        options.AppSecret = ctx.Tenant.FacebookAppSecret;

                        if (shouldUseFolder)
                        {
                            options.CallbackPath = "/" + ctx.Tenant.SiteFolderName + "/signin-facebook";
                        }



                    });
                }

                if (!string.IsNullOrEmpty(ctx.Tenant.MicrosoftClientId))
                {
                    builder.UseMicrosoftAccountAuthentication(options =>
                    {
                        options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;
                        options.ClientId = ctx.Tenant.MicrosoftClientId;
                        options.ClientSecret = ctx.Tenant.MicrosoftClientSecret;
                        if (shouldUseFolder)
                        {
                            options.CallbackPath = "/" + ctx.Tenant.SiteFolderName + "/signin-microsoft";
                        }
                    });
                }

                if (!string.IsNullOrEmpty(ctx.Tenant.TwitterConsumerKey))
                {
                    builder.UseTwitterAuthentication(options =>
                    {
                        options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;
                        options.ConsumerKey = ctx.Tenant.TwitterConsumerKey;
                        options.ConsumerSecret = ctx.Tenant.TwitterConsumerSecret;
                        if (shouldUseFolder)
                        {
                            options.CallbackPath = "/" + ctx.Tenant.SiteFolderName + "/signin-twitter";
                        }

                    });
                }



            });


            return app;

        }

        



    }
}
