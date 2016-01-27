// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2016-01-07
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface IUserRepository : IDisposable
    {
        //Task<bool> SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid, CancellationToken cancellationToken);
        //Task<bool> ConfirmRegistration(Guid registrationGuid, CancellationToken cancellationToken);
        Task<bool> EmailExistsInDB(int siteId, int userId, string email, CancellationToken cancellationToken);
        Task<bool> EmailExistsInDB(int siteId, string email, CancellationToken cancellationToken);
        Task<ISiteUser> Fetch(int siteId, Guid userGuid, CancellationToken cancellationToken);
        Task<ISiteUser> Fetch(int siteId, int userId, CancellationToken cancellationToken);
        Task<ISiteUser> Fetch(int siteId, string email, CancellationToken cancellationToken);
        //Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid, CancellationToken cancellationToken);
        Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback, CancellationToken cancellationToken);
        //Task<ISiteUser> FetchNewest(int siteId);
        Task<bool> Delete(int siteId, int userId, CancellationToken cancellationToken);
        Task<bool> DeleteUsersBySite(int siteId, CancellationToken cancellationToken);
        //Task<bool> Delete(ISiteUser user);
        Task<bool> FlagAsDeleted(int userId, CancellationToken cancellationToken);
        Task<bool> FlagAsNotDeleted(int userId, CancellationToken cancellationToken);
        Task<bool> LockoutAccount(Guid userGuid, CancellationToken cancellationToken);
        Task<bool> UnLockAccount(Guid userGuid, CancellationToken cancellationToken);
        Task<bool> UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount, CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address, CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email, CancellationToken cancellationToken);
        Task<List<IUserInfo>> GetNotApprovedUsers(int siteId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<IUserInfo>> GetPage(int siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode, CancellationToken cancellationToken);
        Task<List<IUserInfo>> GetPageLockedOutUsers(int siteId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<IUserInfo>> GetUserAdminSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode, CancellationToken cancellationToken);
        //Task<List<IUserInfo>> GetUserSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode);
        Task<bool> Save(ISiteUser user, CancellationToken cancellationToken);
        //Task<bool> UpdateTotalRevenue();
        //Task<bool> UpdateTotalRevenue(Guid userGuid);
        //Task<int> GetNewestUserId(int siteId);
        Task<string> GetUserNameFromEmail(int siteId, string email, CancellationToken cancellationToken);
        Task<int> CountUsers(int siteId, string userNameBeginsWith, CancellationToken cancellationToken);
        //Task<int> CountUsersForSearch(int siteId, string searchInput);
        Task<int> CountUsersForAdminSearch(int siteId, string searchInput, CancellationToken cancellationToken);
        Task<int> CountLockedOutUsers(int siteId, CancellationToken cancellationToken);
        Task<int> CountNotApprovedUsers(int siteId, CancellationToken cancellationToken);
        bool LoginExistsInDB(int siteId, string loginName);
        Task<bool> LoginIsAvailable(int siteId, int userId, string loginName, CancellationToken cancellationToken);
        //int UserCount(int siteId, string userNameBeginsWith);
        //int UsersOnlineSinceCount(int siteId, DateTime sinceTime);
        int GetCount(int siteId);
        //bool UpdatePasswordAndSalt(int userId, int passwordFormat, string password, string passwordSalt);
        //DataTable GetUserListForPasswordFormatChange(int siteId);

        //roles
        Task<bool> AddUserToRole(int roleId, Guid roleGuid, int userId, Guid userGuid, CancellationToken cancellationToken);
        //Task<bool> AddUserToDefaultRoles(ISiteUser siteUser);
        Task<int> CountOfRoles(int siteId, string searchInput, CancellationToken cancellationToken);
        //int GetRoleMemberCount(int roleId);
        Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken);
        Task<bool> DeleteRolesBySite(int siteId, CancellationToken cancellationToken);
        Task<bool> DeleteUserRoles(int userId, CancellationToken cancellationToken);
        Task<bool> DeleteUserRolesByRole(int roleId, CancellationToken cancellationToken);
        Task<bool> DeleteUserRolesBySite(int siteId, CancellationToken cancellationToken);
        Task<bool> RoleExists(int siteId, string roleName, CancellationToken cancellationToken);
        Task<ISiteRole> FetchRole(int roleId, CancellationToken cancellationToken);
        Task<ISiteRole> FetchRole(int siteId, string roleName, CancellationToken cancellationToken);
        Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        Task<List<string>> GetUserRoles(int siteId, int userId, CancellationToken cancellationToken);
        //Task<List<int>> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons);
        //IList<ISiteRole> GetRolesUserIsNotIn(int siteId, int userId);
        Task<IList<ISiteUser>> GetUsersInRole(int siteId, string roleName, CancellationToken cancellationToken);
        Task<IList<IUserInfo>> GetUsersInRole(int siteId, int roleId, string searchInput, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<IList<IUserInfo>> GetUsersNotInRole(int siteId, int roleId, string searchInput, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountUsersInRole(int siteId, int roleId, string searchInput, CancellationToken cancellationToken);
        Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput, CancellationToken cancellationToken);
        Task<bool> RemoveUserFromRole(int roleId, int userId, CancellationToken cancellationToken);
        Task<bool> SaveRole(ISiteRole role, CancellationToken cancellationToken);

        //claims
        Task<bool> DeleteClaim(int id, CancellationToken cancellationToken);
        Task<bool> DeleteClaimsBySite(int siteId, CancellationToken cancellationToken);
        Task<bool> DeleteClaimsByUser(int siteId, string userId, CancellationToken cancellationToken);
        Task<bool> DeleteClaimByUser(int siteId, string userId, string claimType, CancellationToken cancellationToken);
        Task<IList<IUserClaim>> GetClaimsByUser(int siteId, string userId, CancellationToken cancellationToken);
        Task<IList<ISiteUser>> GetUsersForClaim(int siteId, string claimType, string claimValue, CancellationToken cancellationToken);
        Task<bool> SaveClaim(IUserClaim userClaim, CancellationToken cancellationToken);

        //logins
        Task<bool> CreateLogin(IUserLogin userLogin, CancellationToken cancellationToken);
        Task<bool> DeleteLogin(int siteId, string loginProvider, string providerKey, string userId, CancellationToken cancellationToken);
        Task<bool> DeleteLoginsBySite(int siteId, CancellationToken cancellationToken);
        Task<bool> DeleteLoginsByUser(int siteId, string userId, CancellationToken cancellationToken);
        Task<IUserLogin> FindLogin(int siteId, string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<IList<IUserLogin>> GetLoginsByUser(int siteId, string userId, CancellationToken cancellationToken);
    }
}
