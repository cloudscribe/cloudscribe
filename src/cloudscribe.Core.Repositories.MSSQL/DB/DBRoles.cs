// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-03
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{

    internal class DBRoles
    {

        internal DBRoles(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have SqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqlClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public async Task<int> RoleCreate(
            Guid roleGuid,
            Guid siteGuid,
            int siteId,
            string roleName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Roles_Insert", 
                4);

            sph.DefineSqlParameter("@RoleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, roleGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int newID = Convert.ToInt32(result);
            return newID;
        }

        public async Task<bool> Update(
            int roleId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Roles_Update", 
                2);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> Delete(
            int roleId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Roles_Delete", 
                1);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteRolesBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_Roles_DeleteBySite",
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteUserRoles(
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserRoles_DeleteUserRoles", 
                1);

            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteUserRolesByRole(
            int roleId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserRoles_DeleteUserRolesByRole", 
                1);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteUserRolesBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_UserRoles_DeleteUserRolesBySite",
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> Exists(
            int siteId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_RoleExists", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return (count > 0);

        }

        public async Task<DbDataReader> GetById(
            int roleId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_SelectOne", 
                1);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetByName(
            int siteId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_SelectOneByName", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleName", SqlDbType.NVarChar, 50, ParameterDirection.Input, roleName);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public DbDataReader GetSiteRoles(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_Select", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public DbDataReader GetRoleMembers(int roleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserRoles_SelectByRoleID", 
                1);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            return sph.ExecuteReader();
        }

        public async Task<int> GetCountOfUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserRoles_CountNotInRole", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetUsersNotInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserRoles_SelectNotInRole", 
                5);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<int> GetCountOfUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserRoles_CountInRole", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserRoles_SelectInRole", 
                5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);

            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public DbDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_SelectRolesUserIsNotIn", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }

        public async Task<bool> AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserRoles_Insert", 
                4);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@RoleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, roleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);

            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> RemoveUser(
            int roleId, 
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserRoles_Delete",
                2);

            sph.DefineSqlParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, roleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }


        public int GetCountOfSiteRoles(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_CountBySite", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);

            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public async Task<int> GetCountOfSiteRoles(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_CountBySearch", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetPage(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Roles_SelectPage", 
                4);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }


    }
}
