using cloudscribe.Core.IdentityServer.EFCore.Services;
using cloudscribe.Core.IdentityServer.EFCore.Stores;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.Core.IdentityServer.EFCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreIdentityServerStores(this IServiceCollection services)
        {
            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            services.AddTransient<IApiResourceQueries, ApiResourceQueries>();
            services.AddTransient<IApiResourceCommands, ApiResourceCommands>();

            services.AddTransient<IIdentityResourceQueries, IdentityResourceQueries>();
            services.AddTransient<IIdentityResourceCommands, IdentityResourceCommands>();

            services.AddTransient<IClientQueries, ClientQueries>();
            services.AddTransient<IClientCommands, ClientCommands>();

            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

            services.AddSingleton<IApiResourceCommandsSingleton, ApiResourceCommands>();
            services.AddSingleton<IApiResourceQueriesSingleton, ApiResourceQueries>();

            services.AddSingleton<IIdentityResourceQueriesSingleton, IdentityResourceQueries>();
            services.AddSingleton<IIdentityResourceCommandsSingleton, IdentityResourceCommands>();

            services.AddSingleton<IClientQueriesSingleton, ClientQueries>();
            services.AddSingleton<IClientCommandsSingleton, ClientCommands>();

            return services;
        }
    }
}
