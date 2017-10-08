using cloudscribe.Core.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using cloudscribe.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityModel;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class IdentityServerCustomClaimsProvider : ICustomClaimProvider
    {
        public Task AddClaims(SiteUser user, ClaimsIdentity identity)
        {
            // TODO should check that these claims don't exist already?

            identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, IdentityServerConstants.LocalIdentityProvider));

            var authTime = DateTime.UtcNow;
            if(user.LastLoginUtc.HasValue) { authTime = user.LastLoginUtc.Value; }


            identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, authTime.ToEpochTime().ToString(), ClaimValueTypes.Integer));

            return Task.FromResult(0);
            
        }
    }
}
