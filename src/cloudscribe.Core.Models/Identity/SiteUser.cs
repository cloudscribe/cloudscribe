// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2016-06-17
// 

using System;

namespace cloudscribe.Core.Models
{
    public class SiteUser : UserInfo, ISiteUser
    {
        
        private string authorBio = string.Empty;
        public string AuthorBio
        {
            get { return authorBio ?? string.Empty; }
            set { authorBio = value; }
        }

        private string comment = string.Empty;
        public string Comment
        {
            get { return comment ?? string.Empty; }
            set { comment = value; }
        }

        private string loweredEmail = string.Empty;
        public string NormalizedEmail
        {
            get { return loweredEmail ?? string.Empty; }
            set { loweredEmail = value; }
        }

        private string normalizedUserName = string.Empty;
        public string NormalizedUserName
        {
            get { return normalizedUserName ?? string.Empty; }
            set { normalizedUserName = value; }
        }

        
        public bool EmailConfirmed { get; set; } = false;

        public DateTime? EmailConfirmSentUtc { get; set; } = null;

        public DateTime? AgreementAcceptedUtc { get; set; } = null;

        public DateTime? LockoutEndDateUtc { get; set; } = null;

        private string newEmail = string.Empty;
        public string NewEmail
        {
            get { return newEmail ?? string.Empty; }
            set { newEmail = value; }
        }

        public bool NewEmailApproved { get; set; } = false;

        public DateTime? LastPasswordChangeUtc { get; set; } //= DateTime.MinValue;
        
        
        public bool MustChangePwd { get; set; } = false;

        private string passwordHash = string.Empty;
        public string PasswordHash
        {
            get { return passwordHash ?? string.Empty; }
            set { passwordHash = value; }
        }

        public bool CanAutoLockout { get; set; } = true;

        public int AccessFailedCount { get; set; } = 0;
        
        public bool RolesChanged { get; set; } = false;

        private string securityStamp = string.Empty;
        public string SecurityStamp
        {
            get { return securityStamp ?? string.Empty; }
            set { securityStamp = value; }
        }

        private string signature = string.Empty;
        public string Signature
        {
            get { return signature ?? string.Empty; }
            set { signature = value; }
        }
        
        public bool TwoFactorEnabled { get; set; } = false;

        //public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public static SiteUser FromISiteUser(ISiteUser user)
        {
            SiteUser u = new SiteUser();
           // u.ConcurrencyStamp = user.ConcurrencyStamp;
            //Guid id = user.Id;
            if (user.Id != Guid.Empty) u.Id = user.Id;
            //SiteUser u = new SiteUser(id);
            
            u.AccessFailedCount = user.AccessFailedCount;
            u.AccountApproved = user.AccountApproved;
            u.AuthorBio = user.AuthorBio;
            u.AvatarUrl = user.AvatarUrl;
            u.CanAutoLockout = user.CanAutoLockout;
            u.Comment = user.Comment;
            //u.Country = user.Country;
            u.CreatedUtc = user.CreatedUtc;
            u.LastModifiedUtc = user.LastModifiedUtc;

            if (user.DateOfBirth.HasValue)
            {
                u.DateOfBirth = user.DateOfBirth.Value;
            }
            
            u.DisplayInMemberList = user.DisplayInMemberList;
            u.DisplayName = user.DisplayName;
            u.Email = user.Email;
            u.EmailConfirmed = user.EmailConfirmed;
            u.FirstName = user.FirstName;
            u.Gender = user.Gender;
            u.IsDeleted = user.IsDeleted;
            u.IsLockedOut = user.IsLockedOut;
            
            if(user.LastLoginUtc.HasValue)
            {
                u.LastLoginUtc = user.LastLoginUtc.Value;
            }
            
            u.LastName = user.LastName;
            if(user.LastPasswordChangeUtc.HasValue)
            {
                u.LastPasswordChangeUtc = user.LastPasswordChangeUtc.Value;
            }
            
            u.LockoutEndDateUtc = user.LockoutEndDateUtc;
            u.MustChangePwd = user.MustChangePwd;
            u.NormalizedEmail = user.NormalizedEmail;
            u.NormalizedUserName = user.NormalizedUserName;
            u.NewEmail = user.NewEmail;
            u.NewEmailApproved = user.NewEmailApproved;
            
            u.PasswordHash = user.PasswordHash;
            u.PhoneNumber = user.PhoneNumber;
            u.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            
            u.RolesChanged = user.RolesChanged;
            u.SecurityStamp = user.SecurityStamp;
            u.Signature = user.Signature;
            u.SiteId = user.SiteId;
            
            //u.State = user.State;
            u.TimeZoneId = user.TimeZoneId;
            u.Trusted = user.Trusted;
            u.TwoFactorEnabled = user.TwoFactorEnabled;
            
            u.UserName = user.UserName;
            u.WebSiteUrl = user.WebSiteUrl;
            u.EmailConfirmSentUtc = user.EmailConfirmSentUtc;
            u.AgreementAcceptedUtc = user.AgreementAcceptedUtc;
           

            return u;
        }

    }
}
