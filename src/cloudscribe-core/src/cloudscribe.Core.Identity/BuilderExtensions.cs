// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-31
// Last Modified:			2015-08-01
// 


using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;
using System;

namespace cloudscribe.Core.Identity
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Adds a cookie-based authentication middleware to your web application pipeline.
        /// </summary>
        /// <param name="app">The IApplicationBuilder passed to your configuration method</param>
        /// <param name="configureOptions">Used to configure the options for the middleware</param>
        /// <param name="optionsName">The name of the options class that controls the middleware behavior, null will use the default options</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseMultiTenantCookieAuthentication(
            this IApplicationBuilder app, 
            Action<CookieAuthenticationOptions> configureOptions = null, 
            string optionsName = "")
        {
            

            return app.UseMiddleware<MultiTenantCookieAuthenticationMiddleware>(
                new ConfigureOptions<CookieAuthenticationOptions>(configureOptions ?? (o => { }))
                {
                    Name = optionsName
                    
                });
        }

        public static IApplicationBuilder UseCloudscribeIdentity(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            MultiTenantCookieAuthenticationNotifications cookieNotifications
                = app.ApplicationServices.GetService<MultiTenantCookieAuthenticationNotifications>();

            app.UseMultiTenantCookieAuthentication(options =>
            {
                options.Notifications = cookieNotifications;
            }
            , 
            AuthenticationScheme.External);

            app.UseMultiTenantCookieAuthentication(options =>
            {
                options.Notifications = cookieNotifications;
            }
            , 
            AuthenticationScheme.TwoFactorRememberMe);

            app.UseMultiTenantCookieAuthentication(options =>
            {
                options.Notifications = cookieNotifications;
            }
            , 
            AuthenticationScheme.TwoFactorUserId);

            app.UseMultiTenantCookieAuthentication(options =>
            {
                options.Notifications = cookieNotifications;
            }
            , 
            AuthenticationScheme.Application);

            return app;
        }

    }
}
