// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-11
// Last Modified:			2016-06-11
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class CacheHelper
    {
        public CacheHelper(
            IMemoryCache cache,
            IDistributedCache distributedCache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
            )
        {
            _cache = cache;
            _distributedCache = distributedCache;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedCache;
        private readonly MultiTenantOptions _multiTenantOptions;

        public void ClearCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public void ClearSiteFolderListCache()
        {
            ClearCache("folderList");
        }

        public async Task SetDistributedCacheTimestamp(Guid siteId, DateTime lastModifiedUtc)
        {
            var cacheKey = "siteModified-" + siteId.ToString();
            var cacheValue = lastModifiedUtc.ToString("s");
            await _distributedCache.SetStringAsync(cacheKey, cacheValue);
        }

        public async Task<DateTime?> GetDistributedCacheTimestamp(Guid siteId)
        {
            var cacheKey = "siteModified-" + siteId.ToString();
            var cacheValue = await _distributedCache.GetStringAsync(cacheKey);
            if(!string.IsNullOrWhiteSpace(cacheValue))
            {
                DateTime result;
                if(DateTime.TryParse(cacheValue, out result))
                {
                   return result;
                }
            }

            return null;
        }

    }
}
