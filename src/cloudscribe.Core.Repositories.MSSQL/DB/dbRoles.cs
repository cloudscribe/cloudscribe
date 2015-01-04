// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2014-08-22
// 
// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using cloudscribe.DbHelpers.MSSQL;

namespace cloudscribe.Core.Repositories.MSSQL
{
   
    internal static class DBRoles
    {
        
        public static int RoleCreate(
            Guid roleGuid,
            Guid siteGuid,
            int siteId,
            string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Roles_Insert", 4);
            sph.DefineSqlParameter("@RoleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, roleGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(int roleId, string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Roles_Update", 2);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool Delete(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Roles_Delete", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteUserRoles(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_DeleteUserRoles", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteUserRolesByRole(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_DeleteUserRolesByRole", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool Exists(int siteId, string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_RoleExists", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);

        }

        public static IDataReader GetById(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectOne", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByName(int siteId, string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectOneByName", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSiteRoles(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_Select", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetRoleMembers(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_SelectByRoleID", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            return sph.ExecuteReader();
        }

        public static int GetCountOfUsersNotInRole(int siteId, int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_CountNotInRole", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);

            return Convert.ToInt32(sph.ExecuteScalar()); 
        }

        public static IDataReader GetUsersNotInRole(
            int siteId, 
            int roleId, 
            int pageNumber, 
            int pageSize, 
			out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountOfUsersNotInRole(siteId, roleId);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_SelectNotInRole", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountOfUsersInRole(int siteId, int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_CountInRole", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);

            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetUsersInRole(
            int siteId,
            int roleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountOfUsersInRole(siteId, roleId);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_SelectInRole", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectRolesUserIsNotIn", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }

        public static bool AddUser(
            int roleId, 
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_Insert", 4);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@RoleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, roleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool RemoveUser(int roleId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_Delete", 2);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static int GetCountOfSiteRoles(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_CountBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            

            return Convert.ToInt32(sph.ExecuteScalar());
        }
       

    }
}
