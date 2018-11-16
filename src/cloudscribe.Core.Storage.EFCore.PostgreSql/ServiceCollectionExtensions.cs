using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.PostgreSql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCorePostgreSqlStorage(
            this IServiceCollection services,
            string connectionString,
            bool useSingletonLifetime = false,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<string> transientErrorCodesToAdd = null
            )
        {
            services.AddCloudscribeCoreEFCommon(useSingletonLifetime);

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<CoreDbContext>(options =>
                    options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
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

            services.AddScoped<ICoreDbContext, CoreDbContext>();
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();

            
            services.AddSingleton<ICoreDbContextFactory, CoreDbContextFactory>();

            return services;
        }
    }
    
}
