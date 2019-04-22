using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

            //await _distributedCache.SetAsync<List<AuthenticationToken>>(cacheKey, tokens, options);

        }

        public async Task<List<AuthenticationToken>> GetOidcTokens(ClaimsPrincipal principal)
        {
            if (principal == null) return null;
            var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
            if (userIdClaim == null) return null;

            var cacheKey = "oidc-tokens-" + userIdClaim.Value;

            //var result = await _distributedCache.GetAsync<List<AuthenticationToken>>(cacheKey);
            var json = await _distributedCache.GetStringAsync(cacheKey);

            if(!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<List<AuthenticationToken>>(json);
            }

           // var result = new List<AuthenticationToken>();

            return null;
        }


        //public async Task CaptureJwt(ClaimsPrincipal principal, string jwt)
        //{
        //    if (principal == null || string.IsNullOrEmpty(jwt)) return;

        //    var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
        //    if (userIdClaim == null) return;

        //    var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));

        //    var cacheKey = "jwt-" + userIdClaim.Value;

        //    var bytes = Encoding.UTF8.GetBytes(jwt);

        //    await _distributedCache.SetAsync(cacheKey, bytes, options);

        //}

        //public async Task<string> GetCurrentJwt(ClaimsPrincipal principal)
        //{
        //    if (principal == null) return null;
        //    var userIdClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub").FirstOrDefault();
        //    if (userIdClaim == null) return null;

        //    var cacheKey = "jwt-" + userIdClaim.Value;
        //    var bytes = await _distributedCache.GetAsync(cacheKey);
        //    if(bytes != null)
        //    {
        //        return Encoding.UTF8.GetString(bytes);
        //    }

        //    return null;

        //}


    }
}
