// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by Joe Audette
// Changed namespaces and modified for easier use in mojoPortal
//
// Change history for this file since original fork:
// 
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudscribe.Caching
{
    public interface ICache
    {
        T Get<T>(string cacheKey) where T : class;
        object GetObject(string cacheKey);
        void Add(string cacheKey, DateTime absoluteExpiry, object dataToAdd);
		void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);
        void InvalidateCacheItem(string cacheKey);
    	void AddToPerRequestCache(string cacheKey, object dataToAdd);
    	CacheSetting CacheType { get; }
    }
}
