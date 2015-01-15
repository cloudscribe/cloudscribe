// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-13
// 
// You must not remove this notice, or any other, from this software.
// 


using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
{
    
    internal static class DBSiteUser
    {
       
        public static IDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, Name, PasswordSalt, Pwd, Email FROM mp_Users WHERE SiteID = @SiteID ORDER BY Email");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetUserCountByYearMonth(int siteId)
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT First " + rowsToGet.ToString());
            sqlCommand.Append(" UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("SiteUser ");

            sqlCommand.Append("FROM (");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("Name As SiteUser ");

            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND (");
            sqlCommand.Append("(Name LIKE @Query) ");
            sqlCommand.Append("OR (FirstName LIKE @Query) ");
            sqlCommand.Append("OR (LastName LIKE @Query) ");
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
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Email LIKE @Query  ");


            sqlCommand.Append(")");

            sqlCommand.Append("ORDER BY SiteUser; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@Query", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = query + "%";


            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader EmailLookup(int siteId, string query, int rowsToGet)
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@Query", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = query + "%";


            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static int UserCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = @SiteID");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(), sqlCommand.ToString(), arParams).ToString());

            return count;

        }

        public static async Task<int> CountLockedOutUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID AND IsLockedOut = 1;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return count;
        }

        public static async Task<int> CountNotApprovedUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID AND ApprovedForForums = 0;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return count;
        }

        public static int UserCount(int siteId, String userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ProfileApproved = 1 ");

            if (userNameBeginsWith.Length == 1)
            {
                sqlCommand.Append("AND LEFT(Name, 1) = @UserNameBeginsWith ");
            }

            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UserNameBeginsWith", FbDbType.VarChar, 1);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userNameBeginsWith;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(), sqlCommand.ToString(), arParams).ToString());

            return count;

        }

        public static int CountUsersByRegistrationDateRange(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@EndDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(), sqlCommand.ToString(), arParams).ToString());

            return count;

        }

        public static int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND LastActivityDate > @SinceTime ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SinceTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND LastActivityDate >= @SinceTime ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SinceTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 50 * FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND LastActivityDate >= @SinceTime   ");
            sqlCommand.Append("ORDER BY LastActivityDate desc   ");
            sqlCommand.Append(" ;   ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SinceTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static async Task<int> GetNewestUserId(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT MAX(UserID) FROM mp_Users WHERE SiteID = @SiteID;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return count;
        }



        public static async Task<int> CountUsers(int siteId, string userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ProfileApproved = 1 ");

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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return count;

        }

        public static async Task<DbDataReader> GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            //int totalRows = Count(siteId, userNameBeginsWith);

            //totalPages = totalRows / pageSize;
            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder = 0;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

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

            sqlCommand.Append("WHERE u.ProfileApproved = 1   ");
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<int> CountUsersForSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND ProfileApproved = 1 ");
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

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);;
            arParams[1].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return count;

        }

        public static async Task<DbDataReader> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            //int totalRows = CountForSearch(siteId, searchInput);

            //totalPages = totalRows / pageSize;
            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder = 0;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

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
            sqlCommand.Append("AND ProfileApproved = 1 ");
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return count;

        }

        public static async Task<DbDataReader> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            //int totalRows = CountForAdminSearch(siteId, searchInput);

            //totalPages = totalRows / pageSize;
            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder = 0;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            //totalPages = 1;
            //int totalRows = CountLockedOutUsers(siteId);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            //totalPages = 1;
            //int totalRows = CountNotApprovedUsers(siteId);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

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
            sqlCommand.Append("u.ApprovedForForums = 0 ");

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


       

        public static async Task<int> AddUser(
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
            int intmustChangePwd = 0;
            if (mustChangePwd) { intmustChangePwd = 1; }

            int intEmailConfirmed = 0;
            if (emailConfirmed) { intEmailConfirmed = 1; }

            int intPhoneNumberConfirmed = 0;
            if (phoneNumberConfirmed) { intPhoneNumberConfirmed = 1; }

            int intTwoFactorEnabled = 0;
            if (twoFactorEnabled) { intTwoFactorEnabled = 1; }

            FbParameter[] arParams = new FbParameter[52];

            arParams[0] = new FbParameter(":SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter(":Name", FbDbType.VarChar, 100);
            arParams[1].Value = loginName;

            arParams[2] = new FbParameter(":LoginName", FbDbType.VarChar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new FbParameter(":Email", FbDbType.VarChar, 100);
            arParams[3].Value = email;

            arParams[4] = new FbParameter(":LoweredEmail", FbDbType.VarChar, 100);
            arParams[4].Value = email.ToLower();

            arParams[5] = new FbParameter(":Password", FbDbType.VarChar, 1000);
            arParams[5].Value = password;

            arParams[6] = new FbParameter(":PasswordQuestion", FbDbType.VarChar, 255);
            arParams[6].Value = "What color is blue?";

            arParams[7] = new FbParameter(":PasswordAnswer", FbDbType.VarChar, 255);
            arParams[7].Value = "blue";

            arParams[8] = new FbParameter(":Gender", FbDbType.Char, 10);
            arParams[8].Value = string.Empty;

            arParams[9] = new FbParameter(":ProfileApproved", FbDbType.SmallInt);
            arParams[9].Value = 1;

            arParams[10] = new FbParameter(":RegisterConfirmGuid", FbDbType.Char, 36);
            arParams[10].Value = Guid.Empty.ToString();

            arParams[11] = new FbParameter(":ApprovedForForums", FbDbType.SmallInt);
            arParams[11].Value = 1;

            arParams[12] = new FbParameter(":Trusted", FbDbType.SmallInt);
            arParams[12].Value = 0;

            arParams[13] = new FbParameter(":DisplayInMemberList", FbDbType.SmallInt);
            arParams[13].Value = 1;

            arParams[14] = new FbParameter(":WebSiteURL", FbDbType.VarChar, 100);
            arParams[14].Value = string.Empty;

            arParams[15] = new FbParameter(":Country", FbDbType.VarChar, 100);
            arParams[15].Value = string.Empty;

            arParams[16] = new FbParameter(":State", FbDbType.VarChar, 100);
            arParams[16].Value = string.Empty;

            arParams[17] = new FbParameter(":Occupation", FbDbType.VarChar, 100);
            arParams[17].Value = string.Empty;

            arParams[18] = new FbParameter(":Interests", FbDbType.VarChar, 100);
            arParams[18].Value = string.Empty;

            arParams[19] = new FbParameter(":MSN", FbDbType.VarChar, 50);
            arParams[19].Value = string.Empty;

            arParams[20] = new FbParameter(":Yahoo", FbDbType.VarChar, 50);
            arParams[20].Value = string.Empty;

            arParams[21] = new FbParameter(":AIM", FbDbType.VarChar, 50);
            arParams[21].Value = string.Empty;

            arParams[22] = new FbParameter(":ICQ", FbDbType.VarChar, 50);
            arParams[22].Value = string.Empty;

            arParams[23] = new FbParameter(":TotalPosts", FbDbType.Integer);
            arParams[23].Value = 0;

            arParams[24] = new FbParameter(":AvatarUrl", FbDbType.VarChar, 255);
            arParams[24].Value = string.Empty;

            arParams[25] = new FbParameter(":TimeOffsetHours", FbDbType.Integer);
            arParams[25].Value = 0;

            arParams[26] = new FbParameter(":Signature", FbDbType.VarChar, 4000);
            arParams[26].Value = string.Empty;

            arParams[27] = new FbParameter(":DateCreated", FbDbType.TimeStamp);
            arParams[27].Value = dateCreated;

            arParams[28] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[28].Value = userGuid.ToString();

            arParams[29] = new FbParameter(":Skin", FbDbType.VarChar, 100);
            arParams[29].Value = string.Empty;

            arParams[30] = new FbParameter(":IsDeleted", FbDbType.SmallInt);
            arParams[30].Value = 0;

            arParams[31] = new FbParameter(":FailedPasswordAttemptCount", FbDbType.Integer);
            arParams[31].Value = 0;

            arParams[32] = new FbParameter(":FailedPwdAnswerAttemptCount", FbDbType.Integer);
            arParams[32].Value = 0;

            arParams[33] = new FbParameter(":IsLockedOut", FbDbType.SmallInt);
            arParams[33].Value = 0;

            arParams[34] = new FbParameter(":MobilePIN", FbDbType.VarChar, 16);
            arParams[34].Value = string.Empty;

            arParams[35] = new FbParameter(":PasswordSalt", FbDbType.VarChar, 128);
            arParams[35].Value = passwordSalt;

            arParams[36] = new FbParameter(":Comment", FbDbType.VarChar);
            arParams[36].Value = string.Empty;

            arParams[37] = new FbParameter(":SiteGuid", FbDbType.Char, 36);
            arParams[37].Value = siteGuid.ToString();

            arParams[38] = new FbParameter(":MustChangePwd", FbDbType.Integer);
            arParams[38].Value = intmustChangePwd;

            arParams[39] = new FbParameter(":FirstName", FbDbType.VarChar, 100);
            arParams[39].Value = firstName;

            arParams[40] = new FbParameter(":LastName", FbDbType.VarChar, 100);
            arParams[40].Value = lastName;

            arParams[41] = new FbParameter(":EmailChangeGuid", FbDbType.Char, 36);
            arParams[41].Value = Guid.Empty.ToString();

            arParams[42] = new FbParameter(":TimeZoneId", FbDbType.VarChar, 32);
            arParams[42].Value = timeZoneId;

            arParams[43] = new FbParameter(":DateOfBirth", FbDbType.TimeStamp);
            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[43].Value = DBNull.Value;
            }
            else
            {
                arParams[43].Value = dateOfBirth;
            }


            arParams[44] = new FbParameter(":PwdFormat", FbDbType.Integer);
            arParams[44].Value = pwdFormat;

            arParams[45] = new FbParameter(":EmailConfirmed", FbDbType.Integer);
            arParams[45].Value = intEmailConfirmed;

            arParams[46] = new FbParameter(":PasswordHash", FbDbType.VarChar);
            arParams[46].Value = passwordHash;

            arParams[47] = new FbParameter(":SecurityStamp", FbDbType.VarChar);
            arParams[47].Value = securityStamp;

            arParams[48] = new FbParameter(":PhoneNumber", FbDbType.VarChar, 50);
            arParams[48].Value = phoneNumber;

            arParams[49] = new FbParameter(":PhoneNumberConfirmed", FbDbType.Integer);
            arParams[49].Value = intPhoneNumberConfirmed;

            arParams[50] = new FbParameter(":TwoFactorEnabled", FbDbType.Integer);
            arParams[50].Value = intTwoFactorEnabled;

            arParams[51] = new FbParameter(":LockoutEndDateUtc", FbDbType.TimeStamp);
            if (lockoutEndDateUtc == null)
            {
                arParams[51].Value = DBNull.Value;
            }
            else
            {
                arParams[51].Value = lockoutEndDateUtc;
            }
            

            string statement = "EXECUTE PROCEDURE MP_USERS_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")";

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                statement,
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public static async Task<bool> UpdateUser(
            int userId,
            String fullName,
            String loginName,
            String email,
            String password,
            string passwordSalt,
            String gender,
            bool profileApproved,
            bool approvedForForums,
            bool trusted,
            bool displayInMemberList,
            String webSiteUrl,
            String country,
            String state,
            String occupation,
            String interests,
            String msn,
            String yahoo,
            String aim,
            String icq,
            String avatarUrl,
            String signature,
            String skin,
            String loweredEmail,
            String passwordQuestion,
            String passwordAnswer,
            String comment,
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
            DateTime? lockoutEndDateUtc)
        {

            #region bit conversion

            byte profileOK = 1;
            if (!profileApproved)
            {
                profileOK = 0;
            }

            byte approvedForForum = 1;
            if (!approvedForForums)
            {
                approvedForForum = 0;
            }

            byte trust = 1;
            if (!trusted)
            {
                trust = 0;
            }

            byte displayInList = 1;
            if (!displayInMemberList)
            {
                displayInList = 0;
            }

            int intmustChangePwd = 0;
            if (mustChangePwd) { intmustChangePwd = 1; }

            int intRolesChanged = 0;
            if (rolesChanged) { intRolesChanged = 1; }

            int intEmailConfirmed = 0;
            if (emailConfirmed) { intEmailConfirmed = 1; }

            int intPhoneNumberConfirmed = 0;
            if (phoneNumberConfirmed) { intPhoneNumberConfirmed = 1; }

            int intTwoFactorEnabled = 0;
            if (twoFactorEnabled) { intTwoFactorEnabled = 1; }

            #endregion


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET Email = @Email ,   ");
            sqlCommand.Append("Name = @FullName,    ");
            sqlCommand.Append("LoginName = @LoginName,    ");
            sqlCommand.Append("Pwd = @UserPassword,    ");
            sqlCommand.Append("PasswordSalt = @PasswordSalt,    ");
            sqlCommand.Append("Gender = @Gender,    ");
            sqlCommand.Append("ProfileApproved = @ProfileApproved,    ");
            sqlCommand.Append("ApprovedForForums = @ApprovedForForums,    ");
            sqlCommand.Append("Trusted = @Trusted,    ");
            sqlCommand.Append("DisplayInMemberList = @DisplayInMemberList,    ");
            sqlCommand.Append("WebSiteURL = @WebSiteURL,    ");
            sqlCommand.Append("Country = @Country,    ");
            sqlCommand.Append("State = @State,    ");
            sqlCommand.Append("Occupation = @Occupation,    ");
            sqlCommand.Append("Interests = @Interests,    ");
            sqlCommand.Append("MSN = @MSN,    ");
            sqlCommand.Append("Yahoo = @Yahoo,   ");
            sqlCommand.Append("AIM = @AIM,   ");
            sqlCommand.Append("ICQ = @ICQ,    ");
            sqlCommand.Append("AvatarUrl = @AvatarUrl,    ");
            sqlCommand.Append("Signature = @Signature,    ");
            sqlCommand.Append("Skin = @Skin,    ");
            sqlCommand.Append("LoweredEmail = @LoweredEmail,    ");
            sqlCommand.Append("PasswordQuestion = @PasswordQuestion,    ");
            sqlCommand.Append("PasswordAnswer = @PasswordAnswer,    ");
            sqlCommand.Append("MustChangePwd = @MustChangePwd,    ");
            sqlCommand.Append("RolesChanged = @RolesChanged,    ");
            sqlCommand.Append("Comment = @Comment,    ");
            sqlCommand.Append("OpenIDURI = @OpenIDURI,    ");
            sqlCommand.Append("WindowsLiveID = @WindowsLiveID, ");

            sqlCommand.Append("FirstName = @FirstName, ");
            sqlCommand.Append("LastName = @LastName, ");
            sqlCommand.Append("TimeZoneId = @TimeZoneId, ");
            sqlCommand.Append("NewEmail = @NewEmail, ");
            sqlCommand.Append("EmailChangeGuid = @EmailChangeGuid, ");
            sqlCommand.Append("PasswordResetGuid = @PasswordResetGuid, ");
            sqlCommand.Append("AuthorBio = @AuthorBio, ");
            sqlCommand.Append("DateOfBirth = @DateOfBirth, ");

            sqlCommand.Append("PwdFormat = @PwdFormat, ");
            sqlCommand.Append("EmailConfirmed = @EmailConfirmed, ");
            sqlCommand.Append("PasswordHash = @PasswordHash, ");
            sqlCommand.Append("SecurityStamp = @SecurityStamp, ");
            sqlCommand.Append("PhoneNumber = @PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed = @PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled = @TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc = @LockoutEndDateUtc, ");

            sqlCommand.Append("TimeOffsetHours = @TimeOffsetHours    ");

            sqlCommand.Append("WHERE UserID = @UserID ;");

            FbParameter[] arParams = new FbParameter[49];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[1].Value = email;

            arParams[2] = new FbParameter("@UserPassword", FbDbType.VarChar, 1000);
            arParams[2].Value = password;

            arParams[3] = new FbParameter("@Gender", FbDbType.VarChar, 1);
            arParams[3].Value = gender;

            arParams[4] = new FbParameter("@ProfileApproved", FbDbType.Integer);
            arParams[4].Value = profileOK;

            arParams[5] = new FbParameter("@ApprovedForForums", FbDbType.Integer);
            arParams[5].Value = approvedForForum;

            arParams[6] = new FbParameter("@Trusted", FbDbType.Integer);
            arParams[6].Value = trust;

            arParams[7] = new FbParameter("@DisplayInMemberList", FbDbType.Integer);
            arParams[7].Value = displayInList;

            arParams[8] = new FbParameter("@WebSiteURL", FbDbType.VarChar, 100);
            arParams[8].Value = webSiteUrl;

            arParams[9] = new FbParameter("@Country", FbDbType.VarChar, 100);
            arParams[9].Value = country;

            arParams[10] = new FbParameter("@State", FbDbType.VarChar, 100);
            arParams[10].Value = state;

            arParams[11] = new FbParameter("@Occupation", FbDbType.VarChar, 100);
            arParams[11].Value = occupation;

            arParams[12] = new FbParameter("@Interests", FbDbType.VarChar, 100);
            arParams[12].Value = interests;

            arParams[13] = new FbParameter("@MSN", FbDbType.VarChar, 100);
            arParams[13].Value = msn;

            arParams[14] = new FbParameter("@Yahoo", FbDbType.VarChar, 100);
            arParams[14].Value = yahoo;

            arParams[15] = new FbParameter("@AIM", FbDbType.VarChar, 100);
            arParams[15].Value = aim;

            arParams[16] = new FbParameter("@ICQ", FbDbType.VarChar, 100);
            arParams[16].Value = icq;

            arParams[17] = new FbParameter("@AvatarUrl", FbDbType.VarChar, 100);
            arParams[17].Value = avatarUrl;

            arParams[18] = new FbParameter("@Signature", FbDbType.VarChar, 4000);
            arParams[18].Value = signature;

            arParams[19] = new FbParameter("@Skin", FbDbType.VarChar, 100);
            arParams[19].Value = skin;

            arParams[20] = new FbParameter("@FullName", FbDbType.VarChar, 50);
            arParams[20].Value = fullName;

            arParams[21] = new FbParameter("@LoginName", FbDbType.VarChar, 50);
            arParams[21].Value = loginName;

            arParams[22] = new FbParameter("@LoweredEmail", FbDbType.VarChar, 100);
            arParams[22].Value = loweredEmail;

            arParams[23] = new FbParameter("@PasswordQuestion", FbDbType.VarChar, 255);
            arParams[23].Value = passwordQuestion;

            arParams[24] = new FbParameter("@PasswordAnswer", FbDbType.VarChar, 255);
            arParams[24].Value = passwordAnswer;

            arParams[25] = new FbParameter("@Comment", FbDbType.VarChar);
            arParams[25].Value = comment;

            arParams[26] = new FbParameter("@TimeOffsetHours", FbDbType.Integer);
            arParams[26].Value = timeOffsetHours;

            arParams[27] = new FbParameter("@OpenIDURI", FbDbType.VarChar, 255);
            arParams[27].Value = openIdUri;

            arParams[28] = new FbParameter("@WindowsLiveID", FbDbType.VarChar, 36);
            arParams[28].Value = windowsLiveId;

            arParams[29] = new FbParameter("@MustChangePwd", FbDbType.Integer);
            arParams[29].Value = intmustChangePwd;

            arParams[30] = new FbParameter("@FirstName", FbDbType.VarChar, 100);
            arParams[30].Value = firstName;

            arParams[31] = new FbParameter("@LastName", FbDbType.VarChar, 100);
            arParams[31].Value = lastName;

            arParams[32] = new FbParameter("@TimeZoneId", FbDbType.VarChar, 32);
            arParams[32].Value = timeZoneId;

            arParams[33] = new FbParameter("@EditorPreference", FbDbType.VarChar, 100);
            arParams[33].Value = editorPreference;

            arParams[34] = new FbParameter("@NewEmail", FbDbType.VarChar, 100);
            arParams[34].Value = newEmail;

            arParams[35] = new FbParameter("@EmailChangeGuid", FbDbType.Char, 36);
            arParams[35].Value = emailChangeGuid.ToString();

            arParams[36] = new FbParameter("@PasswordResetGuid", FbDbType.Char, 36);
            arParams[36].Value = passwordResetGuid.ToString();

            arParams[37] = new FbParameter("@PasswordSalt", FbDbType.VarChar, 128);
            arParams[37].Value = passwordSalt;

            arParams[38] = new FbParameter("@RolesChanged", FbDbType.Integer);
            arParams[38].Value = intRolesChanged;

            arParams[39] = new FbParameter("@AuthorBio", FbDbType.VarChar);
            arParams[39].Value = authorBio;

            arParams[40] = new FbParameter("@DateOfBirth", FbDbType.TimeStamp);
            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[40].Value = DBNull.Value;
            }
            else
            {
                arParams[40].Value = dateOfBirth;
            }

            arParams[41] = new FbParameter("@PwdFormat", FbDbType.Integer);
            arParams[41].Value = pwdFormat;

            arParams[42] = new FbParameter("@EmailConfirmed", FbDbType.Integer);
            arParams[42].Value = intEmailConfirmed;

            arParams[43] = new FbParameter("@PasswordHash", FbDbType.VarChar);
            arParams[43].Value = passwordHash;

            arParams[44] = new FbParameter("@SecurityStamp", FbDbType.VarChar);
            arParams[44].Value = securityStamp;

            arParams[45] = new FbParameter("@PhoneNumber", FbDbType.VarChar, 50);
            arParams[45].Value = phoneNumber;

            arParams[46] = new FbParameter("@PhoneNumberConfirmed", FbDbType.Integer);
            arParams[46].Value = intPhoneNumberConfirmed;

            arParams[47] = new FbParameter("@TwoFactorEnabled", FbDbType.Integer);
            arParams[47].Value = intTwoFactorEnabled;

            arParams[48] = new FbParameter("@LockoutEndDateUtc", FbDbType.TimeStamp);
            if (lockoutEndDateUtc == null)
            {
                arParams[48].Value = DBNull.Value;
            }
            else
            {
                arParams[48].Value = lockoutEndDateUtc;
            }

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static async Task<bool> DeleteUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);
        }

        public static bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastActivityDate = @LastUpdate  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@LastUpdate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastUpdate;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    sqlCommand.ToString(),
                    arParams);
            
            return (rowsAffected > 0);

        }

        public static bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastLoginDate = @LastLoginTime,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@LastLoginTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastLoginTime;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> AccountLockout(Guid userGuid, DateTime lockoutTime)
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
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("LastPasswordChangedDate = @LastPasswordChangedDate  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@LastPasswordChangedDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAttemptStartWindow(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@FailedPasswordAttemptWindowStart", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount)
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
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAnswerAttemptStartWindow(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@FailedPasswordAnswerAttemptWindowStart", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAnswerAttemptCount(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@AttemptCount", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = @RegisterConfirmGuid  ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@RegisterConfirmGuid", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 0,  ");
            sqlCommand.Append("RegisterConfirmGuid = @EmptyGuid  ");
            sqlCommand.Append("WHERE RegisterConfirmGuid = @RegisterConfirmGuid  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@EmptyGuid", FbDbType.VarChar, 36);
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new FbParameter("@RegisterConfirmGuid", FbDbType.VarChar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<bool> AccountClearLockout(Guid userGuid)
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
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static bool UpdatePasswordAndSalt(
            int userId,
            int pwdFormat,
            string password,
            string passwordSalt)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET Pwd = @Password,  ");
            sqlCommand.Append("PasswordSalt = @PasswordSalt,  ");
            sqlCommand.Append("PwdFormat = @PwdFormat  ");

            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@Password", FbDbType.VarChar, 1000);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = password;

            arParams[2] = new FbParameter("@PasswordSalt", FbDbType.VarChar, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = passwordSalt;

            arParams[3] = new FbParameter("@PwdFormat", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pwdFormat;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdatePasswordQuestionAndAnswer(
            Guid userGuid,
            String passwordQuestion,
            String passwordAnswer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET PasswordQuestion = @PasswordQuestion,  ");
            sqlCommand.Append("PasswordAnswer = @PasswordAnswer  ");

            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PasswordQuestion", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = passwordQuestion;

            arParams[2] = new FbParameter("@PasswordAnswer", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = passwordAnswer;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<bool> UpdateTotalRevenue(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = @UserGuid)  ");
            sqlCommand.Append(", 0) ");

            sqlCommand.Append("WHERE UserGuid = @UserGuid  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return rowsAffected > 0;

        }

        public static async Task<bool> UpdateTotalRevenue()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = mp_Users.UserGuid)  ");
            sqlCommand.Append(", 0) ");

            sqlCommand.Append("  ;");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);

            return rowsAffected > 0;
        }



        public static async Task<bool> FlagAsDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 1 ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<bool> FlagAsNotDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 0 ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static bool IncrementTotalPosts(int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET	TotalPosts = TotalPosts + 1 ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);


            return (rowsAffected > 0);
        }

        public static bool DecrementTotalPosts(int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET	TotalPosts = TotalPosts - 1 ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<DbDataReader> GetRolesByUser(int siteId, int userId)
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static async Task<DbDataReader> GetSingleUser(int siteId, string email)
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetCrossSiteUserListByEmail(string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE LoweredEmail = @Email  ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[0].Value = email.ToLower();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetSingleUserByLoginName(int siteId, string loginName, bool allowEmailFallback)
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

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DbDataReader GetSingleUserByLoginNameNonAsync(int siteId, string loginName, bool allowEmailFallback)
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
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetSingleUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE UserID = @UserID ;  ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<DbDataReader> GetSingleUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Users ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid ;  ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetUserGuidFromOpenId(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@OpenIDURI", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = openIdUri;

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

        public static Guid GetUserGuidFromWindowsLiveId(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@WindowsLiveID", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

        public static string LoginByEmail(int siteId, string email, string password)
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            arParams[2] = new FbParameter("@Password", FbDbType.VarChar, 1000);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            string userName = string.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

        public static string Login(int siteId, string loginName, string password)
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@LoginName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            arParams[2] = new FbParameter("@Password", FbDbType.VarChar, 1000);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            string userName = string.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                 ConnectionString.GetReadConnectionString(),
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

        public static DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserGuid", typeof(String));
            dataTable.Columns.Add("PropertyName", typeof(String));
            dataTable.Columns.Add("PropertyValueString", typeof(String));
            dataTable.Columns.Add("PropertyValueBinary", typeof(object));

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["UserGuid"] = reader["UserGuid"].ToString();
                    row["PropertyName"] = reader["PropertyName"].ToString();
                    row["PropertyValueString"] = reader["PropertyValueString"].ToString();
                    row["PropertyValueBinary"] = reader["PropertyValueBinary"];
                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;
        }

        public static IDataReader GetLazyLoadedProperty(Guid userGuid, String propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT FIRST 1  * ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid AND PropertyName = @PropertyName  ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE UserGuid = @UserGuid AND PropertyName = @PropertyName ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static void CreateProperty(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = propertyName;

            arParams[3] = new FbParameter("@PropertyValueString", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = propertyValues;

            arParams[4] = new FbParameter("@PropertyValueBinary", FbDbType.Binary);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = propertyValueb;

            arParams[5] = new FbParameter("@LastUpdatedDate", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new FbParameter("@IsLazyLoaded", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

        }

        public static void UpdateProperty(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new FbParameter("@PropertyName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            arParams[2] = new FbParameter("@PropertyValueString", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = propertyValues;

            arParams[3] = new FbParameter("@PropertyValueBinary", FbDbType.Binary);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = propertyValueb;

            arParams[4] = new FbParameter("@LastUpdatedDate", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new FbParameter("@IsLazyLoaded", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isLazyLoaded;

            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);


        }

        public static bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }
        


    }
}
