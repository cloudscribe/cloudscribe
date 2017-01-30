using System.Threading.Tasks;
using System.Threading;

namespace cloudscribe.Core.Models
{
    public interface IUserContextResolver
    {
        Task<IUserContext> GetCurrentUser(CancellationToken cancellationToken = default(CancellationToken));
    }
}
