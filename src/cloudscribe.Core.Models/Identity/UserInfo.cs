// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// lighter weight version of user for lists and dropdowns
    /// base class for SiteUser
    /// </summary>
    public class UserInfo : IUserInfo
    {
        public UserInfo()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; } 
        public Guid SiteId { get; set; } 
        
        public string Email { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool Trusted { get; set; } = false;
        
        public string AvatarUrl { get; set; }
        
        public DateTime? DateOfBirth { get; set; } 
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
        public bool DisplayInMemberList { get; set; } = true;
        
        public string Gender { get; set; }
        
        public bool IsLockedOut { get; set; } = false;
        
        public DateTime? LastLoginUtc { get; set; } 
        public string PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; } = false;
        public bool AccountApproved { get; set; } = true;
        public string TimeZoneId { get; set; }
        
        public string WebSiteUrl { get; set; }


    }
}
