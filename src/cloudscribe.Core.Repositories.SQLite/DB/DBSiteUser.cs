// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-11-09
// 

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Framework.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBSiteUser
    {
        internal DBSiteUser(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;

        public DbDataReader GetUserCountByYearMonth(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("strftime('%Y', DateCreated) AS Y, ");
            sqlCommand.Append("strftime('%m', DateCreated) AS M, ");
            sqlCommand.Append("strftime('%Y', DateCreated) + '-' + strftime('%m', DateCreated) AS Label, ");
            sqlCommand.Append("COUNT(*) As Users ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID ");
            sqlCommand.Append("GROUP BY strftime('%Y', DateCreated), strftime('%m', DateCreated) ");
            sqlCommand.Append("ORDER BY strftime('%Y', DateCreated), strftime('%m', DateCreated) ");
            sqlCommand.Append("; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }


        public DbDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, Name, PasswordSalt, Pwd, Email FROM mp_Users WHERE SiteID = :SiteID ORDER BY Email");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("Name As SiteUser ");

            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ( ");
            sqlCommand.Append("(Name LIKE :Query) ");
            sqlCommand.Append("OR (FirstName LIKE :Query) ");
            sqlCommand.Append("OR (LastName LIKE :Query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("UNION ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("Email As SiteUser ");

            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Email LIKE :Query  ");

            sqlCommand.Append("ORDER BY SiteUser ");
            sqlCommand.Append("LIMIT " + rowsToGet.ToString());
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":Query", DbType.String);
            arParams[1].Value = query + "%";

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader EmailLookup(int siteId, string query, int rowsToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email ");

            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ( ");
            sqlCommand.Append("(Email LIKE :Query) ");
            sqlCommand.Append("OR (Name LIKE :Query) ");
            sqlCommand.Append("OR (FirstName LIKE :Query) ");
            sqlCommand.Append("OR (LastName LIKE :Query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY Email ");
            sqlCommand.Append("LIMIT " + rowsToGet.ToString());

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":Query", DbType.String);
            arParams[1].Value = query + "%";

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public int UserCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public int CountLockedOutUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID AND IsLockedOut = 1;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public int CountNotApprovedUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID AND AccountApproved = 0;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;
        }

        public int CountUsers(int siteId, String userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");

            if (userNameBeginsWith.Length == 1)
            {
                sqlCommand.Append("AND Name like :UserNameBeginsWith || '%' ");
            }

            sqlCommand.Append("; ");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":UserNameBeginsWith", DbType.String);
            arParams[1].Value = userNameBeginsWith;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND DateCreated >= :BeginDate ");
            sqlCommand.Append("AND DateCreated < :EndDate; ");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":EndDate", DbType.DateTime);
            arParams[2].Value = endDate;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public int GetNewestUserId(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT MAX(UserID) FROM mp_Users WHERE SiteID = :SiteID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;
        }

        public int Count(int siteId, string userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND Name LIKE :UserNameBeginsWith ");
            }
            sqlCommand.Append(" ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":UserNameBeginsWith", DbType.String);
            arParams[1].Value = userNameBeginsWith + "%";

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public DbDataReader GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            sqlCommand.Append("SELECT		u.*  ");
            //sqlCommand.Append(" " + totalPages.ToString() + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE u.AccountApproved = 1   ");
            sqlCommand.Append("AND u.SiteID = :SiteID   ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND u.Name LIKE :UserNameBeginsWith ");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY u.DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY u.LastName, u.FirstName, u.Name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY u.Name ");
                    break;
            }


            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[0].Value = pageNumber;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":UserNameBeginsWith", DbType.String);
            arParams[2].Value = userNameBeginsWith + "%";

            arParams[3] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[3].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountUsersForSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE :SearchInput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (Email LIKE :SearchInput) ");

                sqlCommand.Append(")");


            }
            sqlCommand.Append(" ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[1].Value = "%" + searchInput + "%";

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public DbDataReader GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            sqlCommand.Append("SELECT *  ");

            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE :SearchInput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (Email LIKE :SearchInput) ");

                sqlCommand.Append(")");

            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY LastName, FirstName, Name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY Name ");
                    break;
            }

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[0].Value = pageNumber;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[3].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountUsersForAdminSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = :SiteID ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (Email LIKE :SearchInput) ");

                sqlCommand.Append(")");


            }
            sqlCommand.Append(" ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[1].Value = "%" + searchInput + "%";

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public DbDataReader GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");

            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (Email LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LastName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (FirstName LIKE :SearchInput) ");
                sqlCommand.Append(")");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY LastName, FirstName, Name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY Name ");
                    break;
            }

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[0].Value = pageNumber;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[3].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");

            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsLockedOut = 1 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[0].Value = pageNumber;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[2].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");

            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("AccountApproved = 0 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[0].Value = pageNumber;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[2].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public int AddUser(
            Guid siteGuid,
            int siteId,
            string fullName,
            string loginName,
            string email,
            Guid userGuid,
            DateTime dateCreated,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            DateTime dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc,

            bool accountApproved,
            bool isLockedOut,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string avatarUrl,
            string signature,
            string authorBio,
            string comment
            )
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Users (");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("LoginName, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("TimeZoneId, ");
            sqlCommand.Append("EmailChangeGuid, ");
            sqlCommand.Append("PasswordResetGuid, ");
            sqlCommand.Append("RolesChanged, ");
            sqlCommand.Append("MustChangePwd, ");
            sqlCommand.Append("DateCreated, ");
            sqlCommand.Append("DateOfBirth, ");
            sqlCommand.Append("EmailConfirmed, ");
            sqlCommand.Append("PasswordHash, ");
            sqlCommand.Append("SecurityStamp, ");
            sqlCommand.Append("PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc, ");

            sqlCommand.Append("AccountApproved, ");
            sqlCommand.Append("IsLockedOut, ");
            sqlCommand.Append("DisplayInMemberList, ");
            sqlCommand.Append("WebSiteURL, ");
            sqlCommand.Append("Country, ");
            sqlCommand.Append("State, ");
            sqlCommand.Append("AvatarUrl, ");
            sqlCommand.Append("Signature, ");
            sqlCommand.Append("AuthorBio, ");
            sqlCommand.Append("Comment, ");

            sqlCommand.Append("UserGuid");
            sqlCommand.Append(")");

            sqlCommand.Append("VALUES (");

            sqlCommand.Append(" :SiteID , ");
            sqlCommand.Append(" :SiteGuid , ");
            sqlCommand.Append(" :FullName , ");
            sqlCommand.Append(" :LoginName , ");
            sqlCommand.Append(" :Email , ");
            sqlCommand.Append(":FirstName, ");
            sqlCommand.Append(":LastName, ");
            sqlCommand.Append(":TimeZoneId, ");
            sqlCommand.Append(":EmailChangeGuid, ");
            sqlCommand.Append("'00000000-0000-0000-0000-000000000000', "); //PasswordResetGui
            sqlCommand.Append("0, "); //RolesChanged
            sqlCommand.Append(":MustChangePwd, ");
            sqlCommand.Append(" :DateCreated, ");
            sqlCommand.Append(":DateOfBirth, ");
            sqlCommand.Append(":EmailConfirmed, ");
            sqlCommand.Append(":PasswordHash, ");
            sqlCommand.Append(":SecurityStamp, ");
            sqlCommand.Append(":PhoneNumber, ");
            sqlCommand.Append(":PhoneNumberConfirmed, ");
            sqlCommand.Append(":TwoFactorEnabled, ");
            sqlCommand.Append(":LockoutEndDateUtc, ");

            sqlCommand.Append(":AccountApproved, ");
            sqlCommand.Append(":IsLockedOut, ");
            sqlCommand.Append(":DisplayInMemberList, ");
            sqlCommand.Append(":WebSiteURL, ");
            sqlCommand.Append(":Country, ");
            sqlCommand.Append(":State, ");
            sqlCommand.Append(":AvatarUrl, ");
            sqlCommand.Append(":Signature, ");
            sqlCommand.Append(":AuthorBio, ");
            sqlCommand.Append(":Comment, ");

            sqlCommand.Append(" :UserGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[30];

            arParams[0] = new SqliteParameter(":FullName", DbType.String);
            arParams[0].Value = fullName;

            arParams[1] = new SqliteParameter(":LoginName", DbType.String);
            arParams[1].Value = loginName;

            arParams[2] = new SqliteParameter(":Email", DbType.String);
            arParams[2].Value = email;
            
            arParams[3] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[3].Value = siteId;

            arParams[4] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new SqliteParameter(":DateCreated", DbType.DateTime); ;
            arParams[5].Value = dateCreated;

            arParams[6] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[6].Value = siteGuid.ToString();

            arParams[7] = new SqliteParameter(":MustChangePwd", DbType.Int32);
            arParams[7].Value = mustChangePwd ? 1 : 0;

            arParams[8] = new SqliteParameter(":FirstName", DbType.String);
            arParams[8].Value = firstName;

            arParams[9] = new SqliteParameter(":LastName", DbType.String);
            arParams[9].Value = lastName;

            arParams[10] = new SqliteParameter(":TimeZoneId", DbType.String);
            arParams[10].Value = timeZoneId;

            arParams[11] = new SqliteParameter(":EmailChangeGuid", DbType.String);
            arParams[11].Value = Guid.Empty.ToString();
            
            arParams[12] = new SqliteParameter(":DateOfBirth", DbType.DateTime);

            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[12].Value = DBNull.Value;
            }
            else
            {
                arParams[12].Value = dateOfBirth;
            }

            arParams[13] = new SqliteParameter(":EmailConfirmed", DbType.Int32);
            arParams[13].Value = emailConfirmed ? 1 : 0;
            
            arParams[14] = new SqliteParameter(":PasswordHash", DbType.Object);
            arParams[14].Value = passwordHash;

            arParams[15] = new SqliteParameter(":SecurityStamp", DbType.Object);
            arParams[15].Value = securityStamp;

            arParams[16] = new SqliteParameter(":PhoneNumber", DbType.String);
            arParams[16].Value = phoneNumber;

            arParams[17] = new SqliteParameter(":PhoneNumberConfirmed", DbType.Int32);
            arParams[17].Value = phoneNumberConfirmed ? 1 : 0;

            arParams[18] = new SqliteParameter(":TwoFactorEnabled", DbType.Int32);
            arParams[18].Value = twoFactorEnabled ? 1 : 0;

            arParams[19] = new SqliteParameter(":LockoutEndDateUtc", DbType.DateTime);

            if (lockoutEndDateUtc == null)
            {
                arParams[19].Value = DBNull.Value;
            }
            else
            {
                arParams[19].Value = lockoutEndDateUtc;
            }

            arParams[20] = new SqliteParameter(":AccountApproved", DbType.Int32);
            arParams[20].Value = accountApproved ? 1 : 0;

            arParams[21] = new SqliteParameter(":IsLockedOut", DbType.Int32);
            arParams[21].Value = isLockedOut ? 1 : 0;

            arParams[22] = new SqliteParameter(":DisplayInMemberList", DbType.Int32);
            arParams[22].Value = displayInMemberList ? 1 : 0;

            arParams[23] = new SqliteParameter(":WebSiteURL", DbType.String);
            arParams[23].Value = webSiteUrl;

            arParams[24] = new SqliteParameter(":Country", DbType.String);
            arParams[24].Value = country;

            arParams[25] = new SqliteParameter(":State", DbType.String);
            arParams[25].Value = state;

            arParams[26] = new SqliteParameter(":AvatarUrl", DbType.String);
            arParams[26].Value = avatarUrl;

            arParams[27] = new SqliteParameter(":Signature", DbType.Object);
            arParams[27].Value = signature;

            arParams[28] = new SqliteParameter(":AuthorBio", DbType.Object);
            arParams[28].Value = authorBio;

            arParams[29] = new SqliteParameter(":Comment", DbType.Object);
            arParams[29].Value = comment;


            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                   connectionString,
                   sqlCommand.ToString(),
                   arParams).ToString());

            return newID;

        }

        public bool UpdateUser(
            int userId,
            string name,
            string loginName,
            string email,
            string gender,
            bool accountApproved,
            bool trusted,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string avatarUrl,
            string signature,
            string loweredEmail,
            string comment,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            string newEmail,
            Guid emailChangeGuid,
            Guid passwordResetGuid,
            bool rolesChanged,
            string authorBio,
            DateTime dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc,
            bool isLockedOut
            )
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET Email = :Email ,");
            sqlCommand.Append("Name = :FullName,");
            sqlCommand.Append("LoginName = :LoginName,");
            sqlCommand.Append("FirstName = :FirstName,");
            sqlCommand.Append("LastName = :LastName,");
            sqlCommand.Append("TimeZoneId = :TimeZoneId,");
            sqlCommand.Append("NewEmail = :NewEmail,");
            sqlCommand.Append("EmailChangeGuid = :EmailChangeGuid,");
            sqlCommand.Append("PasswordResetGuid = :PasswordResetGuid,");
            sqlCommand.Append("RolesChanged = :RolesChanged,");
            sqlCommand.Append("MustChangePwd = :MustChangePwd,");
            sqlCommand.Append("Gender = :Gender,");
            sqlCommand.Append("AccountApproved = :AccountApproved,");
            sqlCommand.Append("Trusted = :Trusted,");
            sqlCommand.Append("DisplayInMemberList = :DisplayInMemberList,");
            sqlCommand.Append("WebSiteURL = :WebSiteURL,");
            sqlCommand.Append("Country = :Country,");
            sqlCommand.Append("State = :State,");
            sqlCommand.Append("AvatarUrl = :AvatarUrl,");
            sqlCommand.Append("Signature = :Signature,");
            sqlCommand.Append("AuthorBio = :AuthorBio,");
            sqlCommand.Append("LoweredEmail = :LoweredEmail,");
            sqlCommand.Append("Comment = :Comment,");
            sqlCommand.Append("DateOfBirth = :DateOfBirth,");
            sqlCommand.Append("EmailConfirmed = :EmailConfirmed, ");
            sqlCommand.Append("PasswordHash = :PasswordHash, ");
            sqlCommand.Append("SecurityStamp = :SecurityStamp, ");
            sqlCommand.Append("PhoneNumber = :PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed = :PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled = :TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc = :LockoutEndDateUtc, ");

            sqlCommand.Append("IsLockedOut = :IsLockedOut ");

            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[33];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":Email", DbType.String);
            arParams[1].Value = email;
            
            arParams[2] = new SqliteParameter(":Gender", DbType.String);
            arParams[2].Value = gender;

            arParams[3] = new SqliteParameter(":AccountApproved", DbType.Int32);
            arParams[3].Value = accountApproved ? 1 : 0;
            
            arParams[4] = new SqliteParameter(":Trusted", DbType.Int32);
            arParams[4].Value = trusted ? 1 : 0;

            arParams[5] = new SqliteParameter(":DisplayInMemberList", DbType.Int32);
            arParams[5].Value = displayInMemberList ? 1 : 0;

            arParams[6] = new SqliteParameter(":WebSiteURL", DbType.String);
            arParams[6].Value = webSiteUrl;

            arParams[7] = new SqliteParameter(":Country", DbType.String);
            arParams[7].Value = country;

            arParams[8] = new SqliteParameter(":State", DbType.String);
            arParams[8].Value = state;
            
            arParams[9] = new SqliteParameter(":AvatarUrl", DbType.String);
            arParams[9].Value = avatarUrl;

            arParams[10] = new SqliteParameter(":Signature", DbType.Object);
            arParams[10].Value = signature;
            
            arParams[11] = new SqliteParameter(":FullName", DbType.String);
            arParams[11].Value = name;

            arParams[12] = new SqliteParameter(":LoginName", DbType.String);
            arParams[12].Value = loginName;

            arParams[13] = new SqliteParameter(":LoweredEmail", DbType.String);
            arParams[13].Value = loweredEmail;
            
            arParams[14] = new SqliteParameter(":Comment", DbType.Object);
            arParams[14].Value = comment;
            
            arParams[15] = new SqliteParameter(":MustChangePwd", DbType.Int32);
            arParams[15].Value = mustChangePwd ? 1 : 0;

            arParams[16] = new SqliteParameter(":FirstName", DbType.String);
            arParams[16].Value = firstName;

            arParams[17] = new SqliteParameter(":LastName", DbType.String);
            arParams[17].Value = lastName;

            arParams[18] = new SqliteParameter(":TimeZoneId", DbType.String);
            arParams[18].Value = timeZoneId;
            
            arParams[19] = new SqliteParameter(":NewEmail", DbType.String);
            arParams[19].Value = newEmail;

            arParams[20] = new SqliteParameter(":EmailChangeGuid", DbType.String);
            arParams[20].Value = emailChangeGuid.ToString();

            arParams[21] = new SqliteParameter(":PasswordResetGuid", DbType.String);
            arParams[21].Value = passwordResetGuid.ToString();
            
            arParams[22] = new SqliteParameter(":RolesChanged", DbType.Int32);
            arParams[22].Value = rolesChanged ? 1 : 0;

            arParams[23] = new SqliteParameter(":AuthorBio", DbType.Object);
            arParams[23].Value = authorBio;

            arParams[24] = new SqliteParameter(":DateOfBirth", DbType.DateTime);

            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[24].Value = DBNull.Value;
            }
            else
            {
                arParams[24].Value = dateOfBirth;
            }

            arParams[25] = new SqliteParameter(":EmailConfirmed", DbType.Int32);
            arParams[25].Value = emailConfirmed ? 1 : 0;
            
            arParams[26] = new SqliteParameter(":PasswordHash", DbType.Object);
            arParams[26].Value = passwordHash;

            arParams[27] = new SqliteParameter(":SecurityStamp", DbType.Object);
            arParams[27].Value = securityStamp;

            arParams[28] = new SqliteParameter(":PhoneNumber", DbType.String);
            arParams[28].Value = phoneNumber;

            arParams[29] = new SqliteParameter(":PhoneNumberConfirmed", DbType.Int32);
            arParams[29].Value = phoneNumberConfirmed ? 1 :0;

            arParams[30] = new SqliteParameter(":TwoFactorEnabled", DbType.Int32);
            arParams[30].Value = twoFactorEnabled ? 1 : 0;

            arParams[31] = new SqliteParameter(":LockoutEndDateUtc", DbType.DateTime);

            if (lockoutEndDateUtc == null)
            {
                arParams[31].Value = DBNull.Value;
            }
            else
            {
                arParams[31].Value = lockoutEndDateUtc;
            }

            arParams[32] = new SqliteParameter(":IsLockedOut", DbType.Int32);
            arParams[32].Value = isLockedOut ? 1 : 0;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        


        public bool DeleteUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }


        public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastActivityDate = :LastUpdate  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":LastUpdate", DbType.DateTime);
            arParams[1].Value = lastUpdate;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                    connectionString,
                    sqlCommand.ToString(),
                    arParams);

            return (rowsAffected > 0);

        }

        public bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastLoginDate = :LastLoginTime,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":LastLoginTime", DbType.DateTime);
            arParams[1].Value = lastLoginTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool AccountLockout(Guid userGuid, DateTime lockoutTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsLockedOut = 1,  ");
            sqlCommand.Append("LastLockoutDate = :LockoutTime  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":LockoutTime", DbType.DateTime);
            arParams[1].Value = lockoutTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("LastPasswordChangedDate = :LastPasswordChangedDate  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":LastPasswordChangedDate", DbType.DateTime);
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool UpdateFailedPasswordAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAttemptWindowStart = :FailedPasswordAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":FailedPasswordAttemptWindowStart", DbType.DateTime);
            arParams[1].Value = windowStartTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPasswordAttemptCount = :AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":AttemptCount", DbType.Int32);
            arParams[1].Value = attemptCount;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerWindowStart = :FailedPasswordAnswerAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":FailedPasswordAnswerAttemptWindowStart", DbType.DateTime);
            arParams[1].Value = windowStartTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool UpdateFailedPasswordAnswerAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = :AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":AttemptCount", DbType.Int32);
            arParams[1].Value = attemptCount;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = :RegisterConfirmGuid  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":RegisterConfirmGuid", DbType.String);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 0,  ");
            sqlCommand.Append("RegisterConfirmGuid = :EmptyGuid  ");
            sqlCommand.Append("WHERE RegisterConfirmGuid = :RegisterConfirmGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":EmptyGuid", DbType.String);
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new SqliteParameter(":RegisterConfirmGuid", DbType.String);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND LastActivityDate > :SinceTime ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SinceTime", DbType.DateTime);
            arParams[1].Value = sinceTime;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;
        }

        public DbDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND LastActivityDate > :SinceTime ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SinceTime", DbType.DateTime);
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND LastActivityDate > :SinceTime   ");
            sqlCommand.Append("ORDER BY LastActivityDate desc   ");
            sqlCommand.Append("LIMIT 50 ;   ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SinceTime", DbType.DateTime);
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public bool AccountClearLockout(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsLockedOut = 0,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        
        public bool FlagAsDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 1 ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool FlagAsNotDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 0 ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        
        public DbDataReader GetRolesByUser(int siteId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_Roles.RoleID As RoleID, ");
            sqlCommand.Append("mp_Roles.DisplayName As DisplayName, ");
            sqlCommand.Append("mp_Roles.RoleName As RoleName ");

            sqlCommand.Append("FROM	 mp_UserRoles ");

            sqlCommand.Append("INNER JOIN mp_Users ");
            sqlCommand.Append("ON mp_UserRoles.UserID = mp_Users.UserID ");

            sqlCommand.Append("INNER JOIN mp_Roles ");
            sqlCommand.Append("ON  mp_UserRoles.RoleID = mp_Roles.RoleID ");

            sqlCommand.Append("WHERE mp_Users.SiteID = :SiteID AND mp_Users.UserID = :UserID  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Value = userId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = :SiteID AND RegisterConfirmGuid = :RegisterConfirmGuid;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RegisterConfirmGuid", DbType.String);
            arParams[1].Value = registerConfirmGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }


        public DbDataReader GetSingleUser(int siteId, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = :SiteID AND LoweredEmail = :Email;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":Email", DbType.String);
            arParams[1].Value = email.ToLower();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetCrossSiteUserListByEmail(string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE LoweredEmail = :Email;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":Email", DbType.String);
            arParams[0].Value = email.ToLower();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSingleUserByLoginName(int siteId, string loginName, bool allowEmailFallback)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = :SiteID  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("LoginName = :LoginName ");
                sqlCommand.Append("OR Email = :LoginName ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND LoginName = :LoginName ");
            }

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":LoginName", DbType.String);
            arParams[1].Value = loginName;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSingleUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE UserID = :UserID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSingleUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid ;  ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public Guid GetUserGuidFromOpenId(
            int siteId,
            string openIduri)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND OpenIDURI = :OpenIDURI   ");
            sqlCommand.Append(" ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":OpenIDURI", DbType.String);
            arParams[1].Value = openIduri;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userGuid = new Guid(reader["UserGuid"].ToString());
                }
            }

            return userGuid;

        }

        public Guid GetUserGuidFromWindowsLiveId(
            int siteId,
            string windowsLiveId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND WindowsLiveID = :WindowsLiveID   ");
            sqlCommand.Append(" ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":WindowsLiveID", DbType.String);
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userGuid = new Guid(reader["UserGuid"].ToString());
                }
            }

            return userGuid;

        }

        public string LoginByEmail(int siteId, string email, string password)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM  mp_Users ");

            sqlCommand.Append("WHERE Email = :Email  ");
            sqlCommand.Append("AND SiteID = :SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = :Password;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":Email", DbType.String);
            arParams[1].Value = email;

            arParams[2] = new SqliteParameter(":Password", DbType.String);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userName = reader["Name"].ToString();
                }
            }

            return userName;
        }

        public string Login(int siteId, string loginName, string password)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM  mp_Users ");

            sqlCommand.Append("WHERE LoginName = :LoginName  ");
            sqlCommand.Append("AND SiteID = :SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = :Password;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":LoginName", DbType.String);
            arParams[1].Value = loginName;

            arParams[2] = new SqliteParameter(":Password", DbType.String);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userName = reader["Name"].ToString();
                }

            }

            return userName;
        }


        //public static DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();

        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserProperties ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("UserGuid = :UserGuid ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":UserGuid", DbType.String, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserGuid", typeof(String));
        //    dataTable.Columns.Add("PropertyName", typeof(String));
        //    dataTable.Columns.Add("PropertyValueString", typeof(String));
        //    dataTable.Columns.Add("PropertyValueBinary", typeof(object));

        //    using (IDataReader reader = AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams))
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dataTable.NewRow();
        //            row["UserGuid"] = reader["UserGuid"].ToString();
        //            row["PropertyName"] = reader["PropertyName"].ToString();
        //            row["PropertyValueString"] = reader["PropertyValueString"].ToString();
        //            row["PropertyValueBinary"] = reader["PropertyValueBinary"];
        //            dataTable.Rows.Add(row);
        //        }

        //    }

        //    return dataTable;

        //}

        public DbDataReader GetLazyLoadedProperty(Guid userGuid, String propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid AND PropertyName = :PropertyName  ");
            sqlCommand.Append("LIMIT 1 ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":PropertyName", DbType.String);
            arParams[1].Value = propertyName;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid AND PropertyName = :PropertyName ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":PropertyName", DbType.String);
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public void CreateProperty(
            Guid propertyId,
            Guid userGuid,
            String propertyName,
            String propertyValues,
            byte[] propertyValueb,
            DateTime lastUpdatedDate,
            bool isLazyLoaded)
        {

            byte lazy;
            if (isLazyLoaded)
            {
                lazy = 1;
            }
            else
            {
                lazy = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO mp_UserProperties (");
            sqlCommand.Append("PropertyID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("PropertyName, ");
            sqlCommand.Append("PropertyValueString, ");
            // sqlCommand.Append("PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":PropertyID, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":PropertyName, ");
            sqlCommand.Append(":PropertyValueString, ");
            // sqlCommand.Append(":PropertyValueBinary, ");
            sqlCommand.Append(":LastUpdatedDate, ");
            sqlCommand.Append(":IsLazyLoaded );");


            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":PropertyID", DbType.String);
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqliteParameter(":PropertyName", DbType.String);
            arParams[2].Value = propertyName;

            arParams[3] = new SqliteParameter(":PropertyValueString", DbType.Object);
            arParams[3].Value = propertyValues;

            arParams[4] = new SqliteParameter(":PropertyValueBinary", DbType.Object);
            arParams[4].Value = propertyValueb;

            arParams[5] = new SqliteParameter(":LastUpdatedDate", DbType.DateTime);
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new SqliteParameter(":IsLazyLoaded", DbType.Int32);
            arParams[6].Value = lazy;

            AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public void UpdateProperty(
            Guid userGuid,
            String propertyName,
            String propertyValues,
            byte[] propertyValueb,
            DateTime lastUpdatedDate,
            bool isLazyLoaded)
        {
            byte lazy;
            if (isLazyLoaded)
            {
                lazy = 1;
            }
            else
            {
                lazy = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_UserProperties ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("PropertyValueString = :PropertyValueString, ");
            //sqlCommand.Append("PropertyValueBinary = :PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate = :LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded = :IsLazyLoaded ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = :UserGuid AND PropertyName = :PropertyName ;");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SqliteParameter(":PropertyName", DbType.String);
            arParams[1].Value = propertyName;

            arParams[2] = new SqliteParameter(":PropertyValueString", DbType.Object);
            arParams[2].Value = propertyValues;

            arParams[3] = new SqliteParameter(":PropertyValueBinary", DbType.Object);
            arParams[3].Value = propertyValueb;

            arParams[4] = new SqliteParameter(":LastUpdatedDate", DbType.DateTime);
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new SqliteParameter(":IsLazyLoaded", DbType.Int32);
            arParams[5].Value = lazy;

            AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);


        }

        public bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

    }
}
