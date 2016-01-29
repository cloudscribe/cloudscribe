// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-29
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{

    internal class DBSiteUser
    {
        internal DBSiteUser(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have MySqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(MySqlClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public DbDataReader GetUserCountByYearMonth(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(DateCreated) As Y,  ");
            sqlCommand.Append("MONTH(DateCreated) As M, ");
            sqlCommand.Append("CONCAT(YEAR(DateCreated), '-', MONTH(DateCreated)) As Label, ");
            sqlCommand.Append("COUNT(*) As Users ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = ?SiteID ");
            sqlCommand.Append("GROUP BY YEAR(DateCreated), MONTH(DateCreated) ");
            sqlCommand.Append("ORDER BY YEAR(DateCreated), MONTH(DateCreated) ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public DbDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("PasswordSalt, ");
            sqlCommand.Append("Pwd, ");
            sqlCommand.Append("Email ");
            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Email");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        //public DbDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        //{

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT ");
        //    sqlCommand.Append("UserID, ");
        //    sqlCommand.Append("UserGuid, ");
        //    sqlCommand.Append("Email, ");
        //    sqlCommand.Append("FirstName, ");
        //    sqlCommand.Append("LastName, ");
        //    sqlCommand.Append("Name As SiteUser ");

        //    sqlCommand.Append("FROM mp_Users ");

        //    sqlCommand.Append("WHERE SiteID = ?SiteID ");
        //    sqlCommand.Append("AND IsDeleted = 0 ");
        //    sqlCommand.Append("AND (");
        //    sqlCommand.Append("(Name LIKE ?Query) ");
        //    sqlCommand.Append("OR (FirstName LIKE ?Query) ");
        //    sqlCommand.Append("OR (LastName LIKE ?Query) ");
        //    sqlCommand.Append(") ");

        //    sqlCommand.Append("UNION ");

        //    sqlCommand.Append("SELECT ");
        //    sqlCommand.Append("UserID, ");
        //    sqlCommand.Append("UserGuid, ");
        //    sqlCommand.Append("Email, ");
        //    sqlCommand.Append("FirstName, ");
        //    sqlCommand.Append("LastName, ");
        //    sqlCommand.Append("Email As SiteUser ");

        //    sqlCommand.Append("FROM mp_Users ");
        //    sqlCommand.Append("WHERE SiteID = ?SiteID ");
        //    sqlCommand.Append("AND IsDeleted = 0 ");
        //    sqlCommand.Append("AND Email LIKE ?Query ");

        //    sqlCommand.Append("ORDER BY SiteUser ");
        //    sqlCommand.Append("LIMIT " + rowsToGet.ToString());
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new MySqlParameter("?Query", MySqlDbType.VarChar, 50);
        //    arParams[1].Value = query + "%";

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public DbDataReader EmailLookup(int siteId, string query, int rowsToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email ");

            sqlCommand.Append("FROM mp_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND (");
            sqlCommand.Append("(Email LIKE ?Query) ");
            sqlCommand.Append("OR (Name LIKE ?Query) ");
            sqlCommand.Append("OR (FirstName LIKE ?Query) ");
            sqlCommand.Append("OR (LastName LIKE ?Query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY Email ");
            sqlCommand.Append("LIMIT " + rowsToGet.ToString());
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?Query", MySqlDbType.VarChar, 50);
            arParams[1].Value = query + "%";

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public int UserCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public async Task<int> CountLockedOutUsers(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID AND IsLockedOut = 1;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;

        }

        public async Task<int> CountNotApprovedUsers(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID AND AccountApproved = 0;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;
        }

        public int UserCount(int siteId, String userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");

            if (userNameBeginsWith.Length == 1)
            {
                sqlCommand.Append("AND LEFT(Name, 1) = ?UserNameBeginsWith ");
            }

            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 1);
            arParams[1].Value = userNameBeginsWith;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND DateCreated >= ?BeginDate ");
            sqlCommand.Append("AND DateCreated < ?EndDate; ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Value = endDate;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        //public int CountOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID ");
        //    sqlCommand.Append("AND LastActivityDate > ?SinceTime ;  ");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new MySqlParameter("?SinceTime", MySqlDbType.DateTime);
        //    arParams[1].Value = sinceTime;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams).ToString());

        //    return count;

        //}

        //public DbDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = ?SiteID ");
        //    sqlCommand.Append("AND LastActivityDate >= ?SinceTime   ");
        //    sqlCommand.Append("AND DisplayInMemberList = 1 ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new MySqlParameter("?SinceTime", MySqlDbType.DateTime);
        //    arParams[1].Value = sinceTime;

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);


        //}

        //public DbDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = ?SiteID ");
        //    sqlCommand.Append("AND LastActivityDate >= ?SinceTime   ");
        //    sqlCommand.Append("ORDER BY LastActivityDate desc   ");
        //    sqlCommand.Append("LIMIT 50 ;   ");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new MySqlParameter("?SinceTime", MySqlDbType.DateTime);
        //    arParams[1].Value = sinceTime;

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public async Task<int> GetNewestUserId(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT MAX(UserID) FROM mp_Users WHERE SiteID = ?SiteID;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;
        }



        public async Task<int> CountUsers(
            int siteId, 
            string userNameBeginsWith,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND Name  LIKE ?UserNameBeginsWith ");
            }
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 50);
            arParams[1].Value = userNameBeginsWith + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;

        }

        public async Task<DbDataReader> GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

           
            sqlCommand.Append("SELECT	u.*  ");
            
            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE u.AccountApproved = 1   ");
            sqlCommand.Append("AND u.SiteID = ?SiteID   ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND u.Name LIKE ?UserNameBeginsWith ");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY u.DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY u.LastName, u.FirstName,  u.Name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY u.Name ");
                    break;
            }


            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];


            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 50);
            arParams[1].Value = userNameBeginsWith + "%";

            arParams[2] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[2].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


        public async Task<int> CountUsersForSearch(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;

        }

        public async Task<DbDataReader> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY LastName, FirstName,  Name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY Name ");
                    break;
            }

            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];


            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[2].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountUsersForAdminSearch(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = ?SiteID ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;

        }

        public async Task<DbDataReader> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (Email LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LastName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (FirstName LIKE ?SearchInput) ");
                sqlCommand.Append(")");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY LastName, FirstName,  Name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY Name ");
                    break;
            }

            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];


            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[2].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsLockedOut = 1 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {  
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("AccountApproved = 0 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountEmailUnconfirmed(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID AND EmailConfirmed = 0;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;
        }

        public async Task<DbDataReader> GetPageEmailUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("EmailConfirmed = 0 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountPhoneUnconfirmed(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID AND PhoneNumberConfirmed = 0;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;
        }

        public async Task<DbDataReader> GetPagePhoneUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("PhoneNumberConfirmed = 0 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountFutureLockoutDate(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND LockoutEndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND LockoutEndDateUtc > ?CurrentUtc ");
            sqlCommand.Append(";");


            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?CurrentUtc", MySqlDbType.DateTime);
            arParams[1].Value = DateTime.UtcNow;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;
        }

        public async Task<DbDataReader> GetFutureLockoutPage(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	mp_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");
            sqlCommand.Append("AND LockoutEndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND LockoutEndDateUtc > ?CurrentUtc ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?CurrentUtc", MySqlDbType.DateTime);
            arParams[2].Value = DateTime.UtcNow;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }




        public async Task<int> AddUser(
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
            DateTime? dateOfBirth,
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
            string comment,

            string normalizedUserName,
            string loweredEmail,
            bool canAutoLockout,

            CancellationToken cancellationToken
            )
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Users (");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("LoginName, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("TimeZoneId, ");
            sqlCommand.Append("MustChangePwd, ");
            sqlCommand.Append("RolesChanged, ");
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

            sqlCommand.Append("NormalizedUserName, ");
            sqlCommand.Append("LoweredEmail, ");
            sqlCommand.Append("CanAutoLockout, ");

            sqlCommand.Append("UserGuid");
            sqlCommand.Append(")");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append("?SiteGuid , ");
            sqlCommand.Append("?SiteID , ");
            sqlCommand.Append("?FullName , ");
            sqlCommand.Append("?LoginName , ");
            sqlCommand.Append("?Email , ");
            sqlCommand.Append("?FirstName, ");
            sqlCommand.Append("?LastName, ");
            sqlCommand.Append("?TimeZoneId, ");
            sqlCommand.Append("?MustChangePwd, ");
            sqlCommand.Append("0, "); //RolesChanged
            sqlCommand.Append("?DateCreated, ");
            sqlCommand.Append("?DateOfBirth, ");
            sqlCommand.Append("?EmailConfirmed, ");
            sqlCommand.Append("?PasswordHash, ");
            sqlCommand.Append("?SecurityStamp, ");
            sqlCommand.Append("?PhoneNumber, ");
            sqlCommand.Append("?PhoneNumberConfirmed, ");
            sqlCommand.Append("?TwoFactorEnabled, ");
            sqlCommand.Append("?LockoutEndDateUtc, ");
            sqlCommand.Append("?AccountApproved, ");
            sqlCommand.Append("?IsLockedOut, ");
            sqlCommand.Append("?DisplayInMemberList, ");
            sqlCommand.Append("?WebSiteURL, ");
            sqlCommand.Append("?Country, ");
            sqlCommand.Append("?State, ");
            sqlCommand.Append("?AvatarUrl, ");
            sqlCommand.Append("?Signature, ");
            sqlCommand.Append("?AuthorBio, ");
            sqlCommand.Append("?Comment, ");

            sqlCommand.Append("?NormalizedUserName, ");
            sqlCommand.Append("?LoweredEmail, ");
            sqlCommand.Append("?CanAutoLockout, ");

            sqlCommand.Append(" ?UserGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[32];

            arParams[0] = new MySqlParameter("?FullName", MySqlDbType.VarChar, 100);
            arParams[0].Value = fullName;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Value = loginName;

            arParams[2] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[2].Value = email;
            
            arParams[3] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[3].Value = siteId;

            arParams[4] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new MySqlParameter("?DateCreated", MySqlDbType.DateTime);
            arParams[5].Value = dateCreated;

            arParams[6] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[6].Value = siteGuid.ToString();

            arParams[7] = new MySqlParameter("?MustChangePwd", MySqlDbType.Int32);
            arParams[7].Value = mustChangePwd ? 1 : 0;

            arParams[8] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[8].Value = firstName;

            arParams[9] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[9].Value = lastName;

            arParams[10] = new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 100);
            arParams[10].Value = timeZoneId;
            
            arParams[11] = new MySqlParameter("?DateOfBirth", MySqlDbType.DateTime);
            if (!dateOfBirth.HasValue)
            {
                arParams[11].Value = DBNull.Value;
            }
            else
            {
                arParams[11].Value = dateOfBirth;
            }

            arParams[12] = new MySqlParameter("?EmailConfirmed", MySqlDbType.Int32);
            arParams[12].Value = emailConfirmed ? 1 : 0;
            
            arParams[13] = new MySqlParameter("?PasswordHash", MySqlDbType.Text);
            arParams[13].Value = passwordHash;

            arParams[14] = new MySqlParameter("?SecurityStamp", MySqlDbType.Text);
            arParams[14].Value = securityStamp;

            arParams[15] = new MySqlParameter("?PhoneNumber", MySqlDbType.VarChar, 50);
            arParams[15].Value = phoneNumber;

            arParams[16] = new MySqlParameter("?PhoneNumberConfirmed", MySqlDbType.Int32);
            arParams[16].Value = phoneNumberConfirmed ? 1 : 0;

            arParams[17] = new MySqlParameter("?TwoFactorEnabled", MySqlDbType.Int32);
            arParams[17].Value = twoFactorEnabled ? 1 : 0;

            arParams[18] = new MySqlParameter("?LockoutEndDateUtc", MySqlDbType.DateTime);
            if (!lockoutEndDateUtc.HasValue)
            {
                arParams[18].Value = DBNull.Value;
            }
            else
            {
                arParams[18].Value = lockoutEndDateUtc;
            }

            arParams[19] = new MySqlParameter("?AccountApproved", MySqlDbType.Int32);
            arParams[19].Value = accountApproved ? 1 : 0;

            arParams[20] = new MySqlParameter("?IsLockedOut", MySqlDbType.Int32);
            arParams[20].Value = isLockedOut ? 1 : 0;

            arParams[21] = new MySqlParameter("?DisplayInMemberList", MySqlDbType.Int32);
            arParams[21].Value = displayInMemberList ? 1 : 0;

            arParams[22] = new MySqlParameter("?WebSiteURL", MySqlDbType.VarChar, 100);
            arParams[22].Value = webSiteUrl;

            arParams[23] = new MySqlParameter("?Country", MySqlDbType.VarChar, 100);
            arParams[23].Value = country;

            arParams[24] = new MySqlParameter("?State", MySqlDbType.VarChar, 100);
            arParams[24].Value = state;

            arParams[25] = new MySqlParameter("?AvatarUrl", MySqlDbType.VarChar, 250);
            arParams[25].Value = avatarUrl;

            arParams[26] = new MySqlParameter("?Signature", MySqlDbType.Text);
            arParams[26].Value = signature;

            arParams[27] = new MySqlParameter("?AuthorBio", MySqlDbType.Text);
            arParams[27].Value = authorBio;

            arParams[28] = new MySqlParameter("?Comment", MySqlDbType.Text);
            arParams[28].Value = comment;

            arParams[29] = new MySqlParameter("?NormalizedUserName", MySqlDbType.VarChar, 50);
            arParams[29].Value = normalizedUserName;

            arParams[30] = new MySqlParameter("?LoweredEmail", MySqlDbType.VarChar, 50);
            arParams[30].Value = loweredEmail;

            arParams[31] = new MySqlParameter("?CanAutoLockout", MySqlDbType.Int32);
            arParams[31].Value = canAutoLockout ? 1 : 0;

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public async Task<bool> UpdateUser(
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
            bool rolesChanged,
            string authorBio,
            DateTime? dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc,
            bool isLockedOut,

            string normalizedUserName,
            bool newEmailApproved,
            bool canAutoLockout,
            DateTime? lastPasswordChangedDate,

            CancellationToken cancellationToken)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET Email = ?Email ,   ");
            sqlCommand.Append("Name = ?FullName,    ");
            sqlCommand.Append("LoginName = ?LoginName,    ");
            sqlCommand.Append("FirstName = ?FirstName,    ");
            sqlCommand.Append("LastName = ?LastName,    ");
            sqlCommand.Append("TimeZoneId = ?TimeZoneId,    ");
            sqlCommand.Append("NewEmail = ?NewEmail,    ");
            sqlCommand.Append("MustChangePwd = ?MustChangePwd,    ");
            sqlCommand.Append("RolesChanged = ?RolesChanged,    ");
            sqlCommand.Append("Gender = ?Gender,    ");
            sqlCommand.Append("AccountApproved = ?AccountApproved,    ");
            sqlCommand.Append("Trusted = ?Trusted,    ");
            sqlCommand.Append("DisplayInMemberList = ?DisplayInMemberList,    ");
            sqlCommand.Append("WebSiteURL = ?WebSiteURL,    ");
            sqlCommand.Append("Country = ?Country,    ");
            sqlCommand.Append("State = ?State,    ");
            sqlCommand.Append("AvatarUrl = ?AvatarUrl,    ");
            sqlCommand.Append("Signature = ?Signature,    ");
            sqlCommand.Append("LoweredEmail = ?LoweredEmail, ");
            sqlCommand.Append("Comment = ?Comment, ");
            sqlCommand.Append("AuthorBio = ?AuthorBio, ");
            sqlCommand.Append("DateOfBirth = ?DateOfBirth, ");
            sqlCommand.Append("EmailConfirmed = ?EmailConfirmed, ");
            sqlCommand.Append("PasswordHash = ?PasswordHash, ");
            sqlCommand.Append("SecurityStamp = ?SecurityStamp, ");
            sqlCommand.Append("PhoneNumber = ?PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed = ?PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled = ?TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc = ?LockoutEndDateUtc, ");

            sqlCommand.Append("NormalizedUserName = ?NormalizedUserName, ");
            sqlCommand.Append("NewEmailApproved = ?NewEmailApproved, ");
            sqlCommand.Append("CanAutoLockout = ?CanAutoLockout, ");
            sqlCommand.Append("LastPasswordChangedDate = ?LastPasswordChangedDate, ");

            sqlCommand.Append("IsLockedOut = ?IsLockedOut ");
            
            sqlCommand.Append("WHERE UserID = ?UserID ;");

            MySqlParameter[] arParams = new MySqlParameter[35];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[1].Value = email;
            
            arParams[2] = new MySqlParameter("?Gender", MySqlDbType.VarChar, 1);
            arParams[2].Value = gender;

            arParams[3] = new MySqlParameter("?AccountApproved", MySqlDbType.Int32);
            arParams[3].Value = accountApproved ? 1 : 0;
            
            arParams[4] = new MySqlParameter("?Trusted", MySqlDbType.Int32);
            arParams[4].Value = trusted ? 1 : 0;

            arParams[5] = new MySqlParameter("?DisplayInMemberList", MySqlDbType.Int32);
            arParams[5].Value = displayInMemberList ? 1 : 0;

            arParams[6] = new MySqlParameter("?WebSiteURL", MySqlDbType.VarChar, 100);
            arParams[6].Value = webSiteUrl;

            arParams[7] = new MySqlParameter("?Country", MySqlDbType.VarChar, 100);
            arParams[7].Value = country;

            arParams[8] = new MySqlParameter("?State", MySqlDbType.VarChar, 100);
            arParams[8].Value = state;
            
            arParams[9] = new MySqlParameter("?AvatarUrl", MySqlDbType.VarChar, 100);
            arParams[9].Value = avatarUrl;

            arParams[10] = new MySqlParameter("?Signature", MySqlDbType.Text);
            arParams[10].Value = signature;
            
            arParams[11] = new MySqlParameter("?FullName", MySqlDbType.VarChar, 100);
            arParams[11].Value = name;

            arParams[12] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[12].Value = loginName;

            arParams[13] = new MySqlParameter("?LoweredEmail", MySqlDbType.VarChar, 100);
            arParams[13].Value = loweredEmail;
            
            arParams[14] = new MySqlParameter("?Comment", MySqlDbType.Text);
            arParams[14].Value = comment;
            
            arParams[15] = new MySqlParameter("?MustChangePwd", MySqlDbType.Int32);
            arParams[15].Value = mustChangePwd ? 1 : 0;

            arParams[16] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[16].Value = firstName;

            arParams[17] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[17].Value = lastName;

            arParams[18] = new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 100);
            arParams[18].Value = timeZoneId;
            
            arParams[19] = new MySqlParameter("?NewEmail", MySqlDbType.VarChar, 100);
            arParams[19].Value = newEmail;
            
            arParams[20] = new MySqlParameter("?RolesChanged", MySqlDbType.Int32);
            arParams[20].Value = rolesChanged ? 1 : 0;

            arParams[21] = new MySqlParameter("?AuthorBio", MySqlDbType.Text);
            arParams[21].Value = authorBio;

            arParams[22] = new MySqlParameter("?DateOfBirth", MySqlDbType.DateTime);
            if (!dateOfBirth.HasValue)
            {
                arParams[22].Value = DBNull.Value;
            }
            else
            {
                arParams[22].Value = dateOfBirth;
            }

            arParams[23] = new MySqlParameter("?EmailConfirmed", MySqlDbType.Int32);
            arParams[23].Value = emailConfirmed ? 1 : 0;
            
            arParams[24] = new MySqlParameter("?PasswordHash", MySqlDbType.Text);
            arParams[24].Value = passwordHash;

            arParams[25] = new MySqlParameter("?SecurityStamp", MySqlDbType.Text);
            arParams[25].Value = securityStamp;

            arParams[26] = new MySqlParameter("?PhoneNumber", MySqlDbType.VarChar, 50);
            arParams[26].Value = phoneNumber;

            arParams[27] = new MySqlParameter("?PhoneNumberConfirmed", MySqlDbType.Int32);
            arParams[27].Value = phoneNumberConfirmed ? 1 : 0;

            arParams[28] = new MySqlParameter("?TwoFactorEnabled", MySqlDbType.Int32);
            arParams[28].Value = twoFactorEnabled ? 1 : 0;

            arParams[29] = new MySqlParameter("?LockoutEndDateUtc", MySqlDbType.DateTime);
            if (!lockoutEndDateUtc.HasValue)
            {
                arParams[29].Value = DBNull.Value;
            }
            else
            {
                arParams[29].Value = lockoutEndDateUtc;
            }

            arParams[30] = new MySqlParameter("?IsLockedOut", MySqlDbType.Int32);
            arParams[30].Value = isLockedOut ? 1 : 0;

            arParams[31] = new MySqlParameter("?NormalizedUserName", MySqlDbType.VarChar, 50);
            arParams[31].Value = normalizedUserName;

            arParams[32] = new MySqlParameter("?NewEmailApproved", MySqlDbType.Int32);
            arParams[32].Value = newEmailApproved ? 1 : 0;

            arParams[33] = new MySqlParameter("?CanAutoLockout", MySqlDbType.Int32);
            arParams[33].Value = canAutoLockout ? 1 : 0;

            arParams[34] = new MySqlParameter("?LastPasswordChangedDate", MySqlDbType.DateTime);
            if (!lastPasswordChangedDate.HasValue)
            {
                arParams[34].Value = DBNull.Value;
            }
            else
            {
                arParams[34].Value = lastPasswordChangedDate;
            }

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }


        public async Task<bool> DeleteUser(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> DeleteUsersBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = ?SiteID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        //public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET LastActivityDate = ?LastUpdate  ");
        //    sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    arParams[1] = new MySqlParameter("?LastUpdate", MySqlDbType.DateTime);
        //    arParams[1].Value = lastUpdate;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);

        //}

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid, 
            DateTime lastLoginTime,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastLoginDate = ?LastLoginTime,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LastLoginTime", MySqlDbType.DateTime);
            arParams[1].Value = lastLoginTime;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public async Task<bool> AccountLockout(
            Guid userGuid, 
            DateTime lockoutTime,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsLockedOut = 1,  ");
            sqlCommand.Append("LastLockoutDate = ?LockoutTime  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LockoutTime", MySqlDbType.DateTime);
            arParams[1].Value = lockoutTime;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("LastPasswordChangedDate = ?LastPasswordChangedDate  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LastPasswordChangedDate", MySqlDbType.DateTime);
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
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
            sqlCommand.Append("FailedPwdAttemptWindowStart = ?FailedPasswordAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?FailedPasswordAttemptWindowStart", MySqlDbType.DateTime);
            arParams[1].Value = windowStartTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPasswordAttemptCount = ?AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?AttemptCount", MySqlDbType.Int32);
            arParams[1].Value = attemptCount;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerWindowStart = ?FailedPasswordAnswerAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?FailedPasswordAnswerAttemptWindowStart", MySqlDbType.DateTime);
            arParams[1].Value = windowStartTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
            sqlCommand.Append("FailedPwdAnswerAttemptCount = ?AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?AttemptCount", MySqlDbType.Int32);
            arParams[1].Value = attemptCount;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> SetRegistrationConfirmationGuid(
            Guid userGuid, 
            Guid registrationConfirmationGuid,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = ?RegisterConfirmGuid  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public async Task<bool> ConfirmRegistration(
            Guid emptyGuid, 
            Guid registrationConfirmationGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 0,  ");
            sqlCommand.Append("EmailConfirmed = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = ?EmptyGuid  ");
            sqlCommand.Append("WHERE RegisterConfirmGuid = ?RegisterConfirmGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?EmptyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> AccountClearLockout(
            Guid userGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsLockedOut = 0,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        
        public async Task<bool> FlagAsDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 1 ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> FlagAsNotDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 0 ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        


        public async Task<DbDataReader> GetRolesByUser(
            int siteId, 
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_Roles.RoleID, ");
            sqlCommand.Append("mp_Roles.DisplayName, ");
            sqlCommand.Append("mp_Roles.RoleName ");

            sqlCommand.Append("FROM	 mp_UserRoles ");

            sqlCommand.Append("INNER JOIN mp_Users ");
            sqlCommand.Append("ON mp_UserRoles.UserID = mp_Users.UserID ");

            sqlCommand.Append("INNER JOIN mp_Roles ");
            sqlCommand.Append("ON  mp_UserRoles.RoleID = mp_Roles.RoleID ");

            sqlCommand.Append("WHERE mp_Users.SiteID = ?SiteID AND mp_Users.UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetUserByRegistrationGuid(
            int siteId, 
            Guid registerConfirmGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID AND RegisterConfirmGuid = ?RegisterConfirmGuid  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = registerConfirmGuid;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken
                );

        }


        public async Task<DbDataReader> GetSingleUser(
            int siteId, 
            string email,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID AND LoweredEmail = ?Email  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[1].Value = email.ToLower();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetCrossSiteUserListByEmail(
            string email,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE LoweredEmail = ?Email  ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[0].Value = email.ToLower();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetSingleUserByLoginName(
            int siteId, 
            string loginName, 
            bool allowEmailFallback,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("LoginName = ?LoginName ");
                sqlCommand.Append("OR Email = ?LoginName ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND LoginName = ?LoginName ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Value = loginName;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public DbDataReader GetSingleUserByLoginNameNonAsync(int siteId, string loginName, bool allowEmailFallback)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("LoginName = ?LoginName ");
                sqlCommand.Append("OR Email = ?LoginName ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND LoginName = ?LoginName ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Value = loginName;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> GetSingleUser(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE UserID = ?UserID ;  ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetSingleUser(
            Guid userGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid ;  ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public Guid GetUserGuidFromOpenId(
            int siteId,
            string openIdUri)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID  ");
            sqlCommand.Append("AND OpenIDURI = ?OpenIDURI   ");
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?OpenIDURI", MySqlDbType.VarChar, 255);
            arParams[1].Value = openIdUri;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
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
            sqlCommand.Append("SiteID = ?SiteID  ");
            sqlCommand.Append("AND WindowsLiveID = ?WindowsLiveID   ");
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?WindowsLiveID", MySqlDbType.VarChar, 36);
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
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
            sqlCommand.Append("SELECT Name ");

            sqlCommand.Append("FROM  mp_Users ");

            sqlCommand.Append("WHERE Email = ?Email  ");
            sqlCommand.Append("AND SiteID = ?SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = ?Password ;  ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[1].Value = email;

            arParams[2] = new MySqlParameter("?Password", MySqlDbType.Text);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
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
            sqlCommand.Append("SELECT Name ");

            sqlCommand.Append("FROM  mp_Users ");

            sqlCommand.Append("WHERE LoginName = ?LoginName  ");
            sqlCommand.Append("AND SiteID = ?SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = ?Password ;  ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Value = loginName;

            arParams[2] = new MySqlParameter("?Password", MySqlDbType.Text);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
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
        //    sqlCommand.Append("UserGuid = ?UserGuid ;");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserGuid", typeof(String));
        //    dataTable.Columns.Add("PropertyName", typeof(String));
        //    dataTable.Columns.Add("PropertyValueString", typeof(String));
        //    dataTable.Columns.Add("PropertyValueBinary", typeof(object));

        //    using (IDataReader reader = AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("UserGuid = ?UserGuid AND PropertyName = ?PropertyName  ");
            sqlCommand.Append("LIMIT 1 ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[1].Value = propertyName;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid AND PropertyName = ?PropertyName ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO mp_UserProperties (");
            sqlCommand.Append("PropertyID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("PropertyName, ");
            sqlCommand.Append("PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?PropertyID, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?PropertyName, ");
            sqlCommand.Append("?PropertyValueString, ");
            sqlCommand.Append("?PropertyValueBinary, ");
            sqlCommand.Append("?LastUpdatedDate, ");
            sqlCommand.Append("?IsLazyLoaded );");


            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?PropertyID", MySqlDbType.VarChar, 36);
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[2].Value = propertyName;

            arParams[3] = new MySqlParameter("?PropertyValueString", MySqlDbType.Text);
            arParams[3].Value = propertyValues;

            arParams[4] = new MySqlParameter("?PropertyValueBinary", MySqlDbType.LongBlob);
            arParams[4].Value = propertyValueb;

            arParams[5] = new MySqlParameter("?LastUpdatedDate", MySqlDbType.DateTime);
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new MySqlParameter("?IsLazyLoaded", MySqlDbType.Bit);
            arParams[6].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_UserProperties ");
            sqlCommand.Append("SET  ");
            //sqlCommand.Append("UserGuid = ?UserGuid, ");
            //sqlCommand.Append("PropertyName = ?PropertyName, ");
            sqlCommand.Append("PropertyValueString = ?PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary = ?PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate = ?LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded = ?IsLazyLoaded ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = ?UserGuid AND PropertyName = ?PropertyName ;");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[1].Value = propertyName;

            arParams[2] = new MySqlParameter("?PropertyValueString", MySqlDbType.Text);
            arParams[2].Value = propertyValues;

            arParams[3] = new MySqlParameter("?PropertyValueBinary", MySqlDbType.LongBlob);
            arParams[3].Value = propertyValueb;

            arParams[4] = new MySqlParameter("?LastUpdatedDate", MySqlDbType.DateTime);
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new MySqlParameter("?IsLazyLoaded", MySqlDbType.Bit);
            arParams[5].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


        }

        public bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }



    }
}
