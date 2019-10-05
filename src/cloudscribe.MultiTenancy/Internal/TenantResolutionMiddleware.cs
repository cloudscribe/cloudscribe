using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace cloudscribe.Multitenancy.Internal
{
	public class TenantResolutionMiddleware<TTenant>
    {
        private readonly RequestDelegate next;
        private readonly ILogger log;

        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(loggerFactory, nameof(loggerFactory));

            this.next = next;
            this.log = loggerFactory.CreateLogger<TenantResolutionMiddleware<TTenant>>();
        }

        public async Task Invoke(HttpContext context, ITenantResolver<TTenant> tenantResolver)
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenantResolver, nameof(tenantResolver));

            log.LogDebug("Resolving TenantContext using {loggerType}.", tenantResolver.GetType().Name);

            var tenantContext = await tenantResolver.ResolveAsync(context);

            if (tenantContext != null)
            {
                log.LogDebug("TenantContext Resolved. Adding to HttpContext.");
                context.SetTenantContext(tenantContext);
            }
            else
            {
                log.LogDebug("TenantContext Not Resolved.");
            }

            await next.Invoke(context);
        }
    }
}
