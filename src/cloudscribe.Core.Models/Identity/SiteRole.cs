// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-20
// Last Modified:			2015-12-08
// 
// You must not remove this notice, or any other, from this software.

using System;

namespace cloudscribe.Core.Models
{

    public class SiteRole : ISiteRole
    {

        public SiteRole()
        { }

        
        public int RoleId { get; set; } = -1;
        public Guid RoleGuid { get; set; } = Guid.Empty;
        public int SiteId { get; set; } = -1;
        public Guid SiteGuid { get; set; } = Guid.Empty;

        private string roleName = string.Empty;
        public string RoleName
        {
            get { return roleName ?? string.Empty; }
            set { roleName = value; }
        }

        private string displayName = string.Empty;
        public string DisplayName
        {
            get { return displayName ?? string.Empty; }
            set { displayName = value; }
        }

        
        /// <summary>
        /// note that MemberCount is only populated in some role list retrieval scenarios
        /// if the value is -1 then it has not been populated
        /// </summary>
        public int MemberCount { get; set; } = -1;
        
        public static SiteRole FromISiteRole(ISiteRole i)
        {
            SiteRole r = new SiteRole();
            r.DisplayName = i.DisplayName;
            r.MemberCount = i.MemberCount;
            r.RoleGuid = i.RoleGuid;
            r.RoleId = i.RoleId;
            r.RoleName = i.RoleName;
            r.SiteGuid = i.SiteGuid;
            r.SiteId = i.SiteId;


            return r;
        }
        
    }

}
