using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserPreUpdate
    {
        Task HandleUserPreUpdate(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
