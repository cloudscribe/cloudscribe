using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Core.Storage.EFCore.MySql;
using Microsoft.EntityFrameworkCore;

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

            services.AddEntityFrameworkMySql()
                .AddDbContext<CoreDbContext>(options =>
                    options.UseMySql(connectionString));

            services.AddScoped<ICoreDbContext, CoreDbContext>(); 
            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
            
            return services;
        }

    }
}
