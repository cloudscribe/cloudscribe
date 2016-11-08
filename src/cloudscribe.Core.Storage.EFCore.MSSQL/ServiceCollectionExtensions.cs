

using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.MSSQL;
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
            services.AddCloudscribeCoreEFCommon();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<CoreDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            services.AddScoped<ICoreDbContext, CoreDbContext>();
            
            services.AddScoped<ICoreModelMapper, SqlServerCoreModelMapper>();
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
            
            return services;
        }

    }
}
