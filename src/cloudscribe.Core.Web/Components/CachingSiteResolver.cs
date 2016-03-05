// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:              Joe Audette
// Created:             2016-02-04
// Last Modified:       2016-03-05
// 

//  2016-02-04 found this blog post by Ben Foster
//  http://benfoster.io/blog/asp-net-5-multitenancy
//  and the related project https://github.com/saaskit/saaskit
//  I like his approach better than mine though they are similar
//  his seems a little cleaner so I'm adopting it here to replace my previous pattern
//  actual resolution process is the same as before

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
            IOptions<MultiTenantOptions> multiTenantOptions,
            IOptions<CachingSiteResolverOptions> cachingOptionsAccessor = null
            //,IOptions<MultiTenancyOptions> options
            )
            : base(cache, loggerFactory)
        {
            //this.tenants = options.Value.Tenants;
            siteRepo = siteRepository;
            this.multiTenantOptions = multiTenantOptions.Value;
            this.dataProtector = dataProtector;
            cachingOptions = cachingOptionsAccessor?.Value ?? new CachingSiteResolverOptions();
        }

        private MultiTenantOptions multiTenantOptions;
        private ISiteRepository siteRepo;
        private SiteDataProtector dataProtector;
        private CachingSiteResolverOptions cachingOptions;

        private List<string> GetAllSiteFoldersFolders()
        {
            var listCacheKey = "folderList";
            var result = cache.Get(listCacheKey) as List<string>;
            if(result != null)
            {
                log.LogDebug("Folder List retrieved from cache with key \"{cacheKey}\".", listCacheKey);
                return result;
            }

            result = siteRepo.GetAllSiteFolders();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cachingOptions.FolderListCacheDuration);

            log.LogDebug("Caching folder lsit with keys \"{cacheKey}\".", listCacheKey);
            cache.Set(listCacheKey, result, cacheEntryOptions);

            return result;

        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cachingOptions.SiteCacheDuration); 
        }

        // Determines what information in the current request should be used to do a cache lookup e.g.the hostname.
        protected override string GetContextIdentifier(HttpContext context)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                var siteFolderName = context.Request.Path.StartingSegment();
                var folders = GetAllSiteFoldersFolders();
                if(folders.Contains(siteFolderName)) { return siteFolderName; }
                siteFolderName = "root"; 
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
                else
                {
                    cacheKeys.Add("root");
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

           
        }

        private async Task<TenantContext<SiteSettings>> ResolveByFolderAsync(HttpContext context)
        {
            //var siteFolderName = context.Request.Path.StartingSegment();
            // (siteFolderName.Length == 0) { siteFolderName = "root"; }
            var siteFolderName = GetContextIdentifier(context);

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
