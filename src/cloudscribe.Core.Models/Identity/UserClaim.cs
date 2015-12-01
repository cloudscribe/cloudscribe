// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-12-01
// 

namespace cloudscribe.Core.Models
{
    public class UserClaim : IUserClaim
    {

        public UserClaim()
        { }

        public int SiteId { get; set; } = -1;
        public int Id { get; set; } = -1;
        public string UserId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;

        public static UserClaim FromIUserClaim(IUserClaim i)
        {
            UserClaim u = new UserClaim();

            u.ClaimType = i.ClaimType;
            u.ClaimValue = i.ClaimValue;
            u.Id = i.Id;
            u.SiteId = i.SiteId;
            u.UserId = i.UserId;
           

            return u;
        }

    }

}
