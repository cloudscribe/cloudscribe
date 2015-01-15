// Author:					Joe Audette
// Created:					2014-08-30
// Last Modified:			2015-01-14
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

        public async Task<bool> Save(ISiteUser user)
        {
            bool result = await repo.Save(user);
            CacheManager.Cache.InvalidateCacheItem("user-" + user.Id);
            return result;

        }


        public async Task<bool> Delete(int userId)
        {
            return await repo.Delete(userId);
        }

        public async Task<bool> FlagAsDeleted(int userId)
        {
            return await repo.FlagAsDeleted(userId);
        }

        public async Task<bool> FlagAsNotDeleted(int userId)
        {
            return await repo.FlagAsNotDeleted(userId);
        }

        //public bool UpdatePasswordAndSalt(
        //    int userId,
        //    int passwordFormat,
        //    string password,
        //    string passwordSalt)
        //{
        //    return repo.UpdatePasswordAndSalt(userId, passwordFormat, password, passwordSalt);
        //}

        public async Task<bool> ConfirmRegistration(Guid registrationGuid)
        {
            return await repo.ConfirmRegistration(registrationGuid);
        }


        public async Task<bool> LockoutAccount(Guid userGuid)
        {
            bool result = await repo.LockoutAccount(userGuid);
            CacheManager.Cache.InvalidateCacheItem("user-" + userGuid.ToString());
            return result;
        }

        public async Task<bool> UnLockAccount(Guid userGuid)
        {
            bool result = await  repo.UnLockAccount(userGuid);
            CacheManager.Cache.InvalidateCacheItem("user-" + userGuid.ToString());
            return result;
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            bool result = await repo.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount);
            CacheManager.Cache.InvalidateCacheItem("user-" + userGuid.ToString());
            return result;
        }

        public async Task<bool> UpdateTotalRevenue(Guid userGuid)
        {
            return await repo.UpdateTotalRevenue(userGuid);

        }


        public async Task<bool> UpdateTotalRevenue()
        {
            return await repo.UpdateTotalRevenue();
        }


        //public DataTable GetUserListForPasswordFormatChange(int siteId)
        //{
        //    return repo.GetUserListForPasswordFormatChange(siteId);
        //}

        public int GetCount(int siteId)
        {
            return repo.GetCount(siteId);
        }

        //public int UserCount(int siteId, String userNameBeginsWith)
        //{
        //    return repo.UserCount(siteId, userNameBeginsWith);
        //}

        public int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        {
            return repo.UsersOnlineSinceCount(siteId, sinceTime);
        }


        public async Task<ISiteUser> FetchNewest(int siteId)
        {
            return await repo.FetchNewest(siteId);
        }

        public async Task<ISiteUser> Fetch(int siteId, int userId)
        {
            return await repo.Fetch(siteId, userId);
        }


        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid)
        {
            if (
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteUser == 0)
                )
            {
                return await repo.Fetch(siteId, userGuid);
            }

            //string cachekey = "user-" + userGuid.ToString();

            //DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteUser);

            //try
            //{
            //    ISiteUser user = CacheManager.Cache.Get<ISiteUser>(cachekey, expiration, () =>
            //    {
            //        // This is the anonymous function which gets called if the data is not in the cache.
            //        // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
            //        ISiteUser siteUser = repo.Fetch(siteId, userGuid);
            //        return siteUser;
            //    });

            //    return user;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("failed to get ISiteUser from cache so loading it directly", ex);

            //}

            return await repo.Fetch(siteId, userGuid);
        }

        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            return await repo.FetchByConfirmationGuid(siteId, confirmGuid);
        }


        public async Task<ISiteUser> Fetch(int siteId, string email)
        {
            return await repo.Fetch(siteId, email);
        }

        public async Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            return await repo.FetchByLoginName(siteId, userName, allowEmailFallback);
        }


        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            return await repo.GetByIPAddress(siteGuid, ipv4Address);

        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email)
        {
            return await repo.GetCrossSiteUserListByEmail(email);
        }

        public async Task<int> CountUsers(int siteId, string userNameBeginsWith)
        {
            return await repo.CountUsers(siteId, userNameBeginsWith);
        }

        public async Task<List<IUserInfo>> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            return await repo.GetPage(
                siteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                sortMode);

        }

        public async Task<int> CountUsersForSearch(int siteId, string searchInput)
        {
            return await repo.CountUsersForSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            return await repo.GetUserSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode);


        }

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
        {
            return await repo.CountUsersForAdminSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            return await repo.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode);

        }

        public async Task<int> CountLockedOutUsers(int siteId)
        {
            return await repo.CountLockedOutUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            return await repo.GetPageLockedUsers(
                siteId,
                pageNumber,
                pageSize);
        }

        public async Task<int> CountNotApprovedUsers(int siteId)
        {
            return await repo.CountNotApprovedUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            return await repo.GetNotApprovedUsers(
                siteId,
                pageNumber,
                pageSize);
        }

        
        public async Task<bool> EmailExistsInDB(int siteId, string email)
        {
            return await repo.EmailExistsInDB(siteId, email);
        }

        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email)
        {
            return await repo.EmailExistsInDB(siteId, userId, email);
        }

        public bool LoginExistsInDB(int siteId, string loginName)
        {
            return repo.LoginExistsInDB(siteId, loginName);
        }

        public bool LoginIsAvailable(int siteId, int userId, string loginName)
        {
            return repo.LoginIsAvailable(siteId, userId, loginName);
        }

        public async Task<string> GetUserNameFromEmail(int siteId, string email)
        {
            return await repo.GetUserNameFromEmail(siteId, email);

        }



        public async Task<int> GetNewestUserId(int siteId)
        {
            return await repo.GetNewestUserId(siteId);
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

        public async Task<bool> DeleteUserRoles(int userId)
        {
            return await repo.DeleteUserRoles(userId);
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

        //public int GetRoleMemberCount(int roleId)
        //{
        //    return repo.GetRoleMemberCount(roleId);
        //}

        public async Task<ISiteRole> FetchRole(int roleID)
        {
            return await repo.FetchRole(roleID);
        }

        public async Task<ISiteRole> FetchRole(int siteId, string roleName)
        {
            return await repo.FetchRole(siteId, roleName);

        }

        public async Task<List<string>> GetUserRoles(int siteId, int userId)
        {
            return await repo.GetUserRoles(siteId, userId);
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

        public async Task<List<int>> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        {
            return await repo.GetRoleIds(siteId, roleNamesSeparatedBySemiColons);
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
        public async Task<bool> SaveClaim(IUserClaim userClaim)
        {
            return await repo.SaveClaim(userClaim);
        }

        public async Task<bool> DeleteClaim(int id)
        {
            return await repo.DeleteClaim(id);
        }

        public async Task<bool> DeleteClaimsByUser(string userId)
        {
            return await repo.DeleteClaimsByUser(userId);
        }

        public async Task<bool> DeleteClaimByUser(string userId, string claimType)
        {
            return await repo.DeleteClaimByUser(userId, claimType);
        }

        public async Task<bool> DeleteClaimsBySite(Guid siteGuid)
        {
            return await repo.DeleteClaimsBySite(siteGuid);
        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(string userId)
        {
            return await repo.GetClaimsByUser(userId);

        }


        #endregion

        #region Logins

        /// <summary>
        /// Persists a new instance of UserLogin. Returns true on success.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateLogin(IUserLogin userLogin)
        {
            return await repo.CreateLogin(userLogin);


        }


        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        public async Task<IUserLogin> FindLogin(
            string loginProvider,
            string providerKey)
        {
            return await repo.FindLogin(loginProvider, providerKey);
        }


        /// <summary>
        /// Deletes an instance of UserLogin. Returns true on success.
        /// </summary>
        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        /// <param name="userId"> userId </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteLogin(
            string loginProvider,
            string providerKey,
            string userId)
        {
            return await repo.DeleteLogin(
                loginProvider,
                providerKey,
                userId);
        }

        public async Task<bool> DeleteLoginsByUser(string userId)
        {
            return await repo.DeleteLoginsByUser(userId);
        }

        public async Task<bool> DeleteLoginsBySite(Guid siteGuid)
        {
            return await repo.DeleteLoginsBySite(siteGuid);
        }




        
        public async Task<IList<IUserLogin>> GetLoginsByUser(string userId)
        {
            if (
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteUserLogins == 0)
                )
            {
                return await repo.GetLoginsByUser(userId);
            }

            // TODO: I don't quite understand how to make this anonymous function async
            //string cachekey = "ulogins-" + userId;

            //DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteUserLogins);

            //try
            //{
            //    IList<IUserLogin> userLogins = CacheManager.Cache.Get<IList<IUserLogin>>(cachekey, expiration, () =>
            //    {
            //        // This is the anonymous function which gets called if the data is not in the cache.
            //        // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
            //        IList<IUserLogin> logins = repo.GetLoginsByUser(userId);
            //        return logins;
            //    });

            //    return userLogins;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("failed to get IList<IUserLogin> from cache so loading them directly", ex);

            //}

            return await repo.GetLoginsByUser(userId);

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
