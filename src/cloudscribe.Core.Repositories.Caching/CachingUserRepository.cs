// Author:					Joe Audette
// Created:					2014-08-30
// Last Modified:			2015-01-07
// 

using cloudscribe.Caching;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Caching
{
    public sealed class CachingUserRepository : IUserRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CachingUserRepository));

        private IUserRepository repo = null;

        public CachingUserRepository(IUserRepository implementation)
        {
            if ((implementation == null)||(implementation is CachingUserRepository))
            {
                throw new ArgumentException("you must pass in an implementation of IUserRpository");
            }

            repo = implementation;
        }

        #region User

        public bool Save(ISiteUser user)
        {
            bool result = repo.Save(user);
            CacheManager.Cache.InvalidateCacheItem("user-" + user.Id);
            return result;

        }


        public bool Delete(int userId)
        {
            return repo.Delete(userId);
        }

        public bool FlagAsDeleted(int userId)
        {
            return repo.FlagAsDeleted(userId);
        }

        public bool FlagAsNotDeleted(int userId)
        {
            return repo.FlagAsNotDeleted(userId);
        }

        public bool UpdatePasswordAndSalt(
            int userId,
            int passwordFormat,
            string password,
            string passwordSalt)
        {
            return repo.UpdatePasswordAndSalt(userId, passwordFormat, password, passwordSalt);
        }

        public bool ConfirmRegistration(Guid registrationGuid)
        {
            return repo.ConfirmRegistration(registrationGuid);
        }


        public bool LockoutAccount(Guid userGuid)
        {
            bool result = repo.LockoutAccount(userGuid);
            CacheManager.Cache.InvalidateCacheItem("user-" + userGuid.ToString());
            return result;
        }

        public bool UnLockAccount(Guid userGuid)
        {
            bool result =  repo.UnLockAccount(userGuid);
            CacheManager.Cache.InvalidateCacheItem("user-" + userGuid.ToString());
            return result;
        }

        public bool UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            bool result = repo.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount);
            CacheManager.Cache.InvalidateCacheItem("user-" + userGuid.ToString());
            return result;
        }

        public void UpdateTotalRevenue(Guid userGuid)
        {
            repo.UpdateTotalRevenue(userGuid);

        }

        
        public void UpdateTotalRevenue()
        {
            repo.UpdateTotalRevenue();
        }


        public DataTable GetUserListForPasswordFormatChange(int siteId)
        {
            return repo.GetUserListForPasswordFormatChange(siteId);
        }

        public int GetCount(int siteId)
        {
            return repo.GetCount(siteId);
        }

        public int UserCount(int siteId, String userNameBeginsWith)
        {
            return repo.UserCount(siteId, userNameBeginsWith);
        }

        public int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        {
            return repo.UsersOnlineSinceCount(siteId, sinceTime);
        }


        public ISiteUser FetchNewest(int siteId)
        {
            return repo.FetchNewest(siteId);
        }

        public ISiteUser Fetch(int siteId, int userId)
        {
            return repo.Fetch(siteId, userId);
        }


        public ISiteUser Fetch(int siteId, Guid userGuid)
        {
            if (
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteUser == 0)
                )
            {
                return repo.Fetch(siteId, userGuid);
            }

            string cachekey = "user-" + userGuid.ToString();

            DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteUser);

            try
            {
                ISiteUser user = CacheManager.Cache.Get<ISiteUser>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    ISiteUser siteUser = repo.Fetch(siteId, userGuid);
                    return siteUser;
                });

                return user;
            }
            catch (Exception ex)
            {
                log.Error("failed to get ISiteUser from cache so loading it directly", ex);

            }

            return repo.Fetch(siteId, userGuid);
        }

        public ISiteUser FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            return repo.FetchByConfirmationGuid(siteId, confirmGuid);
        }


        public ISiteUser Fetch(int siteId, string email)
        {
            return repo.Fetch(siteId, email);
        }

        public ISiteUser FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            return repo.FetchByLoginName(siteId, userName, allowEmailFallback);
        }


        public List<IUserInfo> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            return repo.GetByIPAddress(siteGuid, ipv4Address);

        }

        public List<IUserInfo> GetCrossSiteUserListByEmail(string email)
        {
            return repo.GetCrossSiteUserListByEmail(email);
        }

        public List<IUserInfo> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            out int totalPages)
        {
            return repo.GetPage(
                siteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                sortMode,
                out totalPages);

        }

        public List<IUserInfo> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            return repo.GetUserSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode,
                out totalPages);


        }

        public List<IUserInfo> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            return repo.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode,
                out totalPages);

        }

        public List<IUserInfo> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return repo.GetPageLockedUsers(
                siteId,
                pageNumber,
                pageSize,
                out totalPages);
        }

        public List<IUserInfo> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return repo.GetNotApprovedUsers(
                siteId,
                pageNumber,
                pageSize,
                out totalPages);
        }

        
        public bool EmailExistsInDB(int siteId, string email)
        {
            return repo.EmailExistsInDB(siteId, email);
        }

        public bool EmailExistsInDB(int siteId, int userId, string email)
        {
            return repo.EmailExistsInDB(siteId, userId, email);
        }

        public bool LoginExistsInDB(int siteId, string loginName)
        {
            return repo.LoginExistsInDB(siteId, loginName);
        }

        public bool LoginIsAvailable(int siteId, int userId, string loginName)
        {
            return repo.LoginIsAvailable(siteId, userId, loginName);
        }

        public string GetUserNameFromEmail(int siteId, string email)
        {
            return repo.GetUserNameFromEmail(siteId, email);

        }



        public int GetNewestUserId(int siteId)
        {
            return repo.GetNewestUserId(siteId);
        }


        
        #endregion

        #region Roles

        public async Task<bool> SaveRole(ISiteRole role)
        {
            return await repo.SaveRole(role);
        }


        public async Task<bool> DeleteRole(int roleID)
        {
            return await repo.DeleteRole(roleID);
        }

        public async Task<bool> AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid
            )
        {
            return await repo.AddUserToRole(roleId, roleGuid, userId, userGuid);
        }

        public async Task<bool> RemoveUserFromRole(int roleId, int userId)
        {
            return await repo.RemoveUserFromRole(roleId, userId);
        }

        public async Task<bool> AddUserToDefaultRoles(ISiteUser siteUser)
        {
            return await repo.AddUserToDefaultRoles(siteUser);

        }

        public bool DeleteUserRoles(int userId)
        {
            return repo.DeleteUserRoles(userId);
        }

        public async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            return await repo.DeleteUserRolesByRole(roleId);
        }


        public async Task<bool> RoleExists(int siteId, string roleName)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return await repo.RoleExists(siteId, roleName);
        }

        public int GetRoleMemberCount(int roleId)
        {
            return repo.GetRoleMemberCount(roleId);
        }

        public async Task<ISiteRole> FetchRole(int roleID)
        {
            return await repo.FetchRole(roleID);
        }

        public ISiteRole FetchRole(int siteId, string roleName)
        {
            return repo.FetchRole(siteId, roleName);

        }

        public List<string> GetUserRoles(int siteId, int userId)
        {
            return repo.GetUserRoles(siteId, userId);
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            return await repo.GetRolesBySite(siteId, searchInput, pageNumber,pageSize);

        }

        public IList<ISiteRole> GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            return repo.GetRolesUserIsNotIn(siteId, userId);
        }

        public List<int> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        {
            return repo.GetRoleIds(siteId, roleNamesSeparatedBySemiColons);
        }

        //public static List<string> GetRolesNames(string roleNamesSeparatedBySemiColons)
        //{
        //    return repo.GetRolesNames(roleNamesSeparatedBySemiColons);
        //}


        public async Task<int> CountOfRoles(int siteId, string searchInput)
        {
            return await repo.CountOfRoles(siteId, searchInput);
        }

        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput)
        {
            return await repo.CountUsersInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize)
        {
            return await repo.GetUsersInRole(
                siteId,
                roleId,
                searchInput,
                pageNumber,
                pageSize);
        }

        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            return await repo.CountUsersNotInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            int pageNumber, 
            int pageSize)
        {
            return await repo.GetUsersNotInRole(
                siteId,
                roleId,
                searchInput,
                pageNumber,
                pageSize);
        }


        #endregion

        #region Claims

        /// <summary>
        /// Persists a new instance of UserClaim. Returns true on success.
        /// </summary>
        /// <returns></returns>
        public bool SaveClaim(IUserClaim userClaim)
        {
            return repo.SaveClaim(userClaim);
        }

        public bool DeleteClaim(int id)
        {
            return repo.DeleteClaim(id);
        }

        public bool DeleteClaimsByUser(string userId)
        {
            return repo.DeleteClaimsByUser(userId);
        }

        public bool DeleteClaimByUser(string userId, string claimType)
        {
            return repo.DeleteClaimByUser(userId, claimType);
        }

        public bool DeleteClaimsBySite(Guid siteGuid)
        {
            return repo.DeleteClaimsBySite(siteGuid);
        }

        public IList<IUserClaim> GetClaimsByUser(string userId)
        {
            return repo.GetClaimsByUser(userId);

        }


        #endregion

        #region Logins

        /// <summary>
        /// Persists a new instance of UserLogin. Returns true on success.
        /// </summary>
        /// <returns></returns>
        public bool CreateLogin(IUserLogin userLogin)
        {
            return repo.CreateLogin(userLogin);


        }


        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        public IUserLogin FindLogin(
            string loginProvider,
            string providerKey)
        {
            return repo.FindLogin(loginProvider, providerKey);
        }


        /// <summary>
        /// Deletes an instance of UserLogin. Returns true on success.
        /// </summary>
        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        /// <param name="userId"> userId </param>
        /// <returns>bool</returns>
        public bool DeleteLogin(
            string loginProvider,
            string providerKey,
            string userId)
        {
            return repo.DeleteLogin(
                loginProvider,
                providerKey,
                userId);
        }

        public bool DeleteLoginsByUser(string userId)
        {
            return repo.DeleteLoginsByUser(userId);
        }

        public bool DeleteLoginsBySite(Guid siteGuid)
        {
            return repo.DeleteLoginsBySite(siteGuid);
        }




        
        public IList<IUserLogin> GetLoginsByUser(string userId)
        {
            if (
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteUserLogins == 0)
                )
            {
                return repo.GetLoginsByUser(userId);
            }

            string cachekey = "ulogins-" + userId;

            DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteUserLogins);

            try
            {
                IList<IUserLogin> userLogins = CacheManager.Cache.Get<IList<IUserLogin>>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    IList<IUserLogin> logins = repo.GetLoginsByUser(userId);
                    return logins;
                });

                return userLogins;
            }
            catch (Exception ex)
            {
                log.Error("failed to get IList<IUserLogin> from cache so loading them directly", ex);

            }

            return repo.GetLoginsByUser(userId);

        }


        

        #endregion

        #region IDisposable

        public void Dispose()
        {
            repo.Dispose();
        }

        #endregion
    }
}
