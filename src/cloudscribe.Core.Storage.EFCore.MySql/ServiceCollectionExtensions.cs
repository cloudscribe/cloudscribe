using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.MySql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFStorageMySql(
            this IServiceCollection services,
            string connectionString,
            bool useSingletonLifetime = false,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null,
            MySqlDbContextOptionsBuilder mySqlOptions = null
            )
        {
            services.AddCloudscribeCoreEFCommon(useSingletonLifetime);

            //services.AddEntityFrameworkMySql()
            //    .AddDbContext<CoreDbContext>(options =>
            //        options.UseMySql(connectionString));

            services.AddEntityFrameworkMySql()
                .AddDbContext<CoreDbContext>(options =>
                    options.UseMySql(connectionString,
                    mySqlOptionsAction: sqlOptions =>
                    {
                        if (mySqlOptions != null)
                            sqlOptions = mySqlOptions;
                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorNumbersToAdd: transientSqlErrorNumbersToAdd);
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
