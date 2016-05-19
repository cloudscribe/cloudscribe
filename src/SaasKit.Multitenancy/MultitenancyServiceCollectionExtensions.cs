using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SaasKit.Multitenancy;
using SaasKit.Multitenancy.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy<TTenant, TResolver>(this IServiceCollection services) 
            where TResolver : class, ITenantResolver<TTenant>
            where TTenant : class
        {
            Ensure.Argument.NotNull(services, nameof(services));

            services.AddScoped<ITenantResolver<TTenant>, TResolver>();

            // Make Tenant and TenantContext injectable
            services.AddScoped(prov => 
                prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenant<TTenant>());

            services.AddScoped(prov =>
                prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>());

            // Ensure caching is available for caching resolvers
            services.AddMemoryCache();
            //services.AddCaching();

            return services;
        }
    }
}
