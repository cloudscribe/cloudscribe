using cloudscribe.Core.DataProtection;
using cloudscribe.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteContextResolver : ISiteContextResolver
    {
        public SiteContextResolver(
            ISiteQueries siteRepository,
            SiteDataProtector dataProtector,
            IOptions<MultiTenantOptions> multiTenantOptions
            )
        {
            SiteQueries = siteRepository;
            MultiTenantOptions = multiTenantOptions.Value;
            DataProtector = dataProtector;
        }

        protected MultiTenantOptions MultiTenantOptions { get; private set; }
        protected ISiteQueries SiteQueries { get; private set; }
        protected SiteDataProtector DataProtector { get; private set; }

        public virtual async Task<SiteContext> ResolveSite(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var pathStartingSegment = path.StartingSegment();
            ISiteSettings site = null;
            if (MultiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if(string.IsNullOrWhiteSpace(pathStartingSegment))
                {
                    pathStartingSegment = "root";
                }
                site = await SiteQueries.FetchByFolderName(pathStartingSegment, cancellationToken);
            }
            else
            {
                site = await SiteQueries.Fetch(hostName, cancellationToken);
            }

            if (site != null)
            {
                DataProtector.UnProtect(site);
                return new SiteContext(site);  
            }

            return null;

        }

        public virtual async Task<SiteContext> GetById(Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var site = await SiteQueries.Fetch(siteId, cancellationToken);
            if (site != null)
            {
                DataProtector.UnProtect(site);
                return new SiteContext(site);
            }

            return null;
        }

    }
}
