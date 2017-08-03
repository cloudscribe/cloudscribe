using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.pgsql;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFStoragePostgreSql(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddCloudscribeCoreEFCommon();

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<CoreDbContext>(options =>
                    options.UseNpgsql(connectionString));

            services.AddScoped<ICoreDbContext, CoreDbContext>(); 
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
            
            return services;
        }

    }
}
