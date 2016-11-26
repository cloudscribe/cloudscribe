using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleSiteUpdated
    {
        Task HandleSiteUpdated(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
