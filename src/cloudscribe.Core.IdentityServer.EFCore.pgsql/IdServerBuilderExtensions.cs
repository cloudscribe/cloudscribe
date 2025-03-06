﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.Extensions;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.pgsql;
using cloudscribe.Core.IdentityServer.EFCore.Stores;
using cloudscribe.Core.IdentityServerIntegration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeCoreEFIdentityServerStoragePostgreSql(
            this IIdentityServerBuilder builder,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<string> transientErrorCodesToAdd = null
            )
        {
            
            builder.Services.AddCloudscribeCoreIdentityServerEFStoragePostgreSql(connectionString, maxConnectionRetryCount, maxConnectionRetryDelaySeconds, transientErrorCodesToAdd);
            builder.Services.AddScoped<IStorageInfo, StorageInfo>();

            return builder;
        }

        

        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddMemoryCache(); // TODO: remove once update idsvr since it does this
            builder.AddInMemoryCaching();

            builder.Services.AddTransient<ClientStore>();
            builder.Services.AddTransient<ResourceStore>();

            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();

            return builder;
        }
        
        public static IServiceCollection AddCloudscribeCoreIdentityServerEFStoragePostgreSql(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<string> transientErrorCodesToAdd = null
            )
        {
            services.AddCloudscribeCoreIdentityServerStores();

            services.AddDbContext<ConfigurationDbContext>(options =>
                    options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorCodesToAdd: transientErrorCodesToAdd);
                        }
                    }),
                    optionsLifetime: ServiceLifetime.Singleton
                    );

            services.AddDbContext<PersistedGrantDbContext>(options =>
                    options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorCodesToAdd: transientErrorCodesToAdd);
                        }
                    }),
                    optionsLifetime: ServiceLifetime.Singleton
                    );

            services.AddScoped<IConfigurationDbContext, ConfigurationDbContext>();
            services.AddScoped<IPersistedGrantDbContext, PersistedGrantDbContext>();

            services.AddSingleton<IConfigurationDbContextFactory, ConfigurationDbContextFactory>();
            services.AddSingleton<IPersistedGrantDbContextFactory, PersistedGrantDbContextFactory>();

            return services;
        }
    }
}