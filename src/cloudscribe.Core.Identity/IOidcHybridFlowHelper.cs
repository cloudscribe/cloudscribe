using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface IOidcHybridFlowHelper
    {
        Task<string> GetAccessToken(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken));
    }
}
