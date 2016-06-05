// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2016-06-05
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
            

        public string DefaultNewUserRoles { get; set; } = "Authenticated Users";
    }
}
