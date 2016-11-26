using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserPreDelete
    {
        Task HandleUserPreDelete(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
