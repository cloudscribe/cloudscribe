

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Storage.EF;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFStorage(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<CoreDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            //services.AddDbContext<CoreDbContext>(options =>
            //{
            //    options.UseSqlServer(connectionString) ;
            //});

            services.AddScoped<ICoreModelMapper, SqlServerCoreModelMapper>();
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();

            services.AddScoped<ISiteCommands, SiteCommands>();
            services.AddScoped<ISiteQueries, SiteQueries>();

            services.AddScoped<IUserCommands, UserCommands>();
            services.AddScoped<IUserQueries, UserQueries>();

            services.AddScoped<IGeoCommands, GeoCommands>();
            services.AddScoped<IGeoQueries, GeoQueries>();

            return services;
        }

    }
}
