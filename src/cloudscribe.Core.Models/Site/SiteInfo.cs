// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2014-11-24
// 

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

        private string siteFolderName = string.Empty;

        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        private string preferredHostName = string.Empty;

        public string PreferredHostName
        {
            get { return preferredHostName; }
            set { preferredHostName = value; }
        }

        private bool isServerAdminSite = false;
        public bool IsServerAdminSite
        {
            get { return isServerAdminSite; }
            set { isServerAdminSite = value; }
        }
    }
}
