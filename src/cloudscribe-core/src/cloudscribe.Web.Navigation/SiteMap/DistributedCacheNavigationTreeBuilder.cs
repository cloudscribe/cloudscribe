// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-10-12
// Last Modified:			2015-10-17
// 

using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.Web.Navigation
{
    public class DistributedCacheNavigationTreeBuilder : INavigationTreeBuilder
    {
        public DistributedCacheNavigationTreeBuilder(
            INavigationTreeBuilder implementation,
            IDistributedCache cache,
            IOptions<DistributedCacheNavigationTreeBuilderOptions> optionsAccessor,
            INavigationCacheKeyResolver cacheKeyResolver,
            ILogger<DistributedCacheNavigationTreeBuilder> logger)
        {
            if (implementation == null) { throw new ArgumentNullException(nameof(implementation)); }
            if(implementation is DistributedCacheNavigationTreeBuilder) { throw new ArgumentException("implementation cannot be an instance of DistributedCacheNavigationTreeBuilder"); }
            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (optionsAccessor == null) { throw new ArgumentNullException(nameof(optionsAccessor)); }
            if (cacheKeyResolver == null) { throw new ArgumentNullException(nameof(cacheKeyResolver)); }

            this.implementation = implementation;
            this.cache = cache;
            log = logger;
            options = optionsAccessor.Value;
            this.cacheKeyResolver = cacheKeyResolver;

        }

        private INavigationTreeBuilder implementation;
        private ILogger log;
        private IDistributedCache cache;
        private TreeNode<NavigationNode> rootNode = null;
        private DistributedCacheNavigationTreeBuilderOptions options;
        private INavigationCacheKeyResolver cacheKeyResolver;
        

        public async Task<TreeNode<NavigationNode>> GetTree()
        {
            // ultimately we will need to cache sitemap per site
            // we will implement a custom ICacheKeyResolver to resolve multi tenant cache keys

            if (rootNode == null)
            {
                log.LogDebug("rootnode was null so checking distributed cache");
                string cacheKey = cacheKeyResolver.ResolveCacheKey(options.CacheKey);

                NavigationTreeXmlConverter converter = new NavigationTreeXmlConverter();

                await cache.ConnectAsync();
                byte[] bytes = await cache.GetAsync(cacheKey);
                if (bytes != null)
                {
                    log.LogDebug("rootnode was found in distributed cache so deserializing"); 
                    string xml = Encoding.UTF8.GetString(bytes);
                    XDocument doc = XDocument.Parse(xml);

                    rootNode = converter.FromXml(doc);
                }
                else
                {
                    log.LogDebug("rootnode was not in cache so building");

                    rootNode = await implementation.GetTree();
                    string xml2 = converter.ToXmlString(rootNode);

                    await cache.SetAsync(
                                        cacheKey,
                                        Encoding.UTF8.GetBytes(xml2),
                                        new DistributedCacheEntryOptions().SetSlidingExpiration(
                                            TimeSpan.FromSeconds(options.CacheDurationInSeconds))
                                            );
                }


            }

            return rootNode;
        }

        
    }
}
