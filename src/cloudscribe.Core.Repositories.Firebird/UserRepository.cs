// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2016-01-29
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using cloudscribe.DbHelpers;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
{
    public sealed class UserRepository : IUserRepository
    {
        public UserRepository(
            IOptions<ConnectionStringOptions> configuration,
            ILoggerFactory loggerFactory)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(UserRepository).FullName);

            readConnectionString = configuration.Value.ReadConnectionString;
            writeConnectionString = configuration.Value.WriteConnectionString;

            dbSiteUser = new DBSiteUser(readConnectionString, writeConnectionString, logFactory);
            dbUserLogins = new DBUserLogins(readConnectionString, writeConnectionString, logFactory);
            dbUserClaims = new DBUserClaims(readConnectionString, writeConnectionString, logFactory);
            dbUserLocation = new DBUserLocation(readConnectionString, writeConnectionString, logFactory);
            dbRoles = new DBRoles(readConnectionString, writeConnectionString, logFactory);
        }

        private ILoggerFactory logFactory;
        private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;
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

                user.UserId = await dbSiteUser.AddUser(
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
                    user.CanAutoLockout,
                    cancellationToken
                    );

                return user.UserId > -1;

            }
            else
            {
                return await Update(user, cancellationToken);
            }

            

        }

        private async Task<bool> Update(
            ISiteUser user, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            return await dbSiteUser.UpdateUser(
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
                    user.LastPasswordChangedDate,
                    cancellationToken
                    );

            
        }

        //public async Task<bool> Delete(ISiteUser user)
        //{
        //    bool result = await DeleteLoginsByUser(user.SiteId, user.Id);
        //    result = await DeleteClaimsByUser(user.SiteId, user.Id);
        //    result = await DeleteUserRoles(user.UserId);
        //    result = await dbSiteUser.DeleteUser(user.UserId);

        //    return result;
        //}


        
        public async Task<bool> Delete(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ISiteUser user = await Fetch(siteId, userId);
            if(user != null)
            {
                bool result = await DeleteLoginsByUser(user.SiteId, user.Id);
                result = await DeleteClaimsByUser(user.SiteId, user.Id);
                result = await DeleteUserRoles(user.UserId);
            }
            return await dbSiteUser.DeleteUser(userId, cancellationToken);
        }

        public async Task<bool> DeleteUsersBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool result = await DeleteLoginsBySite(siteId);
            result = await DeleteClaimsBySite(siteId);
            result = await DeleteUserRolesBySite(siteId);
            
            return await dbSiteUser.DeleteUsersBySite(siteId, cancellationToken);
        }

        public async Task<bool> FlagAsDeleted(
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.FlagAsDeleted(userId, cancellationToken);
        }

        public async Task<bool> FlagAsNotDeleted(
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.FlagAsNotDeleted(userId, cancellationToken);
        }

        //public async Task<bool> SetRegistrationConfirmationGuid(
        //    Guid userGuid,
        //    Guid registrationConfirmationGuid,
        //    CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();
        //    if (registrationConfirmationGuid == Guid.Empty)
        //    {
        //        return false;
        //    }

        //    return await dbSiteUser.SetRegistrationConfirmationGuid(userGuid, registrationConfirmationGuid, cancellationToken);
        //}

        //public async Task<bool> ConfirmRegistration(
        //    Guid registrationGuid, 
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    if (registrationGuid == Guid.Empty)
        //    {
        //        return false;
        //    }

        //    return await dbSiteUser.ConfirmRegistration(Guid.Empty, registrationGuid, cancellationToken);
        //}


        public async Task<bool> LockoutAccount(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.AccountLockout(userGuid, DateTime.UtcNow, cancellationToken);
        }

        public async Task<bool> UnLockAccount(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.AccountClearLockout(userGuid, cancellationToken);
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid, 
            int failedPasswordAttemptCount, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.UpdateFailedPasswordAttemptCount(
                userGuid, 
                failedPasswordAttemptCount,
                cancellationToken);
        }

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.UpdateLastLoginTime(userGuid, lastLoginTime, cancellationToken);
        }


        //public DataTable GetUserListForPasswordFormatChange(int siteId)
        //{
        //    if (config.UseRelatedSiteMode()) { siteId = config.RelatedSiteId(); }

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("UserID", typeof(int));
        //    dt.Columns.Add("PasswordSalt", typeof(String));
        //    dt.Columns.Add("Pwd", typeof(String));

        //    using (DbDataReader reader = dbSiteUser.GetUserList(siteId))
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dt.NewRow();
        //            row["UserID"] = reader["UserID"];
        //            row["PasswordSalt"] = reader["PasswordSalt"];
        //            row["Pwd"] = reader["Pwd"];
        //            dt.Rows.Add(row);

        //        }
        //    }

        //    return dt;
        //}

        public int GetCount(int siteId)
        {
            return dbSiteUser.UserCount(siteId);
        }

        //public int UserCount(int siteId, String userNameBeginsWith)
        //{
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }
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
            using (DbDataReader reader = await dbSiteUser.GetSingleUser(
                userId,
                cancellationToken))
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

            using (DbDataReader reader = await dbSiteUser.GetSingleUser(
                userGuid,
                cancellationToken))
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
            using (DbDataReader reader = await dbSiteUser.GetSingleUser(
                siteId, 
                email,
                cancellationToken))
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
            using (DbDataReader reader = await dbSiteUser.GetSingleUserByLoginName(
                siteId, 
                userName, 
                allowEmailFallback,
                cancellationToken))
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

            using (DbDataReader reader = await dbUserLocation.GetUsersByIPAddress(
                siteGuid, 
                ipv4Address,
                cancellationToken))
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

            using (DbDataReader reader = await dbSiteUser.GetCrossSiteUserListByEmail(
                email,
                cancellationToken))
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
            return await dbSiteUser.CountUsers(siteId, userNameBeginsWith, cancellationToken);
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

            using (DbDataReader reader
                = await dbSiteUser.GetUserListPage(
                    siteId, 
                    pageNumber, 
                    pageSize, 
                    userNameBeginsWith, 
                    sortMode,
                    cancellationToken))
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
        //    return await dbSiteUser.CountUsersForSearch(siteId, searchInput);
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

        //    using (DbDataReader reader = await dbSiteUser.GetUserSearchPage(
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

        public async Task<int> CountUsersForAdminSearch(
            int siteId, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.CountUsersForAdminSearch(siteId, searchInput, cancellationToken);
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

            using (DbDataReader reader = await dbSiteUser.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode,
                cancellationToken))
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

        public async Task<int> CountLockedByAdmin(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.CountLockedOutUsers(siteId, cancellationToken);
        }

        public async Task<List<IUserInfo>> GetPageLockedByAdmin(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = await dbSiteUser.GetPageLockedUsers(
                siteId,
                pageNumber,
                pageSize,
                cancellationToken))
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
            return await dbSiteUser.CountFutureLockoutDate(siteId, cancellationToken);
        }

        public async Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = await dbSiteUser.GetFutureLockoutPage(
                siteId,
                pageNumber,
                pageSize,
                cancellationToken))
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
            return await dbSiteUser.CountEmailUnconfirmed(siteId, cancellationToken);
        }

        public async Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = await dbSiteUser.GetPageEmailUnconfirmed(
                siteId,
                pageNumber,
                pageSize,
                cancellationToken))
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
            return await dbSiteUser.CountPhoneUnconfirmed(siteId, cancellationToken);
        }

        public async Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = await dbSiteUser.GetPagePhoneUnconfirmed(
                siteId,
                pageNumber,
                pageSize,
                cancellationToken))
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

        public async Task<int> CountNotApprovedUsers(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteUser.CountNotApprovedUsers(siteId, cancellationToken);
        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = await dbSiteUser.GetPageNotApprovedUsers(
                siteId,
                pageNumber,
                pageSize,
                cancellationToken))
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
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

        //    return DBSiteUser.GetSmartDropDownData(siteId, query, rowsToGet);
        //}

        //public IDataReader EmailLookup(int siteId, string query, int rowsToGet)
        //{
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

        //    return DBSiteUser.EmailLookup(siteId, query, rowsToGet);
        //}

        public async Task<bool> EmailExistsInDB(
            int siteId, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            bool found = false;
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader r = await dbSiteUser.GetSingleUser(
                siteId, 
                email,
                cancellationToken))
            {
                while (r.Read()) { found = true; }
            }
            return found;
        }

        public async Task<bool> EmailExistsInDB(
            int siteId, 
            int userId, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            bool found = false;
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader r = await dbSiteUser.GetSingleUser(
                siteId, 
                email,
                cancellationToken))
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

            using (DbDataReader r = dbSiteUser.GetSingleUserByLoginNameNonAsync(siteId, loginName, false))
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
        public async Task<bool> LoginIsAvailable(
            int siteId, 
            int userId, 
            string loginName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool available = true;

            using (DbDataReader r = await dbSiteUser.GetSingleUserByLoginName(
                siteId, 
                loginName, 
                false,
                cancellationToken))
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

        public async Task<string> GetUserNameFromEmail(
            int siteId, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string result = string.Empty;
            if ((email != null) && (email.Length > 0) && (siteId > 0))
            {
                string comma = string.Empty;
                using (DbDataReader reader = await dbSiteUser.GetSingleUser(
                    siteId, 
                    email,
                    cancellationToken))
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
        //    return await dbSiteUser.GetNewestUserId(siteId);
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
        public async Task<bool> SaveRole(
            ISiteRole role, 
            CancellationToken cancellationToken = default(CancellationToken))
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

                role.RoleId = await dbRoles.RoleCreate(
                    role.RoleGuid,
                    role.SiteGuid,
                    role.SiteId,
                    role.DisplayName,
                    cancellationToken
                    );

                role.RoleName = role.DisplayName;

                return (role.RoleId > -1);
            }
            else
            {
                return await dbRoles.Update(
                    role.RoleId,
                    role.DisplayName,
                    cancellationToken);

            }

        }


        public async Task<bool> DeleteRole(
            int roleId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.Delete(roleId, cancellationToken);
        }

        public async Task<bool> DeleteRolesBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.DeleteRolesBySite(siteId, cancellationToken);
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
            return await dbRoles.AddUser(roleId, userId, roleGuid, userGuid, cancellationToken);
        }

        public async Task<bool> RemoveUserFromRole(
            int roleId, 
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.RemoveUser(roleId, userId, cancellationToken);
        }

        //public async Task<bool> AddUserToDefaultRoles(ISiteUser siteUser)
        //{

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

        public async Task<bool> DeleteUserRoles(
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.DeleteUserRoles(userId, cancellationToken);
        }

        public async Task<bool> DeleteUserRolesByRole(
            int roleId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.DeleteUserRolesByRole(roleId, cancellationToken);
        }

        public async Task<bool> DeleteUserRolesBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.DeleteUserRolesBySite(siteId, cancellationToken);
        }


        public async Task<bool> RoleExists(
            int siteId, 
            string roleName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.Exists(siteId, roleName, cancellationToken);
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

        public async Task<ISiteRole> FetchRole(
            int roleId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = await dbRoles.GetById(
                roleId,
                cancellationToken))
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

        public async Task<ISiteRole> FetchRole(
            int siteId, 
            string roleName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            SiteRole role = null;
            using (DbDataReader reader = await dbRoles.GetByName(
                siteId, 
                roleName,
                cancellationToken))
            {
                if (reader.Read())
                {
                    role = new SiteRole();
                    role.LoadFromReader(reader);
                }
            }

            return role;

        }

        public async Task<List<string>> GetUserRoles(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> userRoles = new List<string>();
            using (DbDataReader reader = await dbSiteUser.GetRolesByUser(
                siteId, 
                userId,
                cancellationToken))
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
            using (DbDataReader reader = await dbRoles.GetPage(
                siteId, 
                searchInput, 
                pageNumber, 
                pageSize,
                cancellationToken))
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
        //   IList<ISiteRole> roles = new List<ISiteRole>();
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


        public async Task<int> CountOfRoles(
            int siteId, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.GetCountOfSiteRoles(siteId, searchInput, cancellationToken);
        }

        public async Task<int> CountUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.GetCountOfUsersInRole(siteId, roleId, searchInput, cancellationToken);
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

            using (DbDataReader reader = await dbRoles.GetUsersInRole(
                siteId, 
                roleId, 
                string.Empty, 
                1, 
                100000,
                cancellationToken))
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

            using (DbDataReader reader = await dbRoles.GetUsersInRole(
                siteId, 
                roleId, 
                searchInput, 
                pageNumber, 
                pageSize,
                cancellationToken))
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

        public async Task<int> CountUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbRoles.GetCountOfUsersNotInRole(
                siteId, 
                roleId, 
                searchInput,
                cancellationToken);
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

            using (DbDataReader reader = await dbRoles.GetUsersNotInRole(
                siteId, 
                roleId, 
                searchInput, 
                pageNumber, 
                pageSize,
                cancellationToken))
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
        public async Task<bool> SaveClaim(
            IUserClaim userClaim, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            int newId = await dbUserClaims.Create(
                userClaim.SiteId,
                userClaim.UserId,
                userClaim.ClaimType,
                userClaim.ClaimValue,
                cancellationToken);

            userClaim.Id = newId;

            return (newId > -1);


        }

        public async Task<bool> DeleteClaim(
            int id, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserClaims.Delete(id, cancellationToken);
        }

        public async Task<bool> DeleteClaimsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserClaims.DeleteByUser(siteId, userId, cancellationToken);
        }

        public async Task<bool> DeleteClaimByUser(
            int siteId, 
            string userId, 
            string claimType, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserClaims.DeleteByUser(siteId, userId, claimType, cancellationToken);
        }

        public async Task<bool> DeleteClaimsBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserClaims.DeleteBySite(siteId, cancellationToken);
        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            DbDataReader reader = await dbUserClaims.GetByUser(siteId, userId, cancellationToken);
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

            using (DbDataReader reader = await dbUserClaims.GetUsersByClaim(
                siteId, 
                claimType, 
                claimValue,
                cancellationToken))
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
        public async Task<bool> CreateLogin(
            IUserLogin userLogin, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userLogin.LoginProvider.Length == -1) { return false; }
            if (userLogin.ProviderKey.Length == -1) { return false; }
            if (userLogin.UserId.Length == -1) { return false; }
            cancellationToken.ThrowIfCancellationRequested();

            return await dbUserLogins.Create(
                userLogin.SiteId,
                userLogin.LoginProvider,
                userLogin.ProviderKey,
                userLogin.ProviderDisplayName,
                userLogin.UserId,
                cancellationToken);


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
            using (DbDataReader reader = await dbUserLogins.Find(
                siteId,
                loginProvider,
                providerKey,
                cancellationToken))
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
            return await dbUserLogins.Delete(
                siteId,
                loginProvider,
                providerKey,
                userId,
                cancellationToken);
        }

        public async Task<bool> DeleteLoginsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserLogins.DeleteByUser(siteId, userId, cancellationToken);
        }

        public async Task<bool> DeleteLoginsBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserLogins.DeleteBySite(siteId, cancellationToken);
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
            using (DbDataReader reader = await dbUserLogins.GetByUser(
                siteId, 
                userId,
                cancellationToken))
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
            using (DbDataReader reader = await dbUserLocation.GetOne(
                userGuid,
                ipv4AddressAsLong,
                cancellationToken)
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

            return await dbUserLocation.Create(
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
                userLocation.LastCaptureUtc,
                cancellationToken
                );

        }

        public async Task<bool> UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await dbUserLocation.Update(
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
                userLocation.LastCaptureUtc,
                cancellationToken
                );

        }

        public async Task<bool> DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserLocation.Delete(
                rowGuid,
                cancellationToken);

        }

        public async Task<bool> DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserLocation.DeleteByUser(
                userGuid,
                cancellationToken);

        }

        public async Task<bool> DeleteUserLocationsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserLocation.DeleteBySite(
                siteGuid,
                cancellationToken);

        }

        public async Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbUserLocation.GetCountByUser(
                userGuid,
                cancellationToken);
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
            using (DbDataReader reader = await dbUserLocation.GetPageByUser(
                userGuid,
                pageNumber,
                pageSize,
                cancellationToken)
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
}
