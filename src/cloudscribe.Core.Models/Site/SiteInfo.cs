using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    [Serializable]
    public class SiteInfo : ISiteInfo
    {
        private int siteID = -1;

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private Guid siteGuid = Guid.Empty;

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }

        }

        private string siteName = string.Empty;

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }
    }
}
