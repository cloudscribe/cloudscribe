// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2015-11-24
// 

//using Microsoft.AspNet.Identity;
using System;

namespace cloudscribe.Core.Models
{

    //[Serializable]
    public class SiteUser : UserInfo, ISiteUser
    {
        public SiteUser()
        { }

        public string Id
        {
            get { return UserGuid.ToString(); }
        }
        
        public string AuthorBio { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string LoweredEmail { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public Guid EmailChangeGuid { get; set; } = Guid.Empty;
        public DateTime? LockoutEndDateUtc { get; set; } = null;
        public string NewEmail { get; set; } = string.Empty;
        public DateTime LastPasswordChangedDate { get; set; } = DateTime.MinValue;
        public DateTime LastLockoutDate { get; set; } = DateTime.MinValue;
        
        public bool MustChangePwd { get; set; } = false;

        public string PasswordHash { get; set; } = string.Empty;
        
        public int FailedPasswordAttemptCount { get; set; } = 0;
        public DateTime FailedPasswordAttemptWindowStart { get; set; } = DateTime.MinValue;
        public int FailedPasswordAnswerAttemptCount { get; set; } = 0;
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; } = DateTime.MinValue;
        public Guid PasswordResetGuid { get; set; } = Guid.Empty;
        public Guid RegisterConfirmGuid { get; set; } = Guid.Empty;
        public bool RolesChanged { get; set; } = false;
        public string SecurityStamp { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty; 
        
        public bool TwoFactorEnabled { get; set; } = false;
        
        
        public static SiteUser FromISiteUser(ISiteUser user)
        {
            SiteUser u = new SiteUser();
            u.AccountApproved = user.AccountApproved;
            u.AuthorBio = user.AuthorBio;
            u.AvatarUrl = user.AvatarUrl;
            u.Comment = user.Comment;
            u.Country = user.Country;
            u.CreatedUtc = user.CreatedUtc;
            u.DateOfBirth = user.DateOfBirth;
            u.DisplayInMemberList = user.DisplayInMemberList;
            u.DisplayName = user.DisplayName;
            u.Email = user.Email;
            u.EmailChangeGuid = user.EmailChangeGuid;
            u.EmailConfirmed = user.EmailConfirmed;
            u.FailedPasswordAnswerAttemptCount = user.FailedPasswordAnswerAttemptCount;
            u.FailedPasswordAnswerAttemptWindowStart = user.FailedPasswordAnswerAttemptWindowStart;
            u.FailedPasswordAttemptCount = user.FailedPasswordAttemptCount;
            u.FailedPasswordAttemptWindowStart = user.FailedPasswordAttemptWindowStart;
            u.FirstName = user.FirstName;
            u.Gender = user.Gender;
            //u.Id = user.Id;
            u.IsDeleted = user.IsDeleted;
            u.IsLockedOut = user.IsLockedOut;
            u.LastActivityDate = user.LastActivityDate;
            u.LastLockoutDate = user.LastLockoutDate;
            u.LastLoginDate = user.LastLoginDate;
            u.LastName = user.LastName;
            u.LastPasswordChangedDate = user.LastPasswordChangedDate;
            u.LockoutEndDateUtc = user.LockoutEndDateUtc;
            u.LoweredEmail = user.LoweredEmail;
            u.MustChangePwd = user.MustChangePwd;
            u.NewEmail = user.NewEmail;
            u.PasswordHash = user.PasswordHash;
            u.PasswordResetGuid = user.PasswordResetGuid;
            u.PhoneNumber = user.PhoneNumber;
            u.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            u.RegisterConfirmGuid = user.RegisterConfirmGuid;
            u.RolesChanged = user.RolesChanged;
            u.SecurityStamp = user.SecurityStamp;
            u.Signature = user.Signature;
            u.SiteGuid = user.SiteGuid;
            u.SiteId = user.SiteId;
            u.State = user.State;
            u.TimeZoneId = user.TimeZoneId;
            u.Trusted = user.Trusted;
            u.TwoFactorEnabled = user.TwoFactorEnabled;
            u.UserGuid = user.UserGuid;
            u.UserId = user.UserId;
            u.UserName = user.UserName;
            u.WebSiteUrl = user.WebSiteUrl;
           

            return u;
        }

    }
}
