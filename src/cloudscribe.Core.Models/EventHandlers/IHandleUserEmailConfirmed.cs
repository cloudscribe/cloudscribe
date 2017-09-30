using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserEmailConfirmed
    {
        Task HandleUserEmailConfirmed(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
