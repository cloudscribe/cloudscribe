// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2015-01-15
// 


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SqlCe
{

    //disable warning about not really being async
    // we know it is not, and for SqlCe there is probably no benefit to making it really async
#pragma warning disable 1998

    public sealed class UserRepository : IUserRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository()
        { }

        #region User 

        public async Task<bool> Save(ISiteUser user)
        {
            if (user.SiteId == -1) { throw new ArgumentException("user must have a siteid"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }

            if (user.UserId == -1)
            {
                user.UserGuid = Guid.NewGuid();
                user.CreatedUtc = DateTime.UtcNow;

                user.UserId = DBSiteUser.AddUser(
                    user.SiteGuid,
                    user.SiteId,
                    user.DisplayName,
                    user.UserName,
                    user.Email,
                    user.Password,
                    user.PasswordSalt,
                    user.UserGuid,
                    user.CreatedUtc,
                    user.MustChangePwd,
                    user.FirstName,
                    user.LastName,
                    user.TimeZoneId,
                    user.DateOfBirth,
                    user.EmailConfirmed,
                    user.PasswordFormat,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEndDateUtc);

                
                    //user.LoweredEmail,
                    //user.PasswordQuestion,
                    //user.PasswordAnswer,
                    //user.Gender,
                    //user.ProfileApproved,
                    //user.RegisterConfirmGuid,
                    //user.ApprovedForForums,
                    //user.Trusted,
                    //user.DisplayInMemberList,
                    //user.WebSiteURL,
                    //user.Country,
                    //user.State,
                    //user.Occupation,
                    //user.Interests,
                    //user.MSN,
                    //user.Yahoo,
                    //user.AIM,
                    //user.ICQ,
                    //user.TotalPosts,
                    //user.AvatarUrl,
                    //user.TimeOffsetHours,
                    //user.Signature,
                    //user.Skin,
                    //user.IsDeleted,
                    //user.LastActivityDate,
                    //user.LastLoginDate,
                    //user.LastPasswordChangedDate,
                    //user.LastLockoutDate,
                    //user.FailedPasswordAttemptCount,
                    //user.FailedPwdAttemptWindowStart,
                    //user.FailedPwdAnswerAttemptCount,
                    //user.FailedPwdAnswerWindowStart,
                    //user.IsLockedOut,
                    //user.MobilePIN,    
                    //user.Comment,
                    //user.OpenIDURI,
                    //user.WindowsLiveID,    
                    //user.TotalRevenue,
                    //user.NewEmail,
                    //user.EditorPreference,
                    //user.EmailChangeGuid,
                    //user.PasswordResetGuid,
                    //user.RolesChanged,
                    //user.AuthorBio 
                    //);

               // user.UserID = newId;
            }

            // not all properties are added on insert so update even if we just inserted

            return Update(user);

        }

        private bool Update(ISiteUser user)
        {
            if (string.IsNullOrEmpty(user.LoweredEmail)) { user.LoweredEmail = user.Email.ToLowerInvariant(); }

            return DBSiteUser.UpdateUser(
                    user.UserId,
                    user.DisplayName,
                    user.UserName,
                    user.Email,
                    user.Password,
                    user.PasswordSalt,
                    user.Gender,
                    user.ProfileApproved,
                    user.ApprovedForLogin,
                    user.Trusted,
                    user.DisplayInMemberList,
                    user.WebSiteUrl,
                    user.Country,
                    user.State,
                    string.Empty, // legacy user.Occupation,
                    string.Empty, // legacy user.Interests,
                    string.Empty, // legacy user.MSN,
                    string.Empty, // legacy user.Yahoo,
                    string.Empty, // legacyuser.AIM,
                    string.Empty, // legacy user.ICQ,
                    user.AvatarUrl,
                    user.Signature,
                    user.Skin,
                    user.LoweredEmail,
                    user.PasswordQuestion,
                    user.PasswordAnswer,
                    user.Comment,
                    0, // legacy timeOffsetHours
                    user.OpenIdUri,
                    string.Empty, // legacy user.WindowsLiveId,
                    user.MustChangePwd,
                    user.FirstName,
                    user.LastName,
                    user.TimeZoneId,
                    user.EditorPreference,
                    user.NewEmail,
                    user.EmailChangeGuid,
                    user.PasswordResetGuid,
                    user.RolesChanged,
                    user.AuthorBio,
                    user.DateOfBirth,
                    user.EmailConfirmed,
                    user.PasswordFormat,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEndDateUtc);


            //user.RegisterConfirmGuid,
            //user.TotalPosts,
            //user.TimeOffsetHours,
            //user.DateCreated,
            //user.UserGuid,
            //user.IsDeleted,
            //user.LastActivityDate,
            //user.LastLoginDate,
            //user.LastPasswordChangedDate,
            //user.LastLockoutDate,
            //user.FailedPasswordAttemptCount,
            //user.FailedPwdAttemptWindowStart,
            //user.FailedPwdAnswerAttemptCount,
            //user.FailedPwdAnswerWindowStart,
            //user.IsLockedOut,
            //user.MobilePIN,
            //user.SiteGuid,
            //user.TotalRevenue,

        }

        


        /// <summary>
        /// Deletes an instance of User. Returns true on success.
        /// </summary>
        /// <param name="userID"> userID </param>
        /// <returns>bool</returns>
        public async Task<bool> Delete(int userId)
        {
            return DBSiteUser.DeleteUser(userId);
        }

        public async Task<bool> FlagAsDeleted(int userId)
        {
            return DBSiteUser.FlagAsDeleted(userId);
        }

        public async Task<bool> FlagAsNotDeleted(int userId)
        {
            return DBSiteUser.FlagAsNotDeleted(userId);
        }

        public bool UpdatePasswordAndSalt(
            int userId,
            int passwordFormat,
            string password,
            string passwordSalt)
        {
            return DBSiteUser.UpdatePasswordAndSalt(userId, passwordFormat, password, passwordSalt);
        }

        public async Task<bool> ConfirmRegistration(Guid registrationGuid)
        {
            if (registrationGuid == Guid.Empty)
            {
                return false;
            }

            return DBSiteUser.ConfirmRegistration(Guid.Empty, registrationGuid);
        }


        public async Task<bool> LockoutAccount(Guid userGuid)
        {
            return DBSiteUser.AccountLockout(userGuid, DateTime.UtcNow);
        }

        public async Task<bool> UnLockAccount(Guid userGuid)
        {
            return DBSiteUser.AccountClearLockout(userGuid);
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            return DBSiteUser.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount);
        }

        public async Task<bool> UpdateTotalRevenue(Guid userGuid)
        {
            DBSiteUser.UpdateTotalRevenue(userGuid);
            return true;

        }

        /// <summary>
        /// updates the total revenue for all users
        /// </summary>
        public async Task<bool> UpdateTotalRevenue()
        {
            DBSiteUser.UpdateTotalRevenue();
            return true;
        }


        public DataTable GetUserListForPasswordFormatChange(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("PasswordSalt", typeof(String));
            dt.Columns.Add("Pwd", typeof(String));

            using (IDataReader reader = DBSiteUser.GetUserList(siteId))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["UserID"] = reader["UserID"];
                    row["PasswordSalt"] = reader["PasswordSalt"];
                    row["Pwd"] = reader["Pwd"];
                    dt.Rows.Add(row);

                }
            }

            return dt;
        }

        public int GetCount(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.UserCount(siteId);
        }

        //public int UserCount(int siteId, String userNameBeginsWith)
        //{
        //    //if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
        //    return DBSiteUser.UserCount(siteId, userNameBeginsWith);
        //}

        public int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.CountOnlineSince(siteId, sinceTime);
        }


        public async Task<ISiteUser> FetchNewest(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            int newestUserId = await GetNewestUserId(siteId);
            return await Fetch(siteId, newestUserId);
        }

        public async Task<ISiteUser> Fetch(int siteId, int userId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetSingleUser(userId))
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


        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetSingleUser(userGuid))
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

        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetUserByRegistrationGuid(siteId, confirmGuid))
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


        public async Task<ISiteUser> Fetch(int siteId, string email)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetSingleUser(siteId, email))
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

        public async Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetSingleUserByLoginName(siteId, userName, allowEmailFallback))
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





        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            List<IUserInfo> userList = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteGuid = Guid.Empty; }

            using (IDataReader reader = DBUserLocation.GetUsersByIPAddress(siteGuid, ipv4Address))
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

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email)
        {
            List<IUserInfo> userList = new List<IUserInfo>();

            using (IDataReader reader = DBSiteUser.GetCrossSiteUserListByEmail(email))
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

        public async Task<int> CountUsers(int siteId, String userNameBeginsWith)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.CountUsers(siteId, userNameBeginsWith);
        }

        public async Task<List<IUserInfo>> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = DBSiteUser.GetUserListPage(
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
                    //totalPages = Convert.ToInt32(reader["TotalPages"]);
                }
            }

            return userList;

        }

        public async Task<int> CountUsersForSearch(int siteId, string searchInput)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.CountUsersForSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = DBSiteUser.GetUserSearchPage(
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

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.CountUsersForAdminSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = DBSiteUser.GetUserAdminSearchPage(
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

        public async Task<int> CountLockedOutUsers(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            return DBSiteUser.CountLockedOutUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = DBSiteUser.GetPageLockedUsers(
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

        public async Task<int> CountNotApprovedUsers(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.CountNotApprovedUsers(siteId);
        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            List<IUserInfo> userList = new List<IUserInfo>();

            using (DbDataReader reader = DBSiteUser.GetPageNotApprovedUsers(
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
        //    //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

        //    return DBSiteUser.GetSmartDropDownData(siteId, query, rowsToGet);
        //}

        //public IDataReader EmailLookup(int siteId, string query, int rowsToGet)
        //{
        //    //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

        //    return DBSiteUser.EmailLookup(siteId, query, rowsToGet);
        //}

        public async Task<bool> EmailExistsInDB(int siteId, string email)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUser(siteId, email))
            {
                while (r.Read()) { found = true; }
            }
            return found;
        }

        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUser(siteId, email))
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
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUserByLoginName(siteId, loginName, false))
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
        public bool LoginIsAvailable(int siteId, int userId, string loginName)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            bool available = true;

            using (IDataReader r = DBSiteUser.GetSingleUserByLoginName(siteId, loginName, false))
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

        public async Task<string> GetUserNameFromEmail(int siteId, String email)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            string result = String.Empty;
            if ((email != null) && (email.Length > 0) && (siteId > 0))
            {
                string comma = String.Empty;
                using (IDataReader reader = DBSiteUser.GetSingleUser(siteId, email))
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



        public async Task<int> GetNewestUserId(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            return DBSiteUser.GetNewestUserId(siteId);

        }

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
        public async Task<bool> SaveRole(ISiteRole role)
        {
            if (role.RoleId == -1) // new role
            {
                bool exists = await RoleExists(role.SiteId, role.DisplayName);
                if (exists)
                {
                    log.Error("attempt to create a duplicate role "
                        + role.DisplayName + " for site "
                        + role.SiteId.ToString());

                    return false;
                }

                role.RoleGuid = Guid.NewGuid();

                role.RoleId = DBRoles.RoleCreate(
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
                return DBRoles.Update(
                    role.RoleId,
                    role.DisplayName);

            }

        }


        public async Task<bool> DeleteRole(int roleID)
        {
            return DBRoles.Delete(roleID);
        }

        public async Task<bool> AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid
            )
        {
            return DBRoles.AddUser(roleId, userId, roleGuid, userGuid);
        }

        public async Task<bool> RemoveUserFromRole(int roleId, int userId)
        {
            return DBRoles.RemoveUser(roleId, userId);
        }

        public async Task<bool> AddUserToDefaultRoles(ISiteUser siteUser)
        {
            // moved this to the config setting below instead of hard coded
            //IRole role = Fetch(siteUser.SiteId, "Authenticated Users");
            //if (role.RoleID > -1)
            //{
            //    AddUser(role.RoleID, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
            //}

            ISiteRole role;
            bool result = true;

            string defaultRoles = AppSettings.DefaultRolesForNewUsers;

            if (defaultRoles.Length > 0)
            {
                if (defaultRoles.IndexOf(";") == -1)
                {
                    role = await FetchRole(siteUser.SiteId, defaultRoles);
                    if ((role != null) && (role.RoleId > -1))
                    {
                        result = await AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
                    }
                }
                else
                {
                    string[] roleArray = defaultRoles.Split(';');
                    foreach (string roleName in roleArray)
                    {
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            role = await FetchRole(siteUser.SiteId, roleName);
                            if ((role != null) && (role.RoleId > -1))
                            {
                                result = result && await AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
                            }
                        }
                    }

                }

            }

            return result;
        }

        public async Task<bool> DeleteUserRoles(int userId)
        {
            return DBRoles.DeleteUserRoles(userId);
        }

        public async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            return DBRoles.DeleteUserRolesByRole(roleId);
        }


        public async Task<bool> RoleExists(int siteId, String roleName)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.Exists(siteId, roleName);
        }

        public int GetRoleMemberCount(int roleId)
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            int count = 0;
            using (DbDataReader reader = DBRoles.GetRoleMembers(roleId))
            {
                while (reader.Read())
                {
                    count += 1;
                }
            }

            return count;

        }

        public async Task<ISiteRole> FetchRole(int roleID)
        {
            using (DbDataReader reader = DBRoles.GetById(roleID))
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

        public async Task<ISiteRole> FetchRole(int siteId, string roleName)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            SiteRole role = null;

            using (DbDataReader reader = DBRoles.GetByName(siteId, roleName))
            {
                if (reader.Read())
                {
                    role = new SiteRole();
                    role.LoadFromReader(reader);     
                }
            }

            return role;

        }

        public async Task<List<string>> GetUserRoles(int siteId, int userId)
        {
            List<string> userRoles = new List<string>();
            using (DbDataReader reader = DBSiteUser.GetRolesByUser(siteId, userId))
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
            int pageSize)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            IList<ISiteRole> roles = new List<ISiteRole>();
            using (DbDataReader reader = DBRoles.GetPage(siteId, searchInput, pageNumber, pageSize))
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

        public IList<ISiteRole> GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            IList<ISiteRole> roles = new List<ISiteRole>();
            using (DbDataReader reader = DBRoles.GetRolesUserIsNotIn(siteId, userId))
            {
                SiteRole role = new SiteRole();
                role.LoadFromReader(reader);

                roles.Add(role);
            }
            return roles;
        }

        public async Task<List<int>> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        {
            List<int> roleIds = new List<int>();

            List<string> roleNames = GetRolesNames(roleNamesSeparatedBySemiColons);

            foreach (string roleName in roleNames)
            {
                if (string.IsNullOrEmpty(roleName)) { continue; }
                ISiteRole r = await FetchRole(siteId, roleName);
                if (r == null)
                {
                    log.Debug("could not get roleid for role named " + roleName);
                    continue;
                }
                if (r.RoleId > -1) { roleIds.Add(r.RoleId); }
            }

            return roleIds;
        }

        public static List<string> GetRolesNames(string roleNamesSeparatedBySemiColons)
        {
            List<string> roleNames = new List<string>();
            string[] roles = roleNamesSeparatedBySemiColons.Split(';');
            foreach (string r in roles)
            {
                if (!roleNames.Contains(r)) { roleNames.Add(r); }
            }

            return roleNames;

        }


        public async Task<int> CountOfRoles(int siteId, string searchInput)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; } 
            
            return DBRoles.GetCountOfSiteRoles(siteId, searchInput);
            
        }

        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput)
        {
            return DBRoles.GetCountOfUsersInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            int pageNumber, 
            int pageSize)
        {
            IList<IUserInfo> users = new List<IUserInfo>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            using (DbDataReader reader = DBRoles.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize))
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

        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            return DBRoles.GetCountOfUsersNotInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            int pageNumber, 
            int pageSize)
        {
            IList<IUserInfo> users = new List<IUserInfo>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (DbDataReader reader = DBRoles.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize))
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
        public async Task<bool> SaveClaim(IUserClaim userClaim)
        {
            int newId = DBUserClaims.Create(
                userClaim.UserId,
                userClaim.ClaimType,
                userClaim.ClaimValue);

            userClaim.Id = newId;

            return (newId > -1);

        }


        public async Task<bool> DeleteClaim(int id)
        {
            return DBUserClaims.Delete(id);
        }

        public async Task<bool> DeleteClaimsByUser(string userId)
        {
            return DBUserClaims.DeleteByUser(userId);
        }

        public async Task<bool> DeleteClaimByUser(string userId, string claimType)
        {
            return DBUserClaims.DeleteByUser(userId, claimType);
        }

        public async Task<bool> DeleteClaimsBySite(Guid siteGuid)
        {
            return DBUserClaims.DeleteBySite(siteGuid);
        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(string userId)
        {
            IDataReader reader = DBUserClaims.GetByUser(userId);
            return LoadClaimListFromReader(reader);

        }


        private List<IUserClaim> LoadClaimListFromReader(IDataReader reader)
        {
            List<IUserClaim> userClaimList = new List<IUserClaim>();

            try
            {
                while (reader.Read())
                {
                    UserClaim userClaim = new UserClaim();
                    userClaim.LoadFromReader(reader);
                    userClaimList.Add(userClaim);

                }
            }
            finally
            {
                reader.Close();
            }

            return userClaimList;

        }


        #endregion

        #region Logins

        /// <summary>
        /// Persists a new instance of UserLogin. Returns true on success.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateLogin(IUserLogin userLogin)
        {
            if (userLogin.LoginProvider.Length == -1) { return false; }
            if (userLogin.ProviderKey.Length == -1) { return false; }
            if (userLogin.UserId.Length == -1) { return false; }

            return DBUserLogins.Create(
                userLogin.LoginProvider,
                userLogin.ProviderKey,
                userLogin.UserId);


        }


        /// <param name="loginProvider"> loginProvider </param>
        /// <param name="providerKey"> providerKey </param>
        public async Task<IUserLogin> FindLogin(
            string loginProvider,
            string providerKey)
        {
            using (IDataReader reader = DBUserLogins.Find(
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
            string loginProvider,
            string providerKey,
            string userId)
        {
            return DBUserLogins.Delete(
                loginProvider,
                providerKey,
                userId);
        }

        public async Task<bool> DeleteLoginsByUser(string userId)
        {
            return DBUserLogins.DeleteByUser(userId);
        }

        public async Task<bool> DeleteLoginsBySite(Guid siteGuid)
        {
            return DBUserLogins.DeleteBySite(siteGuid);
        }


        

        /// <summary>
        /// Gets an IList with all instances of UserLogin.
        /// </summary>
        public async Task<IList<IUserLogin>> GetLoginsByUser(string userId)
        {
            List<IUserLogin> userLoginList = new List<IUserLogin>();
            using(IDataReader reader = DBUserLogins.GetByUser(userId))
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

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion
    }

#pragma warning restore 1998

}
