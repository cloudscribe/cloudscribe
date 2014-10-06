// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2014-09-09
// 


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using log4net;
using cloudscribe.Core.Models;
using cloudscribe.Configuration;

namespace cloudscribe.Core.Repositories.pgsql
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
           // if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

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
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.UserCount(siteId);
        }

        public int UserCount(int siteId, String userNameBeginsWith)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.UserCount(siteId, userNameBeginsWith);
        }

        public int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.CountOnlineSince(siteId, sinceTime);
        }


        public ISiteUser FetchNewest(int siteId)
        {
            int newestUserId = GetNewestUserId(siteId);
            return Fetch(siteId, newestUserId);
        }

        public ISiteUser Fetch(int siteId, int userId)
        {
            using (IDataReader reader = DBSiteUser.GetSingleUser(userId))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    LoadFromReader(reader, user);

                    if (user.SiteId == siteId) { return user; }

                }
            }

            return null;
        }


        public ISiteUser Fetch(int siteId, Guid userGuid)
        {
            using (IDataReader reader = DBSiteUser.GetSingleUser(userGuid))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    LoadFromReader(reader, user);

                    if (user.SiteId == siteId) { return user; }

                }
            }

            return null;
        }

        public ISiteUser FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            using (IDataReader reader = DBSiteUser.GetUserByRegistrationGuid(siteId, confirmGuid))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    LoadFromReader(reader, user);

                    if (user.SiteId == siteId) { return user; }

                }
            }

            return null;
        }


        public ISiteUser Fetch(int siteId, string email)
        {
            using (IDataReader reader = DBSiteUser.GetSingleUser(siteId, email))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    LoadFromReader(reader, user);

                    return user;

                }
            }

            return null;
        }

        public ISiteUser FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            using (IDataReader reader = DBSiteUser.GetSingleUserByLoginName(siteId, userName, allowEmailFallback))
            {
                if (reader.Read())
                {
                    SiteUser user = new SiteUser();

                    LoadFromReader(reader, user);

                    return user;

                }
            }

            return null;
        }





        public List<ISiteUser> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            List<ISiteUser> userList = new List<ISiteUser>();

            //if (UseRelatedSiteMode) { siteGuid = Guid.Empty; }

            using (IDataReader reader = DBUserLocation.GetUsersByIPAddress(siteGuid, ipv4Address))
            {
                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
                    userList.Add(user);

                }
            }

            return userList;

        }

        public List<ISiteUser> GetCrossSiteUserListByEmail(string email)
        {
            List<ISiteUser> userList = new List<ISiteUser>();

            using (IDataReader reader = DBSiteUser.GetCrossSiteUserListByEmail(email))
            {
                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
                    userList.Add(user);

                }
            }

            return userList;

        }

        public List<ISiteUser> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            out int totalPages)
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            totalPages = 1;

            List<ISiteUser> userList = new List<ISiteUser>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader
                = DBSiteUser.GetUserListPage(
                    siteId, pageNumber, pageSize, userNameBeginsWith, sortMode, out totalPages))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
                    userList.Add(user);
                    //totalPages = Convert.ToInt32(reader["TotalPages"]);
                }
            }

            return userList;

        }

        public List<ISiteUser> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            List<ISiteUser> userList = new List<ISiteUser>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

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
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
                    userList.Add(user);

                }
            }

            return userList;


        }

        public List<ISiteUser> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            List<ISiteUser> userList = new List<ISiteUser>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

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
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
                    userList.Add(user);

                }
            }

            return userList;


        }

        public List<ISiteUser> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            List<ISiteUser> userList = new List<ISiteUser>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBSiteUser.GetPageLockedUsers(
                siteId,
                pageNumber,
                pageSize,
                out totalPages))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
                    userList.Add(user);

                }
            }

            return userList;
        }

        public List<ISiteUser> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            List<ISiteUser> userList = new List<ISiteUser>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBSiteUser.GetPageNotApprovedUsers(
                siteId,
                pageNumber,
                pageSize,
                out totalPages))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    LoadFromReader(reader, user);
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
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUser(siteId, email))
            {
                while (r.Read()) { found = true; }
            }
            return found;
        }

        public bool EmailExistsInDB(int siteId, int userId, string email)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
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
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUserByLoginName(siteId, loginName, false))
            {
                while (r.Read()) { found = true; }
            }

            return found;
        }

        public String GetUserNameFromEmail(int siteId, String email)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

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
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            return DBSiteUser.GetNewestUserId(siteId);

        }

       


        private void LoadFromReader(IDataReader reader, SiteUser user)
        {
            user.UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
            user.SiteId = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);
            user.DisplayName = reader["Name"].ToString();
            if (AppSettings.UseEmailForLogin)
            {
                user.UserName = reader["Email"].ToString();
            }
            else
            {
                user.UserName = reader["LoginName"].ToString();
            }
            user.Email = reader["Email"].ToString();
            user.LoweredEmail = reader["LoweredEmail"].ToString();
            user.PasswordQuestion = reader["PasswordQuestion"].ToString();
            user.PasswordAnswer = reader["PasswordAnswer"].ToString();
            user.Gender = reader["Gender"].ToString();

            if (reader["ProfileApproved"] != DBNull.Value)
            {
                user.ProfileApproved = Convert.ToBoolean(reader["ProfileApproved"]);
            }

            if (reader["RegisterConfirmGuid"] != DBNull.Value)
            {
                user.RegisterConfirmGuid = new Guid(reader["RegisterConfirmGuid"].ToString());
            }
            if (reader["ApprovedForForums"] != DBNull.Value)
            {
                user.ApprovedForLogin = Convert.ToBoolean(reader["ApprovedForForums"]);
            }
            if (reader["Trusted"] != DBNull.Value)
            {
                user.Trusted = Convert.ToBoolean(reader["Trusted"]);
            }
            if (reader["DisplayInMemberList"] != DBNull.Value)
            {
                user.DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
            }
            user.WebSiteUrl = reader["WebSiteURL"].ToString();
            user.Country = reader["Country"].ToString();
            user.State = reader["State"].ToString();

            //legacy fields
            //user.Occupation = reader["Occupation"].ToString();
            //user.Interests = reader["Interests"].ToString();
            //user.MSN = reader["MSN"].ToString();
            //user.Yahoo = reader["Yahoo"].ToString();
            //user.AIM = reader["AIM"].ToString();
            //user.ICQ = reader["ICQ"].ToString();
            //user.TimeOffsetHours = Convert.ToInt32(reader["TimeOffsetHours"]);

            user.TotalPosts = Convert.ToInt32(reader["TotalPosts"], CultureInfo.InvariantCulture);
            user.AvatarUrl = reader["AvatarUrl"].ToString();
            
            user.Signature = reader["Signature"].ToString();
            if (reader["DateCreated"] != DBNull.Value)
            {
                user.CreatedUtc = Convert.ToDateTime(reader["DateCreated"]);
            }
            if (reader["UserGuid"] != DBNull.Value)
            {
                user.UserGuid = new Guid(reader["UserGuid"].ToString());
            }
            user.Skin = reader["Skin"].ToString();
            if (reader["IsDeleted"] != DBNull.Value)
            {
                user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
            }
            if (reader["LastActivityDate"] != DBNull.Value)
            {
                user.LastActivityDate = Convert.ToDateTime(reader["LastActivityDate"]);
            }
            if (reader["LastLoginDate"] != DBNull.Value)
            {
                user.LastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]);
            }
            if (reader["LastPasswordChangedDate"] != DBNull.Value)
            {
                user.LastPasswordChangedDate = Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            }
            if (reader["LastLockoutDate"] != DBNull.Value)
            {
                user.LastLockoutDate = Convert.ToDateTime(reader["LastLockoutDate"]);
            }
            if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
            {
                user.FailedPasswordAttemptCount = Convert.ToInt32(reader["FailedPasswordAttemptCount"]);
            }
            if (reader["FailedPwdAttemptWindowStart"] != DBNull.Value)
            {
                user.FailedPasswordAttemptWindowStart = Convert.ToDateTime(reader["FailedPwdAttemptWindowStart"]);
            }
            if (reader["FailedPwdAnswerAttemptCount"] != DBNull.Value)
            {
                user.FailedPasswordAnswerAttemptCount = Convert.ToInt32(reader["FailedPwdAnswerAttemptCount"]);
            }
            if (reader["FailedPwdAnswerWindowStart"] != DBNull.Value)
            {
                user.FailedPasswordAnswerAttemptWindowStart = Convert.ToDateTime(reader["FailedPwdAnswerWindowStart"]);
            }
            if (reader["IsLockedOut"] != DBNull.Value)
            {
                user.IsLockedOut = Convert.ToBoolean(reader["IsLockedOut"]);
            }
            user.MobilePin = reader["MobilePIN"].ToString();
            
            user.Comment = reader["Comment"].ToString();
            user.OpenIdUri = reader["OpenIDURI"].ToString();
            user.WindowsLiveId = reader["WindowsLiveID"].ToString();
            user.SiteGuid = new Guid(reader["SiteGuid"].ToString());

            if (reader["TotalRevenue"] != DBNull.Value)
            {
                user.TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]);
            }
            
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();
            
            user.MustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);
            user.NewEmail = reader["NewEmail"].ToString();
            user.EditorPreference = reader["EditorPreference"].ToString();
            user.EmailChangeGuid = new Guid(reader["EmailChangeGuid"].ToString());
            user.TimeZoneId = reader["TimeZoneId"].ToString();
            user.PasswordResetGuid = new Guid(reader["PasswordResetGuid"].ToString());
            user.RolesChanged = Convert.ToBoolean(reader["RolesChanged"]);
            user.AuthorBio = reader["AuthorBio"].ToString();
            if (reader["DateOfBirth"] != DBNull.Value)
            {
                user.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
            }
            
            user.EmailConfirmed = Convert.ToBoolean(reader["EmailConfirmed"]);
            
            user.SecurityStamp = reader["SecurityStamp"].ToString();
            user.PhoneNumber = reader["PhoneNumber"].ToString();
            user.PhoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]);
            user.TwoFactorEnabled = Convert.ToBoolean(reader["TwoFactorEnabled"]);
            if (reader["LockoutEndDateUtc"] != DBNull.Value)
            {
                user.LockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUtc"]);
            }

            user.Password = reader["Pwd"].ToString();
            user.PasswordFormat = Convert.ToInt32(reader["PwdFormat"]);
            user.PasswordHash = reader["PasswordHash"].ToString();
            user.PasswordSalt = reader["PasswordSalt"].ToString();

            if(user.PasswordHash.Length == 0)
            {
                
               user.PasswordHash = 
                   user.Password + "|" 
                   + user.PasswordSalt + "|" 
                   + user.PasswordFormat.ToString(CultureInfo.InvariantCulture)
                   ;
                
            }
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
                    LoadFromReader(reader, role);
                    return role;
                }
            }

            return null;
        }

        public ISiteRole FetchRole(int siteId, string roleName)
        {
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            SiteRole role = null;

            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    string foundName = reader["RoleName"].ToString();
                    if (foundName == roleName)
                    {
                        role = new SiteRole();
                        LoadFromReader(reader, role);
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
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            IList<ISiteRole> roles = new List<ISiteRole>();
            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    SiteRole role = new SiteRole();
                    LoadFromReader(reader, role);
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
            using (IDataReader reader = DBRoles.GetRolesUserIsNotIn(siteId, userId))
            {
                SiteRole role = new SiteRole();
                LoadFromReader(reader, role);

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
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; } 
            return DBRoles.GetCountOfSiteRoles(siteId);
        }

        public IList<IUserInfo> GetUsersInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            IList<IUserInfo> users = new List<IUserInfo>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            using (IDataReader reader = DBRoles.GetUsersInRole(siteId, roleId, pageNumber, pageSize, out totalPages))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    LoadFromReader(reader, user);
                    users.Add(user);

                }

            }

            return users;
        }

        public IList<IUserInfo> GetUsersNotInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            IList<IUserInfo> users = new List<IUserInfo>();

            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBRoles.GetUsersNotInRole(siteId, roleId, pageNumber, pageSize, out totalPages))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    LoadFromReader(reader, user);
                    users.Add(user);

                }

            }

            return users;
        }


        private void LoadFromReader(IDataReader reader, IUserInfo user)
        {
            user.UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
            if (reader["UserGuid"] != DBNull.Value)
            {
                user.UserGuid = new Guid(reader["UserGuid"].ToString());
            }
            user.SiteId = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);
            user.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            user.DisplayName = reader["Name"].ToString();
            user.UserName = reader["LoginName"].ToString();
            user.Email = reader["Email"].ToString();
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();

        }

        private void LoadFromReader(IDataReader reader, ISiteRole role)
        {
            role.RoleId = Convert.ToInt32(reader["RoleID"]);
            role.SiteId = Convert.ToInt32(reader["SiteID"]);
            role.RoleName = reader["RoleName"].ToString();
            role.DisplayName = reader["DisplayName"].ToString();
            role.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            role.RoleGuid = new Guid(reader["RoleGuid"].ToString());

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
            return LoadListFromReader(reader);

        }


        private List<IUserClaim> LoadListFromReader(IDataReader reader)
        {
            List<IUserClaim> userClaimList = new List<IUserClaim>();

            try
            {
                while (reader.Read())
                {
                    UserClaim userClaim = new UserClaim();
                    userClaim.Id = Convert.ToInt32(reader["Id"]);
                    userClaim.UserId = reader["UserId"].ToString();
                    userClaim.ClaimType = reader["ClaimType"].ToString();
                    userClaim.ClaimValue = reader["ClaimValue"].ToString();
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
                    LoadFromReader(reader, userLogin);
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
                    LoadFromReader(reader, userLogin);
                    userLoginList.Add(userLogin);

                }
            }

            return userLoginList;

        }

        
        private void LoadFromReader(IDataReader reader, IUserLogin userLogin)
        {
            userLogin.LoginProvider = reader["LoginProvider"].ToString();
            userLogin.ProviderKey = reader["ProviderKey"].ToString();
            userLogin.UserId = reader["UserId"].ToString();
            
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion
    }
}
