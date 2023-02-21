using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.QueryTool.EFCore.MySql
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryToolEFStorageMySql(
            this IServiceCollection services,
            string connectionString,
            bool useSingletonLifetime = false,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null!
            )
        {
            services.AddDbContext<QueryToolDbContext>(options =>
                    options.UseMySql(connectionString, 
                        ServerVersion.AutoDetect(connectionString),
                        mySqlOptionsAction: sqlOptions =>
                        {
                            if (maxConnectionRetryCount > 0)
                            {
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                sqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: maxConnectionRetryCount,
                                    maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                    errorNumbersToAdd: transientSqlErrorNumbersToAdd
                                );
                            }

                        }),
                        optionsLifetime: ServiceLifetime.Singleton
                        );

            services.AddScoped<IQueryToolDbContext, QueryToolDbContext>();
            services.AddSingleton<IQueryToolDbContextFactory, QueryToolDbContextFactory>();

            return services;
        }
    }
}
