using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public interface IJwtClaimsProcessor<TUser>
    {
        IEnumerable<Claim> ProcessClaims(ProfileDataRequestContext context, IEnumerable<Claim> claims, TUser user);
    }
}
