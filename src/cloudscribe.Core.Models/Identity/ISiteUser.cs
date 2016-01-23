// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2014-11-12
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
        Guid UserGuid { get; set; }
        int UserId { get; set; }
        Guid SiteGuid { get; set; }
        int SiteId { get; set; }
        string UserName { get; set; }
        string DisplayName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        DateTime CreatedUtc { get; set; }
        DateTime DateOfBirth { get; set; }
        bool DisplayInMemberList { get; set; }
        bool Trusted { get; set; }
        string WebSiteUrl { get; set; }
        bool IsDeleted { get; set; }

        /// <summary>
        /// this property indicates if an account has been locked by an administrator
        /// </summary>
        bool IsLockedOut { get; set; }
        DateTime LastActivityDate { get; set; }
        DateTime LastLoginDate { get; set; }
        string TimeZoneId { get; set; }
        
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool AccountApproved { get; set; }
        string AvatarUrl { get; set; }
        string Gender { get; set; }
        string Country { get; set; }
        string State { get; set; }

    }

    public interface ISiteUser : IUserInfo
    {
        string Id { get; }
        
        
        bool EmailConfirmed { get; set; }
        
        int FailedPasswordAnswerAttemptCount { get; set; }
        DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        int FailedPasswordAttemptCount { get; set; }
        DateTime FailedPasswordAttemptWindowStart { get; set; }

        string PasswordHash { get; set; }

        // TODO: this field should not be used, guids don't make good security tokens
        // the Identity System can generate self expiring purpose tokens based on the the user security stamp
        // and we don't need to persist any of the generated tokens
        Guid PasswordResetGuid { get; set; }

        bool MustChangePwd { get; set; }
        DateTime LastPasswordChangedDate { get; set; }

        DateTime LastLockoutDate { get; set; }

        /// <summary>
        /// This property is independendent IsLockedOut, it the property is populated with a future datetime then
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

        // TODO: again we should not be using a guid for email verification
        // this should be removed and we should use Identity system to generate self expiring purpose token based on the security stamp

        Guid RegisterConfirmGuid { get; }
        bool RolesChanged { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        string SecurityStamp { get; set; }
        
        string LoweredEmail { get; set; }

        // TODO: remove this, we should not use guids as tokens for security purposes
        Guid EmailChangeGuid { get; set; }

        string NewEmail { get; set; }
        
        string Signature { get; set; }
        string AuthorBio { get; set; }
        string Comment { get; set; }
        
    }
}
