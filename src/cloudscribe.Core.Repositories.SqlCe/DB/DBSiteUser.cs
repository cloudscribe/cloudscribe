// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2016-01-29
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Text;


namespace cloudscribe.Core.Repositories.SqlCe
{
    internal class DBSiteUser
    {
        internal DBSiteUser(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;

            // possibly will change this later to have SqlCeProviderFactory/DbProviderFactory injected
            AdoHelper = new SqlCeHelper(SqlCeProviderFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private SqlCeHelper AdoHelper;

        public DbDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetUserCountByYearMonth(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("DatePart(year,DateCreated) As Y, ");
            sqlCommand.Append("DatePart(month, DateCreated) As M, ");
            //TODO: this line causes an error, not sure what the solution is in SqlCE
            //sqlCommand.Append("(CONVERT(varchar(10),YEAR(DateCreated)) + '-' + CONVERT(varchar(3),MONTH(DateCreated))) As Label, ");
            sqlCommand.Append("'label' AS Label, ");
            sqlCommand.Append("COUNT(*) As Users ");

            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("DatePart(year, DateCreated), DatePart(month,DateCreated) ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("DatePart(year, DateCreated), DatePart(month,DateCreated) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        //public DbDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") t.* ");
        //    sqlCommand.Append("FROM ");
        //    sqlCommand.Append("(");

        //    sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") ");

        //    sqlCommand.Append("u1.UserID, ");
        //    sqlCommand.Append("u1.UserGuid, ");
        //    sqlCommand.Append("u1.Email, ");
        //    sqlCommand.Append("u1.FirstName, ");
        //    sqlCommand.Append("u1.LastName, ");
        //    sqlCommand.Append("u1.[Name] AS SiteUser ");

        //    sqlCommand.Append("FROM	mp_Users u1 ");
        //    sqlCommand.Append("WHERE ");

        //    sqlCommand.Append("u1.SiteID = @SiteID ");
        //    sqlCommand.Append("AND u1.IsDeleted = 0 ");
        //    sqlCommand.Append("AND ( ");
        //    sqlCommand.Append("(u1.[Name] LIKE @Query + '%') ");
        //    sqlCommand.Append("OR (u1.[FirstName] LIKE @Query + '%') ");
        //    sqlCommand.Append("OR (u1.[LastName] LIKE @Query + '%') ");
        //    sqlCommand.Append(") ");

        //    sqlCommand.Append("ORDER BY ");
        //    sqlCommand.Append("u1.[Name] ");

        //    sqlCommand.Append("UNION ");

        //    sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") ");

        //    sqlCommand.Append("u2.UserID, ");
        //    sqlCommand.Append("u2.UserGuid, ");
        //    sqlCommand.Append("u2.Email, ");
        //    sqlCommand.Append("u2.FirstName, ");
        //    sqlCommand.Append("u2.LastName, ");
        //    sqlCommand.Append("u2.[Email] AS SiteUser ");

        //    sqlCommand.Append("FROM	mp_Users u2 ");

        //    sqlCommand.Append("WHERE ");

        //    sqlCommand.Append("u2.SiteID = @SiteID ");
        //    sqlCommand.Append("AND u2.IsDeleted = 0 ");
        //    sqlCommand.Append("AND u2.[Email] LIKE @Query + '%' ");

        //    sqlCommand.Append("ORDER BY ");
        //    sqlCommand.Append("u2.[Email] ");

        //    sqlCommand.Append(") t ");

        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@Query", SqlDbType.NVarChar, 50);
        //    arParams[1].Value = query;

        //    return AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public DbDataReader EmailLookup(int siteId, string query, int rowsToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") ");

            sqlCommand.Append("u1.UserID, ");
            sqlCommand.Append("u1.UserGuid, ");
            sqlCommand.Append("u1.Email ");

            sqlCommand.Append("FROM	mp_Users u1 ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("u1.SiteID = @SiteID ");
            sqlCommand.Append("AND u1.IsDeleted = 0 ");
            sqlCommand.Append("AND ( ");
            sqlCommand.Append("(u1.[Email] LIKE @Query + '%') ");
            sqlCommand.Append("OR (u1.[Name] LIKE @Query + '%') ");
            sqlCommand.Append("OR (u1.[FirstName] LIKE @Query + '%') ");
            sqlCommand.Append("OR (u1.[LastName] LIKE @Query + '%') ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("u1.[Email] ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@Query", SqlDbType.NVarChar, 50);
            arParams[1].Value = query;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int UserCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public int CountLockedOutUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsLockedOut = 1 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public int CountNotApprovedUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("AccountApproved = 0 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND DateCreated Between @BeginDate And @EndDate ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[2].Value = endDate;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        //public int CountOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_Users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append("AND LastActivityDate > @SinceTime ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@SinceTime", SqlDbType.DateTime);
        //    arParams[1].Value = sinceTime;

        //    return Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams));

        //}

        //public DbDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_Users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append("AND LastActivityDate > @SinceTime ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@SinceTime", SqlDbType.DateTime);
        //    arParams[1].Value = sinceTime;

        //    return AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public DbDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT TOP(50)  * ");
        //    sqlCommand.Append("FROM	mp_Users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append("AND LastActivityDate > @SinceTime ");
        //    sqlCommand.Append("ORDER BY LastActivityDate desc ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@SinceTime", SqlDbType.DateTime);
        //    arParams[1].Value = sinceTime;

        //    return AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public int GetNewestUserId(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  MAX(UserID) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public int CountUsers(int siteId, String userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            if (!string.IsNullOrEmpty(userNameBeginsWith))
            {
                sqlCommand.Append("AND [Name]  LIKE @UserNameBeginsWith + '%' ");
            }
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 50);
            arParams[1].Value = userNameBeginsWith;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            //sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " AS TotalPages ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            if (!string.IsNullOrEmpty(userNameBeginsWith))
            {
                sqlCommand.Append("AND [Name]  LIKE @UserNameBeginsWith + '%' ");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append("ORDER BY LastName, FirstName, [Name]    ");
                    break;

                case 0:
                default:
                    sqlCommand.Append("ORDER BY [Name]   ");
                    break;
            }


            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("t2.[Name]  ");
            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY t2.DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append("ORDER BY t2.LastName, t2.FirstName, t2.[Name]    ");
                    break;

                case 0:
                default:
                    sqlCommand.Append("ORDER BY t2.[Name]   ");
                    break;
            }

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 50);
            arParams[1].Value = userNameBeginsWith;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountUsersForSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");

                sqlCommand.Append("([Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR ([LoginName]  LIKE '%' + @SearchInput + '%') ");

                sqlCommand.Append(")");
            }
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[1].Value = searchInput;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");
            //sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " AS TotalPages ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND AccountApproved = 1 ");
            sqlCommand.Append("AND DisplayInMemberList = 1 ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");

                sqlCommand.Append("([Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR ([LoginName]  LIKE '%' + @SearchInput + '%') ");

                sqlCommand.Append(")");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append("ORDER BY LastName, FirstName, [Name]   ");
                    break;

                case 0:
                default:
                    sqlCommand.Append("ORDER BY [Name]   ");
                    break;
            }

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("t2.[Name]  ");

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY t2.DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append("ORDER BY t2.LastName, t2.FirstName, t2.[Name]   ");
                    break;

                case 0:
                default:
                    sqlCommand.Append("ORDER BY t2.[Name]   ");
                    break;
            }

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[1].Value = searchInput;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountUsersForAdminSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");

            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");

                sqlCommand.Append("([Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR ([LoginName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (Email LIKE '%' + @SearchInput + '%') ");

                sqlCommand.Append(")");
            }
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[1].Value = searchInput;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");
            //sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " AS TotalPages ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");

            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");
                sqlCommand.Append("([Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR ([LoginName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (Email LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (LastName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (FirstName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append(")");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append("ORDER BY LastName, FirstName, [Name]   ");
                    break;

