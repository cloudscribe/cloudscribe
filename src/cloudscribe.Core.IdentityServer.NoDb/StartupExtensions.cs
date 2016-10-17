// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-17
// 

using cloudscribe.Core.IdentityServer.NoDb;
using cloudscribe.Core.IdentityServerIntegration.StorageModels;
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
            builder.Services.AddNoDb<Scope>();

            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IScopeStore, ScopeStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            builder.Services.AddTransient<IScopeQueries, ScopeQueries>();
            builder.Services.AddTransient<IScopeCommands, ScopeCommands>();

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
            builder.Services.AddTransient<ScopeStore>();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddScopeStoreCache<ScopeStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddNoDb<PersistedGrant>();
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            return builder;
        }

    }
}
