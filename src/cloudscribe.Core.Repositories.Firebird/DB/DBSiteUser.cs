// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-29
// 

using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
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

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

        public DbDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, Name, PasswordSalt, Pwd, Email FROM mp_Users WHERE SiteID = @SiteID ORDER BY Email");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetUserCountByYearMonth(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("EXTRACT(YEAR FROM DateCreated) As Y,  ");
            sqlCommand.Append("EXTRACT(MONTH FROM DateCreated) As M, ");
            sqlCommand.Append("EXTRACT(YEAR FROM DateCreated) || '-' || EXTRACT(MONTH FROM DateCreated) As Label, ");
            sqlCommand.Append("COUNT(*) As Users ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("GROUP BY EXTRACT(YEAR FROM DateCreated), EXTRACT(MONTH FROM DateCreated) ");
            sqlCommand.Append("ORDER BY EXTRACT(YEAR FROM DateCreated), EXTRACT(MONTH FROM DateCreated) ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        

        public DbDataReader EmailLookup(int siteId, string query, int rowsToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT First " + rowsToGet.ToString());

            sqlCommand.Append(" UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("Name As SiteUser ");

            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND (");
            sqlCommand.Append("(Email LIKE @Query) ");
            sqlCommand.Append("OR (Name LIKE @Query) ");
            sqlCommand.Append("OR (FirstName LIKE @Query) ");
            sqlCommand.Append("OR (LastName LIKE @Query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY Email; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@Query", FbDbType.VarChar, 50);
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
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = @SiteID");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID AND IsLockedOut = 1;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID AND AccountApproved = 0;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");

            if (userNameBeginsWith.Length == 1)
            {
                sqlCommand.Append("AND LEFT(Name, 1) = @UserNameBeginsWith ");
            }

            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UserNameBeginsWith", FbDbType.VarChar, 1);
            arParams[1].Value = userNameBeginsWith;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND DateCreated >= @BeginDate ");
            sqlCommand.Append("AND DateCreated < @EndDate; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@EndDate", FbDbType.TimeStamp);
            arParams[2].Value = endDate;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        //public int CountOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID ");
        //    sqlCommand.Append("AND LastActivityDate > @SinceTime ;  ");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new FbParameter("@SinceTime", FbDbType.TimeStamp);
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
        //    sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = @SiteID ");
        //    sqlCommand.Append("AND LastActivityDate >= @SinceTime ;  ");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new FbParameter("@SinceTime", FbDbType.TimeStamp);
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
        //    sqlCommand.Append("SELECT FIRST 50 * FROM mp_Users WHERE SiteID = @SiteID ");
        //    sqlCommand.Append("AND LastActivityDate >= @SinceTime   ");
        //    sqlCommand.Append("ORDER BY LastActivityDate desc   ");
        //    sqlCommand.Append(" ;   ");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new FbParameter("@SinceTime", FbDbType.TimeStamp);
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
            sqlCommand.Append("SELECT MAX(UserID) FROM mp_Users WHERE SiteID = @SiteID;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND UPPER(Name)  LIKE UPPER(@UserNameBeginsWith) ");
            }
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UserNameBeginsWith", FbDbType.VarChar, 50);
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

            int skip = pageSize * (pageNumber - 1);

            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" u.*  ");

            //sqlCommand.Append(totalPages.ToString() + " As TotalPages  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE u.AccountApproved = 1   ");
            sqlCommand.Append("AND u.SiteID = @SiteID   ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND UPPER(u.Name) LIKE UPPER(@UserNameBeginsWith) ");
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


            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UserNameBeginsWith", FbDbType.VarChar, 50);
            arParams[1].Value = userNameBeginsWith + "%";

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
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (UPPER(Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(LoginName) LIKE UPPER(@SearchInput)) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (UPPER(Email) LIKE UPPER(@SearchInput)) ");

                sqlCommand.Append(")");


            }
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50); ;
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

            int skip = pageSize * (pageNumber - 1);

            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" *  ");


            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = @SiteID   ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (UPPER(Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(LoginName) LIKE UPPER(@SearchInput)) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (UPPER(Email) LIKE UPPER(@SearchInput)) ");

                sqlCommand.Append(")");
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

            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

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
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = @SiteID ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (UPPER(Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(LoginName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(Email) LIKE UPPER(@SearchInput)) ");

                sqlCommand.Append(")");


            }
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
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
            StringBuilder sqlCommand = new StringBuilder();

            int skip = pageSize * (pageNumber - 1);

            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" *  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = @SiteID   ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (UPPER(Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(LoginName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(Email) LIKE UPPER(@SearchInput)) ");

                sqlCommand.Append(")");
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

            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

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
            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" u.*  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("u.SiteID = @SiteID   ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.IsLockedOut = 1 ");

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" u.*  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("u.SiteID = @SiteID   ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.AccountApproved = 0 ");

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID AND EmailConfirmed = 0;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" u.*  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("u.SiteID = @SiteID   ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.EmailConfirmed = 0 ");

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID AND PhoneNumberConfirmed = 0;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" u.*  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("u.SiteID = @SiteID   ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.PhoneNumberConfirmed = 0 ");

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

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
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND LockoutEndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND LockoutEndDateUtc > @CurrentUtc");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@CurrentUtc", FbDbType.TimeStamp);
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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append(" u.*  ");

            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("u.SiteID = @SiteID   ");
            sqlCommand.Append("AND LockoutEndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND LockoutEndDateUtc > @CurrentUtc");

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@CurrentUtc", FbDbType.TimeStamp);
            arParams[1].Value = DateTime.UtcNow;

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
            
            FbParameter[] arParams = new FbParameter[36];

            arParams[0] = new FbParameter(":SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter(":Name", FbDbType.VarChar, 100);
            arParams[1].Value = fullName;

            arParams[2] = new FbParameter(":LoginName", FbDbType.VarChar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new FbParameter(":Email", FbDbType.VarChar, 100);
            arParams[3].Value = email;

            arParams[4] = new FbParameter(":LoweredEmail", FbDbType.VarChar, 100);
            arParams[4].Value = email.ToLower();
            
            arParams[5] = new FbParameter(":Gender", FbDbType.Char, 10);
            arParams[5].Value = string.Empty;

            arParams[6] = new FbParameter(":AccountApproved", FbDbType.SmallInt);
            arParams[6].Value = accountApproved ? 1 : 0;
            
            arParams[7] = new FbParameter(":Trusted", FbDbType.SmallInt);
            arParams[7].Value = 0;

            arParams[8] = new FbParameter(":DisplayInMemberList", FbDbType.SmallInt);
            arParams[8].Value = displayInMemberList ? 1 : 0;

            arParams[9] = new FbParameter(":WebSiteURL", FbDbType.VarChar, 100);
            arParams[9].Value = webSiteUrl;

            arParams[10] = new FbParameter(":Country", FbDbType.VarChar, 100);
            arParams[10].Value = country;

            arParams[11] = new FbParameter(":State", FbDbType.VarChar, 100);
            arParams[11].Value = state;
            
            arParams[12] = new FbParameter(":AvatarUrl", FbDbType.VarChar, 255);
            arParams[12].Value = avatarUrl;
            
            arParams[13] = new FbParameter(":Signature", FbDbType.VarChar, 4000);
            arParams[13].Value = signature;

            arParams[14] = new FbParameter(":DateCreated", FbDbType.TimeStamp);
            arParams[14].Value = dateCreated;

            arParams[15] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[15].Value = userGuid.ToString();
            
            arParams[16] = new FbParameter(":IsDeleted", FbDbType.SmallInt);
            arParams[16].Value = 0;

            arParams[17] = new FbParameter(":FailedPasswordAttemptCount", FbDbType.Integer);
            arParams[17].Value = 0;
            
            arParams[18] = new FbParameter(":IsLockedOut", FbDbType.SmallInt);
            arParams[18].Value = isLockedOut ? 1 : 0;
            
            arParams[19] = new FbParameter(":Comment", FbDbType.VarChar);
            arParams[19].Value = comment;

            arParams[20] = new FbParameter(":SiteGuid", FbDbType.Char, 36);
            arParams[20].Value = siteGuid.ToString();

            arParams[21] = new FbParameter(":MustChangePwd", FbDbType.Integer);
            arParams[21].Value = mustChangePwd ? 1 : 0;

            arParams[22] = new FbParameter(":FirstName", FbDbType.VarChar, 100);
            arParams[22].Value = firstName;

            arParams[23] = new FbParameter(":LastName", FbDbType.VarChar, 100);
            arParams[23].Value = lastName;
            
            arParams[24] = new FbParameter(":TimeZoneId", FbDbType.VarChar, 32);
            arParams[24].Value = timeZoneId;

            arParams[25] = new FbParameter(":DateOfBirth", FbDbType.TimeStamp);
            if (!dateOfBirth.HasValue)
            {
                arParams[25].Value = DBNull.Value;
            }
            else
            {
                arParams[25].Value = dateOfBirth;
            }

            
            arParams[26] = new FbParameter(":EmailConfirmed", FbDbType.Integer);
            arParams[26].Value = emailConfirmed ? 1 : 0;

            arParams[27] = new FbParameter(":PasswordHash", FbDbType.VarChar);
            arParams[27].Value = passwordHash;

            arParams[28] = new FbParameter(":SecurityStamp", FbDbType.VarChar);
            arParams[28].Value = securityStamp;

            arParams[29] = new FbParameter(":PhoneNumber", FbDbType.VarChar, 50);
            arParams[29].Value = phoneNumber;

            arParams[30] = new FbParameter(":PhoneNumberConfirmed", FbDbType.Integer);
            arParams[30].Value = phoneNumberConfirmed ? 1 : 0;

            arParams[31] = new FbParameter(":TwoFactorEnabled", FbDbType.Integer);
            arParams[31].Value = twoFactorEnabled ? 1 : 0;

            arParams[32] = new FbParameter(":LockoutEndDateUtc", FbDbType.TimeStamp);
            if (!lockoutEndDateUtc.HasValue)
            {
                arParams[32].Value = DBNull.Value;
            }
            else
            {
                arParams[32].Value = lockoutEndDateUtc;
            }

            arParams[33] = new FbParameter(":AuthorBio", FbDbType.VarChar);
            arParams[33].Value = authorBio;

            arParams[34] = new FbParameter(":NormalizedUserName", FbDbType.VarChar, 50);
            arParams[34].Value = normalizedUserName;

            arParams[35] = new FbParameter(":CanAutoLockout", FbDbType.Integer);
            arParams[35].Value = canAutoLockout ? 1 : 0;

            string statement = "EXECUTE PROCEDURE MP_USERS_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")";

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.StoredProcedure,
                statement,
                true,
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public async Task<bool> UpdateUser(
            int userId,
            string fullName,
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

            CancellationToken cancellationToken
            )
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET Email = @Email ,   ");
            sqlCommand.Append("Name = @FullName,    ");
            sqlCommand.Append("LoginName = @LoginName,    ");
            
            sqlCommand.Append("Gender = @Gender,    ");
            sqlCommand.Append("AccountApproved = @AccountApproved,    ");
            sqlCommand.Append("Trusted = @Trusted,    ");
            sqlCommand.Append("DisplayInMemberList = @DisplayInMemberList,    ");
            sqlCommand.Append("WebSiteURL = @WebSiteURL,    ");
            sqlCommand.Append("Country = @Country,    ");
            sqlCommand.Append("State = @State,    ");
            sqlCommand.Append("AvatarUrl = @AvatarUrl,    ");
            sqlCommand.Append("Signature = @Signature,    ");
            sqlCommand.Append("LoweredEmail = @LoweredEmail,    ");
            sqlCommand.Append("MustChangePwd = @MustChangePwd,    ");
            sqlCommand.Append("RolesChanged = @RolesChanged,    ");
            sqlCommand.Append("Comment = @Comment,    ");
            
            sqlCommand.Append("FirstName = @FirstName, ");
            sqlCommand.Append("LastName = @LastName, ");
            sqlCommand.Append("TimeZoneId = @TimeZoneId, ");
            sqlCommand.Append("NewEmail = @NewEmail, ");
            sqlCommand.Append("AuthorBio = @AuthorBio, ");
            sqlCommand.Append("DateOfBirth = @DateOfBirth, ");

            
            sqlCommand.Append("EmailConfirmed = @EmailConfirmed, ");
            sqlCommand.Append("PasswordHash = @PasswordHash, ");
            sqlCommand.Append("SecurityStamp = @SecurityStamp, ");
            sqlCommand.Append("PhoneNumber = @PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed = @PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled = @TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc = @LockoutEndDateUtc, ");

            sqlCommand.Append("NormalizedUserName = @NormalizedUserName, ");
            sqlCommand.Append("NewEmailApproved = @NewEmailApproved, ");
            sqlCommand.Append("CanAutoLockout = @CanAutoLockout, ");
            sqlCommand.Append("LastPasswordChangedDate = @LastPasswordChangedDate, ");



            sqlCommand.Append("IsLockedOut = @IsLockedOut    ");

            sqlCommand.Append("WHERE UserID = @UserID ;");

            FbParameter[] arParams = new FbParameter[37];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[1].Value = email;

            arParams[2] = new FbParameter("@Gender", FbDbType.VarChar, 1);
            arParams[2].Value = gender;

            arParams[3] = new FbParameter("@AccountApproved", FbDbType.Integer);
            arParams[3].Value = accountApproved ? 1 : 0;
            
            arParams[4] = new FbParameter("@Trusted", FbDbType.Integer);
            arParams[4].Value = trusted ? 1 : 0;

            arParams[5] = new FbParameter("@DisplayInMemberList", FbDbType.Integer);
            arParams[5].Value = displayInMemberList ? 1 :0;

            arParams[6] = new FbParameter("@WebSiteURL", FbDbType.VarChar, 100);
            arParams[6].Value = webSiteUrl;

            arParams[7] = new FbParameter("@Country", FbDbType.VarChar, 100);
            arParams[7].Value = country;

            arParams[8] = new FbParameter("@State", FbDbType.VarChar, 100);
            arParams[8].Value = state;
            
            arParams[9] = new FbParameter("@AvatarUrl", FbDbType.VarChar, 100);
            arParams[9].Value = avatarUrl;

            arParams[10] = new FbParameter("@Signature", FbDbType.VarChar, 4000);
            arParams[10].Value = signature;
            
            arParams[11] = new FbParameter("@FullName", FbDbType.VarChar, 50);
            arParams[11].Value = fullName;

            arParams[12] = new FbParameter("@LoginName", FbDbType.VarChar, 50);
            arParams[12].Value = loginName;

            arParams[13] = new FbParameter("@LoweredEmail", FbDbType.VarChar, 100);
            arParams[13].Value = loweredEmail;
            
            arParams[14] = new FbParameter("@Comment", FbDbType.VarChar);
            arParams[14].Value = comment;
            
            arParams[15] = new FbParameter("@MustChangePwd", FbDbType.Integer);
            arParams[15].Value = mustChangePwd ? 1 : 0;

            arParams[16] = new FbParameter("@FirstName", FbDbType.VarChar, 100);
            arParams[16].Value = firstName;

            arParams[17] = new FbParameter("@LastName", FbDbType.VarChar, 100);
            arParams[17].Value = lastName;

            arParams[18] = new FbParameter("@TimeZoneId", FbDbType.VarChar, 32);
            arParams[18].Value = timeZoneId;
            
            arParams[19] = new FbParameter("@NewEmail", FbDbType.VarChar, 100);
            arParams[19].Value = newEmail;
            
            arParams[22] = new FbParameter("@AuthorBio", FbDbType.VarChar);
            arParams[22].Value = authorBio;

            arParams[23] = new FbParameter("@DateOfBirth", FbDbType.TimeStamp);
            if (!dateOfBirth.HasValue)
            {
                arParams[23].Value = DBNull.Value;
            }
            else
            {
                arParams[23].Value = dateOfBirth;
            }

            
            arParams[24] = new FbParameter("@EmailConfirmed", FbDbType.Integer);
            arParams[24].Value = emailConfirmed ? 1 : 0;

            arParams[25] = new FbParameter("@PasswordHash", FbDbType.VarChar);
            arParams[25].Value = passwordHash;

            arParams[26] = new FbParameter("@SecurityStamp", FbDbType.VarChar);
            arParams[26].Value = securityStamp;

            arParams[27] = new FbParameter("@PhoneNumber", FbDbType.VarChar, 50);
            arParams[27].Value = phoneNumber;

            arParams[28] = new FbParameter("@PhoneNumberConfirmed", FbDbType.Integer);
            arParams[28].Value = phoneNumberConfirmed ?1 : 0;

            arParams[29] = new FbParameter("@TwoFactorEnabled", FbDbType.Integer);
            arParams[29].Value = twoFactorEnabled ? 1 : 0;

            arParams[30] = new FbParameter("@LockoutEndDateUtc", FbDbType.TimeStamp);
            if (lockoutEndDateUtc == null)
            {
                arParams[30].Value = DBNull.Value;
            }
            else
            {
                arParams[30].Value = lockoutEndDateUtc;
            }

            arParams[31] = new FbParameter("@IsLockedOut", FbDbType.Integer);
            arParams[31].Value = isLockedOut ? 1 : 0;

            arParams[32] = new FbParameter("@RolesChanged", FbDbType.Integer);
            arParams[32].Value = rolesChanged ? 1 : 0;

            arParams[33] = new FbParameter("@NormalizedUserName", FbDbType.VarChar, 50);
            arParams[33].Value = normalizedUserName;

            arParams[34] = new FbParameter("@NewEmailApproved", FbDbType.Integer);
            arParams[34].Value = newEmailApproved ? 1 : 0;

            arParams[35] = new FbParameter("@CanAutoLockout", FbDbType.Integer);
            arParams[35].Value = canAutoLockout ? 1 : 0;

            arParams[36] = new FbParameter("@LastPasswordChangedDate", FbDbType.TimeStamp);
            if (lastPasswordChangedDate == null)
            {
                arParams[36].Value = DBNull.Value;
            }
            else
            {
                arParams[36].Value = lastPasswordChangedDate;
            }

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("WHERE SiteID = @SiteID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        //public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET LastActivityDate = @LastUpdate  ");
        //    sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    arParams[1] = new FbParameter("@LastUpdate", FbDbType.TimeStamp);
        //    arParams[1].Value = lastUpdate;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //            writeConnectionString,
        //            CommandType.Text,
        //            sqlCommand.ToString(),
        //            true,
        //            arParams);

        //    return (rowsAffected > 0);

        //}

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid, 
            DateTime lastLoginTime,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastLoginDate = @LastLoginTime,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@LastLoginTime", FbDbType.TimeStamp);
            arParams[1].Value = lastLoginTime;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("LastLockoutDate = @LockoutTime  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@LockoutTime", FbDbType.TimeStamp);
            arParams[1].Value = lockoutTime;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public bool UpdateLastPasswordChangeTime(
            Guid userGuid, 
            DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("LastPasswordChangedDate = @LastPasswordChangedDate  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@LastPasswordChangedDate", FbDbType.TimeStamp);
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("FailedPwdAttemptWindowStart = @FailedPasswordAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@FailedPasswordAttemptWindowStart", FbDbType.TimeStamp);
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("FailedPasswordAttemptCount = @AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@AttemptCount", FbDbType.Integer);
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("FailedPwdAnswerWindowStart = @FailedPasswordAnswerAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@FailedPasswordAnswerAttemptWindowStart", FbDbType.TimeStamp);
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("FailedPwdAnswerAttemptCount = @AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@AttemptCount", FbDbType.Integer);
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> SetRegistrationConfirmationGuid(
            Guid userGuid, 
            Guid registrationConfirmationGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = @RegisterConfirmGuid  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@RegisterConfirmGuid", FbDbType.VarChar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("RegisterConfirmGuid = @EmptyGuid  ");
            sqlCommand.Append("WHERE RegisterConfirmGuid = @RegisterConfirmGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@EmptyGuid", FbDbType.VarChar, 36);
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new FbParameter("@RegisterConfirmGuid", FbDbType.VarChar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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

            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        //public bool UpdatePasswordAndSalt(
        //    int userId,
        //    int pwdFormat,
        //    string password,
        //    string passwordSalt)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET Pwd = @Password,  ");
        //    sqlCommand.Append("PasswordSalt = @PasswordSalt,  ");
        //    sqlCommand.Append("PwdFormat = @PwdFormat  ");

        //    sqlCommand.Append("WHERE UserID = @UserID  ;");

        //    FbParameter[] arParams = new FbParameter[4];

        //    arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
        //    arParams[0].Value = userId;

        //    arParams[1] = new FbParameter("@Password", FbDbType.VarChar, 1000);
        //    arParams[1].Value = password;

        //    arParams[2] = new FbParameter("@PasswordSalt", FbDbType.VarChar, 128);
        //    arParams[2].Value = passwordSalt;

        //    arParams[3] = new FbParameter("@PwdFormat", FbDbType.Integer);
        //    arParams[3].Value = pwdFormat;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);

        //}

        //public bool UpdatePasswordQuestionAndAnswer(
        //    Guid userGuid,
        //    String passwordQuestion,
        //    String passwordAnswer)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET PasswordQuestion = @PasswordQuestion,  ");
        //    sqlCommand.Append("PasswordAnswer = @PasswordAnswer  ");

        //    sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

        //    FbParameter[] arParams = new FbParameter[3];

        //    arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    arParams[1] = new FbParameter("@PasswordQuestion", FbDbType.VarChar, 255);
        //    arParams[1].Value = passwordQuestion;

        //    arParams[2] = new FbParameter("@PasswordAnswer", FbDbType.VarChar, 255);
        //    arParams[2].Value = passwordAnswer;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);
        //}

        //public async Task<bool> UpdateTotalRevenue(Guid userGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
        //    sqlCommand.Append("SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = @UserGuid)  ");
        //    sqlCommand.Append(", 0) ");

        //    sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
        //        writeConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return rowsAffected > 0;

        //}

        //public async Task<bool> UpdateTotalRevenue()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
        //    sqlCommand.Append("SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = mp_Users.UserGuid)  ");
        //    sqlCommand.Append(", 0) ");

        //    sqlCommand.Append("  ;");

        //    int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
        //        writeConnectionString,
        //        sqlCommand.ToString(),
        //        null);

        //    return rowsAffected > 0;
        //}



        public async Task<bool> FlagAsDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 1 ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        //public bool IncrementTotalPosts(int userId)
        //{

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET	TotalPosts = TotalPosts + 1 ");
        //    sqlCommand.Append("WHERE UserID = @UserID  ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
        //    arParams[0].Value = userId;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);
        //}

        //public bool DecrementTotalPosts(int userId)
        //{

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET	TotalPosts = TotalPosts - 1 ");
        //    sqlCommand.Append("WHERE UserID = @UserID  ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
        //    arParams[0].Value = userId;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);
        //}

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

            sqlCommand.Append("WHERE mp_Users.SiteID = @SiteID AND mp_Users.UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
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

            sqlCommand.Append("WHERE SiteID = @SiteID AND RegisterConfirmGuid = @RegisterConfirmGuid  ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RegisterConfirmGuid", FbDbType.VarChar, 36);
            arParams[1].Value = registerConfirmGuid;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


        public async Task<DbDataReader> GetSingleUser(
            int siteId, 
            string email,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = @SiteID AND LoweredEmail = @Email  ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
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

            sqlCommand.Append("WHERE LoweredEmail = @Email  ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Email", FbDbType.VarChar, 100);
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

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("LoginName = @LoginName ");
                sqlCommand.Append("OR Email = @LoginName ");
                sqlCommand.Append("OR LoweredEmail = @LoweredLoginName ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("LoginName = @LoginName ");
            }

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@LoginName", FbDbType.VarChar, 50);
            arParams[1].Value = loginName;

            arParams[2] = new FbParameter("@LoweredLoginName", FbDbType.VarChar, 50);
            arParams[2].Value = loginName.ToLowerInvariant();

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

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("LoginName = @LoginName ");
                sqlCommand.Append("OR Email = @LoginName ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("LoginName = @LoginName ");
            }

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@LoginName", FbDbType.VarChar, 50);
            arParams[1].Value = loginName;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
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

            sqlCommand.Append("WHERE UserID = @UserID ;  ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
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
            sqlCommand.Append("WHERE UserGuid = @UserGuid ;  ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
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
            sqlCommand.Append("SiteID = @SiteID  ");
            sqlCommand.Append("AND OpenIDURI = @OpenIDURI   ");
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@OpenIDURI", FbDbType.VarChar, 255);
            arParams[1].Value = openIdUri;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
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
            sqlCommand.Append("SiteID = @SiteID  ");
            sqlCommand.Append("AND WindowsLiveID = @WindowsLiveID   ");
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@WindowsLiveID", FbDbType.VarChar, 36);
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
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

            sqlCommand.Append("WHERE Email = @Email  ");
            sqlCommand.Append("AND SiteID = @SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = @Password ;  ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[1].Value = email;

            arParams[2] = new FbParameter("@Password", FbDbType.VarChar, 1000);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
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

            sqlCommand.Append("WHERE LoginName = @LoginName  ");
            sqlCommand.Append("AND SiteID = @SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = @Password ;  ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@LoginName", FbDbType.VarChar, 50);
            arParams[1].Value = loginName;

            arParams[2] = new FbParameter("@Password", FbDbType.VarChar, 1000);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                 readConnectionString,
                 CommandType.Text,
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
        //    sqlCommand.Append("UserGuid = @UserGuid ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
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

            sqlCommand.Append("SELECT FIRST 1  * ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid AND PropertyName = @PropertyName  ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[1].Value = propertyName;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid AND PropertyName = @PropertyName ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
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
            sqlCommand.Append("@PropertyID, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@PropertyName, ");
            sqlCommand.Append("@PropertyValueString, ");
            sqlCommand.Append("@PropertyValueBinary, ");
            sqlCommand.Append("@LastUpdatedDate, ");
            sqlCommand.Append("@IsLazyLoaded );");


            FbParameter[] arParams = new FbParameter[7];

            arParams[0] = new FbParameter("@PropertyID", FbDbType.VarChar, 36);
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[2].Value = propertyName;

            arParams[3] = new FbParameter("@PropertyValueString", FbDbType.VarChar);
            arParams[3].Value = propertyValues;

            arParams[4] = new FbParameter("@PropertyValueBinary", FbDbType.Binary);
            arParams[4].Value = propertyValueb;

            arParams[5] = new FbParameter("@LastUpdatedDate", FbDbType.TimeStamp);
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new FbParameter("@IsLazyLoaded", FbDbType.SmallInt);
            arParams[6].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            //sqlCommand.Append("UserGuid = @UserGuid, ");
            //sqlCommand.Append("PropertyName = @PropertyName, ");
            sqlCommand.Append("PropertyValueString = @PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary = @PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate = @LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded = @IsLazyLoaded ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid AND PropertyName = @PropertyName ;");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[1].Value = propertyName;

            arParams[2] = new FbParameter("@PropertyValueString", FbDbType.VarChar);
            arParams[2].Value = propertyValues;

            arParams[3] = new FbParameter("@PropertyValueBinary", FbDbType.Binary);
            arParams[3].Value = propertyValueb;

            arParams[4] = new FbParameter("@LastUpdatedDate", FbDbType.TimeStamp);
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new FbParameter("@IsLazyLoaded", FbDbType.SmallInt);
            arParams[5].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams);


        }

        public bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams);

            return (rowsAffected > -1);

        }

    }
}
