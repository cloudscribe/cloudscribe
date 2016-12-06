// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-12-06
// 

using cloudscribe.Core.IdentityServer.NoDb;
using cloudscribe.Core.IdentityServer.NoDb.Models;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeCoreNoDbIdentityServerStorage(
            this IIdentityServerBuilder builder
            )
        {
            builder.AddConfigurationStore();
            builder.AddOperationalStore();

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder)
        {

            builder.Services.AddNoDb<Client>();
            builder.Services.AddNoDb<ClientClaim>();
            builder.Services.AddNoDb<ApiResource>();
            builder.Services.AddNoDb<IdentityResource>();

            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            builder.Services.AddTransient<IApiResourceQueries, ApiResourceQueries>();
            builder.Services.AddTransient<IApiResourceCommands, ApiResourceCommands>();

            builder.Services.AddTransient<IIdentityResourceQueries, IdentityResourceQueries>();
            builder.Services.AddTransient<IIdentityResourceCommands, IdentityResourceCommands>();

            builder.Services.AddTransient<IClientQueries, ClientQueries>();
            builder.Services.AddTransient<IClientCommands, ClientCommands>();

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddMemoryCache(); // TODO: remove once update idsvr since it does this
            builder.AddInMemoryCaching();

            // these need to be registered as concrete classes in DI for
            // the caching decorators to work
            builder.Services.AddTransient<ClientStore>();
            builder.Services.AddTransient<ResourceStore>();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder)
        {

            builder.Services.AddNoDb<GrantItem>();
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            return builder;
        }

    }
}
