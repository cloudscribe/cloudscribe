// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-01-23
// Last Modified:			2018-01-23
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class ApiAwareCookieAuthRedirector : ICookieAuthRedirector
    {
        public Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(
            HttpStatusCode statusCode,
            Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
                context => {
                    
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = (int)statusCode;
                        return Task.CompletedTask;
                    }

                    var tenant = context.HttpContext.GetTenant<SiteContext>();
                    if(tenant != null && !string.IsNullOrWhiteSpace(tenant.SiteFolderName))
                    {
                        if (context.Request.Path.StartsWithSegments("/" + tenant.SiteFolderName + "/api"))
                        {
                            context.Response.StatusCode = (int)statusCode;
                            return Task.CompletedTask;
                        }
                    }


                    return existingRedirector(context);
                };
    }
}
