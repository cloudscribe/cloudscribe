using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleSiteCloned
    {
        Task HandleSiteCloned(
            ISiteSettings newSite,
            ISiteSettings sourceSite,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
