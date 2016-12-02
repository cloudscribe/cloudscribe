// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2016-12-02
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
        
        private string claimType = string.Empty;
        public string ClaimType
        {
            get { return claimType ?? string.Empty; }
            set { claimType = value; }
        }

        private string claimValue = string.Empty;
        public string ClaimValue
        {
            get { return claimValue ?? string.Empty; }
            set { claimValue = value; }
        }
        
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
