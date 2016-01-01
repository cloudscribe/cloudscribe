// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-01
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Core.Repositories.pgsql
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

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
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
            sqlCommand.Append("INSERT INTO mp_roles (");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("rolename, ");
            sqlCommand.Append("displayname, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("roleguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":rolename, ");
            sqlCommand.Append(":displayname, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":roleguid )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_roles_roleid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];
            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("rolename", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = roleName;

            arParams[2] = new NpgsqlParameter("displayname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = roleName;

            arParams[3] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Value = siteGuid.ToString();

            arParams[4] = new NpgsqlParameter("roleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Value = roleGuid.ToString();


            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                    CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            arParams[1] = new NpgsqlParameter("rolename", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Value = roleName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_roles ");
            sqlCommand.Append("SET  ");
           
            sqlCommand.Append("displayname = :rolename ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("roleid = :roleid ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);
        }

        public async Task<bool> Delete(
            int roleId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("roleid = :roleid ");
            sqlCommand.Append("AND rolename <> 'Admins' ");
            sqlCommand.Append("AND rolename <> 'Content Administrators' ");
            sqlCommand.Append("AND rolename <> 'Authenticated Users' ");
            sqlCommand.Append("AND rolename <> 'Role Admins' ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteRolesBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteUserRoles(
            int userId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = userId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userroles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteUserRolesByRole(
            int roleId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userroles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("roleid = :roleid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteUserRolesBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userroles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("roleid IN (SELECT roleid FROM mp_roles where siteid = :siteid) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<DbDataReader> GetById(
            int roleId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("roleid = :roleid ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

        public async Task<DbDataReader> GetByName(
            int siteId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("rolename", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Value = roleName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND rolename = :rolename ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }

        public async Task<bool> Exists(
            int siteId, 
            string roleName,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("rolename", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Value = roleName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND rolename = :rolename ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return (count > 0);

        }

        public DbDataReader GetSiteRoles(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("r.roleid, ");
            sqlCommand.Append("r.siteid, ");
            sqlCommand.Append("r.rolename, ");
            sqlCommand.Append("r.displayname, ");
            sqlCommand.Append("r.siteguid, ");
            sqlCommand.Append("r.roleguid, ");
            sqlCommand.Append("COUNT(ur.userid) As membercount ");

            sqlCommand.Append("FROM	mp_roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON ur.roleid = r.roleid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("r.siteid = :siteid ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.roleid, ");
            sqlCommand.Append("r.siteid, ");
            sqlCommand.Append("r.rolename, ");
            sqlCommand.Append("r.displayname, ");
            sqlCommand.Append("r.siteguid, ");
            sqlCommand.Append("r.roleguid ");

            sqlCommand.Append("ORDER BY r.displayname ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetRoleMembers(int roleId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_userroles.userid, ");
            sqlCommand.Append("mp_users.name, ");
            sqlCommand.Append("mp_users.loginname, ");
            sqlCommand.Append("mp_users.email ");

            sqlCommand.Append("FROM	mp_userroles ");
            sqlCommand.Append("INNER JOIN mp_users ");
            sqlCommand.Append("ON mp_users.userid = mp_userroles.userid ");

            sqlCommand.Append("WHERE mp_userroles.roleid = :roleid ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
               readConnectionString,
               CommandType.Text,
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
            sqlCommand.Append("COUNT(*) ");

            sqlCommand.Append("FROM	mp_users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");
            sqlCommand.Append("AND ur.roleid IS NULL  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.loginname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.email LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.lastname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.firstname LIKE :searchinput) ");
                sqlCommand.Append(")");
            }
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = roleId;

            arParams[2] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
               readConnectionString,
               CommandType.Text,
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


            sqlCommand.Append("FROM	mp_users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");
            sqlCommand.Append("AND ur.roleid IS NULL  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.loginname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.email LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.lastname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.firstname LIKE :searchinput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = roleId;

            arParams[2] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
               readConnectionString,
               CommandType.Text,
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
            sqlCommand.Append("COUNT(*) ");

            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("JOIN mp_userroles ur ");
            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.loginname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.email LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.lastname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.firstname LIKE :searchinput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = roleId;

            arParams[2] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
               readConnectionString,
               CommandType.Text,
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


            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("JOIN mp_userroles ur ");

            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.loginname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.email LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.lastname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.firstname LIKE :searchinput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = roleId;

            arParams[2] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
               readConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               arParams,
               cancellationToken);
        }

        public DbDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = userId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  r.* ");
            sqlCommand.Append("FROM	mp_roles r ");
            sqlCommand.Append("left outer join mp_userroles  ur ");
            sqlCommand.Append("ON ur.roleid = r.roleid ");
            sqlCommand.Append("AND ur.userid = :userid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("r.siteid = :siteid ");
            sqlCommand.Append("AND ur.userid is null ");
            sqlCommand.Append("ORDER BY r.displayname ");
            sqlCommand.Append(";");

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_roles_selectrolesuserisnotin(:siteid,:userid)",
            //    arParams);

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = userId;

            arParams[2] = new NpgsqlParameter("roleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Value = roleGuid.ToString();

            arParams[3] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Value = userGuid.ToString();

            //MS SQL proc checks that no matching record exists, may need to check that
            //here 

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userroles (");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("roleid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("roleguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":roleid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":roleguid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = roleId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = userId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userroles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("roleid = :roleid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");
            sqlCommand.Append("FROM	mp_roles ");
            sqlCommand.Append("WHERE siteid = :siteid  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
               readConnectionString,
               CommandType.Text,
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
            sqlCommand.Append("COUNT(*) ");
            sqlCommand.Append("FROM	mp_roles ");
            sqlCommand.Append("WHERE siteid = :siteid  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (displayname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (rolename LIKE :searchinput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("r.roleid, ");
            sqlCommand.Append("r.siteid, ");
            sqlCommand.Append("r.rolename, ");
            sqlCommand.Append("r.displayname, ");
            sqlCommand.Append("r.siteguid, ");
            sqlCommand.Append("r.roleguid, ");
            sqlCommand.Append("COUNT(ur.userid) As membercount ");

            sqlCommand.Append("FROM	mp_roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON ur.roleid = r.roleid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("r.siteid = :siteid ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (displayname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (rolename LIKE :searchinput) ");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.roleid, ");
            sqlCommand.Append("r.siteid, ");
            sqlCommand.Append("r.rolename, ");
            sqlCommand.Append("r.displayname, ");
            sqlCommand.Append("r.siteguid, ");
            sqlCommand.Append("r.roleguid ");

            sqlCommand.Append("ORDER BY r.displayname ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }

    }
}
