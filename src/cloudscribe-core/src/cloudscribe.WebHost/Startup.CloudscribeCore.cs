// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-10-17
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
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Caching;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.Localization;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Internal;
using Microsoft.Framework.Configuration;
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
using cloudscribe.Core.Identity.OAuth;

namespace cloudscribe.WebHost
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

           

            

            //// Add cookie-based authentication to the request pipeline.
            ////https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/BuilderExtensions.cs
            //app.UseIdentity();
            app.UseCloudscribeIdentity();

            app.UseMultiTenantFacebookAuthentication(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            app.UseMultiTenantGoogleAuthentication(options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            app.UseMultiTenantMicrosoftAccountAuthentication(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });

            app.UseMultiTenantTwitterAuthentication(options =>
            {
                options.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                options.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            });

            //app.UseCultureReplacer();

            //app.UseRequestLocalization();


            return app;
            
        }

        public static IApplicationBuilder UseMultiTenantMicrosoftAccountAuthentication(
            this IApplicationBuilder app, 
            Action<MicrosoftAccountOptions> configureOptions)
        {
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountOptions.cs
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountDefaults.cs

            var options = new MicrosoftAccountOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

            return app.UseMiddleware<MultiTenantMicrosoftAccountMiddleware>(options);

        }

        public static IApplicationBuilder UseMultiTenantGoogleAuthentication(
            this IApplicationBuilder app, 
            Action<GoogleOptions> configureOptions)
        {
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.Google/GoogleOptions.cs
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.Google/GoogleDefaults.cs

            var options = new GoogleOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

            return app.UseMiddleware<MultiTenantGoogleMiddleware>(options);
        }


        public static IApplicationBuilder UseMultiTenantFacebookAuthentication(
            this IApplicationBuilder app, 
            Action<FacebookOptions> configureOptions)
        {
            //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookOptions.cs
            // https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.OAuth/OAuthOptions.cs

            var options = new FacebookOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

            return app.UseMiddleware<MultiTenantFacebookMiddleware>(options);
        }

       
        public static IApplicationBuilder UseMultiTenantTwitterAuthentication(
            this IApplicationBuilder app, 
            Action<TwitterOptions> configureOptions)
        {
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.Twitter/TwitterOptions.cs

            var options = new TwitterOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

            return app.UseMiddleware<MultiTenantTwitterMiddleware>(options);
        }
        


    }
}
