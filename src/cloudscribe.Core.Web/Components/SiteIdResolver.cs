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
            _currentSite = currentSite;
        }

        private SiteContext _currentSite;

        public Guid GetCurrentSiteId()
        {
            return _currentSite.Id;
        }

    }
}
