// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2019-04-20
// 

using cloudscribe.Common.Gdpr;
using System;

namespace cloudscribe.Core.Models
{
    public class SiteUser : UserInfo, ISiteUser
    {

        [ProtectedPersonalDataMarker]
        public string AuthorBio { get; set; }

        public string Comment { get; set; }

        public string NormalizedEmail { get; set; }

        public string NormalizedUserName { get; set; }

        [PersonalDataMarker]
        public bool EmailConfirmed { get; set; } = false;

        public DateTime? EmailConfirmSentUtc { get; set; } = null;

        public DateTime? AgreementAcceptedUtc { get; set; } = null;

        public DateTime? LockoutEndDateUtc { get; set; } = null;

        public string NewEmail { get; set; }

        public bool NewEmailApproved { get; set; } = false;

        public DateTime? LastPasswordChangeUtc { get; set; } //= DateTime.MinValue;
        
        
        public bool MustChangePwd { get; set; } = false;

     
        public string PasswordHash { get; set; }

        public bool CanAutoLockout { get; set; } = true;

        public int AccessFailedCount { get; set; } = 0;
        
        public bool RolesChanged { get; set; } = false;

    
        public string SecurityStamp { get; set; }

        [ProtectedPersonalDataMarker]
        public string Signature { get; set; }

        [PersonalDataMarker]
        public bool TwoFactorEnabled { get; set; } = false;

        public string BrowserKey { get; set; }



        public static SiteUser FromISiteUser(ISiteUser user)
        {
            SiteUser u = new SiteUser();
          
            if (user.Id != Guid.Empty) u.Id = user.Id;
           
            u.AccessFailedCount = user.AccessFailedCount;
            u.AccountApproved = user.AccountApproved;
            u.AuthorBio = user.AuthorBio;
            u.AvatarUrl = user.AvatarUrl;
            u.BrowserKey = user.BrowserKey;
            u.CanAutoLockout = user.CanAutoLockout;
            u.Comment = user.Comment;
      
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
            
            u.TwoFactorEnabled = user.TwoFactorEnabled;
            
            u.UserName = user.UserName;
            u.WebSiteUrl = user.WebSiteUrl;
            u.EmailConfirmSentUtc = user.EmailConfirmSentUtc;
            u.AgreementAcceptedUtc = user.AgreementAcceptedUtc;
           

            return u;
        }

    }
}
