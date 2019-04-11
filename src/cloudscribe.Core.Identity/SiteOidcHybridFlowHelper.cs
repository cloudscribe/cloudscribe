using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteOidcHybridFlowHelper : IOidcHybridFlowHelper
    {
        public SiteOidcHybridFlowHelper(
            IDistributedCache distributedCache
            )
        {
            _distributedCache = distributedCache;
        }

        private readonly IDistributedCache _distributedCache;

        public async Task CaptureJwt(ClaimsPrincipal principal, string jwt)
        {
            if (principal == null || string.IsNullOrEmpty(jwt)) return;

            var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return;

            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));

            var cacheKey = "jwt-" + userIdClaim.Value;

            var bytes = Encoding.UTF8.GetBytes(jwt);

            await _distributedCache.SetAsync(cacheKey, bytes, options);

        }

        public async Task<string> GetCurrentJwt(ClaimsPrincipal principal)
        {
            if (principal == null) return null;
            var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return null;

            var cacheKey = "jwt-" + userIdClaim.Value;
            var bytes = await _distributedCache.GetAsync(cacheKey);
            if(bytes != null)
            {
                return Encoding.UTF8.GetString(bytes);
            }

            return null;

        }


    }
}
