//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:              Joe Audette
//// Created:             2016-02-04
//// Last Modified:       2018-03-13
//// 

////  2016-02-04 found this blog post by Ben Foster
////  http://benfoster.io/blog/asp-net-5-multitenancy
////  and the related project https://github.com/saaskit/saaskit
////  I like his approach better than mine though they are similar
////  his seems a little cleaner so I'm adopting it here to replace my previous pattern
////  actual resolution process is the same as before

//using cloudscribe.Core.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using SaasKit.Multitenancy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Web.Components
//{
//    public class CachingSiteResolver : MemoryCacheTenantResolverBase<SiteContext>
//    {
        
//        public CachingSiteResolver(
//            ISiteContextResolver siteContextResolver,
//            IMemoryCache cache,
//            ILoggerFactory loggerFactory,
//            ISiteQueries siteRepository,
//            IOptions<MultiTenantOptions> multiTenantOptions,
//            IOptions<CachingSiteResolverOptions> cachingOptionsAccessor
//            )
//            : base(cache, loggerFactory)
//        {
//            _siteContextResolver = siteContextResolver;
//            _siteQueries = siteRepository;
//            _multiTenantOptions = multiTenantOptions.Value;
//            _cachingOptions = cachingOptionsAccessor.Value;
//        }

//        private ISiteContextResolver _siteContextResolver;
//        private MultiTenantOptions _multiTenantOptions;
//        private ISiteQueries _siteQueries;
//        private CachingSiteResolverOptions _cachingOptions;

//        private async Task<List<string>> GetAllSiteFoldersFolders()
//        {
//            var listCacheKey = "folderList";
//            if (cache.Get(listCacheKey) is List<string> result)
//            {
//                log.LogDebug("Folder List retrieved from cache with key \"{cacheKey}\".", listCacheKey);
//                return result;
//            }

//            result = await _siteQueries.GetAllSiteFolders();
//            var cacheEntryOptions = new MemoryCacheEntryOptions()
//                .SetAbsoluteExpiration(_cachingOptions.FolderListCacheDuration);

//            log.LogDebug("Caching folder list with keys \"{cacheKey}\".", listCacheKey);
//            cache.Set(listCacheKey, result, cacheEntryOptions);

//            return result;

//        }

//        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
//        {
//            return new MemoryCacheEntryOptions()
//                .SetAbsoluteExpiration(_cachingOptions.SiteCacheDuration); 
//        }

//        // Determines what information in the current request should be used to do a cache lookup e.g.the hostname.
//        protected override async Task<string> GetContextIdentifier(HttpContext context)
//        {
//            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                var fullPath = context.Request.PathBase + context.Request.Path;
//                var siteFolderName = fullPath.StartingSegment();
//                // I really want this to be async
//                // checking with Ben if he will make the base class method async
//                var folders = await GetAllSiteFoldersFolders();

//                return folders.Contains(siteFolderName) ? siteFolderName : "root";
//            }

//            return context.Request.Host.Value.ToLower();
//        }

//        /// <summary>
//        /// Determines the identifiers (keys) used to cache the tenant context. 
//        /// In our example tenants can have multiple domains, so we return each of the 
//        /// hostnames as identifiers.
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<SiteContext> context)
//        {
//            var identifiers = new List<string>();

//            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if (!string.IsNullOrWhiteSpace(context.Tenant.SiteFolderName))
//                {
//                    identifiers.Add(context.Tenant.SiteFolderName);
//                }
//                else
//                {
//                    identifiers.Add("root");
//                }
//            }

//            var siteGuid = context.Tenant.Id.ToString();

//            identifiers.Add(siteGuid);

//            return identifiers;
//        }

//        //Resolve a tenant context from the current request. This will only be executed on cache misses.
//        protected override async Task<TenantContext<SiteContext>> ResolveAsync(HttpContext context)
//        {
//            TenantContext<SiteContext> tenantContext = null;
//            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;
//            var site = await _siteContextResolver.ResolveSite(context.Request.Host.Value, context.Request.Path.StartingSegment(), cancellationToken);

//            if (site != null)
//            {
//                tenantContext = new TenantContext<SiteContext>(site);
//            }

//            return tenantContext;
  
//        }
//    }
//}
