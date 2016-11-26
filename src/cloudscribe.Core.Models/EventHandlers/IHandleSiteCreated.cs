using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleSiteCreated
    {
        Task HandleSiteCreated(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
