using cloudscribe.Multitenancy;
using cloudscribe.Multitenancy.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			// Make Tenant and TenantContext injectable
			services.AddScoped(prov => prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>());
			services.AddScoped(prov => prov.GetService<TenantContext<TTenant>>()?.Tenant);

			// Make ITenant injectable for handling null injection, similar to IOptions
			services.AddScoped<ITenant<TTenant>>(prov => new TenantWrapper<TTenant>(prov.GetService<TTenant>()));

            return services;
        }
    }
}
