// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-11
// Last Modified:			2019-10-05
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

        private const string listCacheKey = "folderList";

        public void ClearLocalCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public object GetItemFromLocalCache(string key)
        {
            return _cache.Get(key);
        }

        public void AddToCache(string key, SiteContext site, TimeSpan expiration)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                //this was a breaking change in 3.0, was getting an error here if not setting Size, never had to set it before
                //but according to docs this should not be needed unless setting SizeLimit on the MemoryCache
                //and I can't find any place where we are doing that.
                //It seems that EFCore may be doing that as the problem does not happen when using NoDb
                //The memory size limit does not have a defined unit of measure because the cache has no mechanism to measure the size of entries.
                //The app could specify the size of all entries as 1, and the size limit is the count of entries.
                //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.0

                Size = _multiTenantOptions.SiteSettingsCacheSize
            }
            ;
            cacheOptions.SetAbsoluteExpiration(expiration);

            _cache.Set(
                        key,
                        site,
                        cacheOptions
                         );

        }

        public async Task ClearSiteFolderListCache()
        {
            //ClearLocalCache("folderList");
            await _distributedCache.RemoveAsync(listCacheKey);
        }

        public async Task<List<string>> GetSiteFoldersFromCache()
        {
            var listCsv = await _distributedCache.GetStringAsync(listCacheKey);

            if(!string.IsNullOrWhiteSpace(listCsv))
            {
                return listCsv.Split(',').ToList();
            }
            
            return null;
        }

        public async Task AddSiteFoldersToCache(List<string> siteFolders, TimeSpan expiration)
        {
            var csv = string.Join(",", siteFolders);
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(expiration);
            await _distributedCache.SetStringAsync(listCacheKey, csv, options);
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
