// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Stores;
using IdentityServer4.Stores.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerServiceCollectionExtensions
    {
        public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
        {
            return new IdentityServerBuilder(services);
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();

            builder.AddRequiredPlatformServices();
            
            builder.AddCoreServices();
            builder.AddDefaultEndpoints();
            builder.AddPluggableServices();
            builder.AddValidators();
            builder.AddResponseGenerators();
            
            builder.AddDefaultSecretParsers();
            builder.AddDefaultSecretValidators();

            // provide default in-memory implementation, not suitable for most production scenarios
            builder.AddInMemoryPersistedGrants();

            return new IdentityServerBuilder(services);
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServer();
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityServerOptions>(configuration);
            return services.AddIdentityServer();
        }
    }
}