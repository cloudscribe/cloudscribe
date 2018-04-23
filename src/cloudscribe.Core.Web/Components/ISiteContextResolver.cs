using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public interface ISiteContextResolver
    {
        Task<SiteContext> ResolveSite(
            string hostName,
            PathString path,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<SiteContext> GetById(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
