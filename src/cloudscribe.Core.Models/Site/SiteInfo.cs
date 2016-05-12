// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2016-04-28
// 

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    //[Serializable]
    public class SiteInfo : ISiteInfo
    {
        
        //public int SiteId { get; set; } = -1;
        public Guid Id { get; set; } = Guid.Empty;

        private string aliasId = null;
        /// <summary>
        /// an optional identifier for the site, should be unique per site
        /// can be used instead of guid as folder name for tenant files/themes
        /// therefore no spaces and use only chars that are reasonable for a folder name
        /// main purpose is so we don't have to use an ugly guid string for folder name for
        /// site specific files
        /// if not specificed then the SiteGuid.ToString wil be used
        /// </summary>
        public string AliasId
        {
            get { return aliasId ?? Id.ToString(); }
            set {
                if (value != Id.ToString())
                {
                    aliasId = value;
                }  
            }
        }

        private string siteName = string.Empty;
        public string SiteName
        {
            get { return siteName ?? string.Empty; }
            set { siteName = value; }
        }

        private string siteFolderName = string.Empty;
        public string SiteFolderName
        {
            get { return siteFolderName ?? string.Empty; }
            set { siteFolderName = value; }
        }

        private string preferredHostName = string.Empty;
        public string PreferredHostName
        {
            get { return preferredHostName ?? string.Empty; }
            set { preferredHostName = value; }
        }

        
        public bool IsServerAdminSite { get; set; } = false;
        
    }
}
