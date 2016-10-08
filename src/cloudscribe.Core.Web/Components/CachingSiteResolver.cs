// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:              Joe Audette
// Created:             2016-02-04
// Last Modified:       2016-10-08
// 

//  2016-02-04 found this blog post by Ben Foster
//  http://benfoster.io/blog/asp-net-5-multitenancy
//  and the related project https://github.com/saaskit/saaskit
//  I like his approach better than mine though they are similar
//  his seems a little cleaner so I'm adopting it here to replace my previous pattern
//  actual resolution process is the same as before

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class CachingSiteResolver : MemoryCacheTenantResolverBase<SiteContext>
    {
        
        public CachingSiteResolver(
            IMemoryCache cache,
            ILoggerFactory loggerFactory,
            ISiteQueries siteRepository,
            SiteDataProtector dataProtector,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IOptions<CachingSiteResolverOptions> cachingOptionsAccessor
            )
            : base(cache, loggerFactory)
        {
            siteRepo = siteRepository;
            this.multiTenantOptions = multiTenantOptions.Value;
            this.dataProtector = dataProtector;
            cachingOptions = cachingOptionsAccessor.Value;
        }

        private MultiTenantOptions multiTenantOptions;
        private ISiteQueries siteRepo;
        private SiteDataProtector dataProtector;
        private CachingSiteResolverOptions cachingOptions;

        private async Task<List<string>> GetAllSiteFoldersFolders()
        {
            var listCacheKey = "folderList";
            var result = cache.Get(listCacheKey) as List<string>;
            if(result != null)
            {
                log.LogDebug("Folder List retrieved from cache with key \"{cacheKey}\".", listCacheKey);
                return result;
            }

            result = await siteRepo.GetAllSiteFolders();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cachingOptions.FolderListCacheDuration);

            log.LogDebug("Caching folder list with keys \"{cacheKey}\".", listCacheKey);
            cache.Set(listCacheKey, result, cacheEntryOptions);

            return result;

        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cachingOptions.SiteCacheDuration); 
        }

        // Determines what information in the current request should be used to do a cache lookup e.g.the hostname.
        protected override async Task<string> GetContextIdentifier(HttpContext context)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                var fullPath = context.Request.PathBase + context.Request.Path;
                var siteFolderName = fullPath.StartingSegment();
                // I really want this to be async
                // checking with Ben if he will make the base class method async
                var folders = await GetAllSiteFoldersFolders();

                return folders.Contains(siteFolderName) ? siteFolderName : "root";
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
        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<SiteContext> context)
        {
            var identifiers = new List<string>();

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (context.Tenant.SiteFolderName.Length > 0)
                {
                    identifiers.Add(context.Tenant.SiteFolderName);
                }
                else
                {
                    identifiers.Add("root");
                }
            }

            var siteGuid = context.Tenant.Id.ToString();

            identifiers.Add(siteGuid);

            return identifiers;
        }

        //Resolve a tenant context from the current request. This will only be executed on cache misses.
        protected override Task<TenantContext<SiteContext>> ResolveAsync(HttpContext context)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                return ResolveByFolderAsync(context);
            }

            return ResolveByHostAsync(context);           
        }

        private async Task<TenantContext<SiteContext>> ResolveByFolderAsync(HttpContext context)
        {
            var siteFolderName = await GetContextIdentifier(context);

            TenantContext<SiteContext> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            var site = await siteRepo.FetchByFolderName(siteFolderName, cancellationToken);

            if (site != null)
            {
                dataProtector.UnProtect(site);
                var siteContext = new SiteContext(site);
                tenantContext = new TenantContext<SiteContext>(siteContext);
            }

            return tenantContext;
        }

        private async Task<TenantContext<SiteContext>> ResolveByHostAsync(HttpContext context)
        {
            TenantContext<SiteContext> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            ISiteSettings site = await siteRepo.Fetch(context.Request.Host.Value, cancellationToken);

            if (site != null)
            {
                dataProtector.UnProtect(site);
                var siteContext = new SiteContext(site);
                tenantContext = new TenantContext<SiteContext>(siteContext);
            }

            return tenantContext;
        }
    }
}
