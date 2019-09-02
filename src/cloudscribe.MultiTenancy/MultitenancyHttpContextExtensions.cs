using cloudscribe.Multitenancy;

namespace Microsoft.AspNetCore.Http
{
	/// <summary>
    /// Multitenant extensions for <see cref="HttpContext"/>.
    /// </summary>
    public static class MultitenancyHttpContextExtensions
    {
        private const string TenantContextKey = "cloudscribe.TenantContext";

        public static void SetTenantContext<TTenant>(this HttpContext context, TenantContext<TTenant> tenantContext)
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenantContext, nameof(tenantContext));

            context.Items[TenantContextKey] = tenantContext;
        }

        public static TenantContext<TTenant> GetTenantContext<TTenant>(this HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            object tenantContext;
            if (context.Items.TryGetValue(TenantContextKey, out tenantContext))
            {
                return tenantContext as TenantContext<TTenant>;
            }

            return null;
        }

        public static TTenant GetTenant<TTenant>(this HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            var tenantContext = GetTenantContext<TTenant>(context);

            if (tenantContext != null)
            {
                return tenantContext.Tenant;
            }

            return default(TTenant);
        }
    }
}
