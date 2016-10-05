// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore;
using cloudscribe.Core.IdentityServer.EFCore.DbContexts;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Services;
using cloudscribe.Core.IdentityServer.EFCore.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerEntityFrameworkBuilderExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeCoreEFIdentityServerStorage(
            this IIdentityServerBuilder builder,
            string connectionString
            )
        {
            //builder.AddConfigurationStore(contextBuilder =>
            //                contextBuilder.UseSqlServer(connectionString));
            builder.AddConfigurationStore(connectionString);

            //builder.AddOperationalStore(contextBuilder =>
            //                contextBuilder.UseSqlServer(connectionString));
            builder.AddOperationalStore(connectionString);


            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder, 
            string connectionString,
            Action<DbContextOptionsBuilder> optionsAction = null)
        {
            builder.Services.AddScoped<IConfigurationModelMapper, SqlServerConfigurationModelMapper>();

            //builder.Services.AddDbContext<ConfigurationDbContext>(optionsAction);
            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<ConfigurationDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );


            builder.Services.AddScoped<IConfigurationDbContext, ConfigurationDbContext>();

            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IScopeStore, ScopeStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

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
            this IIdentityServerBuilder builder,
            string connectionString,
            Action<DbContextOptionsBuilder> optionsAction = null)
        {
            builder.Services.AddScoped<IPersistedGrantModelMapper, SqlServerPersistedGrantModelMapper>();
            //builder.Services.AddDbContext<PersistedGrantDbContext>(optionsAction);

            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<PersistedGrantDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            builder.Services.AddScoped<IPersistedGrantDbContext, PersistedGrantDbContext>();

            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            
            return builder;
        }

        

    }
}