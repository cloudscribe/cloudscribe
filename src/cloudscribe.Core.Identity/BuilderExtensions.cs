// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-31
// Last Modified:			2016-05-18
// 


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
        //public static IApplicationBuilder UseMultiTenantCookieAuthentication(
        //    this IApplicationBuilder app, 
        //    Action<CookieAuthenticationOptions> configureOptions = null, 
        //    string optionsName = "")
        //{


        //    return app.UseMiddleware<MultiTenantCookieAuthenticationMiddleware>(
        //        new ConfigureOptions<CookieAuthenticationOptions>(configureOptions ?? (o => { }))
        //        {
        //            Name = optionsName

        //        });
        //}

        //public static IApplicationBuilder UseMultiTenantCookieAuthentication(
        //    this IApplicationBuilder app,
        //    CookieAuthenticationOptions options)
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException(nameof(app));
        //    }

        //    if (options == null)
        //    {
        //        throw new ArgumentNullException(nameof(options));
        //    }


        //    return app.UseMiddleware<MultiTenantCookieAuthenticationMiddleware>(options);
                
        //}

        //public static IApplicationBuilder UseCloudscribeIdentity(this IApplicationBuilder app)
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException(nameof(app));
        //    }

        //    MultiTenantCookieAuthenticationEvents cookieEvents
        //        = app.ApplicationServices.GetService<MultiTenantCookieAuthenticationEvents>();




        //    //app.UseMultiTenantCookieAuthentication(options =>
        //    //{

        //    //    options.Notifications = cookieNotifications;
        //    //}
        //    // , AuthenticationScheme.External
        //    //);

        //    var options = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
        //    if(options == null) { throw new ArgumentException("failed to get identity options"); }
        //    if (options.Cookies.ApplicationCookie == null) { throw new ArgumentException("failed to get identity application cookie options"); }

        //    options.Cookies.ExternalCookie.Events = cookieEvents;
        //    options.Cookies.ExternalCookie.CookieName = AuthenticationScheme.External;
        //    options.Cookies.ExternalCookie.AuthenticationScheme = AuthenticationScheme.External;

        //    options.Cookies.TwoFactorRememberMeCookie.Events = cookieEvents;
        //    options.Cookies.TwoFactorRememberMeCookie.CookieName = AuthenticationScheme.TwoFactorRememberMe;
        //    options.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe;

        //    options.Cookies.TwoFactorUserIdCookie.Events = cookieEvents;
        //    options.Cookies.TwoFactorUserIdCookie.CookieName = AuthenticationScheme.TwoFactorUserId;
        //    options.Cookies.TwoFactorUserIdCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId;


        //    options.Cookies.ApplicationCookie.CookieName = AuthenticationScheme.Application;
        //    options.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application;
        //    options.Cookies.ApplicationCookie.Events = cookieEvents;

        //    // these need to be resolved from site settings
        //    //options.Lockout.DefaultLockoutTimeSpan
        //    //options.Lockout.MaxFailedAccessAttempts
        //    //options.SecurityStampValidationInterval
        //    //options.SignIn.RequireConfirmedEmail
        //    //options.SignIn.RequireConfirmedPhoneNumber
        //    //options.Password.RequireDigit
        //    //options.Password.RequiredLength
        //    //options.Password.RequireLowercase
        //    //options.Password.RequireNonLetterOrDigit
        //    //options.Password.RequireUppercase
        //    //options.User.AllowedUserNameCharacters
        //    //options.User.RequireUniqueEmail
            
        //    app.UseMultiTenantCookieAuthentication(options.Cookies.ExternalCookie);

            
        //    app.UseMultiTenantCookieAuthentication(options.Cookies.TwoFactorRememberMeCookie);

            

        //    app.UseMultiTenantCookieAuthentication(options.Cookies.TwoFactorUserIdCookie);

            

        //    app.UseMultiTenantCookieAuthentication(options.Cookies.ApplicationCookie);

        //    return app;
        //}

    }
}
