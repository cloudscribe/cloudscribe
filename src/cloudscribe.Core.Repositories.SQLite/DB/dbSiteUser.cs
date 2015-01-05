// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2014-09-08
// 
// You must not remove this notice, or any other, from this software.
// 


using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
   
    internal static class DBSiteUser
    {
        

        public static IDataReader GetUserCountByYearMonth(int siteId)
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

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static IDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, Name, PasswordSalt, Pwd, Email FROM mp_Users WHERE SiteID = :SiteID ORDER BY Email");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":Query", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = query + "%";


            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader EmailLookup(int siteId, string query, int rowsToGet)
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":Query", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = query + "%";


            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int UserCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ProfileApproved = 1 ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int CountLockedOutUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID AND IsLockedOut = 1;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int CountNotApprovedUsers(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID AND ApprovedForForums = 0;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;
        }

        public static int UserCount(int siteId, String userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users ");
            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ProfileApproved = 1 ");

            if (userNameBeginsWith.Length == 1)
            {
                sqlCommand.Append("AND Name like :UserNameBeginsWith || '%' ");
            }

            sqlCommand.Append("; ");


            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":UserNameBeginsWith", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userNameBeginsWith;


            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND DateCreated >= :BeginDate ");
            sqlCommand.Append("AND DateCreated < :EndDate; ");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SQLiteParameter(":EndDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int GetNewestUserId(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT MAX(UserID) FROM mp_Users WHERE SiteID = :SiteID;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;
        }

        public static int Count(int siteId, string userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND ProfileApproved = 1 ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND Name LIKE :UserNameBeginsWith ");
            }
            sqlCommand.Append(" ;  ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":UserNameBeginsWith", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userNameBeginsWith + "%";

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            out int totalPages)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int totalRows
                = UserCount(siteId, userNameBeginsWith);
            totalPages = 1;
            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            sqlCommand.Append("SELECT		u.*,  ");
            sqlCommand.Append(" " + totalPages.ToString() + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_Users u  ");

            sqlCommand.Append("WHERE u.ProfileApproved = 1   ");
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

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":PageNumber", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SQLiteParameter(":UserNameBeginsWith", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userNameBeginsWith + "%";

            arParams[3] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        private static int CountForSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND ProfileApproved = 1 ");
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":SearchInput", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int totalRows = CountForSearch(siteId, searchInput);
            totalPages = 1;
            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            sqlCommand.Append("SELECT *  ");
           
            
            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND ProfileApproved = 1 ");
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

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":PageNumber", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SQLiteParameter(":SearchInput", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        private static int CountForAdminSearch(int siteId, string searchInput)
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":SearchInput", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" +searchInput + "%";

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            out int totalPages)
        {
            
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int totalRows = CountForAdminSearch(siteId, searchInput);
            totalPages = 1;
            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

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

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":PageNumber", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SQLiteParameter(":SearchInput", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = CountLockedOutUsers(siteId);
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");

            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsLockedOut = 1 ");
            
            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":PageNumber", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = CountNotApprovedUsers(siteId);
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT *  ");

            sqlCommand.Append("FROM	mp_Users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ApprovedForForums = 0 ");

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":PageNumber", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        

        public static int AddUser(
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

            #region bit conversion

            int intmustChangePwd = 0;
            if(mustChangePwd) { intmustChangePwd = 1; }
            int intEmailConfirmed = 0;
            if (emailConfirmed) { intEmailConfirmed = 1; }
            int intPhoneNumberConfirmed = 0;
            if (phoneNumberConfirmed) { intPhoneNumberConfirmed = 1; }
            int intTwoFactorEnabled = 0;
            if (twoFactorEnabled) { intTwoFactorEnabled = 1; }

            #endregion

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

            sqlCommand.Append("Pwd, ");
            sqlCommand.Append("PasswordSalt, ");
            sqlCommand.Append("RolesChanged, ");
            sqlCommand.Append("MustChangePwd, ");
            sqlCommand.Append("DateCreated, ");
            sqlCommand.Append("TotalPosts, ");
            sqlCommand.Append("TotalRevenue, ");
            sqlCommand.Append("DateOfBirth, ");

            sqlCommand.Append("EmailConfirmed, ");
            sqlCommand.Append("PwdFormat, ");
            sqlCommand.Append("PasswordHash, ");
            sqlCommand.Append("SecurityStamp, ");
            sqlCommand.Append("PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc, ");

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
            sqlCommand.Append("'00000000-0000-0000-0000-000000000000', ");

            sqlCommand.Append(" :Password, ");
            sqlCommand.Append(":PasswordSalt, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":MustChangePwd, ");
            sqlCommand.Append(" :DateCreated, ");
            sqlCommand.Append(" 0, ");
            sqlCommand.Append(" 0.0, ");
            sqlCommand.Append(":DateOfBirth, ");

            sqlCommand.Append(":EmailConfirmed, ");
            sqlCommand.Append(":PwdFormat, ");
            sqlCommand.Append(":PasswordHash, ");
            sqlCommand.Append(":SecurityStamp, ");
            sqlCommand.Append(":PhoneNumber, ");
            sqlCommand.Append(":PhoneNumberConfirmed, ");
            sqlCommand.Append(":TwoFactorEnabled, ");
            sqlCommand.Append(":LockoutEndDateUtc, ");

            sqlCommand.Append(" :UserGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SQLiteParameter[] arParams = new SQLiteParameter[23];

            arParams[0] = new SQLiteParameter(":FullName", DbType.String, 100);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fullName;

            arParams[1] = new SQLiteParameter(":LoginName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            arParams[2] = new SQLiteParameter(":Email", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = email;

            arParams[3] = new SQLiteParameter(":Password", DbType.String, 1000);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = password;

            arParams[4] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = siteId;

            arParams[5] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new SQLiteParameter(":DateCreated", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            arParams[7] = new SQLiteParameter(":SiteGuid", DbType.String, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = siteGuid.ToString();

            arParams[8] = new SQLiteParameter(":MustChangePwd", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intmustChangePwd;

            arParams[9] = new SQLiteParameter(":FirstName", DbType.String, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = firstName;

            arParams[10] = new SQLiteParameter(":LastName", DbType.String, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastName;

            arParams[11] = new SQLiteParameter(":TimeZoneId", DbType.String, 32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = timeZoneId;

            arParams[12] = new SQLiteParameter(":EmailChangeGuid", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = Guid.Empty.ToString();

            arParams[13] = new SQLiteParameter(":PasswordSalt", DbType.String, 128);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = passwordSalt;

            arParams[14] = new SQLiteParameter(":DateOfBirth", DbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[14].Value = DBNull.Value;
            }
            else
            {
                arParams[14].Value = dateOfBirth;
            }

            arParams[15] = new SQLiteParameter(":EmailConfirmed", DbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intEmailConfirmed;

            arParams[16] = new SQLiteParameter(":PwdFormat", DbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = pwdFormat;

            arParams[17] = new SQLiteParameter(":PasswordHash", DbType.Object);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = passwordHash;

            arParams[18] = new SQLiteParameter(":SecurityStamp", DbType.Object);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = securityStamp;

            arParams[19] = new SQLiteParameter(":PhoneNumber", DbType.String, 128);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = phoneNumber;

            arParams[20] = new SQLiteParameter(":PhoneNumberConfirmed", DbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intPhoneNumberConfirmed;

            arParams[21] = new SQLiteParameter(":TwoFactorEnabled", DbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intTwoFactorEnabled;

            arParams[22] = new SQLiteParameter(":LockoutEndDateUtc", DbType.DateTime);
            arParams[22].Direction = ParameterDirection.Input;
            if (lockoutEndDateUtc == null)
            {
                arParams[22].Value = DBNull.Value;
            }
            else
            {
                arParams[22].Value = lockoutEndDateUtc;
            }

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                   ConnectionString.GetConnectionString(),
                   sqlCommand.ToString(),
                   arParams).ToString());

            return newID;

        }

        public static bool UpdateUser(
            int userId,
            String fullName,
            String loginName,
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

            byte approved = 1;
            if (!profileApproved)
            {
                approved = 0;
            }

            byte canPost = 1;
            if (!approvedForForums)
            {
                canPost = 0;
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

            int introlesChanged = 0;
            if (rolesChanged) { introlesChanged = 1; }

            int intEmailConfirmed = 0;
            if (emailConfirmed) { intEmailConfirmed = 1; }

            int intPhoneNumberConfirmed = 0;
            if (phoneNumberConfirmed) { intPhoneNumberConfirmed = 1; }

            int intTwoFactorEnabled = 0;
            if (twoFactorEnabled) { intTwoFactorEnabled = 1; }

            #endregion


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET Email = :Email ,   ");
            sqlCommand.Append("Name = :FullName,    ");
            sqlCommand.Append("LoginName = :LoginName,    ");

            sqlCommand.Append("FirstName = :FirstName,    ");
            sqlCommand.Append("LastName = :LastName,    ");
            sqlCommand.Append("TimeZoneId = :TimeZoneId,    ");
            sqlCommand.Append("EditorPreference = :EditorPreference,    ");
            sqlCommand.Append("NewEmail = :NewEmail,    ");
            sqlCommand.Append("EmailChangeGuid = :EmailChangeGuid,    ");
            sqlCommand.Append("PasswordResetGuid = :PasswordResetGuid,    ");

            sqlCommand.Append("Pwd = :Password,    ");
            sqlCommand.Append("PasswordSalt = :PasswordSalt,    ");
            sqlCommand.Append("RolesChanged = :RolesChanged,    ");
            sqlCommand.Append("MustChangePwd = :MustChangePwd,    ");
            sqlCommand.Append("Gender = :Gender,    ");
            sqlCommand.Append("ProfileApproved = :ProfileApproved,    ");
            sqlCommand.Append("ApprovedForForums = :ApprovedForForums,    ");
            sqlCommand.Append("Trusted = :Trusted,    ");
            sqlCommand.Append("DisplayInMemberList = :DisplayInMemberList,    ");
            sqlCommand.Append("WebSiteURL = :WebSiteURL,    ");
            sqlCommand.Append("Country = :Country,    ");
            sqlCommand.Append("State = :State,    ");
            sqlCommand.Append("Occupation = :Occupation,    ");
            sqlCommand.Append("Interests = :Interests,    ");
            sqlCommand.Append("MSN = :MSN,    ");
            sqlCommand.Append("Yahoo = :Yahoo,   ");
            sqlCommand.Append("AIM = :AIM,   ");
            sqlCommand.Append("ICQ = :ICQ,    ");
            sqlCommand.Append("AvatarUrl = :AvatarUrl,    ");
            sqlCommand.Append("Signature = :Signature,    ");
            sqlCommand.Append("Skin = :Skin,    ");
            sqlCommand.Append("AuthorBio = :AuthorBio,    ");

            sqlCommand.Append("LoweredEmail = :LoweredEmail,    ");
            sqlCommand.Append("PasswordQuestion = :PasswordQuestion,    ");
            sqlCommand.Append("PasswordAnswer = :PasswordAnswer,    ");
            sqlCommand.Append("Comment = :Comment,    ");
            sqlCommand.Append("OpenIDURI = :OpenIDURI,    ");
            sqlCommand.Append("WindowsLiveID = :WindowsLiveID,    ");
            sqlCommand.Append("DateOfBirth = :DateOfBirth,    ");

            sqlCommand.Append("EmailConfirmed = :EmailConfirmed, ");
            sqlCommand.Append("PwdFormat = :PwdFormat, ");
            sqlCommand.Append("PasswordHash = :PasswordHash, ");
            sqlCommand.Append("SecurityStamp = :SecurityStamp, ");
            sqlCommand.Append("PhoneNumber = :PhoneNumber, ");
            sqlCommand.Append("PhoneNumberConfirmed = :PhoneNumberConfirmed, ");
            sqlCommand.Append("TwoFactorEnabled = :TwoFactorEnabled, ");
            sqlCommand.Append("LockoutEndDateUtc = :LockoutEndDateUtc, ");


            sqlCommand.Append("TimeOffsetHours = :TimeOffsetHours    ");

            sqlCommand.Append("WHERE UserID = :UserID ;");

            SQLiteParameter[] arParams = new SQLiteParameter[49];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SQLiteParameter(":Email", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            arParams[2] = new SQLiteParameter(":Password", DbType.String, 1000);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            arParams[3] = new SQLiteParameter(":Gender", DbType.String, 1);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = gender;

            arParams[4] = new SQLiteParameter(":ProfileApproved", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approved;

            arParams[5] = new SQLiteParameter(":ApprovedForForums", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = canPost;

            arParams[6] = new SQLiteParameter(":Trusted", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = trust;

            arParams[7] = new SQLiteParameter(":DisplayInMemberList", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = displayInList;

            arParams[8] = new SQLiteParameter(":WebSiteURL", DbType.String, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = webSiteUrl;

            arParams[9] = new SQLiteParameter(":Country", DbType.String, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = country;

            arParams[10] = new SQLiteParameter(":State", DbType.String, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = state;

            arParams[11] = new SQLiteParameter(":Occupation", DbType.String, 100);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = occupation;

            arParams[12] = new SQLiteParameter(":Interests", DbType.String, 100);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = interests;

            arParams[13] = new SQLiteParameter(":MSN", DbType.String, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = msn;

            arParams[14] = new SQLiteParameter(":Yahoo", DbType.String, 100);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = yahoo;

            arParams[15] = new SQLiteParameter(":AIM", DbType.String, 100);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = aim;

            arParams[16] = new SQLiteParameter(":ICQ", DbType.String, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = icq;

            arParams[17] = new SQLiteParameter(":AvatarUrl", DbType.String, 100);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = avatarUrl;

            arParams[18] = new SQLiteParameter(":Signature", DbType.Object);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = signature;

            arParams[19] = new SQLiteParameter(":Skin", DbType.String, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new SQLiteParameter(":FullName", DbType.String, 50);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = fullName;

            arParams[21] = new SQLiteParameter(":LoginName", DbType.String, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = loginName;

            arParams[22] = new SQLiteParameter(":LoweredEmail", DbType.String, 100);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = loweredEmail;

            arParams[23] = new SQLiteParameter(":PasswordQuestion", DbType.String, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = passwordQuestion;

            arParams[24] = new SQLiteParameter(":PasswordAnswer", DbType.String, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = passwordAnswer;

            arParams[25] = new SQLiteParameter(":Comment", DbType.Object);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = comment;

            arParams[26] = new SQLiteParameter(":TimeOffsetHours", DbType.Int32);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = timeOffsetHours;

            arParams[27] = new SQLiteParameter(":OpenIDURI", DbType.String, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = openIdUri;

            arParams[28] = new SQLiteParameter(":WindowsLiveID", DbType.String, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = windowsLiveId;

            arParams[29] = new SQLiteParameter(":MustChangePwd", DbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intmustChangePwd;

            arParams[30] = new SQLiteParameter(":FirstName", DbType.String, 100);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = firstName;

            arParams[31] = new SQLiteParameter(":LastName", DbType.String, 100);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = lastName;

            arParams[32] = new SQLiteParameter(":TimeZoneId", DbType.String, 32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = timeZoneId;

            arParams[33] = new SQLiteParameter(":EditorPreference", DbType.String, 100);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = editorPreference;

            arParams[34] = new SQLiteParameter(":NewEmail", DbType.String, 100);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = newEmail;

            arParams[35] = new SQLiteParameter(":EmailChangeGuid", DbType.String, 36);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = emailChangeGuid.ToString();

            arParams[36] = new SQLiteParameter(":PasswordResetGuid", DbType.String, 36);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = passwordResetGuid.ToString();

            arParams[37] = new SQLiteParameter(":PasswordSalt", DbType.String, 128);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = passwordSalt;

            arParams[38] = new SQLiteParameter(":RolesChanged", DbType.Int32);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = introlesChanged;

            arParams[39] = new SQLiteParameter(":AuthorBio", DbType.Object);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = authorBio;

            arParams[40] = new SQLiteParameter(":DateOfBirth", DbType.DateTime);
            arParams[40].Direction = ParameterDirection.Input;
            if (dateOfBirth == DateTime.MinValue)
            {
                arParams[40].Value = DBNull.Value;
            }
            else
            {
                arParams[40].Value = dateOfBirth;
            }

            arParams[41] = new SQLiteParameter(":EmailConfirmed", DbType.Int32);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = intEmailConfirmed;

            arParams[42] = new SQLiteParameter(":PwdFormat", DbType.Int32);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = pwdFormat;

            arParams[43] = new SQLiteParameter(":PasswordHash", DbType.Object);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = passwordHash;

            arParams[44] = new SQLiteParameter(":SecurityStamp", DbType.Object);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = securityStamp;

            arParams[45] = new SQLiteParameter(":PhoneNumber", DbType.String, 128);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = phoneNumber;

            arParams[46] = new SQLiteParameter(":PhoneNumberConfirmed", DbType.Int32);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = intPhoneNumberConfirmed;

            arParams[47] = new SQLiteParameter(":TwoFactorEnabled", DbType.Int32);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = intTwoFactorEnabled;

            arParams[48] = new SQLiteParameter(":LockoutEndDateUtc", DbType.DateTime);
            arParams[48].Direction = ParameterDirection.Input;
            if (lockoutEndDateUtc == null)
            {
                arParams[48].Value = DBNull.Value;
            }
            else
            {
                arParams[48].Value = lockoutEndDateUtc;
            }

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Pwd = :Password,    ");
            sqlCommand.Append("PasswordSalt = :PasswordSalt, ");
            sqlCommand.Append("PwdFormat = :PwdFormat ");

            sqlCommand.Append("WHERE UserID = :UserID ;");

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SQLiteParameter(":Password", DbType.String, 1000);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = password;

            arParams[2] = new SQLiteParameter(":PasswordSalt", DbType.String, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = passwordSalt;

            arParams[3] = new SQLiteParameter(":PwdFormat", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pwdFormat;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }


        public static bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastActivityDate = :LastUpdate  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":LastUpdate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastUpdate;

            int rowsAffected = 0;

            try
            {
                rowsAffected = AdoHelper.ExecuteNonQuery(
                    ConnectionString.GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

            }
            catch (DbException)
            { }


            return (rowsAffected > 0);

        }

        public static bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET LastLoginDate = :LastLoginTime,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":LastLoginTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastLoginTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool AccountLockout(Guid userGuid, DateTime lockoutTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsLockedOut = 1,  ");
            sqlCommand.Append("LastLockoutDate = :LockoutTime  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":LockoutTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lockoutTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("LastPasswordChangedDate = :LastPasswordChangedDate  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":LastPasswordChangedDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("FailedPwdAttemptWindowStart = :FailedPasswordAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":FailedPasswordAttemptWindowStart", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPasswordAttemptCount = :AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":AttemptCount", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerWindowStart = :FailedPasswordAnswerAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":FailedPasswordAnswerAttemptWindowStart", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAnswerAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = :AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":AttemptCount", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }

        public static bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = :RegisterConfirmGuid  ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":RegisterConfirmGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 0,  ");
            sqlCommand.Append("RegisterConfirmGuid = :EmptyGuid  ");
            sqlCommand.Append("WHERE RegisterConfirmGuid = :RegisterConfirmGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":EmptyGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new SQLiteParameter(":RegisterConfirmGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND LastActivityDate > :SinceTime ;  ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":SinceTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            int count = Convert.ToInt32(
                AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND LastActivityDate > :SinceTime ;  ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":SinceTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM mp_Users WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND LastActivityDate > :SinceTime   ");
            sqlCommand.Append("ORDER BY LastActivityDate desc   ");
            sqlCommand.Append("LIMIT 50 ;   ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":SinceTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static bool AccountClearLockout(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsLockedOut = 0,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("SET PasswordQuestion = :PasswordQuestion,  ");
            sqlCommand.Append("PasswordAnswer = :PasswordAnswer  ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":PasswordQuestion", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = passwordQuestion;

            arParams[2] = new SQLiteParameter(":PasswordAnswer", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = passwordAnswer;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static void UpdateTotalRevenue(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = :UserGuid)  ");
            sqlCommand.Append(", 0) ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static void UpdateTotalRevenue()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = mp_Users.UserGuid)  ");
            sqlCommand.Append(", 0) ");

            sqlCommand.Append("  ;");

            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }




        public static bool FlagAsDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 1 ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool FlagAsNotDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET IsDeleted = 0 ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool IncrementTotalPosts(int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET	TotalPosts = TotalPosts + 1 ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DecrementTotalPosts(int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET	TotalPosts = TotalPosts - 1 ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetRolesByUser(int siteId, int userId)
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = :SiteID AND RegisterConfirmGuid = :RegisterConfirmGuid;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":RegisterConfirmGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registerConfirmGuid;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSingleUser(int siteId, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE SiteID = :SiteID AND LoweredEmail = :Email;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":Email", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email.ToLower();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetCrossSiteUserListByEmail(string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE LoweredEmail = :Email;");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":Email", DbType.String, 100);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = email.ToLower();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleUserByLoginName(int siteId, string loginName, bool allowEmailFallback)
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":LoginName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE UserID = :UserID;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Users ");

            sqlCommand.Append("WHERE UserGuid = :UserGuid ;  ");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetUserGuidFromOpenId(
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

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":OpenIDURI", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = openIduri;

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("SiteID = :SiteID  ");
            sqlCommand.Append("AND WindowsLiveID = :WindowsLiveID   ");
            sqlCommand.Append(" ;  ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":WindowsLiveID", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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

            sqlCommand.Append("WHERE Email = :Email  ");
            sqlCommand.Append("AND SiteID = :SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = :Password;");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":Email", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            arParams[2] = new SQLiteParameter(":Password", DbType.String, 1000);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            string userName = string.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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

            sqlCommand.Append("WHERE LoginName = :LoginName  ");
            sqlCommand.Append("AND SiteID = :SiteID  ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND Pwd = :Password;");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SQLiteParameter(":LoginName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            arParams[2] = new SQLiteParameter(":Password", DbType.String, 1000);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            string userName = string.Empty;

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("UserGuid = :UserGuid ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserGuid", typeof(String));
            dataTable.Columns.Add("PropertyName", typeof(String));
            dataTable.Columns.Add("PropertyValueString", typeof(String));
            dataTable.Columns.Add("PropertyValueBinary", typeof(object));

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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

            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid AND PropertyName = :PropertyName  ");
            sqlCommand.Append("LIMIT 1 ; ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":PropertyName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_UserProperties ");
            sqlCommand.Append("WHERE UserGuid = :UserGuid AND PropertyName = :PropertyName ; ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":PropertyName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
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


            SQLiteParameter[] arParams = new SQLiteParameter[7];

            arParams[0] = new SQLiteParameter(":PropertyID", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SQLiteParameter(":PropertyName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = propertyName;

            arParams[3] = new SQLiteParameter(":PropertyValueString", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = propertyValues;

            arParams[4] = new SQLiteParameter(":PropertyValueBinary", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = propertyValueb;

            arParams[5] = new SQLiteParameter(":LastUpdatedDate", DbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new SQLiteParameter(":IsLazyLoaded", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lazy;

            AdoHelper.ExecuteNonQuery(ConnectionString.GetConnectionString(), sqlCommand.ToString(), arParams);

        }

        public static void UpdateProperty(
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

            SQLiteParameter[] arParams = new SQLiteParameter[6];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":PropertyName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            arParams[2] = new SQLiteParameter(":PropertyValueString", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = propertyValues;

            arParams[3] = new SQLiteParameter(":PropertyValueBinary", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = propertyValueb;

            arParams[4] = new SQLiteParameter(":LastUpdatedDate", DbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new SQLiteParameter(":IsLazyLoaded", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lazy;

            AdoHelper.ExecuteNonQuery(ConnectionString.GetConnectionString(), sqlCommand.ToString(), arParams);


        }

        public static bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }
        

    }
}
