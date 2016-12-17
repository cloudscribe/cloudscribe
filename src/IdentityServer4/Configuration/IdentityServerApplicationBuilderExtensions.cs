// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Hosting;
using IdentityServer4.Stores;
using IdentityServer4.Stores.InMemory;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class IdentityServerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this IApplicationBuilder app)
        {
            app.Validate();

            app.UseMiddleware<BaseUrlMiddleware>();

            app.ConfigureCors();
            app.ConfigureCookies();

            app.UseMiddleware<IdentityServerMiddleware>();

            return app;
        }

        internal static void Validate(this IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger("IdentityServer4.Startup");

            // todo: which other services to test for?
            app.TestService(typeof(IPersistedGrantStore), logger, "No storage mechanism for grants specified. Use the 'AddInMemoryPersistedGrants' extension method to register a development version.");
            app.TestService(typeof(IClientStore), logger, "No storage mechanism for clients specified. Use the 'AddInMemoryClients' extension method to register a development version.");
            app.TestService(typeof(IResourceStore), logger, "No storage mechanism for resources specified. Use the 'AddInMemoryResources' extension method to register a development version.");

            var persistedGrants = app.ApplicationServices.GetService(typeof(IPersistedGrantStore));
            if (persistedGrants.GetType().FullName == typeof(InMemoryPersistedGrantStore).FullName)
            {
                logger.LogInformation("You are using the in-memory version of the persisted grant store. This will store consent decisions, authorization codes, refresh and reference tokens in memory only. If you are using any of those features in production, you want to switch to a different store implementation.");
            }
        }

        internal static object TestService(this IApplicationBuilder app, Type service, ILogger logger, string message = null, bool doThrow = true)
        {
            var appService = app.ApplicationServices.GetService(service);

            if (appService == null)
            {
                var error = message ?? $"Required service {service.FullName} is not registered in the DI container. Aborting startup";

                logger.LogCritical(error);

                if (doThrow)
                {
                    throw new InvalidOperationException(error);
                }
            }

            return appService;
        }
    }
}