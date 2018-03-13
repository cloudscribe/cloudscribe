// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2017-05-12
// 

using System;

namespace cloudscribe.Core.Models
{
    public class MultiTenantOptions
    {
        /// <summary>
        /// MultiTenantMode.FolderName by default 
        /// </summary>
        public MultiTenantMode Mode { get; set; } = MultiTenantMode.FolderName;


        private bool useRelatedSitesMode = false;
        /// <summary>
        /// if true then all sites will share the same users and roles attached to the relatedSiteGuid
        /// this method will return false if RelatedSiteGuid is equal to Guid.Empty
        /// you must specify the related siteguid to use related sites mode
        /// </summary>
        public bool UseRelatedSitesMode
        {
            get {
                if(Mode == MultiTenantMode.None) { return false; }
                if(relatedSiteId == Guid.Empty) { return false; }
                return useRelatedSitesMode;
            }
            set { useRelatedSitesMode = value; }
        }

        /// <summary>
        /// the siteId of the site whose users and roles are shared when UseRelatedSitesMode is true
        /// </summary>
        //public int RelatedSiteId { get; set; } = 1;
        private Guid relatedSiteId = Guid.Empty;
        public Guid RelatedSiteId
        {
            get {  return relatedSiteId;
            }
            set { relatedSiteId = value; }
        }

        /// <summary>
        /// AliasId is a string used as an alternative (to the guid siteid) to identify a site for use in the file system
        /// it is used for locating site theme files under /sitefiles/[aliasid/themes
        /// the default is false, in which case we enforce that a site admin cannot change his site aliasid to match
        /// another site's aliasid. Setting this to true would allow more than one site to use the same aliasid and thus
        /// themes would be shared.
        /// Note that if this is true and aliasid is configured them same in more than one site, changing it back to false
        /// will not automatically be changed, so use this carefully.
        /// This setting is useful if the sites belong to the same client/customer and you want to use common themes amoung them.
        /// </summary>
        public bool AllowSharedAliasId { get; set; } = false;

        //public string DefaultNewUserRoles { get; set; } = "Authenticated Users";

        public string SiteFilesFolderName { get; set; } = "sitefiles";

        public string SiteThemesFolderName { get; set; } = "themes";

        public string SiteContentFolderName { get; set; } = "wwwroot";

        public bool UserPerSiteWwwRoot { get; set; } = true;

        /// <summary>
        /// whether to try to auto create the tenant wwwroot folder if it does not exist
        /// </summary>
        public bool TryEnsurePerSiteWwwRoot { get; set; } = true;

        public string ThemeStaticFilesFolderName { get; set; } = "wwwroot";

        public bool UserPerSiteThemes { get; set; } = true;

        public bool UseSharedThemes { get; set; } = true;

        public string SharedThemesFolderName { get; set; } = "SharedThemes";


    }
}
