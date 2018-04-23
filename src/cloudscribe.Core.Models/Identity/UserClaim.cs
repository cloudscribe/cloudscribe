// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2018-04-23
// 

using System;

namespace cloudscribe.Core.Models
{
    public class UserClaim : IUserClaim
    {

        public UserClaim()
        {
            Id = Guid.NewGuid();
        }

        
        public Guid Id { get; set; }
        public Guid SiteId { get; set; } 
        public Guid UserId { get; set; } 
        
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public static UserClaim FromIUserClaim(IUserClaim i)
        {
            UserClaim u = new UserClaim
            {
                ClaimType = i.ClaimType,
                ClaimValue = i.ClaimValue,
                Id = i.Id,
                SiteId = i.SiteId,
                UserId = i.UserId
            };


            return u;
        }

    }

}
