// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2015-08-04
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
        public string EditorPreference { get; set; } = string.Empty; //use site defsault
        public string LoweredEmail { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public Guid EmailChangeGuid { get; set; } = Guid.Empty;
        public DateTime? LockoutEndDateUtc { get; set; } = null;
        public string NewEmail { get; set; } = string.Empty;
        public DateTime LastPasswordChangedDate { get; set; } = DateTime.MinValue;
        public DateTime LastLockoutDate { get; set; } = DateTime.MinValue;
        public string MobilePin { get; set; } = string.Empty;
        public string OpenIdUri { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool MustChangePwd { get; set; } = false;
        

        private string passwordHash = string.Empty; // used for asp.net identity pwd field is still used for membership

        /// <summary>
        /// since aspnet identity doesn't directly support salt
        /// we have to store the hash, the salt and the format in a single field
        /// so our custom hasher will have it
        /// http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity
        /// https://aspnetidentity.codeplex.com/workitem/2333
        /// </summary>
        public string PasswordHash
        {
            get
            {
                if (passwordHash.Length == 0)
                {
                    return Password + "|" + passwordSalt + "|" + passwordFormat.ToString();
                }
                return passwordHash;
            }
            set { passwordHash = value; }
        }

        private string passwordSalt = string.Empty;

        public string PasswordSalt
        {
            get { return passwordSalt; }
            set { passwordSalt = value; }
        }

        private int passwordFormat = 0; //plain text 1= hashed, 2 = encrypted

        public int PasswordFormat
        {
            get { return passwordFormat; }
            set { passwordFormat = value; }
        }

        public string PasswordQuestion { get; set; } = string.Empty;
        public string PasswordAnswer { get; set; } = string.Empty;
        public int FailedPasswordAttemptCount { get; set; } = 0;
        public DateTime FailedPasswordAttemptWindowStart { get; set; } = DateTime.MinValue;
        public int FailedPasswordAnswerAttemptCount { get; set; } = 0;
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; } = DateTime.MinValue;
        public Guid PasswordResetGuid { get; set; } = Guid.Empty;
        public Guid RegisterConfirmGuid { get; set; } = Guid.Empty;
        public bool RolesChanged { get; set; } = false;
        public string SecurityStamp { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty; 
        public string Skin { get; set; } = string.Empty;
        public bool TwoFactorEnabled { get; set; } = false;
        public string WindowsLiveId { get; set; } = string.Empty;





        //private string occupation = string.Empty;
        //private string interests = string.Empty;
        //private string msn = string.Empty;
        //private string yahoo = string.Empty;
        //private string aim = string.Empty;
        //private string icq = string.Empty;

        //private double timeOffsetHours = 0;
        //private bool nonLazyLoadedProfilePropertiesLoaded = false;
        //private bool rolesLoaded = false;



    }
}
