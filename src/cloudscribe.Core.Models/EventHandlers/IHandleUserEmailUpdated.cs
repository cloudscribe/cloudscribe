using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserEmailUpdated
    {
        Task HandleUserEmailUpdated(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
