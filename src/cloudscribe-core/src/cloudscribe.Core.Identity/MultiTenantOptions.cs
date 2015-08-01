// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2015-08-01
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class MultiTenantOptions
    {
        /// <summary>
        /// MultiTenantMode.FolderName by default 
        /// </summary>
        public MultiTenantMode Mode { get; set; } = MultiTenantMode.FolderName;

        /// <summary>
        /// if true then all sites will share the same users and roles attached to the relatedSiteID
        /// </summary>
        public bool UseRelatedSitesMode { get; set; } = false;

        /// <summary>
        /// the siteId of the site whose users and roles are shared when UseRelatedSitesMode is true
        /// </summary>
        public int RelatedSiteId { get; set; } = 1;
    }
}
