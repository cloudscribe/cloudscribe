//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2015-07-14
//// Last Modified:			2016-01-07
//// 


//using cloudscribe.Core.Models;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Repositories.Caching
//{
    
//    public class MemoryCacheUserRepository : IUserRepository
//    {
//        private IUserRepository implementation;
//        private IMemoryCache cache;
//        private ILogger log;

//        public MemoryCacheUserRepository(
//            IUserRepository implementation,
//            IMemoryCache cache,
//            ILogger<MemoryCacheUserRepository> logger)
//        {
//            if (implementation == null) { throw new ArgumentNullException(nameof(implementation)); }
//            if (implementation is MemoryCacheUserRepository) { throw new ArgumentException("implementation cannot be an instance of MemoryCacheUserRepository"); }
//            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
//            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }

//            this.cache = cache;
//            this.implementation = implementation;
//            log = logger;

//        }

//        //TODO: implement caching logic

//        #region User 

//        public async Task<bool> Save(ISiteUser user, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.Save(user, cancellationToken);
//        }

//        //public async Task<bool> Delete(ISiteUser user)
//        //{
//        //    return await implementation.Delete(user);
//        //}

//        public async Task<bool> Delete(int siteId, int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.Delete(siteId, userId, cancellationToken);
//        }

//        public async Task<bool> DeleteUsersBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteUsersBySite(siteId, cancellationToken);
//        }

//        public async Task<bool> FlagAsDeleted(int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FlagAsDeleted(userId, cancellationToken);
//        }

//        public async Task<bool> FlagAsNotDeleted(int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FlagAsNotDeleted(userId, cancellationToken);
//        }

//        public async Task<bool> SetRegistrationConfirmationGuid(
//            Guid userGuid,
//            Guid registrationConfirmationGuid,
//            CancellationToken cancellationToken)
//        {
//            return await implementation.SetRegistrationConfirmationGuid(userGuid, registrationConfirmationGuid, cancellationToken);
//        }

//        public async Task<bool> ConfirmRegistration(Guid registrationGuid, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.ConfirmRegistration(registrationGuid, cancellationToken);
//        }

//        public async Task<bool> LockoutAccount(Guid userGuid, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.LockoutAccount(userGuid, cancellationToken);
//        }

//        public async Task<bool> UnLockAccount(Guid userGuid, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.UnLockAccount(userGuid, cancellationToken);
//        }

//        public async Task<bool> UpdateFailedPasswordAttemptCount(
//            Guid userGuid, 
//            int failedPasswordAttemptCount, 
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount, cancellationToken);
//        }

//        //public async Task<bool> UpdateTotalRevenue(Guid userGuid)
//        //{
//        //    return await implementation.UpdateTotalRevenue(userGuid);

//        //}

//        public int GetCount(int siteId)
//        {
//            return implementation.GetCount(siteId);
//        }

//        public async Task<ISiteUser> Fetch(int siteId, int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.Fetch(siteId, userId, cancellationToken);
//        }

//        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.Fetch(siteId, userGuid, cancellationToken);
//        }

//        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FetchByConfirmationGuid(siteId, confirmGuid, cancellationToken);
//        }

//        public async Task<ISiteUser> Fetch(int siteId, string email, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.Fetch(siteId, email, cancellationToken);
//        }

//        public async Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FetchByLoginName(siteId, userName, allowEmailFallback, cancellationToken);
//        }

//        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetByIPAddress(siteGuid, ipv4Address, cancellationToken);
//        }

//        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetCrossSiteUserListByEmail(email, cancellationToken);
//        }

//        public async Task<int> CountUsers(int siteId, String userNameBeginsWith, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountUsers(siteId, userNameBeginsWith, cancellationToken);
//        }

//        public async Task<List<IUserInfo>> GetPage(
//            int siteId,
//            int pageNumber,
//            int pageSize,
//            string userNameBeginsWith,
//            int sortMode,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetPage(siteId, pageNumber, pageSize, userNameBeginsWith, sortMode, cancellationToken);
//        }

//        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountUsersForAdminSearch(siteId, searchInput, cancellationToken);
//        }

//        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
//            int siteId,
//            int pageNumber,
//            int pageSize,
//            string searchInput,
//            int sortMode,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUserAdminSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode, cancellationToken);
//        }

//        public async Task<int> CountLockedOutUsers(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountLockedOutUsers(siteId, cancellationToken);
//        }

//        public async Task<List<IUserInfo>> GetPageLockedOutUsers(
//            int siteId,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetPageLockedOutUsers(siteId, pageNumber, pageSize, cancellationToken);
//        }

//        public async Task<int> CountNotApprovedUsers(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountNotApprovedUsers(siteId, cancellationToken);
//        }

//        public async Task<List<IUserInfo>> GetNotApprovedUsers(
//            int siteId,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetNotApprovedUsers(siteId, pageNumber, pageSize, cancellationToken);
//        }

//        public async Task<bool> EmailExistsInDB(int siteId, string email, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.EmailExistsInDB(siteId, email, cancellationToken);
//        }

//        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.EmailExistsInDB(siteId, userId, email, cancellationToken);
//        }

//        public bool LoginExistsInDB(int siteId, string loginName)
//        {
//            return implementation.LoginExistsInDB(siteId, loginName);
//        }

//        public async Task<bool> LoginIsAvailable(int siteId, int userId, string loginName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.LoginIsAvailable(siteId, userId, loginName, cancellationToken);
//        }

//        public async Task<string> GetUserNameFromEmail(int siteId, string email, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUserNameFromEmail(siteId, email, cancellationToken);
//        }

//        #endregion



