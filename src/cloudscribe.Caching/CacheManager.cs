// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by Joe Audette
// Changed namespaces and modified for use in mojoPortal
//


using System;
using cloudscribe.Configuration;

namespace cloudscribe.Caching
{
    public static class CacheManager
    {
    	private static ICacheProvider _cacheProvider;
    	private static ICache _cache;

        private const string AppFabricProvider = "cloudscribe.Caching.AppFabricCacheAdapter, mojoPortal.Web";
    	
    	static CacheManager()
    	{
    		PreStartInitialise();
    	}
        
        public static void PreStartInitialise()
        {
            switch (AppSettings.CacheProviderType)
            {


                case CacheTypes.AppFabricCache:

                    _cache = new AppFabricCacheAdapter();

                    break;


                case CacheTypes.MemoryCache:
                default:
                    _cache = new MemoryCacheAdapter();

                    // http://msdn.microsoft.com/en-us/library/wcxyzt4d.aspx
                    //_cache = Activator.CreateInstance(Type.GetType(WebConfigSettings.CacheProviderType, _logger)) as ICache;


                    break;


            }


			_cacheProvider = new CacheProvider(_cache);

		}

		public static ICacheProvider Cache { get { return _cacheProvider; } }
    }
}
