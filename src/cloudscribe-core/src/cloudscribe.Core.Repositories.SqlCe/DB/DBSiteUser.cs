// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2015-06-23
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SqlCe;
using Microsoft.Framework.Logging;
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


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;


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

        public DbDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") t.* ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("(");

            sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") ");

            sqlCommand.Append("u1.UserID, ");
            sqlCommand.Append("u1.UserGuid, ");
            sqlCommand.Append("u1.Email, ");
            sqlCommand.Append("u1.FirstName, ");
            sqlCommand.Append("u1.LastName, ");
            sqlCommand.Append("u1.[Name] AS SiteUser ");

            sqlCommand.Append("FROM	mp_Users u1 ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("u1.SiteID = @SiteID ");
            sqlCommand.Append("AND u1.IsDeleted = 0 ");
            sqlCommand.Append("AND ( ");
            sqlCommand.Append("(u1.[Name] LIKE @Query + '%') ");
            sqlCommand.Append("OR (u1.[FirstName] LIKE @Query + '%') ");
            sqlCommand.Append("OR (u1.[LastName] LIKE @Query + '%') ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("u1.[Name] ");

            sqlCommand.Append("UNION ");

            sqlCommand.Append("SELECT TOP(" + rowsToGet.ToString(CultureInfo.InvariantCulture) + ") ");

            sqlCommand.Append("u2.UserID, ");
            sqlCommand.Append("u2.UserGuid, ");
            sqlCommand.Append("u2.Email, ");
            sqlCommand.Append("u2.FirstName, ");
            sqlCommand.Append("u2.LastName, ");
            sqlCommand.Append("u2.[Email] AS SiteUser ");

            sqlCommand.Append("FROM	mp_Users u2 ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("u2.SiteID = @SiteID ");
            sqlCommand.Append("AND u2.IsDeleted = 0 ");
            sqlCommand.Append("AND u2.[Email] LIKE @Query + '%' ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("u2.[Email] ");

            sqlCommand.Append(") t ");

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
            sqlCommand.Append("AND ProfileApproved = 1 ");
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
            sqlCommand.Append("ApprovedForForums = 0 ");
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

        public int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND LastActivityDate > @SinceTime ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SinceTime", SqlDbType.DateTime);
            arParams[1].Value = sinceTime;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND LastActivityDate > @SinceTime ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SinceTime", SqlDbType.DateTime);
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(50)  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND LastActivityDate > @SinceTime ");
            sqlCommand.Append("ORDER BY LastActivityDate desc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SinceTime", SqlDbType.DateTime);
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

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
            sqlCommand.Append("AND ProfileApproved = 1 ");
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
            sqlCommand.Append("AND ProfileApproved = 1 ");
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
            sqlCommand.Append("AND ProfileApproved = 1 ");
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
            sqlCommand.Append("AND ProfileApproved = 1 ");
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
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") u.* ");

            sqlCommand.Append("FROM	mp_Users u  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.ApprovedForForums = 0 ");

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

        public int AddUser(
            Guid siteGuid,
            int siteId,
            string fullName,
            String loginName,
            string email,
            string password,
            string passwordSalt,
            Guid userGuid,
            DateTime dateCreated,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            DateTime dateOfBirth,
            bool emailConfirmed,
            int pwdFormat,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Users ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("LoginName, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("LoweredEmail, ");
            sqlCommand.Append("ProfileApproved, ");
            sqlCommand.Append("ApprovedForForums, ");
            sqlCommand.Append("Trusted, ");
            sqlCommand.Append("DisplayInMemberList, ");
            sqlCommand.Append("TotalPosts, ");
            sqlCommand.Append("DateCreated, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("FailedPasswordAttemptCount, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount, ");
            sqlCommand.Append("IsLockedOut, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("TotalRevenue, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("Pwd, ");
            sqlCommand.Append("PasswordSalt, ");
            sqlCommand.Append("RolesChanged, ");
            sqlCommand.Append("MustChangePwd, ");
            sqlCommand.Append("EmailChangeGuid, ");
            sqlCommand.Append("PasswordResetGuid, ");
            sqlCommand.Append("DateOfBirth, ");

            sqlCommand.Append("EmailConfirmed, ");
            sqlCommand.Append("PwdFormat, ");
            sqlCommand.Append("PasswordHash, ");
            sqlCommand.Append("SecurityStamp, ");
            sqlCommand.Append("PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc, ");

            sqlCommand.Append("TimeZoneId ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@LoginName, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@LoweredEmail, ");
            sqlCommand.Append("@ProfileApproved, ");
            sqlCommand.Append("@ApprovedForForums, ");
            sqlCommand.Append("@Trusted, ");
            sqlCommand.Append("@DisplayInMemberList, ");
            sqlCommand.Append("@TotalPosts, ");
            sqlCommand.Append("@DateCreated, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@IsDeleted, ");
            sqlCommand.Append("@FailedPasswordAttemptCount, ");
            sqlCommand.Append("@FailedPwdAnswerAttemptCount, ");
            sqlCommand.Append("@IsLockedOut, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@TotalRevenue, ");
            sqlCommand.Append("@FirstName, ");
            sqlCommand.Append("@LastName, ");
            sqlCommand.Append("@Pwd, ");
            sqlCommand.Append("@PasswordSalt, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("@MustChangePwd, ");
            sqlCommand.Append("'00000000-0000-0000-0000-000000000000', ");
            sqlCommand.Append("'00000000-0000-0000-0000-000000000000', ");
            sqlCommand.Append("@DateOfBirth, ");

            sqlCommand.Append("@EmailConfirmed, ");
            sqlCommand.Append("@PwdFormat, ");
            sqlCommand.Append("@PasswordHash, ");
            sqlCommand.Append("@SecurityStamp, ");
            sqlCommand.Append("@PhoneNumber, ");
            sqlCommand.Append("@PhoneNumberConfirmed, ");
            sqlCommand.Append("@TwoFactorEnabled, ");
            sqlCommand.Append("@LockoutEndDateUtc, ");

            sqlCommand.Append("@TimeZoneId ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[33];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[1].Value = fullName;

            arParams[2] = new SqlCeParameter("@LoginName", SqlDbType.NVarChar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[3].Value = email.ToLower();

            arParams[4] = new SqlCeParameter("@LoweredEmail", SqlDbType.NVarChar, 100);
            arParams[4].Value = email.ToLower();

            arParams[5] = new SqlCeParameter("@ProfileApproved", SqlDbType.Bit);
            arParams[5].Value = true;

            arParams[6] = new SqlCeParameter("@ApprovedForForums", SqlDbType.Bit);
            arParams[6].Value = true;

            arParams[7] = new SqlCeParameter("@Trusted", SqlDbType.Bit);
            arParams[7].Value = false;

            arParams[8] = new SqlCeParameter("@DisplayInMemberList", SqlDbType.Bit);
            arParams[8].Value = true;

            arParams[9] = new SqlCeParameter("@TotalPosts", SqlDbType.Int);
            arParams[9].Value = 0;

            arParams[10] = new SqlCeParameter("@DateCreated", SqlDbType.DateTime);
            arParams[10].Value = dateCreated;

            arParams[11] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Value = userGuid;

            arParams[12] = new SqlCeParameter("@IsDeleted", SqlDbType.Bit);
            arParams[12].Value = false;

            arParams[13] = new SqlCeParameter("@FailedPasswordAttemptCount", SqlDbType.Int);
            arParams[13].Value = 0;

            arParams[14] = new SqlCeParameter("@FailedPwdAnswerAttemptCount", SqlDbType.Int);
            arParams[14].Value = 0;

            arParams[15] = new SqlCeParameter("@IsLockedOut", SqlDbType.Bit);
            arParams[15].Value = false;

            arParams[16] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[16].Value = siteGuid;

            arParams[17] = new SqlCeParameter("@TotalRevenue", SqlDbType.Decimal);
            arParams[17].Value = 0;

            arParams[18] = new SqlCeParameter("@FirstName", SqlDbType.NVarChar, 100);
            arParams[18].Value = firstName;

            arParams[19] = new SqlCeParameter("@LastName", SqlDbType.NVarChar, 100);
            arParams[19].Value = lastName;

            arParams[20] = new SqlCeParameter("@Pwd", SqlDbType.NVarChar, 1000);
            arParams[20].Value = password;

            arParams[21] = new SqlCeParameter("@MustChangePwd", SqlDbType.Bit);
            arParams[21].Value = mustChangePwd;

            arParams[22] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 32);
            arParams[22].Value = timeZoneId;

            arParams[23] = new SqlCeParameter("@PasswordSalt", SqlDbType.NVarChar, 128);
            arParams[23].Value = passwordSalt;

            arParams[24] = new SqlCeParameter("@DateOfBirth", SqlDbType.DateTime);
            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[24].Value = DBNull.Value;
            }
            else
            {
                arParams[24].Value = dateOfBirth;
            }

            arParams[25] = new SqlCeParameter("@EmailConfirmed", SqlDbType.Bit);
            arParams[25].Value = emailConfirmed;

            arParams[26] = new SqlCeParameter("@PwdFormat", SqlDbType.Int);
            arParams[26].Value = pwdFormat;

            arParams[27] = new SqlCeParameter("@PasswordHash", SqlDbType.NText);
            arParams[27].Value = passwordHash;

            arParams[28] = new SqlCeParameter("@SecurityStamp", SqlDbType.NText);
            arParams[28].Value = securityStamp;

            arParams[29] = new SqlCeParameter("@PhoneNumber", SqlDbType.NVarChar, 50);
            arParams[29].Value = phoneNumber;

            arParams[30] = new SqlCeParameter("@PhoneNumberConfirmed", SqlDbType.Bit);
            arParams[30].Value = phoneNumberConfirmed;

            arParams[31] = new SqlCeParameter("@TwoFactorEnabled", SqlDbType.Bit);
            arParams[31].Value = twoFactorEnabled;

            arParams[32] = new SqlCeParameter("@LockoutEndDateUtc", SqlDbType.DateTime);
            if (lockoutEndDateUtc == null)
            {
                arParams[32].Value = DBNull.Value;
            }
            else
            {
                arParams[32].Value = lockoutEndDateUtc;
            }


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
            string password,
            string passwordSalt,
            string gender,
            bool profileApproved,
            bool approvedForForums,
            bool trusted,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string occupation,
            string interests,
            string msn,
            string yahoo,
            string aim,
            string icq,
            string avatarUrl,
            string signature,
            string skin,
            string loweredEmail,
            string passwordQuestion,
            string passwordAnswer,
            string comment,
            int timeOffsetHours,
            string openIdUri,
            string windowsLiveId,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            string editorPreference,
            string newEmail,
            Guid emailChangeGuid,
            Guid passwordResetGuid,
            bool rolesChanged,
            string authorBio,
            DateTime dateOfBirth,
            bool emailConfirmed,
            int pwdFormat,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("Name = @Name, ");
            sqlCommand.Append("LoginName = @LoginName, ");
            sqlCommand.Append("Email = @Email, ");
            sqlCommand.Append("LoweredEmail = @LoweredEmail, ");
            sqlCommand.Append("PasswordQuestion = @PasswordQuestion, ");
            sqlCommand.Append("PasswordAnswer = @PasswordAnswer, ");
            sqlCommand.Append("Gender = @Gender, ");
            sqlCommand.Append("ProfileApproved = @ProfileApproved, ");
            sqlCommand.Append("ApprovedForForums = @ApprovedForForums, ");
            sqlCommand.Append("Trusted = @Trusted, ");
            sqlCommand.Append("DisplayInMemberList = @DisplayInMemberList, ");
            sqlCommand.Append("WebSiteURL = @WebSiteURL, ");
            sqlCommand.Append("Country = @Country, ");
            sqlCommand.Append("State = @State, ");
            sqlCommand.Append("Occupation = @Occupation, ");
            sqlCommand.Append("Interests = @Interests, ");
            sqlCommand.Append("MSN = @MSN, ");
            sqlCommand.Append("Yahoo = @Yahoo, ");
            sqlCommand.Append("AIM = @AIM, ");
            sqlCommand.Append("ICQ = @ICQ, ");
            sqlCommand.Append("AvatarUrl = @AvatarUrl, ");
            sqlCommand.Append("TimeOffsetHours = @TimeOffsetHours, ");
            sqlCommand.Append("Signature = @Signature, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("Comment = @Comment, ");
            sqlCommand.Append("OpenIDURI = @OpenIDURI, ");
            sqlCommand.Append("WindowsLiveID = @WindowsLiveID, ");
            sqlCommand.Append("FirstName = @FirstName, ");
            sqlCommand.Append("LastName = @LastName, ");
            sqlCommand.Append("Pwd = @Pwd, ");
            sqlCommand.Append("PasswordSalt = @PasswordSalt, ");
            sqlCommand.Append("RolesChanged = @RolesChanged, ");
            sqlCommand.Append("MustChangePwd = @MustChangePwd, ");
            sqlCommand.Append("NewEmail = @NewEmail, ");
            sqlCommand.Append("EditorPreference = @EditorPreference, ");
            sqlCommand.Append("EmailChangeGuid = @EmailChangeGuid, ");
            sqlCommand.Append("PasswordResetGuid = @PasswordResetGuid, ");
            sqlCommand.Append("AuthorBio = @AuthorBio, ");
            sqlCommand.Append("DateOfBirth = @DateOfBirth, ");

            sqlCommand.Append("EmailConfirmed = @EmailConfirmed, ");
            sqlCommand.Append("PwdFormat = @PwdFormat, ");
            sqlCommand.Append("PasswordHash = @PasswordHash, ");
            sqlCommand.Append("SecurityStamp = @SecurityStamp, ");
            sqlCommand.Append("PhoneNumber = @PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed = @PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled = @TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc = @LockoutEndDateUtc, ");

            sqlCommand.Append("TimeZoneId = @TimeZoneId ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[49];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[1].Value = name;

            arParams[2] = new SqlCeParameter("@LoginName", SqlDbType.NVarChar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[3].Value = email.ToLower();

            arParams[4] = new SqlCeParameter("@LoweredEmail", SqlDbType.NVarChar, 100);
            arParams[4].Value = loweredEmail.ToLower();

            arParams[5] = new SqlCeParameter("@PasswordQuestion", SqlDbType.NVarChar, 255);
            arParams[5].Value = passwordQuestion;

            arParams[6] = new SqlCeParameter("@PasswordAnswer", SqlDbType.NVarChar, 255);
            arParams[6].Value = passwordAnswer;

            arParams[7] = new SqlCeParameter("@Gender", SqlDbType.NChar, 10);
            arParams[7].Value = gender;

            arParams[8] = new SqlCeParameter("@ProfileApproved", SqlDbType.Bit);
            arParams[8].Value = profileApproved;

            arParams[9] = new SqlCeParameter("@ApprovedForForums", SqlDbType.Bit);
            arParams[9].Value = approvedForForums;

            arParams[10] = new SqlCeParameter("@Trusted", SqlDbType.Bit);
            arParams[10].Value = trusted;

            arParams[11] = new SqlCeParameter("@DisplayInMemberList", SqlDbType.Bit);
            arParams[11].Value = displayInMemberList;

            arParams[12] = new SqlCeParameter("@WebSiteURL", SqlDbType.NVarChar, 100);
            arParams[12].Value = webSiteUrl;

            arParams[13] = new SqlCeParameter("@Country", SqlDbType.NVarChar, 100);
            arParams[13].Value = country;

            arParams[14] = new SqlCeParameter("@State", SqlDbType.NVarChar, 100);
            arParams[14].Value = state;

            arParams[15] = new SqlCeParameter("@Occupation", SqlDbType.NVarChar, 100);
            arParams[15].Value = occupation;

            arParams[16] = new SqlCeParameter("@Interests", SqlDbType.NVarChar, 100);
            arParams[16].Value = interests;

            arParams[17] = new SqlCeParameter("@MSN", SqlDbType.NVarChar, 50);
            arParams[17].Value = msn;

            arParams[18] = new SqlCeParameter("@Yahoo", SqlDbType.NVarChar, 50);
            arParams[18].Value = yahoo;

            arParams[19] = new SqlCeParameter("@AIM", SqlDbType.NVarChar, 50);
            arParams[19].Value = aim;

            arParams[20] = new SqlCeParameter("@ICQ", SqlDbType.NVarChar, 50);
            arParams[20].Value = icq;

            arParams[21] = new SqlCeParameter("@AvatarUrl", SqlDbType.NVarChar, 255);
            arParams[21].Value = avatarUrl;

            arParams[22] = new SqlCeParameter("@TimeOffsetHours", SqlDbType.Int);
            arParams[22].Value = timeOffsetHours;

            arParams[23] = new SqlCeParameter("@Signature", SqlDbType.NText);
            arParams[23].Value = signature;

            arParams[24] = new SqlCeParameter("@Skin", SqlDbType.NVarChar, 100);
            arParams[24].Value = skin;

            arParams[25] = new SqlCeParameter("@Comment", SqlDbType.NText);
            arParams[25].Value = comment;

            arParams[26] = new SqlCeParameter("@OpenIDURI", SqlDbType.NVarChar, 255);
            arParams[26].Value = openIdUri;

            arParams[27] = new SqlCeParameter("@WindowsLiveID", SqlDbType.NVarChar, 36);
            arParams[27].Value = windowsLiveId;

            arParams[28] = new SqlCeParameter("@FirstName", SqlDbType.NVarChar, 100);
            arParams[28].Value = firstName;

            arParams[29] = new SqlCeParameter("@LastName", SqlDbType.NVarChar, 100);
            arParams[29].Value = lastName;

            arParams[30] = new SqlCeParameter("@Pwd", SqlDbType.NVarChar, 1000);
            arParams[30].Value = password;

            arParams[31] = new SqlCeParameter("@MustChangePwd", SqlDbType.Bit);
            arParams[31].Value = mustChangePwd;

            arParams[32] = new SqlCeParameter("@NewEmail", SqlDbType.NVarChar, 100);
            arParams[32].Value = newEmail;

            arParams[33] = new SqlCeParameter("@EditorPreference", SqlDbType.NVarChar, 100);
            arParams[33].Value = editorPreference;

            arParams[34] = new SqlCeParameter("@EmailChangeGuid", SqlDbType.UniqueIdentifier);
            arParams[34].Value = emailChangeGuid;

            arParams[35] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 32);
            arParams[35].Value = timeZoneId;

            arParams[36] = new SqlCeParameter("@PasswordResetGuid", SqlDbType.UniqueIdentifier);
            arParams[36].Value = passwordResetGuid;

            arParams[37] = new SqlCeParameter("@PasswordSalt", SqlDbType.NVarChar, 128);
            arParams[37].Value = passwordSalt;

            arParams[38] = new SqlCeParameter("@RolesChanged", SqlDbType.Bit);
            arParams[38].Value = rolesChanged;

            arParams[39] = new SqlCeParameter("@AuthorBio", SqlDbType.NText);
            arParams[39].Value = authorBio;

            arParams[40] = new SqlCeParameter("@DateOfBirth", SqlDbType.DateTime);
            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[40].Value = DBNull.Value;
            }
            else
            {
                arParams[40].Value = dateOfBirth;
            }

            arParams[41] = new SqlCeParameter("@EmailConfirmed", SqlDbType.Bit);
            arParams[41].Value = emailConfirmed;

            arParams[42] = new SqlCeParameter("@PwdFormat", SqlDbType.Int);
            arParams[42].Value = pwdFormat;

            arParams[43] = new SqlCeParameter("@PasswordHash", SqlDbType.NText);
            arParams[43].Value = passwordHash;

            arParams[44] = new SqlCeParameter("@SecurityStamp", SqlDbType.NText);
            arParams[44].Value = securityStamp;

            arParams[45] = new SqlCeParameter("@PhoneNumber", SqlDbType.NVarChar, 50);
            arParams[45].Value = phoneNumber;

            arParams[46] = new SqlCeParameter("@PhoneNumberConfirmed", SqlDbType.Bit);
            arParams[46].Value = phoneNumberConfirmed;

            arParams[47] = new SqlCeParameter("@TwoFactorEnabled", SqlDbType.Bit);
            arParams[47].Value = twoFactorEnabled;

            arParams[48] = new SqlCeParameter("@LockoutEndDateUtc", SqlDbType.DateTime);
            if (lockoutEndDateUtc == null)
            {
                arParams[48].Value = DBNull.Value;
            }
            else
            {
                arParams[48].Value = lockoutEndDateUtc;
            }

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool UpdatePasswordAndSalt(
            int userId,
            int pwdFormat,
            string password,
            string passwordSalt)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("Pwd = @Pwd, ");
            sqlCommand.Append("PasswordSalt = @PasswordSalt, ");
            sqlCommand.Append("PwdFormat = @PwdFormat ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@Pwd", SqlDbType.NVarChar, 1000);
            arParams[1].Value = password;

            arParams[2] = new SqlCeParameter("@PasswordSalt", SqlDbType.NVarChar, 128);
            arParams[2].Value = passwordSalt;

            arParams[3] = new SqlCeParameter("@PwdFormat", SqlDbType.Int);
            arParams[3].Value = pwdFormat;

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



        public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LastActivityDate = @LastActivityDate ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@LastActivityDate", SqlDbType.DateTime);
            arParams[1].Value = lastUpdate;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LastLoginDate = @LastLoginDate, ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

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

        public bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LastPasswordChangedDate = @LastPasswordChangedDate ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@LastPasswordChangedDate", SqlDbType.DateTime);
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool UpdateFailedPasswordAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FailedPwdAnswerWindowStart = @FailedPwdAnswerWindowStart ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@FailedPwdAnswerWindowStart", SqlDbType.DateTime);
            arParams[1].Value = windowStartTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

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

        public bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FailedPwdAnswerWindowStart = @FailedPwdAnswerWindowStart ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@FailedPwdAnswerWindowStart", SqlDbType.DateTime);
            arParams[1].Value = windowStartTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool UpdateFailedPasswordAnswerAttemptCount(
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

        public bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("RegisterConfirmGuid = @RegisterConfirmGuid, ");
            sqlCommand.Append("IsLockedOut = 1 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Value = registrationConfirmationGuid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("RegisterConfirmGuid = @EmptyGuid, ");
            sqlCommand.Append("IsLockedOut = 0 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RegisterConfirmGuid = @RegisterConfirmGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@EmptyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = emptyGuid;

            arParams[1] = new SqlCeParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Value = registrationConfirmationGuid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public bool UpdatePasswordQuestionAndAnswer(
            Guid userGuid,
            String passwordQuestion,
            String passwordAnswer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PasswordQuestion = @PasswordQuestion, ");
            sqlCommand.Append("PasswordAnswer = @PasswordAnswer ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PasswordQuestion", SqlDbType.NVarChar, 255);
            arParams[1].Value = passwordQuestion;

            arParams[2] = new SqlCeParameter("@PasswordAnswer", SqlDbType.NVarChar, 255);
            arParams[2].Value = passwordAnswer;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public void UpdateTotalRevenue(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(SUM(SubTotal),0) FROM mp_CommerceReport WHERE UserGuid = @UserGuid; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            decimal totalRevenue = Convert.ToDecimal(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            UpdateTotalRevenue(userGuid, totalRevenue);

        }

        private void UpdateTotalRevenue(Guid userGuid, decimal totalRevenue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalRevenue =@TotalRevenue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@TotalRevenue", SqlDbType.Decimal);
            arParams[1].Value = totalRevenue;

            AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public void UpdateTotalRevenue()
        {
            // this is a workaround to pretty ugly limitation of SQL CE
            // where we can't do a scalar update with a SET statement and a sub query
            // so we have to update each row one at a time

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(SUM(SubTotal),0) As Revenue, UserGuid FROM mp_CommerceReport ; ");

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("UserGuid", typeof(Guid));
            dataTable.Columns.Add("Revenue", typeof(decimal));


            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["UserGuid"] = new Guid(reader["UserGuid"].ToString());
                    row["Revenue"] = Convert.ToDecimal(reader["Revenue"]);
                    dataTable.Rows.Add(row);
                }


            }

            foreach (DataRow row in dataTable.Rows)
            {
                Guid userGuid = new Guid(row["UserGuid"].ToString());
                decimal revenue = Convert.ToDecimal(row["Revenue"]);
                UpdateTotalRevenue(userGuid, revenue);
            }


        }

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

        public bool IncrementTotalPosts(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalPosts = TotalPosts + 1 ");

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

        public bool DecrementTotalPosts(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalPosts = TotalPosts - 1 ");

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

        public DbDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND RegisterConfirmGuid = @RegisterConfirmGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Value = registerConfirmGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

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

        public Guid GetUserGuidFromOpenId(
            int siteId,
            string openIduri)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND OpenIDURI = @OpenIDURI ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@OpenIDURI", SqlDbType.NVarChar, 50);
            arParams[1].Value = openIduri;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND WindowsLiveID = @WindowsLiveID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@WindowsLiveID", SqlDbType.NVarChar, 50);
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
