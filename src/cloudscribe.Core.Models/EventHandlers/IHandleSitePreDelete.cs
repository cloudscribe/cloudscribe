using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleSitePreDelete
    {
        Task HandleSitePreDelete(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
