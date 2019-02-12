using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.Web.Common.Serialization;

namespace cloudscribe.Web.Common
{
    public static class DistributedCacheExtensions
    {
        
        public async static Task SetAsync<T>(
            this IDistributedCache distributedCache, 
            string key, 
            T value, 
            DistributedCacheEntryOptions options, 
            CancellationToken token = default(CancellationToken))
        {
            await distributedCache.SetAsync(key, value.ToByteArray(), options, token);
        }

        public async static Task<T> GetAsync<T>(
            this IDistributedCache distributedCache, 
            string key, 
            CancellationToken token = default(CancellationToken)) where T : class
        {
            var result = await distributedCache.GetAsync(key, token);
            return result.FromByteArray<T>();
        }
        

    }
}
