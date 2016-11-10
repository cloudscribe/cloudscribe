

using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.MySql;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFStorageMySql(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddCloudscribeCoreEFCommon();

            //services.AddEntityFrameworkSqlServer()
            //    .AddDbContext<CoreDbContext>((serviceProvider, options) =>
            //    options.UseSqlServer(connectionString)
            //           .UseInternalServiceProvider(serviceProvider)
            //           );
            services.AddEntityFrameworkMySQL()
                .AddDbContext<CoreDbContext>((serviceProvider, options) =>
                options.UseMySQL(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            services.AddScoped<ICoreDbContext, CoreDbContext>(); 
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
            
            return services;
        }

    }
}
