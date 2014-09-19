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
    public enum CacheSetting
    {
        Memory,
        Web,
        AppFabric,
		memcached
    }
}
