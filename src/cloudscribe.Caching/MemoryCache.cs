// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by Joe Audette
// Changed namespaces and modified for easier use in mojoPortal
//
// Change history for this file since original fork:
// 2011-08-03 Joe Audette modified to support .NET 3.5
// 2012-03-22 added support for use of Cache Dependency files (.net 3.5) and HostFileChangeMonitor (.net 4)
// 2014-08-21 adapted for cloudscribe, changed namespace

using cloudscribe.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace cloudscribe.Caching
{

    //http://stackoverflow.com/questions/13704000/system-runtime-caching-memorycache-vs-httpruntime-cache-are-there-any-differen

    
    public class MemoryCacheAdapter : ICache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MemoryCacheAdapter));
        private static bool debugLog = log.IsDebugEnabled;
        private MemoryCache _cache = MemoryCache.Default;
        

        public MemoryCacheAdapter()
        {
            
        }

        private HostFileChangeMonitor GetHostFileChangeMonitor(string cacheKey)
        {
            string pathToDependency = FileDependencyHelper.GetPathToCacheDependencyFile(cacheKey);
            FileDependencyHelper.EnsureCacheFile(pathToDependency);
            List<string> filePaths = new List<string>();
            filePaths.Add(pathToDependency);
            return new HostFileChangeMonitor(filePaths);
        }

        public void Add(string cacheKey, DateTime expiry, object dataToAdd)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(expiry);

            if (AppSettings.UseCacheDependencyFiles)
            {
                policy.ChangeMonitors.Add(GetHostFileChangeMonitor(cacheKey));
            }

            if (dataToAdd != null)
            {
                _cache.Add(cacheKey, dataToAdd, policy);
                if (debugLog) { log.Debug(string.Format("Adding data to cache with cache key: {0}, expiry date {1}", cacheKey, expiry.ToString("yyyy/MM/dd hh:mm:ss"))); }

            }
        }

        public object GetObject(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public T Get<T>(string cacheKey) where T : class
        {
            T data = _cache.Get(cacheKey) as T;
            return data;
        }

        public void InvalidateCacheItem(string cacheKey)
        {
            if (_cache.Contains(cacheKey))
                _cache.Remove(cacheKey);
        }

        public void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
        {
            if (dataToAdd != null)
            {
                var item = new CacheItem(cacheKey, dataToAdd);
                var policy = new CacheItemPolicy() { SlidingExpiration = slidingExpiryWindow };

                if (AppSettings.UseCacheDependencyFiles)
                {
                    policy.ChangeMonitors.Add(GetHostFileChangeMonitor(cacheKey));
                }

                _cache.Add(item, policy);
                if (debugLog) { log.Debug(string.Format("Adding data to cache with cache key: {0}, sliding expiry window in seconds {1}", cacheKey, slidingExpiryWindow.TotalSeconds)); }

            }
        }

        public void AddToPerRequestCache(string cacheKey, object dataToAdd)
        {
            // memory cache does not have a per request concept nor does it need to since all cache nodes should be in sync
            // You could simulate this in code with a dependency on the ASP.NET framework and its inbuilt request
            // objects but we wont do that here. We simply add it into the cache for 1 second.
            // Its hacky but this behaviour will be specific to the scenario at hand.
            Add(cacheKey, TimeSpan.FromSeconds(1), dataToAdd);
        }


        public CacheSetting CacheType
        {
            get { return CacheSetting.Memory; }
        }
    }
}






