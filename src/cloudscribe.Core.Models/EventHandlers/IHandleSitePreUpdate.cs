using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleSitePreUpdate
    {
        Task HandleSitePreUpdate(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
