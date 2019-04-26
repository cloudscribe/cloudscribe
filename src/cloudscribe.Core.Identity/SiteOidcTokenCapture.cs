using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteOidcTokenCapture : ICaptureOidcTokens
    {
        public SiteOidcTokenCapture(
            IDistributedCache distributedCache
            )
        {
            _distributedCache = distributedCache;
        }

        private readonly IDistributedCache _distributedCache;


        public async Task CaptureOidcTokens(ClaimsPrincipal principal, List<AuthenticationToken> tokens)
        {
            if (principal == null || tokens == null || tokens.Count == 0) return;

            var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return;
            
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));

            var cacheKey = "oidc-tokens-" + userIdClaim.Value;

            var serialized = JsonConvert.SerializeObject(tokens);

            await _distributedCache.SetStringAsync(cacheKey, serialized, options);
            
        }

        public async Task<List<AuthenticationToken>> GetOidcTokens(ClaimsPrincipal principal)
        {
            if (principal == null) return null;
            var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return null;

            var cacheKey = "oidc-tokens-" + userIdClaim.Value;
            var json = await _distributedCache.GetStringAsync(cacheKey);

            if(!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<List<AuthenticationToken>>(json);
            }

            return null;
        }
        
    }
}
