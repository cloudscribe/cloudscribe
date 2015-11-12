// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2015-11-07
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
        
        

    }
}
