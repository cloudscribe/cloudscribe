using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserUpdated
    {
        Task HandleUserUpdated(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
