using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasKit.Multitenancy
{
    // modified from here:
    // https://github.com/saaskit/saaskit/blob/master/src/SaasKit.Multitenancy/MemoryCacheTenantResolver.cs
    // I needed to change the GetContextIdentifier to be async

    public abstract class MemoryCacheTenantResolverBase<TTenant> : ITenantResolver<TTenant>
    {
        protected readonly IMemoryCache cache;
        protected readonly ILogger log;

        public MemoryCacheTenantResolverBase(IMemoryCache cache, ILoggerFactory loggerFactory)
        {
            Ensure.Argument.NotNull(cache, nameof(cache));
            Ensure.Argument.NotNull(loggerFactory, nameof(loggerFactory));

            this.cache = cache;
            this.log = loggerFactory.CreateLogger<MemoryCacheTenantResolverBase<TTenant>>();
        }

        protected virtual MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(new TimeSpan(1, 0, 0))
                .RegisterPostEvictionCallback((key, value, reason, state)
                    => DisposeTenantContext(key, value as TenantContext<TTenant>));
        }

        protected virtual void DisposeTenantContext(object cacheKey, TenantContext<TTenant> tenantContext)
        {
            if (tenantContext != null)
            {
                log.LogDebug("Disposing TenantContext:{id} instance with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
                tenantContext.Dispose();
            }
        }

        protected abstract Task<string> GetContextIdentifier(HttpContext context);
        protected abstract IEnumerable<string> GetTenantIdentifiers(TenantContext<TTenant> context);
        protected abstract Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);

        async Task<TenantContext<TTenant>> ITenantResolver<TTenant>.ResolveAsync(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            // Obtain the key used to identify cached tenants from the current request
            var cacheKey = await GetContextIdentifier(context);

            var tenantContext = cache.Get(cacheKey) as TenantContext<TTenant>;

            if (tenantContext == null)
            {
                log.LogDebug("TenantContext not present in cache with key \"{cacheKey}\". Attempting to resolve.", cacheKey);
                tenantContext = await ResolveAsync(context);

                if (tenantContext != null)
                {
                    var tenantIdentifiers = GetTenantIdentifiers(tenantContext);
                    var cacheEntryOptions = CreateCacheEntryOptions();

                    log.LogDebug("TenantContext:{id} resolved. Caching with keys \"{tenantIdentifiers}\".", tenantContext.Id, tenantIdentifiers);

                    foreach (var identifier in tenantIdentifiers)
                    {
                        cache.Set(identifier, tenantContext, cacheEntryOptions);
                    }
                }
            }
            else
            {
                log.LogDebug("TenantContext:{id} retrieved from cache with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
            }

            return tenantContext;
        }
    }
}
