// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by Joe Audette
// Changed namespaces and modified for easier use in mojoPortal
//
// Change history for this file since original fork:
// 
//


namespace cloudscribe.Caching
{
    internal static class CacheTypes
    {
        public const string MemoryCache = "Memory";
        //public const string WebCache = "web";
        public const string AppFabricCache = "AppFabricCache";
    	//public const string memcached = "memcached";

    }
}
