using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserCreated
    {
        Task HandleUserCreated(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
