using cloudscribe.Core.IdentityServer.EFCore.Extensions;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.SQLite;
using cloudscribe.Core.IdentityServer.EFCore.Stores;
using cloudscribe.Core.IdentityServerIntegration;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeCoreEFIdentityServerStorageSQLite(
           this IIdentityServerBuilder builder,
           string connectionString
           )
        {
            
            builder.Services.AddCloudscribeCoreIdentityServerEFStorageSQLite(connectionString);
            builder.Services.AddScoped<IStorageInfo, StorageInfo>();

            return builder;
        }

       
        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddMemoryCache(); // TODO: remove once update idsvr since it does this
            builder.AddInMemoryCaching();

            // these need to be registered as concrete classes in DI for
            // the caching decorators to work
            builder.Services.AddTransient<ClientStore>();
            builder.Services.AddTransient<ResourceStore>();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();

            return builder;
        }

        

        public static IServiceCollection AddCloudscribeCoreIdentityServerEFStorageSQLite(
            this IServiceCollection services,
            string connectionString
            )
        {
            services
                .AddDbContext<ConfigurationDbContext>(options =>
                    options.UseSqlite(connectionString));

            services.AddCloudscribeCoreIdentityServerStores();

            services.AddScoped<IConfigurationDbContext, ConfigurationDbContext>();

            services
                .AddDbContext<PersistedGrantDbContext>(options =>
                    options.UseSqlite(connectionString));

            services.AddScoped<IPersistedGrantDbContext, PersistedGrantDbContext>();

            return services;
        }

    }
}