                case 0:
                default:
                    sqlCommand.Append("ORDER BY [Name]   ");
                    break;
            }

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("t2.[Name]  ");

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY t2.DateCreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append("ORDER BY t2.LastName, t2.FirstName, t2.[Name]   ");
                    break;

                case 0:
                default:
                    sqlCommand.Append("ORDER BY t2.[Name]   ");
                    break;
            }

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[1].Value = searchInput;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.IsLockedOut = 1 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[Name]  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.[Name]  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) 
                + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.AccountApproved = 0 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[Name]  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.[Name]  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountEmailUnconfirmed(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("EmailConfirmed = 0 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetPageEmailUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.EmailConfirmed = 0 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[Name]  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.[Name]  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountPhoneUnconfirmed(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("PhoneNumberConfirmed = 0 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetPagePhoneUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.PhoneNumberConfirmed = 0 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[Name]  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.[Name]  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int CountFutureLockoutDate(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND LockoutEndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND LockoutEndDateUtc > @CurrentUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@CurrentUtc", SqlDbType.DateTime);
            arParams[1].Value = DateTime.UtcNow;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetFutureLockoutPage(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteID ");
            sqlCommand.Append("AND LockoutEndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND LockoutEndDateUtc > @CurrentUtc ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[Name]  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.[Name] DESC ");

            sqlCommand.Append(") AS t2 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.[Name]  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@CurrentUtc", SqlDbType.DateTime);
            arParams[1].Value = DateTime.UtcNow;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
            bool canAutoLockout

            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Users ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("LoginName, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("LoweredEmail, ");
            sqlCommand.Append("AccountApproved, ");
            sqlCommand.Append("Trusted, ");
            sqlCommand.Append("DisplayInMemberList, ");
            sqlCommand.Append("DateCreated, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("FailedPasswordAttemptCount, ");
            sqlCommand.Append("IsLockedOut, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("RolesChanged, ");
            sqlCommand.Append("MustChangePwd, ");  
            sqlCommand.Append("DateOfBirth, ");
            sqlCommand.Append("EmailConfirmed, ");
            sqlCommand.Append("PasswordHash, ");
            sqlCommand.Append("SecurityStamp, ");
            sqlCommand.Append("PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc, ");      
            sqlCommand.Append("WebSiteURL, ");
            sqlCommand.Append("Country, ");
            sqlCommand.Append("State, ");
            sqlCommand.Append("AvatarUrl, ");
            sqlCommand.Append("Signature, ");
            sqlCommand.Append("AuthorBio, ");
            sqlCommand.Append("Comment, ");

            sqlCommand.Append("NormalizedUserName, ");
            sqlCommand.Append("CanAutoLockout, ");
            
            sqlCommand.Append("TimeZoneId ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@LoginName, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@LoweredEmail, ");
            sqlCommand.Append("@AccountApproved, ");
            sqlCommand.Append("@Trusted, ");
            sqlCommand.Append("@DisplayInMemberList, ");
            sqlCommand.Append("@DateCreated, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@IsDeleted, ");
            sqlCommand.Append("@FailedPasswordAttemptCount, ");
            sqlCommand.Append("@IsLockedOut, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FirstName, ");
            sqlCommand.Append("@LastName, ");
            sqlCommand.Append("0, "); //RolesChanged
            sqlCommand.Append("@MustChangePwd, ");
            sqlCommand.Append("@DateOfBirth, ");
            sqlCommand.Append("@EmailConfirmed, ");
            sqlCommand.Append("@PasswordHash, ");
            sqlCommand.Append("@SecurityStamp, ");
            sqlCommand.Append("@PhoneNumber, ");
            sqlCommand.Append("@PhoneNumberConfirmed, ");
            sqlCommand.Append("@TwoFactorEnabled, ");
            sqlCommand.Append("@LockoutEndDateUtc, ");
            sqlCommand.Append("@WebSiteURL, ");
            sqlCommand.Append("@Country, ");
            sqlCommand.Append("@State, ");
            sqlCommand.Append("@AvatarUrl, ");
            sqlCommand.Append("@Signature, ");
            sqlCommand.Append("@AuthorBio, ");
            sqlCommand.Append("@Comment, ");

            sqlCommand.Append("@NormalizedUserName, ");
            sqlCommand.Append("@CanAutoLockout, ");

            sqlCommand.Append("@TimeZoneId ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[35];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[1].Value = fullName;

            arParams[2] = new SqlCeParameter("@LoginName", SqlDbType.NVarChar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[3].Value = email.ToLower();

            arParams[4] = new SqlCeParameter("@LoweredEmail", SqlDbType.NVarChar, 100);
            arParams[4].Value = loweredEmail;

            arParams[5] = new SqlCeParameter("@AccountApproved", SqlDbType.Bit);
            arParams[5].Value = accountApproved;
            
            arParams[6] = new SqlCeParameter("@Trusted", SqlDbType.Bit);
            arParams[6].Value = false;

            arParams[7] = new SqlCeParameter("@DisplayInMemberList", SqlDbType.Bit);
            arParams[7].Value = displayInMemberList;
            
            arParams[8] = new SqlCeParameter("@DateCreated", SqlDbType.DateTime);
            arParams[8].Value = dateCreated;

            arParams[9] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Value = userGuid;

            arParams[10] = new SqlCeParameter("@IsDeleted", SqlDbType.Bit);
            arParams[10].Value = false;

            arParams[11] = new SqlCeParameter("@FailedPasswordAttemptCount", SqlDbType.Int);
            arParams[11].Value = 0;
            
            arParams[12] = new SqlCeParameter("@IsLockedOut", SqlDbType.Bit);
            arParams[12].Value = isLockedOut;

            arParams[13] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[13].Value = siteGuid;
            
            arParams[14] = new SqlCeParameter("@FirstName", SqlDbType.NVarChar, 100);
            arParams[14].Value = firstName;

            arParams[15] = new SqlCeParameter("@LastName", SqlDbType.NVarChar, 100);
            arParams[15].Value = lastName;
            
            arParams[16] = new SqlCeParameter("@MustChangePwd", SqlDbType.Bit);
            arParams[16].Value = mustChangePwd;

            arParams[17] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 32);
            arParams[17].Value = timeZoneId;
            
            arParams[18] = new SqlCeParameter("@DateOfBirth", SqlDbType.DateTime);
            if (!dateOfBirth.HasValue)
            {
                arParams[18].Value = DBNull.Value;
            }
            else
            {
                arParams[18].Value = dateOfBirth;
            }

            arParams[19] = new SqlCeParameter("@EmailConfirmed", SqlDbType.Bit);
            arParams[19].Value = emailConfirmed;
            
            arParams[20] = new SqlCeParameter("@PasswordHash", SqlDbType.NText);
            arParams[20].Value = passwordHash;

            arParams[21] = new SqlCeParameter("@SecurityStamp", SqlDbType.NText);
            arParams[21].Value = securityStamp;

            arParams[22] = new SqlCeParameter("@PhoneNumber", SqlDbType.NVarChar, 50);
            arParams[22].Value = phoneNumber;

            arParams[23] = new SqlCeParameter("@PhoneNumberConfirmed", SqlDbType.Bit);
            arParams[23].Value = phoneNumberConfirmed;

            arParams[24] = new SqlCeParameter("@TwoFactorEnabled", SqlDbType.Bit);
            arParams[24].Value = twoFactorEnabled;

            arParams[25] = new SqlCeParameter("@LockoutEndDateUtc", SqlDbType.DateTime);
            if (!lockoutEndDateUtc.HasValue)
            {
                arParams[25].Value = DBNull.Value;
            }
            else
            {
                arParams[25].Value = lockoutEndDateUtc;
            }

            arParams[26] = new SqlCeParameter("@WebSiteURL", SqlDbType.NVarChar, 100);
            arParams[26].Value = webSiteUrl;

            arParams[27] = new SqlCeParameter("@Country", SqlDbType.NVarChar, 100);
            arParams[27].Value = country;

            arParams[28] = new SqlCeParameter("@State", SqlDbType.NVarChar, 100);
            arParams[28].Value = state;

            arParams[29] = new SqlCeParameter("@AvatarUrl", SqlDbType.NVarChar, 250);
            arParams[29].Value = avatarUrl;

            arParams[30] = new SqlCeParameter("@Signature", SqlDbType.NText);
            arParams[30].Value = signature;

            arParams[31] = new SqlCeParameter("@AuthorBio", SqlDbType.NText);
            arParams[31].Value = authorBio;

            arParams[32] = new SqlCeParameter("@Comment", SqlDbType.NText);
            arParams[32].Value = comment;

            arParams[33] = new SqlCeParameter("@NormalizedUserName", SqlDbType.NVarChar, 250);
            arParams[33].Value = normalizedUserName;

            arParams[34] = new SqlCeParameter("@CanAutoLockout", SqlDbType.Bit);
            arParams[34].Value = canAutoLockout;

            int newId = Convert.ToInt32(AdoHelper.DoInsertGetIdentitiy(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


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
            DateTime? lastPasswordChangedDate
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("Name = @Name, ");
            sqlCommand.Append("LoginName = @LoginName, ");
            sqlCommand.Append("Email = @Email, ");
            sqlCommand.Append("LoweredEmail = @LoweredEmail, ");
            sqlCommand.Append("Gender = @Gender, ");
            sqlCommand.Append("AccountApproved = @AccountApproved, ");
            sqlCommand.Append("Trusted = @Trusted, ");
            sqlCommand.Append("DisplayInMemberList = @DisplayInMemberList, ");
            sqlCommand.Append("WebSiteURL = @WebSiteURL, ");
            sqlCommand.Append("Country = @Country, ");
            sqlCommand.Append("State = @State, ");
            sqlCommand.Append("AvatarUrl = @AvatarUrl, ");
            sqlCommand.Append("Signature = @Signature, ");
            sqlCommand.Append("Comment = @Comment, ");
            sqlCommand.Append("FirstName = @FirstName, ");
            sqlCommand.Append("LastName = @LastName, ");
            sqlCommand.Append("RolesChanged = @RolesChanged, ");
            sqlCommand.Append("MustChangePwd = @MustChangePwd, ");
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
            sqlCommand.Append("IsLockedOut = @IsLockedOut, ");

            sqlCommand.Append("NormalizedUserName = @NormalizedUserName, ");
            sqlCommand.Append("NewEmailApproved = @NewEmailApproved, ");
            sqlCommand.Append("CanAutoLockout = @CanAutoLockout, ");
            sqlCommand.Append("LastPasswordChangedDate = @LastPasswordChangedDate, ");

            sqlCommand.Append("TimeZoneId = @TimeZoneId ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[34];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[1].Value = name;

            arParams[2] = new SqlCeParameter("@LoginName", SqlDbType.NVarChar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[3].Value = email.ToLower();

            arParams[4] = new SqlCeParameter("@LoweredEmail", SqlDbType.NVarChar, 100);
            arParams[4].Value = loweredEmail;
            
            arParams[5] = new SqlCeParameter("@Gender", SqlDbType.NChar, 10);
            arParams[5].Value = gender;

            arParams[6] = new SqlCeParameter("@AccountApproved", SqlDbType.Bit);
            arParams[6].Value = accountApproved;
            
            arParams[7] = new SqlCeParameter("@Trusted", SqlDbType.Bit);
            arParams[7].Value = trusted;

            arParams[8] = new SqlCeParameter("@DisplayInMemberList", SqlDbType.Bit);
            arParams[8].Value = displayInMemberList;

            arParams[9] = new SqlCeParameter("@WebSiteURL", SqlDbType.NVarChar, 100);
            arParams[9].Value = webSiteUrl;

            arParams[10] = new SqlCeParameter("@Country", SqlDbType.NVarChar, 100);
            arParams[10].Value = country;

            arParams[11] = new SqlCeParameter("@State", SqlDbType.NVarChar, 100);
            arParams[11].Value = state;
            
            arParams[12] = new SqlCeParameter("@AvatarUrl", SqlDbType.NVarChar, 255);
            arParams[12].Value = avatarUrl;
            
            arParams[13] = new SqlCeParameter("@Signature", SqlDbType.NText);
            arParams[13].Value = signature;
            
            arParams[14] = new SqlCeParameter("@Comment", SqlDbType.NText);
            arParams[14].Value = comment;
            
            arParams[15] = new SqlCeParameter("@FirstName", SqlDbType.NVarChar, 100);
            arParams[15].Value = firstName;

            arParams[16] = new SqlCeParameter("@LastName", SqlDbType.NVarChar, 100);
            arParams[16].Value = lastName;
            
            arParams[17] = new SqlCeParameter("@MustChangePwd", SqlDbType.Bit);
            arParams[17].Value = mustChangePwd;

            arParams[18] = new SqlCeParameter("@NewEmail", SqlDbType.NVarChar, 100);
            arParams[18].Value = newEmail;
            
            arParams[19] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 32);
            arParams[19].Value = timeZoneId;
            
            arParams[20] = new SqlCeParameter("@RolesChanged", SqlDbType.Bit);
            arParams[20].Value = rolesChanged;

            arParams[21] = new SqlCeParameter("@AuthorBio", SqlDbType.NText);
            arParams[21].Value = authorBio;

            arParams[22] = new SqlCeParameter("@DateOfBirth", SqlDbType.DateTime);
            if (!dateOfBirth.HasValue)
            {
                arParams[22].Value = DBNull.Value;
            }
            else
            {
                arParams[22].Value = dateOfBirth;
            }

            arParams[23] = new SqlCeParameter("@EmailConfirmed", SqlDbType.Bit);
            arParams[23].Value = emailConfirmed;
            
            arParams[24] = new SqlCeParameter("@PasswordHash", SqlDbType.NText);
            arParams[24].Value = passwordHash;

            arParams[24] = new SqlCeParameter("@SecurityStamp", SqlDbType.NText);
            arParams[24].Value = securityStamp;

            arParams[25] = new SqlCeParameter("@PhoneNumber", SqlDbType.NVarChar, 50);
            arParams[25].Value = phoneNumber;

            arParams[26] = new SqlCeParameter("@PhoneNumberConfirmed", SqlDbType.Bit);
            arParams[26].Value = phoneNumberConfirmed;

            arParams[27] = new SqlCeParameter("@TwoFactorEnabled", SqlDbType.Bit);
            arParams[27].Value = twoFactorEnabled;

            arParams[28] = new SqlCeParameter("@LockoutEndDateUtc", SqlDbType.DateTime);
            if (!lockoutEndDateUtc.HasValue)
            {
                arParams[28].Value = DBNull.Value;
            }
            else
            {
                arParams[28].Value = lockoutEndDateUtc;
            }

            arParams[29] = new SqlCeParameter("@IsLockedOut", SqlDbType.Bit);
            arParams[29].Value = isLockedOut;

            arParams[30] = new SqlCeParameter("@NormalizedUserName", SqlDbType.NVarChar, 50);
            arParams[30].Value = normalizedUserName;

            arParams[31] = new SqlCeParameter("@NewEmailApproved", SqlDbType.Bit);
            arParams[31].Value = newEmailApproved;

            arParams[32] = new SqlCeParameter("@CanAutoLockout", SqlDbType.Bit);
            arParams[32].Value = canAutoLockout;

            arParams[33] = new SqlCeParameter("@LastPasswordChangedDate", SqlDbType.DateTime);
            if (!lastPasswordChangedDate.HasValue)
            {
                arParams[33].Value = DBNull.Value;
            }
            else
            {
                arParams[33].Value = lastPasswordChangedDate;
            }

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        

        public bool DeleteUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool DeleteUsersBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



        //public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("LastActivityDate = @LastActivityDate ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    arParams[1] = new SqlCeParameter("@LastActivityDate", SqlDbType.DateTime);
        //    arParams[1].Value = lastUpdate;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        public bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LastLoginDate = @LastLoginDate, ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@LastLoginDate", SqlDbType.DateTime);
            arParams[1].Value = lastLoginTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        //public bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("LastPasswordChangedDate = @LastPasswordChangedDate ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    arParams[1] = new SqlCeParameter("@LastPasswordChangedDate", SqlDbType.DateTime);
        //    arParams[1].Value = lastPasswordChangeTime;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        //public bool UpdateFailedPasswordAttemptStartWindow(
        //    Guid userGuid,
        //    DateTime windowStartTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("FailedPwdAnswerWindowStart = @FailedPwdAnswerWindowStart ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    arParams[1] = new SqlCeParameter("@FailedPwdAnswerWindowStart", SqlDbType.DateTime);
        //    arParams[1].Value = windowStartTime;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        public bool UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = @FailedPwdAnswerAttemptCount ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@FailedPwdAnswerAttemptCount", SqlDbType.Int);
            arParams[1].Value = attemptCount;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        //public bool UpdateFailedPasswordAnswerAttemptStartWindow(
        //    Guid userGuid,
        //    DateTime windowStartTime)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("FailedPwdAnswerWindowStart = @FailedPwdAnswerWindowStart ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    arParams[1] = new SqlCeParameter("@FailedPwdAnswerWindowStart", SqlDbType.DateTime);
        //    arParams[1].Value = windowStartTime;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        //public bool UpdateFailedPasswordAnswerAttemptCount(
        //    Guid userGuid,
        //    int attemptCount)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("FailedPwdAnswerAttemptCount = @FailedPwdAnswerAttemptCount ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    arParams[1] = new SqlCeParameter("@FailedPwdAnswerAttemptCount", SqlDbType.Int);
        //    arParams[1].Value = attemptCount;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);


        //}


        public bool AccountLockout(Guid userGuid, DateTime lockoutTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LastLockoutDate = @LastLockoutDate, ");
            sqlCommand.Append("IsLockedOut = 1 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@LastLockoutDate", SqlDbType.DateTime);
            arParams[1].Value = lockoutTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool AccountClearLockout(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0, ");
            sqlCommand.Append("IsLockedOut = 0 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        //public bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("RegisterConfirmGuid = @RegisterConfirmGuid, ");
        //    sqlCommand.Append("IsLockedOut = 1 ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    arParams[1] = new SqlCeParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier);
        //    arParams[1].Value = registrationConfirmationGuid;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        //public bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("RegisterConfirmGuid = @EmptyGuid, ");
        //    sqlCommand.Append("EmailConfirmed = 1,  ");
        //    sqlCommand.Append("IsLockedOut = 0 ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("RegisterConfirmGuid = @RegisterConfirmGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@EmptyGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = emptyGuid;

        //    arParams[1] = new SqlCeParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier);
        //    arParams[1].Value = registrationConfirmationGuid;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        

        public bool FlagAsDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("IsDeleted = 1 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool FlagAsNotDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("IsDeleted = 0 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        //public bool IncrementTotalPosts(int userId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("TotalPosts = TotalPosts + 1 ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserID = @UserID ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
        //    arParams[0].Value = userId;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        //public bool DecrementTotalPosts(int userId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_Users ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("TotalPosts = TotalPosts - 1 ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("UserID = @UserID ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
        //    arParams[0].Value = userId;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        public DbDataReader GetRolesByUser(int siteId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("mp_Roles.RoleName, ");
            sqlCommand.Append("mp_Roles.DisplayName, ");
            sqlCommand.Append("mp_Roles.RoleID ");
            sqlCommand.Append(" ");

            sqlCommand.Append("FROM	mp_UserRoles ");
            sqlCommand.Append("JOIN mp_Users  ");
            sqlCommand.Append("ON mp_UserRoles.UserID = mp_Users.UserID ");

            sqlCommand.Append("JOIN mp_Roles ");
            sqlCommand.Append("ON mp_UserRoles.RoleID = mp_Roles.RoleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mp_Users.SiteID = @SiteID ");
            sqlCommand.Append("AND mp_UserRoles.UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Value = userId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        //public DbDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_Users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append("AND RegisterConfirmGuid = @RegisterConfirmGuid ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier);
        //    arParams[1].Value = registerConfirmGuid;

        //    return AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public DbDataReader GetSingleUser(int siteId, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND LoweredEmail = @Email ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[1].Value = email.ToLower();

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public DbDataReader GetCrossSiteUserListByEmail(string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append(" LoweredEmail = @Email ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[0].Value = email.ToLower();

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public DbDataReader GetSingleUserByLoginName(int siteId, string loginName, bool allowEmailFallback)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");

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
                sqlCommand.Append("AND LoginName = @LoginName ");
            }


            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@LoginName", SqlDbType.NVarChar, 50);
            arParams[1].Value = loginName;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public DbDataReader GetSingleUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserID = @UserID ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetSingleUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        //public Guid GetUserGuidFromOpenId(
        //    int siteId,
        //    string openIduri)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_Users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append("AND OpenIDURI = @OpenIDURI ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@OpenIDURI", SqlDbType.NVarChar, 50);
        //    arParams[1].Value = openIduri;

        //    Guid userGuid = Guid.Empty;

        //    using (DbDataReader reader = AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams))
        //    {
        //        if (reader.Read())
        //        {
        //            userGuid = new Guid(reader["UserGuid"].ToString());
        //        }
        //    }

        //    return userGuid;

        //}

        //public Guid GetUserGuidFromWindowsLiveId(
        //    int siteId,
        //    string windowsLiveId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_Users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append("AND WindowsLiveID = @WindowsLiveID ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqlCeParameter("@WindowsLiveID", SqlDbType.NVarChar, 50);
        //    arParams[1].Value = windowsLiveId;

        //    Guid userGuid = Guid.Empty;

        //    using (DbDataReader reader = AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams))
        //    {
        //        if (reader.Read())
        //        {
        //            userGuid = new Guid(reader["UserGuid"].ToString());
        //        }
        //    }

        //    return userGuid;

        //}

        public string LoginByEmail(int siteId, string email, string password)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Email = @Email ");
            sqlCommand.Append("AND [Pwd] = @Password ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[1].Value = email;

            arParams[2] = new SqlCeParameter("@Password", SqlDbType.NVarChar, 1000);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND LoginName = @LoginName ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND [Pwd] = @Password ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@LoginName", SqlDbType.NVarChar, 50);
            arParams[1].Value = loginName;

            arParams[2] = new SqlCeParameter("@Password", SqlDbType.NVarChar, 1000);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserGuid", typeof(String));
        //    dataTable.Columns.Add("PropertyName", typeof(String));
        //    dataTable.Columns.Add("PropertyValueString", typeof(String));
        //    dataTable.Columns.Add("PropertyValueBinary", typeof(object));

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserProperties ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("UserGuid = @UserGuid ");
        //    sqlCommand.Append("AND IsLazyLoaded = 0 ");

        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = userGuid;

        //    using (IDataReader reader = AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
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
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("AND PropertyName = @PropertyName ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PropertyName", SqlDbType.NVarChar, 255);
            arParams[1].Value = propertyName;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("AND PropertyName = @PropertyName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PropertyName", SqlDbType.NVarChar, 255);
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public void CreateProperty(
            Guid propertyId,
            Guid userGuid,
            String propertyName,
            String propertyValue,
            byte[] propertyValueBinary,
            DateTime lastUpdateDate,
            bool isLazyLoaded)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserProperties ");
            sqlCommand.Append("(");
            sqlCommand.Append("PropertyID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("PropertyName, ");
            sqlCommand.Append("PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@PropertyID, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@PropertyName, ");
            sqlCommand.Append("@PropertyValueString, ");
            sqlCommand.Append("@PropertyValueBinary, ");
            sqlCommand.Append("@LastUpdatedDate, ");
            sqlCommand.Append("@IsLazyLoaded ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@PropertyID", SqlDbType.UniqueIdentifier);
            arParams[0].Value = propertyId;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Value = userGuid;

            arParams[2] = new SqlCeParameter("@PropertyName", SqlDbType.NVarChar, 255);
            arParams[2].Value = propertyName;

            arParams[3] = new SqlCeParameter("@PropertyValueString", SqlDbType.NText);
            arParams[3].Value = propertyValue;

            arParams[4] = new SqlCeParameter("@PropertyValueBinary", SqlDbType.Image);
            arParams[4].Value = propertyValueBinary;

            arParams[5] = new SqlCeParameter("@LastUpdatedDate", SqlDbType.DateTime);
            arParams[5].Value = lastUpdateDate;

            arParams[6] = new SqlCeParameter("@IsLazyLoaded", SqlDbType.Bit);
            arParams[6].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public void UpdateProperty(
            Guid userGuid,
            String propertyName,
            String propertyValue,
            byte[] propertyValueBinary,
            DateTime lastUpdateDate,
            bool isLazyLoaded)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_UserProperties ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("PropertyValueString = @PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary = @PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate = @LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded = @IsLazyLoaded ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("AND [PropertyName] = @PropertyName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PropertyName", SqlDbType.NVarChar, 255);
            arParams[1].Value = propertyName;

            arParams[2] = new SqlCeParameter("@PropertyValueString", SqlDbType.NText);
            arParams[2].Value = propertyValue;

            arParams[3] = new SqlCeParameter("@PropertyValueBinary", SqlDbType.Image);
            arParams[3].Value = propertyValueBinary;

            arParams[4] = new SqlCeParameter("@LastUpdatedDate", SqlDbType.DateTime);
            arParams[4].Value = lastUpdateDate;

            arParams[5] = new SqlCeParameter("@IsLazyLoaded", SqlDbType.Bit);
            arParams[5].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

    }
}
