// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-01
// 

using cloudscribe.DbHelpers;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBRoles
    {
        internal DBRoles(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;

            // possibly will change this later to have SqliteFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqliteFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private AdoHelper AdoHelper;

        public int RoleCreate(
            Guid roleGuid,
            Guid siteGuid,
            int siteId,
            string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Roles (");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("RoleName, ");
            sqlCommand.Append("DisplayName, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("RoleGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":SiteID, ");
            sqlCommand.Append(":RoleName, ");
            sqlCommand.Append(":RoleName, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":RoleGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleName", DbType.String);
            arParams[1].Value = roleName;

            arParams[2] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new SqliteParameter(":RoleGuid", DbType.String);
            arParams[3].Value = roleGuid.ToString();

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                    connectionString,
                    sqlCommand.ToString(),
                    arParams).ToString());

            return newID;

        }

        public bool Update(int roleId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Roles ");
            sqlCommand.Append("SET DisplayName = :RoleName  ");
            sqlCommand.Append("WHERE RoleID = :RoleID  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            arParams[1] = new SqliteParameter(":RoleName", DbType.String);
            arParams[1].Value = roleName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool Delete(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE RoleID = :RoleID AND RoleName <> 'Admins' AND RoleName <> 'Content Administrators' AND RoleName <> 'Authenticated Users' AND RoleName <> 'Role Admins'  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool DeleteRolesBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE SiteID = :SiteID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool DeleteUserRoles(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = :UserID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool DeleteUserRolesByRole(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = :RoleID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool DeleteUserRolesBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID IN (SELECT RoleID FROM mp_Roles WHERE SiteID = :SiteID);");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public DbDataReader GetById(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE RoleID = :RoleID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetByName(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = :SiteID AND RoleName = :RoleName ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleName", DbType.String);
            arParams[1].Value = roleName;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public bool Exists(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = :SiteID AND RoleName = :RoleName ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleName", DbType.String);
            arParams[1].Value = roleName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public DbDataReader GetSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("r.RoleID AS RoleID, ");
            sqlCommand.Append("r.SiteID AS SiteID, ");
            sqlCommand.Append("r.RoleName AS RoleName, ");
            sqlCommand.Append("r.DisplayName AS DisplayName, ");
            sqlCommand.Append("r.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("r.RoleGuid AS RoleGuid, ");
            sqlCommand.Append("COUNT(ur.UserID) As MemberCount ");

            sqlCommand.Append("FROM	mp_Roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON ur.RoleID = r.RoleID ");

            sqlCommand.Append("WHERE r.SiteID = :SiteID  ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetRoleMembers(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_UserRoles.UserID As UserID, ");
            sqlCommand.Append("mp_Users.Name As Name, ");
            sqlCommand.Append("mp_Users.LoginName As LoginName, ");
            sqlCommand.Append("mp_Users.Email As Email ");

            sqlCommand.Append("FROM	mp_UserRoles ");
            sqlCommand.Append("INNER JOIN mp_Users ");
            sqlCommand.Append("ON mp_Users.UserID = mp_UserRoles.UserID ");

            sqlCommand.Append("WHERE mp_UserRoles.RoleID = :RoleID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public int GetCountOfUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");

            sqlCommand.Append("FROM	mp_Users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = :RoleID ");

            sqlCommand.Append("WHERE u.SiteID = :SiteID  ");
            sqlCommand.Append("AND ur.RoleID IS NULL  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE :SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[2].Value = "%" + searchInput + "%";

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetUsersNotInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.UserID as UserID, ");
            //sqlCommand.Append("u.Name As Name, ");
            //sqlCommand.Append("u.Email As Email, ");
            //sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = :RoleID ");

            sqlCommand.Append("WHERE u.SiteID = :SiteID  ");
            sqlCommand.Append("AND ur.RoleID IS NULL  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE :SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.Name  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public int GetCountOfUsersInRole(int siteId, int roleId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserRoles ur ");

            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = :RoleID ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE :SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("WHERE u.SiteID = :SiteID  ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[2].Value = "%" + searchInput + "%";

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.UserID as UserID, ");
            //sqlCommand.Append("u.Name As Name, ");
            //sqlCommand.Append("u.Email As Email, ");
            //sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserRoles ur ");

            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = :RoleID ");

            sqlCommand.Append("WHERE u.SiteID = :SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (u.Name LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LoginName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.Email LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.LastName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (u.FirstName LIKE :SearchInput) ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY u.Name  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[1].Value = roleId;

            arParams[2] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[2].Value = "%" + searchInput + "%";

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

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
            sqlCommand.Append("AND ur.UserID = :UserID ");

            sqlCommand.Append("WHERE r.SiteID = :SiteID  ");
            sqlCommand.Append("AND ur.UserID IS NULL  ");
            sqlCommand.Append("ORDER BY r.DisplayName  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Value = userId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public bool AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
        {
            //MS SQL proc checks that no matching record exists, may need to check that
            //here 
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserRoles (UserID, RoleID, UserGuid, RoleGuid) ");
            sqlCommand.Append("VALUES ( :UserID , :RoleID, :UserGuid, :RoleGuid); ");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":UserGuid", DbType.String);
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new SqliteParameter(":RoleGuid", DbType.String);
            arParams[3].Value = roleGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                    connectionString,
                    sqlCommand.ToString(),
                    arParams);

            return (rowsAffected > -1);

        }

        public bool RemoveUser(int roleId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = :UserID  ");
            sqlCommand.Append("AND RoleID = :RoleID  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":RoleID", DbType.Int32);
            arParams[0].Value = roleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = :SiteID  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public int GetCountOfSiteRoles(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = :SiteID  ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (DisplayName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (RoleName LIKE :SearchInput) ");
                sqlCommand.Append(")");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[1].Value = "%" + searchInput + "%";

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetPage(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("r.RoleID AS RoleID, ");
            sqlCommand.Append("r.SiteID AS SiteID, ");
            sqlCommand.Append("r.RoleName AS RoleName, ");
            sqlCommand.Append("r.DisplayName AS DisplayName, ");
            sqlCommand.Append("r.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("r.RoleGuid AS RoleGuid, ");
            sqlCommand.Append("COUNT(ur.UserID) As MemberCount ");

            sqlCommand.Append("FROM	mp_Roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON ur.RoleID = r.RoleID ");

            sqlCommand.Append("WHERE r.SiteID = :SiteID  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (DisplayName LIKE :SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (RoleName LIKE :SearchInput) ");
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

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SearchInput", DbType.String);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

    }
}
