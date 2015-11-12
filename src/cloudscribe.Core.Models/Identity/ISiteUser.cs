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
        
        Guid EmailChangeGuid { get; set; }
        bool EmailConfirmed { get; set; }
        
        int FailedPasswordAnswerAttemptCount { get; set; }
        DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        int FailedPasswordAttemptCount { get; set; }
        DateTime FailedPasswordAttemptWindowStart { get; set; }

        string PasswordHash { get; set; }
        Guid PasswordResetGuid { get; set; }

        bool MustChangePwd { get; set; }
        DateTime LastPasswordChangedDate { get; set; }

        DateTime LastLockoutDate { get; set; }
        DateTime? LockoutEndDateUtc { get; set; }

        bool TwoFactorEnabled { get; set; }
        Guid RegisterConfirmGuid { get; }
        bool RolesChanged { get; set; }
        string SecurityStamp { get; set; }
        
        string LoweredEmail { get; set; }
        string NewEmail { get; set; }
        
        string Signature { get; set; }
        string AuthorBio { get; set; }
        string Comment { get; set; }
        
    }
}
