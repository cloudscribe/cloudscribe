// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2018-04-23
// 

using System;

namespace cloudscribe.Core.Models
{
    public class SiteInfo : ISiteInfo
    {
        public Guid Id { get; set; } 

     
        /// <summary>
        /// an optional identifier for the site, should be unique per site
        /// can be used instead of guid as folder name for tenant files/themes
        /// therefore no spaces and use only chars that are reasonable for a folder name
        /// main purpose is so we don't have to use an ugly guid string for folder name for
        /// site specific files
        /// if not specificed then the SiteGuid.ToString wil be used
        /// </summary>
        public string AliasId { get; set; }
       
        public string SiteName { get; set; }

        public string SiteFolderName { get; set; }

        public string PreferredHostName { get; set; }
        
        public bool IsServerAdminSite { get; set; } = false;
        
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;



    }
}
