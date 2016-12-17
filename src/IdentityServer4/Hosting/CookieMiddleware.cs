// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Hosting
{
    public static class CookieMiddlewareExtensions
    {
        public static void ConfigureCookies(this IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(CookieMiddlewareExtensions).FullName);

            var options = app.ApplicationServices.GetRequiredService<IdentityServerOptions>();

            // only do stuff with cookies if we're showing UI
            if (options.Endpoints.EnableAuthorizeEndpoint)
            {
                if (options.AuthenticationOptions.AuthenticationScheme.IsMissing())
                {
                    logger.LogDebug("Using built-in CookieAuthentication middleware with scheme: {authenticationScheme}", options.AuthenticationOptions.EffectiveAuthenticationScheme);
                    app.UseCookieAuthentication(new CookieAuthenticationOptions
                    {
                        AuthenticationScheme = options.AuthenticationOptions.EffectiveAuthenticationScheme,
                        AutomaticAuthenticate = true,
                        SlidingExpiration = false,
                        ExpireTimeSpan = Constants.DefaultCookieTimeSpan,
                        CookieName = IdentityServerConstants.DefaultCookieAuthenticationScheme,
                    });
                }
                else
                {
                    logger.LogDebug("Using hosting application's CookieAuthentication middleware with scheme: {authenticationScheme}", options.AuthenticationOptions.EffectiveAuthenticationScheme);
                }

                app.UseMiddleware<AuthenticationMiddleware>();
                app.UseMiddleware<FederatedSignOutMiddleware>();
            }
        }
    }
}
