using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class IdentityServerCustomClaimsProvider : ICustomClaimProvider
    {
        public Task AddClaims(SiteUser user, ClaimsIdentity identity)
        {
            
            identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, IdentityServerConstants.LocalIdentityProvider));

            var authTime = DateTime.UtcNow;
            if(user.LastLoginUtc.HasValue) { authTime = user.LastLoginUtc.Value; }


            identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, authTime.ToEpochTime().ToString(), ClaimValueTypes.Integer));

            return Task.FromResult(0);
            
        }
    }
}
