// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-20
// Last Modified:			2018-04-23
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
        
        public string NormalizedRoleName { get; set; }

        public string RoleName { get; set; }
        
        /// <summary>
        /// note that MemberCount is only populated in some role list retrieval scenarios
        /// if the value is -1 then it has not been populated
        /// </summary>
        public int MemberCount { get; set; } = -1;
        
        public static SiteRole FromISiteRole(ISiteRole i)
        {
            SiteRole r = new SiteRole
            {
                RoleName = i.RoleName,
                MemberCount = i.MemberCount,
                Id = i.Id,
                NormalizedRoleName = i.NormalizedRoleName,
                SiteId = i.SiteId
            };
            // r.ConcurrencyStamp = i.ConcurrencyStamp;


            return r;
        }
        
    }

}
