using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using IdentityModel;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class DefaultJwtClaimsProcessor : IJwtClaimsProcessor<SiteUser>
    {
        public IEnumerable<Claim> ProcessClaims(ProfileDataRequestContext context, IEnumerable<Claim> claims, SiteUser user)
        {
            // filter all claims from principal down to ones requested
            var result = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
            // map some claim types to jwt claim types
            if (context.RequestedClaimTypes.Contains(JwtClaimTypes.Subject))
            {
                var sub = result.Where(x => x.Type == JwtClaimTypes.Subject).FirstOrDefault();
                if (sub == null)
                {
                    // sub needs to be added using userid
                    var id = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                    if (id != null)
                    {
                        result.Add(new Claim(JwtClaimTypes.Subject, id.Value));
                    }
                }
            }

            if (context.RequestedClaimTypes.Contains(JwtClaimTypes.Name))
            {
                var nm = result.Where(x => x.Type == JwtClaimTypes.Name).FirstOrDefault();
                if (nm == null)
                {
                    // name needs to be added
                    var n = claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
                    if (n != null)
                    {
                        result.Add(new Claim(JwtClaimTypes.Name, n.Value));
                    }
                }
            }
            
            if (context.RequestedClaimTypes.Contains(JwtClaimTypes.Role))
            {
                var loadedRoles = result.Where(x => x.Type == JwtClaimTypes.Role).ToList();
                if (loadedRoles.Count == 0)
                {
                    // roles requested but none loaded so far
                    // convert ClaimTypes.Role to JwtClaimTypes.Role
                    var roleClaims = claims.Where(x => x.Type == ClaimTypes.Role);  // "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    foreach (var roleClaim in roleClaims)
                    {
                        result.Add(new Claim(JwtClaimTypes.Role, roleClaim.Value));
                        // result.Add(new Claim(roleClaim.Type, roleClaim.Value));
                    }
                }


            }

            return result;

        }
    }
}
