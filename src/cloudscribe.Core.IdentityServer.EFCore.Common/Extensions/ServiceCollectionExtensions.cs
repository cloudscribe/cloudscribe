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
            services.AddTransient<IScopeStore, ScopeStore>();
            services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            services.AddTransient<IScopeQueries, ScopeQueries>();
            services.AddTransient<IScopeCommands, ScopeCommands>();

            services.AddTransient<IClientQueries, ClientQueries>();
            services.AddTransient<IClientCommands, ClientCommands>();

            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            return services;
        }
    }
}
