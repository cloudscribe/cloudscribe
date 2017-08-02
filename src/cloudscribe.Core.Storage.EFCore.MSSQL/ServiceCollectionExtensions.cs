

using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFStorageMSSQL(
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

            //services.AddEntityFrameworkSqlServer()
            //    .AddDbContext<CoreDbContext>(options => 
            //    {
            //        options.UseSqlServer(connectionString);

            //    },ServiceLifetime.Scoped);

            services.AddScoped<ICoreDbContext, CoreDbContext>(); 
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
            
            return services;
        }

    }
}
