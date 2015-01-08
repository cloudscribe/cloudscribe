// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-07
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;


namespace cloudscribe.Core.Repositories.Firebird
{
    
    internal static class DBRoles
    {

        public static async Task<int> RoleCreate(
            Guid roleGuid,
            Guid siteGuid,
            int siteId,
            string roleName)
        {
            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter(":SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter(":RoleName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            arParams[2] = new FbParameter(":DisplayName", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = roleName;

            arParams[3] = new FbParameter(":SiteGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteGuid.ToString();

            arParams[4] = new FbParameter(":RoleGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = roleGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_ROLES_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public static async Task<bool> Update(int roleId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Roles ");
            sqlCommand.Append("SET DisplayName = @RoleName  ");
            sqlCommand.Append("WHERE RoleID = @RoleID  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            arParams[1] = new FbParameter("@RoleName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<bool> Delete(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE RoleID = @RoleID AND RoleName <> 'Admins' AND RoleName <> 'Content Administrators' AND RoleName <> 'Authenticated Users' AND RoleName <> 'Role Admins'  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DeleteUserRoles(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = @UserID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = @RoleID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static async Task<IDataReader> GetById(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE RoleID = @RoleID ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByName(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID AND RoleName = @RoleName ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RoleName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<bool> Exists(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID AND RoleName = @RoleName ; ");


            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RoleName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return (count > 0);

        }

        public static IDataReader GetSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid, ");
            sqlCommand.Append("COUNT(ur.UserID) As MemberCount ");

            sqlCommand.Append("FROM	mp_Roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON ur.RoleID = r.RoleID ");

            sqlCommand.Append("WHERE r.SiteID = @SiteID  ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetRoleMembers(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_UserRoles.UserID, ");
            sqlCommand.Append("mp_Users.Name, ");
            sqlCommand.Append("mp_Users.LoginName, ");
            sqlCommand.Append("mp_Users.Email ");

            sqlCommand.Append("FROM	mp_UserRoles ");
            sqlCommand.Append("INNER JOIN mp_Users ");
            sqlCommand.Append("ON mp_Users.UserID = mp_UserRoles.UserID ");

            sqlCommand.Append("WHERE mp_UserRoles.RoleID = @RoleID ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<int> GetCountOfUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");
            
            sqlCommand.Append("FROM	mp_Users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (UPPER(u.Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LoginName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.Email) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LastName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.FirstName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("AND ur.RoleID IS NULL  ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return Convert.ToInt32(result);

        }

        public static async Task<IDataReader> GetUsersNotInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountOfUsersNotInRole(siteId, roleId, searchInput);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }

            
            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.Name, ");
            //sqlCommand.Append("u.Email, ");
            //sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID  ");
            sqlCommand.Append("AND ur.RoleID IS NULL  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (UPPER(u.Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LoginName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.Email) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LastName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.FirstName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.Name  ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + searchInput + "%";

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<int> GetCountOfUsersInRole(int siteId, int roleId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserRoles ur ");

            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (UPPER(u.Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LoginName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.Email) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LastName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.FirstName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(")");
            }
            
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return Convert.ToInt32(result);

        }

        public static async Task<IDataReader> GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountOfUsersInRole(siteId, roleId, searchInput);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }


            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.Name, ");
            //sqlCommand.Append("u.Email, ");
            //sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserRoles ur ");

            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (UPPER(u.Name) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LoginName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.Email) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.LastName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(u.FirstName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(")");
            }
 
            sqlCommand.Append("ORDER BY u.Name  ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + searchInput + "%";

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT r.* ");
            sqlCommand.Append("FROM	mp_Roles r ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON r.RoleID = ur.RoleID ");
            sqlCommand.Append("AND ur.UserID = @UserID ");

            sqlCommand.Append("WHERE r.SiteID = @SiteID  ");
            sqlCommand.Append("AND ur.UserID IS NULL  ");
            sqlCommand.Append("ORDER BY r.DisplayName  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<bool> AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
        {
            //MS SQL proc checks that no matching record exists, may need to check that
            //here 
            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter(":UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter(":RoleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = roleGuid.ToString();

            arParams[3] = new FbParameter(":RoleGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = roleGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_USERROLES_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                arParams);

            int newID = Convert.ToInt32(result);

            return (newID > -1);

        }

        public static async Task<bool> RemoveUser(int roleId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = @UserID  ");
            sqlCommand.Append("AND RoleID = @RoleID  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@RoleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static async Task<int> GetCountOfSiteRoles(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (UPPER(DisplayName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(RoleName) LIKE UPPER(@SearchInput)) ");
                
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return Convert.ToInt32(result);

        }

        public static async Task<IDataReader> GetPage(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountOfSiteRoles(siteId, searchInput);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }

            sqlCommand.Append("	r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid, ");
            sqlCommand.Append("COUNT(ur.UserID) As MemberCount ");

            sqlCommand.Append("FROM	mp_Roles r  ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON ur.RoleID = r.RoleID ");

            sqlCommand.Append("WHERE r.SiteID = @SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (UPPER(r.DisplayName) LIKE UPPER(@SearchInput)) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (UPPER(r.RoleName) LIKE UPPER(@SearchInput)) ");

                sqlCommand.Append(")");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName  ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchInput", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



        



    }
}
