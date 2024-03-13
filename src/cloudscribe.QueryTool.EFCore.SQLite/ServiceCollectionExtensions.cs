using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.QueryTool.EFCore.SQLite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryToolEFStorageSQLite(
            this IServiceCollection services,
            string connectionString,
            bool useSingletonLifetime = false,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            int commandTimeout = 30
            )
        {
            services.AddDbContext<QueryToolDbContext>(options =>
                    options.UseSqlite(connectionString,
                        sqliteOptionsAction: sqlOptions =>
                        {
                            if (maxConnectionRetryCount > 0)
                            {
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                sqlOptions.CommandTimeout(commandTimeout);
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
