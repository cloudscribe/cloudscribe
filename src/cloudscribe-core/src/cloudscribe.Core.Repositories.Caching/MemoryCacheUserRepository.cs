// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-10-13
// 


using cloudscribe.Core.Models;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Caching
{
    
    public class MemoryCacheUserRepository
    {
        private IUserRepository implementation;
        private IMemoryCache cache;
        private ILogger log;

        public MemoryCacheUserRepository(
            IUserRepository implementation,
            IMemoryCache cache,
            ILogger<MemoryCacheUserRepository> logger)
        {
            if (implementation == null) { throw new ArgumentNullException(nameof(implementation)); }
            if (implementation is MemoryCacheUserRepository) { throw new ArgumentException("implementation cannot be an instance of MemoryCacheUserRepository"); }
            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }

            this.cache = cache;
            this.implementation = implementation;
            log = logger;

        }

        #region User 

        public async Task<bool> Save(ISiteUser user)
        {
            return await implementation.Save(user);
        }

        public async Task<bool> Delete(int userId)
        {
            return await implementation.Delete(userId);
        }

        public async Task<bool> FlagAsDeleted(int userId)
        {
            return await implementation.FlagAsDeleted(userId);
        }

        public async Task<bool> FlagAsNotDeleted(int userId)
        {
            return await implementation.FlagAsNotDeleted(userId);
        }

        //public async Task<bool> ConfirmRegistration(Guid registrationGuid)
        //{    
        //    return await implementation.ConfirmRegistration(Guid.Empty, registrationGuid);
        //}

        //public async Task<bool> LockoutAccount(Guid userGuid)
        //{
        //    return await implementation.AccountLockout(userGuid, DateTime.UtcNow);
        //}

        //public async Task<bool> UnLockAccount(Guid userGuid)
        //{
        //    return await implementation.AccountClearLockout(userGuid);
        //}

        public async Task<bool> UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            return await implementation.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount);
        }

        //public async Task<bool> UpdateTotalRevenue(Guid userGuid)
        //{
        //    return await implementation.UpdateTotalRevenue(userGuid);

        //}

        public int GetCount(int siteId)
        {
            return implementation.GetCount(siteId);
        }

        public async Task<ISiteUser> Fetch(int siteId, int userId)
        {
            return await implementation.Fetch(siteId, userId);
        }

        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid)
        {
            return await implementation.Fetch(siteId, userGuid);
        }

        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            return await implementation.FetchByConfirmationGuid(siteId, confirmGuid);
        }

        public async Task<ISiteUser> Fetch(int siteId, string email)
        {
            return await implementation.Fetch(siteId, email);
        }

        public async Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            return await implementation.FetchByLoginName(siteId, userName, allowEmailFallback);
        }

        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            return await implementation.GetByIPAddress(siteGuid, ipv4Address);
        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email)
        {
            return await implementation.GetCrossSiteUserListByEmail(email);
        }

        public async Task<int> CountUsers(int siteId, String userNameBeginsWith)
        {
            return await implementation.CountUsers(siteId, userNameBeginsWith);
        }

        public async Task<List<IUserInfo>> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            return await implementation.GetPage(siteId, pageNumber, pageSize, userNameBeginsWith, sortMode);
        }

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
        {
            return await implementation.CountUsersForAdminSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            return await implementation.GetUserAdminSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode);
        }

        public async Task<int> CountLockedOutUsers(int siteId)
        {
            return await implementation.CountLockedOutUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            return await implementation.GetPageLockedUsers(siteId, pageNumber, pageSize);
        }

        public async Task<int> CountNotApprovedUsers(int siteId)
        {
            return await implementation.CountNotApprovedUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            return await implementation.GetNotApprovedUsers(siteId, pageNumber, pageSize);
        }

        public async Task<bool> EmailExistsInDB(int siteId, string email)
        {
            return await implementation.EmailExistsInDB(siteId, email);
        }

        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email)
        {
            return await implementation.EmailExistsInDB(siteId, userId, email);
        }

        public bool LoginExistsInDB(int siteId, string loginName)
        {
            return implementation.LoginExistsInDB(siteId, loginName);
        }

        public async Task<bool> LoginIsAvailable(int siteId, int userId, string loginName)
        {
            return await implementation.LoginIsAvailable(siteId, userId, loginName);
        }

        public async Task<string> GetUserNameFromEmail(int siteId, string email)
        {
            return await implementation.GetUserNameFromEmail(siteId, email);
        }

        #endregion



        #region Roles

        public async Task<bool> SaveRole(ISiteRole role)
        {
            return await implementation.SaveRole(role);
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            return await implementation.DeleteRole(roleId);
        }

        public async Task<bool> AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid
            )
        {
            return await implementation.AddUserToRole(roleId, roleGuid, userId, userGuid);
        }

        public async Task<bool> RemoveUserFromRole(int roleId, int userId)
        {
            return await implementation.RemoveUserFromRole(roleId, userId);
        }

        public async Task<bool> DeleteUserRoles(int userId)
        {
            return await implementation.DeleteUserRoles(userId);
        }

        public async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            return await implementation.DeleteUserRolesByRole(roleId);
        }

        public async Task<bool> RoleExists(int siteId, string roleName)
        {
            return await implementation.RoleExists(siteId, roleName);
        }

        //public int GetRoleMemberCount(int roleId)
        //{
        //    return implementation.GetRoleMemberCount(roleId);
        //}

        public async Task<ISiteRole> FetchRole(int roleId)
        {
            return await implementation.FetchRole(roleId);
        }

        public async Task<ISiteRole> FetchRole(int siteId, string roleName)
        {
            return await implementation.FetchRole(siteId, roleName);
        }

        public async Task<List<string>> GetUserRoles(int siteId, int userId)
        {
            return await implementation.GetUserRoles(siteId, userId);
        }

        public async Task<int> CountOfRoles(int siteId, string searchInput)
        {
            return await implementation.CountOfRoles(siteId, searchInput);
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            return await implementation.GetRolesBySite(siteId, searchInput, pageNumber, pageSize);
        }

        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput)
        {
            return await implementation.CountUsersInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            return await implementation.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize);
        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            int siteId,
            string roleName)
        {
            return await implementation.GetUsersInRole(siteId, roleName);
        }

        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            return await implementation.CountUsersNotInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            return await implementation.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize);
        }

        #endregion


        #region Claims

        public async Task<bool> SaveClaim(IUserClaim userClaim)
        {
            return await implementation.SaveClaim(userClaim);
        }

        #endregion


            #region Logins

        public async Task<IList<IUserLogin>> GetLoginsByUser(
            int siteId,
            string userId)
        {

            return await implementation.GetLoginsByUser(siteId, userId);
        }

        #endregion




    }
}
