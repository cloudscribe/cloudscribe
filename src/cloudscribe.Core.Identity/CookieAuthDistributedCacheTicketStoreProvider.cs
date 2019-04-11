using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;

namespace cloudscribe.Core.Identity
{
    public class CookieAuthDistributedCacheTicketStoreProvider : ICookieAuthTicketStoreProvider
    {
        public CookieAuthDistributedCacheTicketStoreProvider(
            IDistributedCache distributedCache
            )
        {
            _distributedCache = distributedCache;
        }

        private readonly IDistributedCache _distributedCache;

        public ITicketStore GetTicketStore()
        {
            return new DistributedCacheTicketStore(_distributedCache);
        }


    }
}
