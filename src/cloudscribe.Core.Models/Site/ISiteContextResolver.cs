using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteContextResolver
    {
        Task<SiteContext> ResolveSite(
            string hostName,
            string pathStartingSegment,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<SiteContext> GetById(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
