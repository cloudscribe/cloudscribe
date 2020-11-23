using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common
{
    public static class DistributedCacheExtensions
    {
        public async static Task SetAsync<T>(this IDistributedCache distributedCache,
                                             string key,
                                             T value,
                                             DistributedCacheEntryOptions options,
                                             CancellationToken token = default(CancellationToken))
        {
            var json = JsonConvert.SerializeObject(value);

            await distributedCache.SetStringAsync(key, json, options, token);
        }


        public async static Task<T> GetAsync<T>(this IDistributedCache distributedCache,
                                                string key,
                                                CancellationToken token = default(CancellationToken))
                                                where T : class
        {
            var json = await distributedCache.GetStringAsync(key);

            return json != null ? JsonConvert.DeserializeObject<T>(json) : null;
        }
    }
}
