// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-10-12
// Last Modified:			2015-10-17
// 

using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Web.Navigation
{
    public class MemoryCacheNavigationTreeBuilder : INavigationTreeBuilder
    {
        public MemoryCacheNavigationTreeBuilder(
            INavigationTreeBuilder implementation,
            IMemoryCache cache,
            IOptions<MemoryCacheNavigationTreeBuilderOptions> optionsAccessor,
            INavigationCacheKeyResolver cacheKeyResolver,
            ILogger<MemoryCacheNavigationTreeBuilder> logger)
        {
            if (implementation == null) { throw new ArgumentNullException(nameof(implementation)); }
            if (implementation is MemoryCacheNavigationTreeBuilder) { throw new ArgumentException("implementation cannot be an instance of MemoryCacheNavigationTreeBuilder"); }
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
        private IMemoryCache cache;
        private TreeNode<NavigationNode> rootNode = null;
        private MemoryCacheNavigationTreeBuilderOptions options;
        private INavigationCacheKeyResolver cacheKeyResolver;

        public async Task<TreeNode<NavigationNode>> GetTree()
        {
            // ultimately we will need to cache sitemap per site
            // we will implement a custom ICacheKeyResolver to resolve multi tenant cache keys

            if (rootNode == null)
            {
                log.LogDebug("rootnode was null so checking cache");
                string cacheKey = cacheKeyResolver.ResolveCacheKey(options.CacheKey);

                rootNode = (TreeNode<NavigationNode>)cache.Get(cacheKey);
               
                if(rootNode == null)
                {
                    log.LogDebug("rootnode was not in cache so building");
                    rootNode = await implementation.GetTree();
                    if (rootNode != null)
                    {
                        cache.Set(
                            cacheKey,
                            rootNode,
                            new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(options.CacheDurationInSeconds)));
                    }
                }
                else
                {
                    log.LogDebug("rootnode was found in cache");
                }
                
            }

            return rootNode;
        }

    }
}
