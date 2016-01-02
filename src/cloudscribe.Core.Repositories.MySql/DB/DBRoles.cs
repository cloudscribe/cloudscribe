// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-02
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
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

            // possibly will change this later to have MySqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(MySqlClientFactory.Instance);
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Roles (");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("RoleName, ");
            sqlCommand.Append("DisplayName, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("RoleGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?SiteID, ");
            sqlCommand.Append("?RoleName, ");
            sqlCommand.Append("?DisplayName, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?RoleGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50);
            arParams[1].Value = roleName;

            arParams[2] = new MySqlParameter("?DisplayName", MySqlDbType.VarChar, 50);
            arParams[2].Value = roleName;

            arParams[3] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[3].Value = siteGuid.ToString();

            arParams[4] = new MySqlParameter("?RoleGuid", MySqlDbType.VarChar, 36);
            arParams[4].Value = roleGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public async Task<bool> Update(
            int roleId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Roles ");
            sqlCommand.Append("SET DisplayName = ?RoleName  ");
            sqlCommand.Append("WHERE RoleID = ?RoleID  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            arParams[1] = new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50);
            arParams[1].Value = roleName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> Delete(
            int roleId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID AND RoleName <> 'Admins' AND RoleName <> 'Content Administrators' AND RoleName <> 'Authenticated Users' AND RoleName <> 'Role Admins'  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> DeleteRolesBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID;");

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

        public async Task<bool> DeleteUserRoles(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> DeleteUserRolesByRole(
            int roleId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<bool> DeleteUserRolesBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID IN (SELECT RoleID FROM mp_Roles WHERE SiteID = ?SiteID) ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        public async Task<DbDataReader> GetById(
            int roleId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetByName(
            int siteId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID AND RoleName = ?RoleName ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50);
            arParams[1].Value = roleName;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<bool> Exists(
            int siteId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID AND RoleName = ?RoleName ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50);
            arParams[1].Value = roleName;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return (count > 0);

        }

        public DbDataReader GetSiteRoles(int siteId)
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

            sqlCommand.Append("WHERE r.SiteID = ?SiteID  ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetRoleMembers(int roleId)
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

            sqlCommand.Append("WHERE mp_UserRoles.RoleID = ?RoleID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<int> GetCountOfUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID NOT IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE ?SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("u.* ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID NOT IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE ?SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.Name  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[3].Value = pageSize;

            arParams[4] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[4].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> GetCountOfUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE ?SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.Name, ");
            //sqlCommand.Append("u.Email, ");
            //sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE ?SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.Name  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[3].Value = pageSize;

            arParams[4] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[4].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public DbDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT r.* ");
            sqlCommand.Append("FROM	mp_Roles r ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON r.RoleID = ur.RoleID ");
            sqlCommand.Append("AND ur.UserID = ?UserID ");

            sqlCommand.Append("WHERE r.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ur.UserID IS NULL  ");
            sqlCommand.Append("ORDER BY r.DisplayName  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Value = userId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<bool> AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken
            )
        {
            //MS SQL proc checks that no matching record exists, may need to check that
            //here 
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserRoles (UserID, RoleID, RoleGuid, UserGuid) ");
            sqlCommand.Append("VALUES ( ?UserID , ?RoleID, ?RoleGuid, ?UserGuid); ");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Value = userId;

            arParams[2] = new MySqlParameter("?RoleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Value = roleGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Value = userGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> RemoveUser(
            int roleId, 
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = ?UserID  ");
            sqlCommand.Append("AND RoleID = ?RoleID  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Value = roleId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID  ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public async Task<int> GetCountOfSiteRoles(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (DisplayName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (RoleName LIKE ?SearchInput) ");


                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

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

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetPage(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

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

            sqlCommand.Append("WHERE r.SiteID = ?SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (DisplayName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (RoleName LIKE ?SearchInput) ");


                sqlCommand.Append(")");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");


            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

    }
}
