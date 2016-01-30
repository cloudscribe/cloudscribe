// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2016-01-30
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using cloudscribe.DbHelpers.SQLite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SQLite
{
    //disable warning about not really being async
    // we know it is not, and for Sqlite there is probably no benefit to making it really async
#pragma warning disable 1998

    public sealed class UserRepository : IUserRepository
    {
        public UserRepository(
            SqliteConnectionstringResolver connectionStringResolver,
            ILoggerFactory loggerFactory)
        {
            if (connectionStringResolver == null) { throw new ArgumentNullException(nameof(connectionStringResolver)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }

            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(UserRepository).FullName);
            connectionString = connectionStringResolver.Resolve();

            dbSiteUser = new DBSiteUser(connectionString, logFactory);
            dbUserLogins = new DBUserLogins(connectionString, logFactory);
            dbUserClaims = new DBUserClaims(connectionString, logFactory);
            dbUserLocation = new DBUserLocation(connectionString, logFactory);
            dbRoles = new DBRoles(connectionString, logFactory);
        }

        private ILoggerFactory logFactory;
        private ILogger log;
        private string connectionString;
        private DBSiteUser dbSiteUser;
        private DBUserLogins dbUserLogins;
        private DBUserClaims dbUserClaims;
        private DBUserLocation dbUserLocation;
        private DBRoles dbRoles;

        #region User 

        public async Task<bool> Save(
            ISiteUser user, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user.SiteId == -1) { throw new ArgumentException("user must have a siteid"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }
            cancellationToken.ThrowIfCancellationRequested();

            if (user.UserId == -1)
            {
                user.UserGuid = Guid.NewGuid();
                user.CreatedUtc = DateTime.UtcNow;

                user.UserId = dbSiteUser.AddUser(
                    user.SiteGuid,
                    user.SiteId,
                    user.DisplayName,
                    user.UserName,
                    user.Email,
                    user.UserGuid,
                    user.CreatedUtc,
                    user.MustChangePwd,
                    user.FirstName,
                    user.LastName,
                    user.TimeZoneId,
                    user.DateOfBirth,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEndDateUtc,
                    user.AccountApproved,
                    user.IsLockedOut,
                    user.DisplayInMemberList,
                    user.WebSiteUrl,
                    user.Country,
                    user.State,
                    user.AvatarUrl,
                    user.Signature,
                    user.AuthorBio,
                    user.Comment,
                    user.NormalizedUserName,
                    user.NormalizedEmail,
                    user.CanAutoLockout

                    );

                return user.UserId > -1;
            }
            else
            {
                return Update(user, cancellationToken);
            }

            

        }

        private bool Update(
            ISiteUser user, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
           
            return dbSiteUser.UpdateUser(
                    user.UserId,
                    user.DisplayName,
                    user.UserName,
                    user.Email,
                    user.Gender,
                    user.AccountApproved,
                    user.Trusted,
                    user.DisplayInMemberList,
                    user.WebSiteUrl,
                    user.Country,
                    user.State,
                    user.AvatarUrl,
                    user.Signature,
                    user.NormalizedEmail,
                    user.Comment,
                    user.MustChangePwd,
                    user.FirstName,
                    user.LastName,
                    user.TimeZoneId,
                    user.NewEmail,
                    user.RolesChanged,
                    user.AuthorBio,
                    user.DateOfBirth,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEndDateUtc,
                    user.IsLockedOut,
                    user.NormalizedUserName,
                    user.NewEmailApproved,
                    user.CanAutoLockout,
                    user.LastPasswordChangedDate
                    );

        }

        

        public async Task<bool> Delete(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ISiteUser user = await Fetch(siteId, userId);
            if (user != null)
            {
                bool result = await DeleteLoginsByUser(user.SiteId, user.Id);
                result = await DeleteClaimsByUser(user.SiteId, user.Id);
                result = await DeleteUserRoles(user.UserId);
            }

            return dbSiteUser.DeleteUser(userId);
        }

        public async Task<bool> DeleteUsersBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool result = await DeleteLoginsBySite(siteId);
            result = await DeleteClaimsBySite(siteId);
            result = await DeleteUserRolesBySite(siteId);

            return dbSiteUser.DeleteUsersBySite(siteId);
        }

        public async Task<bool> FlagAsDeleted(
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.FlagAsDeleted(userId);
        }

        public async Task<bool> FlagAsNotDeleted(
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.FlagAsNotDeleted(userId);
        }
        
        public async Task<bool> LockoutAccount(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.AccountLockout(userGuid, DateTime.UtcNow);
        }

        public async Task<bool> UnLockAccount(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.AccountClearLockout(userGuid);
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid, 
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount);
        }

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.UpdateLastLoginTime(userGuid, lastLoginTime);
        }



        public int GetCount(int siteId)
        {
            return dbSiteUser.UserCount(siteId);
        }

        //public int UserCount(int siteId, String userNameBeginsWith)
        //{
        //    if (config.UseRelatedSiteMode()) { siteId = config.RelatedSiteId(); }

        //    return DBSiteUser.UserCount(siteId, userNameBeginsWith);
        //}

        //public int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        //{
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

        //    return dbSiteUser.CountOnlineSince(siteId, sinceTime);
        //}


        //public async Task<ISiteUser> FetchNewest(int siteId)
        //{
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

        //    int newestUserId = await GetNewestUserId(siteId);
        //    return await Fetch(siteId, newestUserId);
        //}

        public async Task<ISiteUser> Fetch(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = dbSiteUser.GetSingleUser(userId))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    user.LoadFromReader(reader);

                    if (user.SiteId == siteId) { return user; }

                }
            }

            return null;
        }


        public async Task<ISiteUser> Fetch(
            int siteId, 
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (DbDataReader reader = dbSiteUser.GetSingleUser(userGuid))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    user.LoadFromReader(reader);

                    if (user.SiteId == siteId) { return user; }

                }
            }

            return null;
        }

        

        public async Task<ISiteUser> Fetch(
            int siteId, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = dbSiteUser.GetSingleUser(siteId, email))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    user.LoadFromReader(reader);

                    return user;

                }
            }

            return null;
        }

        public async Task<ISiteUser> FetchByLoginName(
            int siteId, 
            string userName, 
            bool allowEmailFallback, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = dbSiteUser.GetSingleUserByLoginName(siteId, userName, allowEmailFallback))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    user.LoadFromReader(reader);

                    return user;

                }
            }

            return null;
        }
        
        public async Task<List<IUserInfo>> GetByIPAddress(
            Guid siteGuid, 
            string ipv4Address, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbUserLocation.GetUsersByIPAddress(siteGuid, ipv4Address))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);
                }
            }

            return userList;
        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetCrossSiteUserListByEmail(email))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);
                }
            }

            return userList;
        }

        public async Task<int> CountUsers(
            int siteId, 
            string userNameBeginsWith, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountUsers(siteId, userNameBeginsWith);
        }

        public async Task<List<IUserInfo>> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetUserListPage(
                    siteId,
                    pageNumber,
                    pageSize,
                    userNameBeginsWith,
                    sortMode))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);
                }
            }

            return userList;

        }

        //public async Task<int> CountUsersForSearch(int siteId, string searchInput)
        //{
        //    return dbSiteUser.CountUsersForSearch(siteId, searchInput);
        //}

        //public async Task<List<IUserInfo>> GetUserSearchPage(
        //    int siteId,
        //    int pageNumber,
        //    int pageSize,
        //    string searchInput,
        //    int sortMode)
        //{
        //    //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

        //    List<IUserInfo> userList = new List<IUserInfo>();

        //    using (DbDataReader reader = dbSiteUser.GetUserSearchPage(
        //        siteId,
        //        pageNumber,
        //        pageSize,
        //        searchInput,
        //        sortMode))
        //    {
        //        while (reader.Read())
        //        {
        //            UserInfo user = new UserInfo();
        //            user.LoadFromReader(reader);
        //            userList.Add(user);
        //        }
        //    }

        //    return userList;

        //}

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountUsersForAdminSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode))
            {

                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);

                }
            }

            return userList;


        }

        public async Task<int> CountLockedByAdmin(int siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountLockedOutUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetPageLockedByAdmin(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetPageLockedUsers(
                siteId,
                pageNumber,
                pageSize))
            {

                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);

                }
            }

            return userList;
        }

        public async Task<int> CountNotApprovedUsers(int siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountNotApprovedUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetPageNotApprovedUsers(
                siteId,
                pageNumber,
                pageSize))
            {

                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);

                }
            }

            return userList;
        }

        public async Task<int> CountFutureLockoutEndDate(
            int siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountFutureLockoutDate(siteId);
        }

        public async Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetFutureLockoutPage(
                siteId,
                pageNumber,
                pageSize
                ))
            {

                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);

                }
            }

            return userList;
        }

        public async Task<int> CountUnconfirmedEmail(
            int siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountEmailUnconfirmed(siteId);
        }

        public async Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetPageEmailUnconfirmed(
                siteId,
                pageNumber,
                pageSize
                ))
            {

                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);

                }
            }

            return userList;
        }

        public async Task<int> CountUnconfirmedPhone(
            int siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSiteUser.CountPhoneUnconfirmed(siteId);
        }

        public async Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = dbSiteUser.GetPagePhoneUnconfirmed(
                siteId,
                pageNumber,
                pageSize))
            {

                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    userList.Add(user);

                }
            }

            return userList;
        }

        //public IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        //{
        //    //if (config.UseRelatedSiteMode()) { siteId = config.RelatedSiteId(); }

        //    return DBSiteUser.GetSmartDropDownData(siteId, query, rowsToGet);
        //}

        //public IDataReader EmailLookup(int siteId, string query, int rowsToGet)
        //{
        //    //if (config.UseRelatedSiteMode()) { siteId = config.RelatedSiteId(); }

        //    return DBSiteUser.EmailLookup(siteId, query, rowsToGet);
        //}

        public async Task<bool> EmailExistsInDB(int siteId, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool found = false;
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader r = dbSiteUser.GetSingleUser(siteId, email))
            {
                while (r.Read()) { found = true; }
            }
            return found;
        }

        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool found = false;
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader r = dbSiteUser.GetSingleUser(siteId, email))
            {
                while (r.Read())
                {
                    int foundId = Convert.ToInt32(r["UserID"]);
                    found = (foundId != userId);
                    if (found) { return found; }
                }
            }
            return found;
        }

        public bool LoginExistsInDB(int siteId, string loginName)
        {
            bool found = false;

            using (DbDataReader r = dbSiteUser.GetSingleUserByLoginName(siteId, loginName, false))
            {
                while (r.Read()) { found = true; }
            }

            return found;
        }

        /// <summary>
        /// available only if the found user matches the passed in one
        /// or if not found
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public async Task<bool> LoginIsAvailable(int siteId, int userId, string loginName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool available = true;

            using (DbDataReader r = dbSiteUser.GetSingleUserByLoginName(siteId, loginName, false))
            {
                while (r.Read())
                {
                    int foundId = Convert.ToInt32(r["UserID"]);

                    available = (foundId == userId);
                    if (!available) { return available; }
                }
            }

            return available;
        }

        public async Task<string> GetUserNameFromEmail(int siteId, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string result = string.Empty;
            if ((email != null) && (email.Length > 0) && (siteId > 0))
            {
                string comma = string.Empty;
                using (DbDataReader reader = dbSiteUser.GetSingleUser(siteId, email))
                {
                    while (reader.Read())
                    {
                        result += comma + reader["LoginName"].ToString();
                        comma = ", ";

                    }
                }
            }

            return result;

        }



        //public async Task<int> GetNewestUserId(int siteId)
        //{
        //    return dbSiteUser.GetNewestUserId(siteId);
        //}

        #endregion

        #region Roles

        /// <summary>
        /// Persists a new instance of Role. Returns true on success.
        /// when a role is created displayname corresponds to rolename
        /// but rolename can never change since it is used in a cookies and coded
        /// into security checks in some cases
        /// so subsequent changes to rolename really only effect displayname
        /// ie for localization or customization
        /// to really change a rolename you can delete the role and create a new one with the desired name
        /// some specific required system roles (Admin, Content Administrators) 
        /// are also not allowed to be deleted
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveRole(ISiteRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role.RoleId == -1) // new role
            {
                bool exists = await RoleExists(role.SiteId, role.DisplayName, cancellationToken);
                if (exists)
                {
                    log.LogError("attempt to create a duplicate role "
                        + role.DisplayName + " for site "
                        + role.SiteId.ToString());

                    return false;
                }

                role.RoleGuid = Guid.NewGuid();

                role.RoleId = dbRoles.RoleCreate(
                    role.RoleGuid,
                    role.SiteGuid,
                    role.SiteId,
                    role.DisplayName
                    );

                role.RoleName = role.DisplayName;

                return (role.RoleId > -1);
            }
            else
            {
                return dbRoles.Update(
                    role.RoleId,
                    role.DisplayName);

            }

        }

        public async Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.Delete(roleId);
        }

        public async Task<bool> DeleteRolesBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.DeleteRolesBySite(siteId);
        }

        public async Task<bool> AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.AddUser(roleId, userId, roleGuid, userGuid);
        }

        public async Task<bool> RemoveUserFromRole(int roleId, int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.RemoveUser(roleId, userId);
        }

        //public async Task<bool> AddUserToDefaultRoles(ISiteUser siteUser)
        //{
        //    // moved this to the config setting below instead of hard coded
        //    //IRole role = Fetch(siteUser.SiteId, "Authenticated Users");
        //    //if (role.RoleID > -1)
        //    //{
        //    //    AddUser(role.RoleID, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
        //    //}

        //    ISiteRole role;
        //    bool result = true;
        //    string defaultRoles = AppSettings.DefaultRolesForNewUsers;

        //    if (defaultRoles.Length > 0)
        //    {
        //        if (defaultRoles.IndexOf(";") == -1)
        //        {
        //            role = await FetchRole(siteUser.SiteId, defaultRoles);
        //            if ((role != null) && (role.RoleId > -1))
        //            {
        //                result = await AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
        //            }
        //        }
        //        else
        //        {
        //            string[] roleArray = defaultRoles.Split(';');
        //            foreach (string roleName in roleArray)
        //            {
        //                if (!string.IsNullOrEmpty(roleName))
        //                {
        //                    role = await FetchRole(siteUser.SiteId, roleName);
        //                    if ((role != null) && (role.RoleId > -1))
        //                    {
        //                        result = result && await AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
        //                    }
        //                }
        //            }

        //        }

        //    }

        //    return result;
        //}

        public async Task<bool> DeleteUserRoles(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.DeleteUserRoles(userId);
        }

        public async Task<bool> DeleteUserRolesByRole(int roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.DeleteUserRolesByRole(roleId);
        }

        public async Task<bool> DeleteUserRolesBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.DeleteUserRolesBySite(siteId);
        }

        public async Task<bool> RoleExists(int siteId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.Exists(siteId, roleName);
        }

        public int GetRoleMemberCount(int roleId)
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            int count = 0;
            using (DbDataReader reader = dbRoles.GetRoleMembers(roleId))
            {
                while (reader.Read())
                {
                    count += 1;
                }
            }

            return count;

        }

        public async Task<ISiteRole> FetchRole(int roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = dbRoles.GetById(roleId))
            {
                if (reader.Read())
                {
                    SiteRole role = new SiteRole();
                    role.LoadFromReader(reader);
                    return role;
                }
            }

            return null;
        }

        public async Task<ISiteRole> FetchRole(int siteId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            SiteRole role = null;

            using (DbDataReader reader = dbRoles.GetByName(siteId, roleName))
            {
                if (reader.Read())
                {
                    role = new SiteRole();
                    role.LoadFromReader(reader);
                }
            }

            return role;

        }

        public async Task<List<string>> GetUserRoles(int siteId, int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> userRoles = new List<string>();
            using (DbDataReader reader = dbSiteUser.GetRolesByUser(siteId, userId))
            {
                while (reader.Read())
                {
                    userRoles.Add(reader["RoleName"].ToString());
                }

            }

            return userRoles;
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IList<ISiteRole> roles = new List<ISiteRole>();
            using (DbDataReader reader = dbRoles.GetPage(siteId, searchInput, pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    SiteRole role = new SiteRole();
                    role.LoadFromReader(reader);
                    role.MemberCount = Convert.ToInt32(reader["MemberCount"]);

                    roles.Add(role);
                }
            }

            return roles;

        }

        //public IList<ISiteRole> GetRolesUserIsNotIn(
        //    int siteId,
        //    int userId)
        //{
        //    IList<ISiteRole> roles = new List<ISiteRole>();
        //    using (DbDataReader reader = dbRoles.GetRolesUserIsNotIn(siteId, userId))
        //    {
        //        SiteRole role = new SiteRole();
        //        role.LoadFromReader(reader);

        //        roles.Add(role);
        //    }
        //    return roles;
        //}

        //public async Task<List<int>> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        //{
        //    List<int> roleIds = new List<int>();

        //    List<string> roleNames = GetRolesNames(roleNamesSeparatedBySemiColons);

        //    foreach (string roleName in roleNames)
        //    {
        //        if (string.IsNullOrEmpty(roleName)) { continue; }
        //        ISiteRole r = await FetchRole(siteId, roleName);
        //        if (r == null)
        //        {
        //            log.LogDebug("could not get roleid for role named " + roleName);
        //            continue;
        //        }
        //        if (r.RoleId > -1) { roleIds.Add(r.RoleId); }
        //    }

        //    return roleIds;
        //}

        //public static List<string> GetRolesNames(string roleNamesSeparatedBySemiColons)
        //{
        //    List<string> roleNames = new List<string>();
        //    string[] roles = roleNamesSeparatedBySemiColons.Split(';');
        //    foreach (string r in roles)
        //    {
        //        if (!roleNames.Contains(r)) { roleNames.Add(r); }
        //    }

        //    return roleNames;

        //}


        public async Task<int> CountOfRoles(int siteId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.GetCountOfSiteRoles(siteId, searchInput);
        }

        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.GetCountOfUsersInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IList<IUserInfo> users = new List<IUserInfo>();

            using (DbDataReader reader = dbRoles.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    users.Add(user);

                }

            }

            return users;
        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            int siteId,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IList<ISiteUser> users = new List<ISiteUser>();

            ISiteRole role = await FetchRole(siteId, roleName);
            int roleId = -3;
            if (role != null)
            {
                roleId = role.RoleId;
            }

            using (DbDataReader reader = dbRoles.GetUsersInRole(siteId, roleId, string.Empty, 1, 100000))
            {
                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    user.LoadFromReader(reader);
                    users.Add(user);

                }

            }

            return users;
        }

        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbRoles.GetCountOfUsersNotInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            IList<IUserInfo> users = new List<IUserInfo>();

            using (DbDataReader reader = dbRoles.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.LoadFromReader(reader);
                    users.Add(user);

                }

            }

            return users;
        }


        #endregion

        #region Claims

        /// <summary>
        /// Persists a new instance of UserClaim. Returns true on success.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveClaim(IUserClaim userClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            int newId = dbUserClaims.Create(
                userClaim.SiteId,
                userClaim.UserId,
                userClaim.ClaimType,
                userClaim.ClaimValue);

            userClaim.Id = newId;

            return (newId > -1);
        }

        public async Task<bool> DeleteClaim(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserClaims.Delete(id);
        }

        public async Task<bool> DeleteClaimsByUser(int siteId, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserClaims.DeleteByUser(siteId, userId);
        }

        public async Task<bool> DeleteClaimByUser(int siteId, string userId, string claimType, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserClaims.DeleteByUser(siteId, userId, claimType);
        }

        public async Task<bool> DeleteClaimsBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserClaims.DeleteBySite(siteId);
        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(int siteId, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            DbDataReader reader = dbUserClaims.GetByUser(siteId, userId);
            return LoadClaimListFromReader(reader);

        }

        public async Task<IList<ISiteUser>> GetUsersForClaim(
            int siteId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IList<ISiteUser> users = new List<ISiteUser>();

            using (DbDataReader reader = dbUserClaims.GetUsersByClaim(siteId, claimType, claimValue))
            {
                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    user.LoadFromReader(reader);
                    users.Add(user);

                }

            }

            return users;
        }


        private List<IUserClaim> LoadClaimListFromReader(DbDataReader reader)
        {
            List<IUserClaim> userClaimList = new List<IUserClaim>();

            using(reader)
            {
                while (reader.Read())
                {
                    UserClaim userClaim = new UserClaim();
                    userClaim.LoadFromReader(reader);
                    userClaimList.Add(userClaim);

                }
            }
            
            return userClaimList;

        }


        #endregion

        #region Logins

        /// <summary>
        /// Persists a new instance of UserLogin. Returns true on success.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateLogin(IUserLogin userLogin, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userLogin.LoginProvider.Length == -1) { return false; }
            if (userLogin.ProviderKey.Length == -1) { return false; }
            if (userLogin.UserId.Length == -1) { return false; }
            cancellationToken.ThrowIfCancellationRequested();

            return dbUserLogins.Create(
                userLogin.SiteId,
                userLogin.LoginProvider,
                userLogin.ProviderKey,
                userLogin.ProviderDisplayName,
                userLogin.UserId);


        }


        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        public async Task<IUserLogin> FindLogin(
            int siteId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = dbUserLogins.Find(
                siteId,
                loginProvider,
                providerKey))
            {
                if (reader.Read())
                {
                    UserLogin userLogin = new UserLogin();
                    userLogin.LoadFromReader(reader);
                    return userLogin;

                }
            }

            return null;
        }


        /// <summary>
        /// Deletes an instance of UserLogin. Returns true on success.
        /// </summary>
        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        /// <param name="userId"> userId </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteLogin(
            int siteId,
            string loginProvider,
            string providerKey,
            string userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserLogins.Delete(
                siteId,
                loginProvider,
                providerKey,
                userId);
        }

        public async Task<bool> DeleteLoginsByUser(int siteId, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserLogins.DeleteByUser(siteId, userId);
        }

        public async Task<bool> DeleteLoginsBySite(int siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserLogins.DeleteBySite(siteId);
        }




        /// <summary>
        /// Gets an IList with all instances of UserLogin.
        /// </summary>
        public async Task<IList<IUserLogin>> GetLoginsByUser(
            int siteId,
            string userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserLogin> userLoginList = new List<IUserLogin>();
            using (DbDataReader reader = dbUserLogins.GetByUser(siteId, userId))
            {
                while (reader.Read())
                {
                    UserLogin userLogin = new UserLogin();
                    userLogin.LoadFromReader(reader);
                    userLoginList.Add(userLogin);

                }
            }

            return userLoginList;

        }




        #endregion

        #region UserLocation

        public async Task<IUserLocation> FetchByUserAndIpv4Address(
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = dbUserLocation.GetOne(
                userGuid,
                ipv4AddressAsLong)
                )
            {
                if (reader.Read())
                {
                    UserLocation userLocation = new UserLocation();
                    userLocation.LoadFromReader(reader);
                    return userLocation;
                }
            }

            return null;

        }

        public async Task<bool> AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return dbUserLocation.Create(
                userLocation.RowId,
                userLocation.UserGuid,
                userLocation.SiteGuid,
                userLocation.IpAddress,
                userLocation.IpAddressLong,
                userLocation.HostName,
                userLocation.Longitude,
                userLocation.Latitude,
                userLocation.Isp,
                userLocation.Continent,
                userLocation.Country,
                userLocation.Region,
                userLocation.City,
                userLocation.TimeZone,
                userLocation.CaptureCount,
                userLocation.FirstCaptureUtc,
                userLocation.LastCaptureUtc

                );

        }

        public async Task<bool> UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return dbUserLocation.Update(
                userLocation.RowId,
                userLocation.UserGuid,
                userLocation.SiteGuid,
                userLocation.IpAddress,
                userLocation.IpAddressLong,
                userLocation.HostName,
                userLocation.Longitude,
                userLocation.Latitude,
                userLocation.Isp,
                userLocation.Continent,
                userLocation.Country,
                userLocation.Region,
                userLocation.City,
                userLocation.TimeZone,
                userLocation.CaptureCount,
                userLocation.LastCaptureUtc
                
                );

        }

        public async Task<bool> DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserLocation.Delete(rowGuid);

        }

        public async Task<bool> DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserLocation.DeleteByUser(userGuid);

        }

        public async Task<bool> DeleteUserLocationsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbUserLocation.DeleteBySite(siteGuid);

        }

        public async Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return  dbUserLocation.GetCountByUser(userGuid);
        }

        public async Task<IList<IUserLocation>> GetUserLocationsByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<IUserLocation> userLocationList = new List<IUserLocation>();
            using (DbDataReader reader = dbUserLocation.GetPageByUser(
                userGuid,
                pageNumber,
                pageSize)
                )
            {
                while (reader.Read())
                {
                    UserLocation userLocation = new UserLocation();
                    userLocation.LoadFromReader(reader);
                    userLocationList.Add(userLocation);

                }
            }

            return userLocationList;

        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SiteRoleStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }

#pragma warning restore 1998

}
