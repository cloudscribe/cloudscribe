using cloudscribe.Multitenancy;
using cloudscribe.Multitenancy.Internal;

namespace Microsoft.AspNetCore.Builder
{
    public static class MultitenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy<TTenant>(this IApplicationBuilder app)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            return app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();
        }
    }
}
