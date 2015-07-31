// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-31
// Last Modified:			2015-07-31
// 


using Microsoft.AspNet.Builder;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Cookies.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
