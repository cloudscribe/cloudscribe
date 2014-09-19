
using cloudscribe.Configuration;
using log4net;
using Microsoft.ApplicationServer.Caching;
using System;

//http://msdn.microsoft.com/en-us/library/windowsazure/gg278356.aspx
//http://msdn.microsoft.com/en-us/library/windowsazure/gg618004.aspx


namespace cloudscribe.Caching
{
    public class AppFabricCacheAdapter : ICache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AppFabricCacheAdapter));
        private static bool debugLog = log.IsDebugEnabled;
        private DataCache _cache;
    	

        public AppFabricCacheAdapter()
        {
        	
        	var factory = new AppFabricCacheFactory();
            _cache = factory.ConstructCache(AppSettings.DistributedCacheServers);
        }

        public void Add(string cacheKey, DateTime expiry, object dataToAdd)
        {
            if (expiry > DateTime.Now && dataToAdd != null)
            {
				TimeSpan timeout = expiry - DateTime.Now;
                _cache.Add(cacheKey, dataToAdd, timeout);
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
            _cache.Remove(cacheKey);
        }


		public void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
		{
			if (dataToAdd != null)
			{
                if (debugLog) { log.Debug(string.Format("Adding data to cache with cache key: {0}, sliding window expiry in seconds {1}", cacheKey, slidingExpiryWindow.TotalSeconds)); }
				_cache.Add(cacheKey, dataToAdd, slidingExpiryWindow);
			}
		}

		public void AddToPerRequestCache(string cacheKey, object dataToAdd)
		{
			// AppFabric does not have a per request concept nor does it need to since all cache nodes should be in sync
			// You could simulate this in code with a dependency on the ASP.NET framework and its inbuilt request
			// objects but we wont do that here. We simply add it into the cache for 1 second.
			// Its hacky but this behaviour will be specific to the scenario at hand.
			Add(cacheKey,TimeSpan.FromSeconds(1),dataToAdd);
		}


		public CacheSetting CacheType
		{
			get { return CacheSetting.AppFabric; }
		}
	}
}
