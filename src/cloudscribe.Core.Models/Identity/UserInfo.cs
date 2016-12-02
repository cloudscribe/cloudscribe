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
        
        private string email = string.Empty;
        public string Email
        {
            get { return email ?? string.Empty; }
            set { email = value; }
        }

        private string userName = string.Empty;
        public string UserName
        {
            get { return userName ?? string.Empty; }
            set { userName = value; }
        }

        private string displayName = string.Empty;
        public string DisplayName
        {
            get { return displayName ?? string.Empty; }
            set { displayName = value; }
        }

        private string firstName = string.Empty;
        public string FirstName
        {
            get { return firstName ?? string.Empty; }
            set { firstName = value; }
        }

        private string lastName = string.Empty;
        public string LastName
        {
            get { return lastName ?? string.Empty; }
            set { lastName = value; }
        }
        
        public bool IsDeleted { get; set; } = false;
        public bool Trusted { get; set; } = false;

        private string avatarUrl = string.Empty;
        public string AvatarUrl
        {
            get { return avatarUrl ?? string.Empty; }
            set { avatarUrl = value; }
        }

        
        public DateTime? DateOfBirth { get; set; } 
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
        public bool DisplayInMemberList { get; set; } = true;

        private string gender = string.Empty;
        public string Gender
        {
            get { return gender ?? string.Empty; }
            set { gender = value; }
        }

        public bool IsLockedOut { get; set; } = false;

        //private string country = string.Empty;
        //public string Country
        //{
        //    get { return country ?? string.Empty; }
        //    set { country = value; }
        //}

        //private string state = string.Empty;
        //public string State
        //{
        //    get { return state ?? string.Empty; }
        //    set { state = value; }
        //}
        
        public DateTime? LastLoginUtc { get; set; } 

        private string phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get { return phoneNumber ?? string.Empty; }
            set { phoneNumber = value; }
        }

        public bool PhoneNumberConfirmed { get; set; } = false;
        public bool AccountApproved { get; set; } = true;

        private string timeZoneId = string.Empty;
        public string TimeZoneId
        {
            get { return timeZoneId ?? string.Empty; }
            set { timeZoneId = value; }
        }

        
        private string webSiteUrl = string.Empty;
        public string WebSiteUrl
        {
            get { return webSiteUrl ?? string.Empty; }
            set { webSiteUrl = value; }
        }

       

    }
}
