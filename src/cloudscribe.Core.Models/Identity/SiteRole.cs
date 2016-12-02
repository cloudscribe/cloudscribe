// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-20
// Last Modified:			2016-12-02
// 
// You must not remove this notice, or any other, from this software.

using System;

namespace cloudscribe.Core.Models
{

    public class SiteRole : ISiteRole
    {

        public SiteRole()
        {
            Id = Guid.NewGuid();
        }

        
        public Guid Id { get; set; } 
        public Guid SiteId { get; set; } 

        private string normalizedRoleName = string.Empty;
        public string NormalizedRoleName
        {
            get { return normalizedRoleName ?? string.Empty; }
            set { normalizedRoleName = value; }
        }

        private string roleName = string.Empty;
        public string RoleName
        {
            get { return roleName ?? string.Empty; }
            set { roleName = value; }
        }

        //public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();


        /// <summary>
        /// note that MemberCount is only populated in some role list retrieval scenarios
        /// if the value is -1 then it has not been populated
        /// </summary>
        public int MemberCount { get; set; } = -1;
        
        public static SiteRole FromISiteRole(ISiteRole i)
        {
            SiteRole r = new SiteRole();
            r.RoleName = i.RoleName;
            r.MemberCount = i.MemberCount;
            r.Id = i.Id;
            r.NormalizedRoleName = i.NormalizedRoleName;
            r.SiteId = i.SiteId;
           // r.ConcurrencyStamp = i.ConcurrencyStamp;


            return r;
        }
        
    }

}
