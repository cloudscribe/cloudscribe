// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2015-09-04
// 


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
        /// if true then all sites will share the same users and roles attached to the relatedSiteID
        /// </summary>
        public bool UseRelatedSitesMode
        {
            get {
                if(Mode == MultiTenantMode.None) { return false; }
                return useRelatedSitesMode;
            }
            set { useRelatedSitesMode = value; }
        }

        /// <summary>
        /// the siteId of the site whose users and roles are shared when UseRelatedSitesMode is true
        /// </summary>
        public int RelatedSiteId { get; set; } = 1;

        public string DefaultNewUserRoles { get; set; } = "Authenticated Users";
    }
}
