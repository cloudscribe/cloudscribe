// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// lighter weight version of user for lists and dropdowns
    /// base class for SiteUser
    /// </summary>
    //[Serializable]
    public class UserInfo : IUserInfo
    {
        public UserInfo()
        { }

        
        public int UserId { get; set; } = -1;
        public Guid UserGuid { get; set; } = Guid.Empty;
        public Guid SiteGuid { get; set; } = Guid.Empty;
        public int SiteId { get; set; } = -1;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public bool Trusted { get; set; } = false;
        public string AvatarUrl { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public bool DisplayInMemberList { get; set; } = true;
        public string Gender { get; set; } = string.Empty;
        public bool IsLockedOut { get; set; } = false;  
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime LastActivityDate { get; set; } = DateTime.MinValue;
        public DateTime LastLoginDate { get; set; } = DateTime.MinValue;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; } = false;
        public bool AccountApproved { get; set; } = true;
        public string TimeZoneId { get; set; } = "Eastern Standard Time";
        public string WebSiteUrl { get; set; } = string.Empty;

    }
}
