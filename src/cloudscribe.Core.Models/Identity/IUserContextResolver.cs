using System.Threading.Tasks;
using System.Threading;

namespace cloudscribe.Core.Models
{
    public interface IUserContextResolver
    {
        Task<IUserContext> GetCurrentUser(CancellationToken cancellationToken = default(CancellationToken));
        Task<IUserContext> GetUserById(string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IUserContext> GetUserByEmail(string emailAddress, CancellationToken cancellationToken = default(CancellationToken));
    }
}
