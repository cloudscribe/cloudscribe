
using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class CachingSiteResolver : MemoryCacheTenantResolver<SiteSettings>
    {
        //private readonly IEnumerable<SiteSettings> tenants;

        public CachingSiteResolver(
            IMemoryCache cache,
            ILoggerFactory loggerFactory,
            ISiteRepository siteRepository,
            SiteDataProtector dataProtector,
            IOptions<MultiTenantOptions> multiTenantOptions
            //,IOptions<MultiTenancyOptions> options
            )
            : base(cache, loggerFactory)
        {
            //this.tenants = options.Value.Tenants;
            siteRepo = siteRepository;
            this.multiTenantOptions = multiTenantOptions.Value;
            this.dataProtector = dataProtector;
        }

        private MultiTenantOptions multiTenantOptions;
        private ISiteRepository siteRepo;
        private SiteDataProtector dataProtector;

        // Determines what information in the current request should be used to do a cache lookup e.g.the hostname.
        protected override string GetContextIdentifier(HttpContext context)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                var siteFolderName = context.Request.Path.StartingSegment();
                if (siteFolderName.Length == 0) { siteFolderName = "root"; }
                return siteFolderName;
            }
            return context.Request.Host.Value.ToLower();
        }

        /// <summary>
        /// Determines the identifiers (keys) used to cache the tenant context. 
        /// In our example tenants can have multiple domains, so we return each of the 
        /// hostnames as identifiers.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<SiteSettings> context)
        {
            List<string> cacheKeys = new List<string>();
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if(context.Tenant.SiteFolderName.Length > 0)
                {
                    cacheKeys.Add(context.Tenant.SiteFolderName);
                }
            }
            var id =  context.Tenant.SiteGuid.ToString();
            var id2 = "site-" + context.Tenant.SiteId.ToInvariantString();
            cacheKeys.Add(id2);
            cacheKeys.Add(id);

            return cacheKeys;
        }

        //Resolve a tenant context from the current request. This will only be executed on cache misses.
        protected override Task<TenantContext<SiteSettings>> ResolveAsync(HttpContext context)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                return ResolveByFolderAsync(context);
            }

            return ResolveByHostAsync(context);

            //TenantContext<SiteSettings> tenantContext = null;

            //var tenant = tenants.FirstOrDefault(t =>
            //    t.Hostnames.Any(h => h.Equals(context.Request.Host.Value.ToLower())));

            //if (tenant != null)
            //{
            //    tenantContext = new TenantContext<SiteSettings>(tenant);
            //}

            //return Task.FromResult(tenantContext);
        }

        private async Task<TenantContext<SiteSettings>> ResolveByFolderAsync(HttpContext context)
        {
            var siteFolderName = context.Request.Path.StartingSegment();
            if (siteFolderName.Length == 0) { siteFolderName = "root"; }

            TenantContext<SiteSettings> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            ISiteSettings site
                = await siteRepo.FetchByFolderName(siteFolderName, cancellationToken);

            if (site != null)
            {
                dataProtector.UnProtect(site);

                tenantContext = new TenantContext<SiteSettings>((SiteSettings)site);
            }

            return tenantContext;


        }

        private async Task<TenantContext<SiteSettings>> ResolveByHostAsync(HttpContext context)
        {
            TenantContext<SiteSettings> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            ISiteSettings site
                = await siteRepo.Fetch(context.Request.Host.Value, cancellationToken);

            if (site != null)
            {
                dataProtector.UnProtect(site);

                tenantContext = new TenantContext<SiteSettings>((SiteSettings)site);
            }

            return tenantContext;
        }


    }
}
