using cloudscribe.Core.Models;
using IdentityServer4.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class MultiTenantEndSessionProtocolRouteMatcher : IMatchEndSessionProtocolRoutePaths
    {
        public MultiTenantEndSessionProtocolRouteMatcher(
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor)
        {
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            site = currentSite;
        }

        private MultiTenantOptions multiTenantOptions;
        private SiteContext site;


        public bool IsEndSessionPath(string requestPath)
        {
            if (requestPath == CustomConstants.ProtocolRoutePaths.EndSession.EnsureLeadingSlash())
                return true;

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName && !multiTenantOptions.UseRelatedSitesMode)
            {
                if (!string.IsNullOrEmpty(site.SiteFolderName))
                {
                    if (requestPath == "/" + site.SiteFolderName + CustomConstants.ProtocolRoutePaths.EndSession.EnsureLeadingSlash())
                        return true;
                }
            }

            return false;
        }

        public bool IsEndSessionCallbackPath(string requestPath)
        {
            if (requestPath == CustomConstants.ProtocolRoutePaths.EndSessionCallback.EnsureLeadingSlash())
                return true;

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName && !multiTenantOptions.UseRelatedSitesMode)
            {
                if (!string.IsNullOrEmpty(site.SiteFolderName))
                {
                    if (requestPath == "/" + site.SiteFolderName + CustomConstants.ProtocolRoutePaths.EndSessionCallback.EnsureLeadingSlash())
                        return true;
                }
            }

            return false;
        }

    }
}
