// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2014-08-17
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
        int TotalPosts { get; set; }
        decimal TotalRevenue { get; set; }
        bool DisplayInMemberList { get; set; }
        bool Trusted { get; set; }
        string WebSiteUrl { get; set; }
        bool IsDeleted { get; set; }
        bool IsLockedOut { get; set; }
        DateTime LastActivityDate { get; set; }
        DateTime LastLoginDate { get; set; }
        string TimeZoneId { get; set; }
        bool ApprovedForLogin { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool ProfileApproved { get; set; }
        string AvatarUrl { get; set; }
        string Gender { get; set; }
        string Country { get; set; }
        string State { get; set; }

    }

    public interface ISiteUser : IUserInfo
    {

        string AuthorBio { get; set; }

        string Comment { get; set; }



        string EditorPreference { get; set; }

        Guid EmailChangeGuid { get; set; }
        bool EmailConfirmed { get; set; }
        //bool EnableLiveMessengerOnProfile { get; set; }
        int FailedPasswordAnswerAttemptCount { get; set; }
        DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        int FailedPasswordAttemptCount { get; set; }
        DateTime FailedPasswordAttemptWindowStart { get; set; }


        //string Interests { get; set; }


        DateTime LastLockoutDate { get; set; }


        DateTime LastPasswordChangedDate { get; set; }
        //string LiveMessengerDelegationToken { get; set; }
        //string LiveMessengerId { get; set; }
        DateTime? LockoutEndDateUtc { get; set; }

        string LoweredEmail { get; set; }
        bool MustChangePwd { get; set; }

        string NewEmail { get; set; }
        string OpenIdUri { get; set; }
        string Password { get; set; }
        string PasswordAnswer { get; set; }
        int PasswordFormat { get; set; }
        string PasswordHash { get; set; }
        string PasswordQuestion { get; set; }
        Guid PasswordResetGuid { get; set; }
        string PasswordSalt { get; set; }

        Guid RegisterConfirmGuid { get; }
        bool RolesChanged { get; set; }
        string SecurityStamp { get; set; }
        string Signature { get; set; }

        string Skin { get; set; }



        bool TwoFactorEnabled { get; set; }
        string Id { get; }


    }
}
