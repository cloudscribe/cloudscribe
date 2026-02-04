using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    /// <summary>
    /// Handler interface for operations that should occur after a user has been successfully deleted.
    /// Implementations should handle cleanup of user-related data in external systems or components.
    /// These handlers only execute if the user deletion was successful.
    /// </summary>
    public interface IHandleUserPostDelete
    {
        Task HandleUserPostDelete(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}