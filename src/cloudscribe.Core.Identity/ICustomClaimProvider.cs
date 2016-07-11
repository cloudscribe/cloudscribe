using cloudscribe.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface ICustomClaimProvider
    {
        Task AddClaims(SiteUser user, ClaimsIdentity identity);
    }
}
