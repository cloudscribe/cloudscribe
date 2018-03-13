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
            _siteQueries = siteRepository;
            _multiTenantOptions = multiTenantOptions.Value;
            _dataProtector = dataProtector;
        }

        private MultiTenantOptions _multiTenantOptions;
        private ISiteQueries _siteQueries;
        private SiteDataProtector _dataProtector;

        public async Task<SiteContext> ResolveSite(
            string hostName, 
            string pathStartingSegment,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ISiteSettings site = null;
            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if(string.IsNullOrWhiteSpace(pathStartingSegment))
                {
                    pathStartingSegment = "root";
                }
                site = await _siteQueries.FetchByFolderName(pathStartingSegment, cancellationToken);
            }
            else
            {
                site = await _siteQueries.Fetch(hostName, cancellationToken);
            }

            if (site != null)
            {
                _dataProtector.UnProtect(site);
                return new SiteContext(site);  
            }

            return null;

        }

        public async Task<SiteContext> GetById(Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var site = await _siteQueries.Fetch(siteId, cancellationToken);
            if (site != null)
            {
                _dataProtector.UnProtect(site);
                return new SiteContext(site);
            }

            return null;
        }

    }
}
