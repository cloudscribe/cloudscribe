// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-08
// 
// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Data.Common;
using cloudscribe.DbHelpers.MSSQL;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{
   
    internal static class DBRoles
    {
        
        public static async Task<int> RoleCreate(
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
            object result = await sph.ExecuteScalarAsync();
            int newID = Convert.ToInt32(result);
            return newID;
        }

        public static async Task<bool> Update(int roleId, string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Roles_Update", 2);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }

        public static async Task<bool> Delete(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Roles_Delete", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }

        public static async Task<bool> DeleteUserRoles(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_DeleteUserRoles", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }

        public static async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_DeleteUserRolesByRole", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }

        public static async Task<bool> Exists(int siteId, string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_RoleExists", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            object result = await sph.ExecuteScalarAsync();
            int count = Convert.ToInt32(result);
            return (count > 0);

        }

        public static async Task<DbDataReader> GetById(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectOne", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            return await sph.ExecuteReaderAsync();
        }

        public static async Task<DbDataReader> GetByName(int siteId, string roleName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectOneByName", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            return await sph.ExecuteReaderAsync();
        }

        public static DbDataReader GetSiteRoles(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_Select", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static DbDataReader GetRoleMembers(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_SelectByRoleID", 1);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            return sph.ExecuteReader();
        }

        public static async Task<int> GetCountOfUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_CountNotInRole", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result); 
        }

        public static async Task<DbDataReader> GetUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            int pageNumber, 
            int pageSize)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_SelectNotInRole", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();
        }

        public static async Task<int> GetCountOfUsersInRole(int siteId, int roleId, string searchInput)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_CountInRole", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public static async Task<DbDataReader> GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserRoles_SelectInRole", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);

            return await sph.ExecuteReaderAsync();
        }

        public static DbDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectRolesUserIsNotIn", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }

        public static async Task<bool> AddUser(
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
            
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }

        public static async Task<bool> RemoveUser(int roleId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserRoles_Delete", 2);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }


        public static int GetCountOfSiteRoles(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_CountBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static async Task<int> GetCountOfSiteRoles(int siteId, string searchInput)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_CountBySearch", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public static async Task<DbDataReader> GetPage(
            int siteId, 
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            //totalPages = 1;
            //int totalRows
            //    = GetCountOfSiteRoles(siteId, searchInput);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Roles_SelectPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }
       

    }
}
