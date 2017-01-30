using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteIdResolver : ISiteIdResolver
    {
        public SiteIdResolver(
            SiteContext currentSite
            )
        {
            this.currentSite = currentSite;
        }

        private SiteContext currentSite;

        public Guid GetCurrentSiteId()
        {
            return currentSite.Id;
        }

    }
}
