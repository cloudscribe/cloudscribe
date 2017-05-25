// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2016-06-17
// 

using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Models
{
    // lighter weight version of user for lists, search etc
    // where full model is not needed
    // we can add more to IUserInfo, but only fields that exist in mp_Users
    public interface IUserInfo
    {
        Guid Id { get; }
        Guid SiteId { get; set; }
        string UserName { get; set; }
        string DisplayName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        DateTime CreatedUtc { get; set; }
        DateTime LastModifiedUtc { get; set; }
        DateTime? DateOfBirth { get; set; }
        bool DisplayInMemberList { get; set; }
        bool Trusted { get; set; }
        string WebSiteUrl { get; set; }
        bool IsDeleted { get; set; }

        /// <summary>
        /// this property indicates if an account has been locked by an administrator
        /// </summary>
        bool IsLockedOut { get; set; }
        
        DateTime? LastLoginUtc { get; set; }
        string TimeZoneId { get; set; }
        
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool AccountApproved { get; set; }
        string AvatarUrl { get; set; }
        string Gender { get; set; }
        //string Country { get; set; }
        //string State { get; set; }

    }

    public interface ISiteUser : IUserInfo
    {
        
        int AccessFailedCount { get; set; } // maps to FailedPasswordAttemptCount in ado data layers
        string PasswordHash { get; set; }
        bool MustChangePwd { get; set; }
        DateTime? LastPasswordChangeUtc { get; set; }
        
        /// <summary>
        /// This property is independendent of IsLockedOut, if the property is populated with a future datetime then
        /// the user is locked out until that datetime. This property is used for lockouts related to failed authentication attempts,
        /// as opposed to IsLockedOut which is a property the admin can use to permanently lock out an account.
        /// </summary>
        DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// This property determines whether a user account can be locked out using LockouEndDateUtc,
        /// ie whether failed login attempts can cause the account to be locked by setting the LockoutEndDate.
        /// It should be true for most accounts but perhaps for admin accounts you may not want it to be possible 
        /// for an admin user to be locked out
        /// </summary>
        //bool CanBeLockedOut { get; set; } // TODO: add this property

        bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// indicates if this account can be automatically locked out (temporarily) due to AccessFailedCount
        /// >= site.MaxInvalidPasswordAttempts
        /// If false then this account will not be locked out by failed access attempts.
        /// </summary>
        bool CanAutoLockout { get; set; }

        //TODO: implement a middleware to detect this and reset the the auth/role cookie
        // set this true whenever a users roles have been changed and set false after cookie reset
        bool RolesChanged { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        string SecurityStamp { get; set; }

        bool EmailConfirmed { get; set; }
        /// <summary>
        /// last time we sent a verification email to this user email account
        /// mainly used for periodic retry if user tries to login and still not verified
        /// </summary>
        DateTime? EmailConfirmSentUtc { get; set; }

        /// <summary>
        /// if populated the user accepted the terms
        /// if you change the terms you could rest this to nukk in the db to force users to accept the new terms at login
        /// </summary>
        DateTime? AgreementAcceptedUtc { get; set; }

        string NormalizedEmail { get; set; } // maps to LoweredEmail in ado data layers
        string NormalizedUserName { get; set; }

        string NewEmail { get; set; }
        

        /// <summary>
        /// when a user requests a change to their currently confirmed account email
        /// we should send them an approval link to their current email
        /// if the user clicks the link we should set this to true, then send an email with 
        /// a link to confirm the new email. Once they click that link we should move the new email to the
        /// Email and NormalizedEmail fields, mark it as confirmed and clear the value from NewEmail,
        /// and set NewEmailApproved to false
        /// This strategy should make it difficult to take over an account even if a session hijack
        /// somehow had been achieved.
        /// </summary>
        bool NewEmailApproved { get; set; }

        string Signature { get; set; }
        string AuthorBio { get; set; }
        string Comment { get; set; }
       // string ConcurrencyStamp { get; set; }

       

    }
}