//        #region Roles

//        public async Task<bool> SaveRole(ISiteRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.SaveRole(role, cancellationToken);
//        }

//        public async Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteRole(roleId, cancellationToken);
//        }

//        public async Task<bool> DeleteRolesBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteRolesBySite(siteId, cancellationToken);
//        }

//        public async Task<bool> AddUserToRole(
//            int roleId,
//            Guid roleGuid,
//            int userId,
//            Guid userGuid,
//            CancellationToken cancellationToken = default(CancellationToken)
//            )
//        {
//            return await implementation.AddUserToRole(roleId, roleGuid, userId, userGuid, cancellationToken);
//        }

//        public async Task<bool> RemoveUserFromRole(int roleId, int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.RemoveUserFromRole(roleId, userId, cancellationToken);
//        }

//        public async Task<bool> DeleteUserRoles(int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteUserRoles(userId, cancellationToken);
//        }

//        public async Task<bool> DeleteUserRolesByRole(int roleId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteUserRolesByRole(roleId, cancellationToken);
//        }

//        public async Task<bool> DeleteUserRolesBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteUserRolesBySite(siteId, cancellationToken);
//        }

//        public async Task<bool> RoleExists(int siteId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.RoleExists(siteId, roleName, cancellationToken);
//        }

//        //public int GetRoleMemberCount(int roleId)
//        //{
//        //    return implementation.GetRoleMemberCount(roleId);
//        //}

//        public async Task<ISiteRole> FetchRole(int roleId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FetchRole(roleId, cancellationToken);
//        }

//        public async Task<ISiteRole> FetchRole(int siteId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FetchRole(siteId, roleName, cancellationToken);
//        }

//        public async Task<List<string>> GetUserRoles(int siteId, int userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUserRoles(siteId, userId, cancellationToken);
//        }

//        public async Task<int> CountOfRoles(int siteId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountOfRoles(siteId, searchInput, cancellationToken);
//        }

//        public async Task<IList<ISiteRole>> GetRolesBySite(
//            int siteId,
//            string searchInput,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetRolesBySite(siteId, searchInput, pageNumber, pageSize, cancellationToken);
//        }

//        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountUsersInRole(siteId, roleId, searchInput, cancellationToken);
//        }

//        public async Task<IList<IUserInfo>> GetUsersInRole(
//            int siteId,
//            int roleId,
//            string searchInput,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize, cancellationToken);
//        }

//        public async Task<IList<ISiteUser>> GetUsersInRole(
//            int siteId,
//            string roleName,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUsersInRole(siteId, roleName, cancellationToken);
//        }

//        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CountUsersNotInRole(siteId, roleId, searchInput, cancellationToken);
//        }

//        public async Task<IList<IUserInfo>> GetUsersNotInRole(
//            int siteId,
//            int roleId,
//            string searchInput,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize, cancellationToken);
//        }

//        #endregion


//        #region Claims

//        public async Task<bool> SaveClaim(IUserClaim userClaim, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.SaveClaim(userClaim, cancellationToken);
//        }

//        public async Task<bool> DeleteClaim(int id, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteClaim(id, cancellationToken);
//        }

//        public async Task<bool> DeleteClaimsByUser(int siteId, string userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteClaimsByUser(siteId, userId, cancellationToken);
//        }

//        public async Task<bool> DeleteClaimByUser(int siteId, string userId, string claimType, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteClaimByUser(siteId, userId, claimType, cancellationToken);
//        }

//        public async Task<bool> DeleteClaimsBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteClaimsBySite(siteId, cancellationToken);
//        }

//        public async Task<IList<IUserClaim>> GetClaimsByUser(int siteId, string userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetClaimsByUser(siteId, userId, cancellationToken);
//        }

//        public async Task<IList<ISiteUser>> GetUsersForClaim(
//            int siteId,
//            string claimType,
//            string claimValue,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.GetUsersForClaim(siteId, claimType, claimValue, cancellationToken);
//        }

//        #endregion


//        #region Logins

//        public async Task<bool> CreateLogin(IUserLogin userLogin, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.CreateLogin(userLogin, cancellationToken);
//        }

//        public async Task<IUserLogin> FindLogin(
//            int siteId,
//            string loginProvider,
//            string providerKey,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.FindLogin(siteId, loginProvider, providerKey, cancellationToken);
//        }

//        public async Task<bool> DeleteLogin(
//            int siteId,
//            string loginProvider,
//            string providerKey,
//            string userId,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteLogin(siteId, loginProvider, providerKey, userId, cancellationToken);
//        }

//        public async Task<bool> DeleteLoginsByUser(int siteId, string userId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteLoginsByUser(siteId, userId, cancellationToken);
//        }

//        public async Task<bool> DeleteLoginsBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await implementation.DeleteLoginsBySite(siteId, cancellationToken);
//        }

//        public async Task<IList<IUserLogin>> GetLoginsByUser(
//            int siteId,
//            string userId,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {

//            return await implementation.GetLoginsByUser(siteId, userId, cancellationToken);
//        }

//        #endregion

//        #region IDisposable Support
//        private bool disposedValue = false; // To detect redundant calls

//        void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    // TODO: dispose managed state (managed objects).
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
//                // TODO: set large fields to null.

//                disposedValue = true;
//            }
//        }

//        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
//        // ~SiteRoleStore() {
//        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//        //   Dispose(false);
//        // }

//        // This code added to correctly implement the disposable pattern.
//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//            Dispose(true);
//            // TODO: uncomment the following line if the finalizer is overridden above.
//            // GC.SuppressFinalize(this);
//        }
//        #endregion



//    }
//}
