using cloudscribe.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class DoNothingCustomClaimProvider : ICustomClaimProvider
    {
        public Task AddClaims(SiteUser user, ClaimsIdentity identity)
        {
            // do nothing
            return Task.FromResult(0);
        }
    }
}
