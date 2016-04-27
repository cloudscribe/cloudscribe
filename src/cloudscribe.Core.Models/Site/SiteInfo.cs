// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2016-04-27
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
        public Guid SiteGuid { get; set; } = Guid.Empty;

        private string tenantId = null;
        /// <summary>
        /// an optional identifier for the site, should be unique per site
        /// can be used instead of guid as folder name for tenant themes
        /// </summary>
        public string TenantId
        {
            get { return tenantId ?? SiteGuid.ToString(); }
            set {
                if (value != SiteGuid.ToString())
                {
                    tenantId = value;
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
