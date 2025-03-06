﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2019-04-20
// 

using System;

namespace cloudscribe.Core.Models
{
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
        
        string WebSiteUrl { get; set; }
        

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

        /// <summary>
        /// a random guid string generated upon successful login, also stored as a claim.
        /// if siteSettings.SingleBrowserSessions is true then middleware will sign out an authenticated user whose claim doesn't match the current value of this property
        /// to prevent account sharing and simulatenous use from multiple devices or web browsers.
        /// </summary>
        string BrowserKey { get; set; }
    }
}
