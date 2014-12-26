// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2014-12-26
// 


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace cloudscribe.Core.Repositories.Firebird
{
    public sealed class UserRepository : IUserRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository()
        { }

        #region User 

        public bool Save(ISiteUser user)
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

                //Role.AddUserToDefaultRoles(this);



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
        public bool Delete(int userId)
        {
            return DBSiteUser.DeleteUser(userId);
        }

        public bool FlagAsDeleted(int userId)
        {
            return DBSiteUser.FlagAsDeleted(userId);
        }

        public bool FlagAsNotDeleted(int userId)
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

        public bool ConfirmRegistration(Guid registrationGuid)
        {
            if (registrationGuid == Guid.Empty)
            {
                return false;
            }

            return DBSiteUser.ConfirmRegistration(Guid.Empty, registrationGuid);
        }


        public bool LockoutAccount(Guid userGuid)
        {
            return DBSiteUser.AccountLockout(userGuid, DateTime.UtcNow);
        }

        public bool UnLockAccount(Guid userGuid)
        {
            return DBSiteUser.AccountClearLockout(userGuid);
        }

        public bool UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            return DBSiteUser.UpdateFailedPasswordAttemptCount(userGuid, failedPasswordAttemptCount);
        }

        public void UpdateTotalRevenue(Guid userGuid)
        {
            DBSiteUser.UpdateTotalRevenue(userGuid);

        }

        /// <summary>
        /// updates the total revenue for all users
        /// </summary>
        public void UpdateTotalRevenue()
        {
            DBSiteUser.UpdateTotalRevenue();
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

        public int UserCount(int siteId, String userNameBeginsWith)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            return DBSiteUser.UserCount(siteId, userNameBeginsWith);
        }

        public int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            return DBSiteUser.CountOnlineSince(siteId, sinceTime);
        }


        public ISiteUser FetchNewest(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            int newestUserId = GetNewestUserId(siteId);
            return Fetch(siteId, newestUserId);
        }

        public ISiteUser Fetch(int siteId, int userId)
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


        public ISiteUser Fetch(int siteId, Guid userGuid)
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

        public ISiteUser FetchByConfirmationGuid(int siteId, Guid confirmGuid)
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


        public ISiteUser Fetch(int siteId, string email)
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

        public ISiteUser FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
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





        public List<IUserInfo> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            List<IUserInfo> userList = new List<IUserInfo>();

            //if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

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

        public List<IUserInfo> GetCrossSiteUserListByEmail(string email)
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

        public List<IUserInfo> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            out int totalPages)
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            totalPages = 1;

            List<IUserInfo> userList = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader
                = DBSiteUser.GetUserListPage(
                    siteId, pageNumber, pageSize, userNameBeginsWith, sortMode, out totalPages))
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

        public List<IUserInfo> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            List<IUserInfo> userList = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetUserSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode,
                out totalPages))
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

        public List<IUserInfo> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            List<IUserInfo> userList = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                sortMode,
                out totalPages))
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

        public List<IUserInfo> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            List<IUserInfo> userList = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetPageLockedUsers(
                siteId,
                pageNumber,
                pageSize,
                out totalPages))
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

        public List<IUserInfo> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            List<IUserInfo> userList = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBSiteUser.GetPageNotApprovedUsers(
                siteId,
                pageNumber,
                pageSize,
                out totalPages))
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

        public bool EmailExistsInDB(int siteId, string email)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUser(siteId, email))
            {
                while (r.Read()) { found = true; }
            }
            return found;
        }

        public bool EmailExistsInDB(int siteId, int userId, string email)
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

        public String GetUserNameFromEmail(int siteId, String email)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            String result = String.Empty;
            if ((email != null) && (email.Length > 0) && (siteId > 0))
            {
                String comma = String.Empty;
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



        public int GetNewestUserId(int siteId)
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
        public bool SaveRole(ISiteRole role)
        {
            if (role.RoleId == -1) // new role
            {
                if (RoleExists(role.SiteId, role.DisplayName))
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






        public bool DeleteRole(int roleID)
        {
            return DBRoles.Delete(roleID);
        }

        public bool AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid
            )
        {
            return DBRoles.AddUser(roleId, userId, roleGuid, userGuid);
        }

        public bool RemoveUserFromRole(int roleId, int userId)
        {
            return DBRoles.RemoveUser(roleId, userId);
        }

        public void AddUserToDefaultRoles(ISiteUser siteUser)
        {
            // moved this to the config setting below instead of hard coded
            //IRole role = Fetch(siteUser.SiteId, "Authenticated Users");
            //if (role.RoleID > -1)
            //{
            //    AddUser(role.RoleID, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
            //}

            ISiteRole role;

            string defaultRoles = AppSettings.DefaultRolesForNewUsers;

            if (defaultRoles.Length > 0)
            {
                if (defaultRoles.IndexOf(";") == -1)
                {
                    role = FetchRole(siteUser.SiteId, defaultRoles);
                    if ((role != null) && (role.RoleId > -1))
                    {
                        AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
                    }
                }
                else
                {
                    string[] roleArray = defaultRoles.Split(';');
                    foreach (string roleName in roleArray)
                    {
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            role = FetchRole(siteUser.SiteId, roleName);
                            if ((role != null) && (role.RoleId > -1))
                            {
                                AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid);
                            }
                        }
                    }

                }

            }


        }

        public bool DeleteUserRoles(int userId)
        {
            return DBRoles.DeleteUserRoles(userId);
        }


        public bool RoleExists(int siteId, String roleName)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.Exists(siteId, roleName);
        }

        public int GetRoleMemberCount(int roleId)
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            int count = 0;
            using (IDataReader reader = DBRoles.GetRoleMembers(roleId))
            {
                while (reader.Read())
                {
                    count += 1;
                }
            }

            return count;

        }

        public ISiteRole FetchRole(int roleID)
        {
            using (IDataReader reader = DBRoles.GetById(roleID))
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

        public ISiteRole FetchRole(int siteId, string roleName)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            SiteRole role = null;

            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    string foundName = reader["RoleName"].ToString();
                    if (foundName == roleName)
                    {
                        role = new SiteRole();
                        role.LoadFromReader(reader);
                        break;
                    }
                }
            }

            return role;

        }

        public List<string> GetUserRoles(int siteId, int userId)
        {
            List<string> userRoles = new List<string>();
            using (IDataReader reader = DBSiteUser.GetRolesByUser(siteId, userId))
            {
                while (reader.Read())
                {
                    userRoles.Add(reader["RoleName"].ToString());
                }

            }

            return userRoles;
        }

        public IList<ISiteRole> GetRolesBySite(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            IList<ISiteRole> roles = new List<ISiteRole>();
            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
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
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            IList<ISiteRole> roles = new List<ISiteRole>();
            using (IDataReader reader = DBRoles.GetRolesUserIsNotIn(siteId, userId))
            {
                SiteRole role = new SiteRole();
                role.LoadFromReader(reader);

                roles.Add(role);
            }
            return roles;
        }

        public List<int> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        {
            List<int> roleIds = new List<int>();

            List<string> roleNames = GetRolesNames(roleNamesSeparatedBySemiColons);

            foreach (string roleName in roleNames)
            {
                if (string.IsNullOrEmpty(roleName)) { continue; }
                ISiteRole r = FetchRole(siteId, roleName);
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


        public int CountOfRoles(int siteId)
        {
            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }
            return DBRoles.GetCountOfSiteRoles(siteId);
        }

        public IList<IUserInfo> GetUsersInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            IList<IUserInfo> users = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBRoles.GetUsersInRole(siteId, roleId, pageNumber, pageSize, out totalPages))
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

        public IList<IUserInfo> GetUsersNotInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            IList<IUserInfo> users = new List<IUserInfo>();

            if (AppSettings.UseRelatedSiteMode) { siteId = AppSettings.RelatedSiteId; }

            using (IDataReader reader = DBRoles.GetUsersNotInRole(siteId, roleId, pageNumber, pageSize, out totalPages))
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
        public bool SaveClaim(IUserClaim userClaim)
        {

            int newId = DBUserClaims.Create(
                userClaim.UserId,
                userClaim.ClaimType,
                userClaim.ClaimValue);

            userClaim.Id = newId;

            return (newId > -1);


        }



        //public UserClaim Fetch(int id)
        //{
        //    using (IDataReader reader = DBUserClaims.GetOne(id))
        //    {
        //        if (reader.Read())
        //        {
        //            UserClaim userClaim = new UserClaim();
        //            userClaim.Id = Convert.ToInt32(reader["Id"]);
        //            userClaim.UserId = reader["UserId"].ToString();
        //            userClaim.ClaimType = reader["ClaimType"].ToString();
        //            userClaim.ClaimValue = reader["ClaimValue"].ToString();

        //            return userClaim;

        //        }
        //    }

        //    return null;
        //}



        public bool DeleteClaim(int id)
        {
            return DBUserClaims.Delete(id);
        }

        public bool DeleteClaimsByUser(string userId)
        {
            return DBUserClaims.DeleteByUser(userId);
        }

        public bool DeleteClaimByUser(string userId, string claimType)
        {
            return DBUserClaims.DeleteByUser(userId, claimType);
        }

        public bool DeleteClaimsBySite(Guid siteGuid)
        {
            return DBUserClaims.DeleteBySite(siteGuid);
        }

        public IList<IUserClaim> GetClaimsByUser(string userId)
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
        public bool CreateLogin(IUserLogin userLogin)
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
        public IUserLogin FindLogin(
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
        public bool DeleteLogin(
            string loginProvider,
            string providerKey,
            string userId)
        {
            return DBUserLogins.Delete(
                loginProvider,
                providerKey,
                userId);
        }

        public bool DeleteLoginsByUser(string userId)
        {
            return DBUserLogins.DeleteByUser(userId);
        }

        public bool DeleteLoginsBySite(Guid siteGuid)
        {
            return DBUserLogins.DeleteBySite(siteGuid);
        }


        

        /// <summary>
        /// Gets an IList with all instances of UserLogin.
        /// </summary>
        public IList<IUserLogin> GetLoginsByUser(string userId)
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
}
