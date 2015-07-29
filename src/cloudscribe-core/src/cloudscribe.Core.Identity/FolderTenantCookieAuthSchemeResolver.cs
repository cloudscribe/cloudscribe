// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-29
// Last Modified:		    2015-07-29
// 
//

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;


namespace cloudscribe.Core.Identity
{
    public class FolderTenantCookieAuthSchemeResolver : ICookieAuthenticationSchemeSet
    {
        public FolderTenantCookieAuthSchemeResolver(ISiteResolver siteResolver)
        {
            this.siteResolver = siteResolver;
            //site = siteResolver.Resolve();
        }

        private ISiteResolver siteResolver;
        private ISiteSettings site = null;
        private ISiteSettings Site
        {
            get
            {
                if(site == null)
                {
                    site = siteResolver.Resolve();
                }

                return site;
            }
        }

        public string ApplicationScheme {
            get {

                if(Site.SiteFolderName.Length > 0)
                {
                    return "Application-" + Site.SiteFolderName;
                }

                return "Application-cloudscribeApp";
            }
        } 

        public string ExternalScheme
        { 
            get
            {
                if (Site.SiteFolderName.Length > 0)
                {
                    return "External-" + Site.SiteFolderName;
                }

                return "External-cloudscribeApp";
            }
        }

        public string TwoFactorUserIdScheme
        {
            get
            {
                if (Site.SiteFolderName.Length > 0)
                {
                    return "TwoFactorUserId-" + Site.SiteFolderName;
                }

                return "TwoFactorUserId-cloudscribeApp";
            }
        }

        public string TwoFactorRememberMeScheme
        {
            get
            {
                if (Site.SiteFolderName.Length > 0)
                {
                    return "TwoFactorRememberMe-" + Site.SiteFolderName;
                }

                return "TwoFactorRememberMe-cloudscribeApp";
            }
        }



       

    }
}
