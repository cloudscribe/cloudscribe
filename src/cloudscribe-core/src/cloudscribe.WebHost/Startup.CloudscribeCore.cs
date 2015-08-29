// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-08-26
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
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Internal;
using Microsoft.Framework.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
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
            IConfiguration config,
            IOptions<MultiTenantOptions> multiTenantOptions)
        {

            // the only thing we are using session for is Alerts
            app.UseSession();
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(20));
            app.UseStatusCodePages();

            

            //// Add cookie-based authentication to the request pipeline.
            ////https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/BuilderExtensions.cs
            //app.UseIdentity();
            app.UseCloudscribeIdentity();

            //app.UseFacebookAuthentication();
            app.UseMultiTenantFacebookAuthentication();




            return app;
            
        }

        public static IApplicationBuilder UseMultiTenantMicrosoftAccountAuthentication(
            this IApplicationBuilder app, 
            Action<MicrosoftAccountAuthenticationOptions> configureOptions = null, 
            string optionsName = "")
        {
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountAuthenticationOptions.cs
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountAuthenticationDefaults.cs

            return app.UseMiddleware<MultiTenantMicrosoftAccountAuthenticationMiddleware>(
                 new ConfigureOptions<MicrosoftAccountAuthenticationOptions>(configureOptions ?? (o => { }))
                 {
                     Name = optionsName
                 });
        }

        public static IApplicationBuilder UseMultiTenantGoogleAuthentication(
            this IApplicationBuilder app, 
            Action<GoogleAuthenticationOptions> configureOptions = null, 
            string optionsName = "")
        {
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.Google/GoogleAuthenticationOptions.cs
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.Google/GoogleAuthenticationDefaults.cs

            return app.UseMiddleware<MultiTenantGoogleAuthenticationMiddleware>(
                 new ConfigureOptions<GoogleAuthenticationOptions>(configureOptions ?? (o => { }))
                 {
                     Name = optionsName
                 });
        }


        public static IApplicationBuilder UseMultiTenantFacebookAuthentication(
            this IApplicationBuilder app, 
            Action<FacebookAuthenticationOptions> configureOptions = null, 
            string optionsName = "")
        {
            //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookAuthenticationOptions.cs
            // https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.OAuth/OAuthAuthenticationOptions.cs

            return app.UseMiddleware<MultiTenantFacebookAuthenticationMiddleware>(
                 new ConfigureOptions<FacebookAuthenticationOptions>(configureOptions ?? (o => { }))
                 {
                     Name = optionsName
                 });
        }

       
        public static IApplicationBuilder UseMultiTenantTwitterAuthentication(
            this IApplicationBuilder app, 
            Action<TwitterAuthenticationOptions> configureOptions = null, string optionsName = "")
        {
            //https://github.com/aspnet/Security/blob/582f562bbb20fc76f37023086e2b2d861eb4d43d/src/Microsoft.AspNet.Authentication.Twitter/TwitterAuthenticationOptions.cs

            return app.UseMiddleware<MultiTenantTwitterAuthenticationMiddleware>(
                    new ConfigureOptions<TwitterAuthenticationOptions>(configureOptions ?? (o => { }))
                    {
                        Name = optionsName
                    });
        }
        


    }
}
