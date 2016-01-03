// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2016-01-02
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Text;

namespace cloudscribe.Core.Repositories.SqlCe
{
    internal class DBRoles
    {
        internal DBRoles(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;

            // possibly will change this later to have SqlCeProviderFactory/DbProviderFactory injected
            AdoHelper = new SqlCeHelper(SqlCeProviderFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private SqlCeHelper AdoHelper;

        public int RoleCreate(
            Guid roleGuid,
            Guid siteGuid,
            int siteId,
            string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Roles ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("RoleName, ");
            sqlCommand.Append("DisplayName, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("RoleGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@RoleName, ");
            sqlCommand.Append("@DisplayName, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@RoleGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleName", SqlDbType.NVarChar, 50);
            arParams[1].Value = roleName;

            arParams[2] = new SqlCeParameter("@DisplayName", SqlDbType.NVarChar, 50);
            arParams[2].Value = roleName;

            arParams[3] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Value = siteGuid;

            arParams[4] = new SqlCeParameter("@RoleGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Value = roleGuid;

            int newId = Convert.ToInt32(AdoHelper.DoInsertGetIdentitiy(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

        }

        public bool Update(int roleId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Roles ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("DisplayName = @DisplayName ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RoleID = @RoleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[0].Value = roleId;

            arParams[1] = new SqlCeParameter("@DisplayName", SqlDbType.NVarChar, 50);
            arParams[1].Value = roleName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool Delete(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RoleID = @RoleID ");
            //these roles are not allowed to be deleted
            sqlCommand.Append("AND RoleName  <> 'Admins' ");
            sqlCommand.Append("AND RoleName <> 'Content Administrators' ");
            sqlCommand.Append("AND RoleName <> 'Authenticated Users' ");
            sqlCommand.Append("AND RoleName <> 'Role Admins' ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[0].Value = roleId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool DeleteRolesBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool DeleteUserRoles(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
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

        public bool DeleteUserRolesByRole(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RoleID = @RoleID ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[0].Value = roleId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool DeleteUserRolesBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RoleID IN (SELECT RoleID FROM mp_Roles WHERE SiteID = @SiteID) ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool Exists(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("AND (RoleName = @RoleName OR DisplayName = @RoleName) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleName", SqlDbType.NVarChar, 50);
            arParams[1].Value = roleName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);
        }

        public DbDataReader GetById(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RoleID = @RoleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetByName(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND RoleName = @RoleName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleName", SqlDbType.NVarChar, 50);
            arParams[1].Value = roleName;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("r.SiteID = @SiteID ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");
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

        public DbDataReader GetRoleMembers(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ur.UserID, ");
            sqlCommand.Append("u.[Name], ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.LoginName ");


            sqlCommand.Append("FROM	mp_UserRoles ur ");

            sqlCommand.Append("JOIN mp_Users  u ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ur.RoleID = @RoleID ");

            sqlCommand.Append("ORDER BY u.[Name] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[0].Value = roleId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int GetCountOfUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users  u ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID ");
            sqlCommand.Append("AND ur.RoleID IS NULL ");
            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");
                sqlCommand.Append("(u.[Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.[LoginName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.Email LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.LastName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.FirstName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[1].Value = roleId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");
            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.UserID, ");
            //sqlCommand.Append("u.[Name], ");
            //sqlCommand.Append("u.Email, ");
            //sqlCommand.Append("u.LoginName ");


            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID ");
            sqlCommand.Append("AND ur.RoleID IS NULL ");

            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");
                sqlCommand.Append("(u.[Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.[LoginName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.Email LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.LastName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.FirstName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.[Name]  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[1].Value = roleId;

            arParams[2] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[2].Value = searchInput;


            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int GetCountOfUsersInRole(int siteId, int roleId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Users  u ");

            sqlCommand.Append("JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID ");
            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");
                sqlCommand.Append("(u.[Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.[LoginName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.Email LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.LastName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.FirstName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[1].Value = roleId;

            arParams[2] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[2].Value = searchInput;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");
            sqlCommand.Append("u.* ");
            //sqlCommand.Append("u.UserID, ");
            //sqlCommand.Append("u.[Name], ");
            //sqlCommand.Append("u.Email, ");
            //sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON u.UserID = ur.UserID ");
            sqlCommand.Append("AND ur.RoleID = @RoleID ");

            sqlCommand.Append("WHERE u.SiteID = @SiteID ");
            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");
                sqlCommand.Append("(u.[Name]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.[LoginName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.Email LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.LastName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR (u.FirstName LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append(")");
            }

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.[Name]  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[1].Value = roleId;

            arParams[2] = new SqlCeParameter("@SearchInput", SqlDbType.NVarChar, 50);
            arParams[2].Value = searchInput;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  r.* ");
            sqlCommand.Append("FROM	mp_Roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON r.RoleID = ur.RoleID ");
            sqlCommand.Append("AND ur.UserID = @UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("r.SiteID = @SiteID ");
            sqlCommand.Append("AND ur.UserID IS NULL ");
            sqlCommand.Append("ORDER BY	r.[DisplayName] ");
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

        public bool AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserRoles ");
            sqlCommand.Append("(");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("RoleID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("RoleGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@RoleID, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@RoleGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[1].Value = roleId;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Value = userGuid;

            arParams[3] = new SqlCeParameter("@RoleGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Value = roleGuid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool RemoveUser(int roleId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("RoleID = @RoleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@RoleID", SqlDbType.Int);
            arParams[1].Value = roleId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
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

        public int GetCountOfSiteRoles(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");

            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");

                sqlCommand.Append("([DisplayName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR ([RoleName]  LIKE '%' + @SearchInput + '%') ");

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

        public DbDataReader GetPage(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("r.SiteID = @SiteID ");

            if (!string.IsNullOrEmpty(searchInput))
            {
                sqlCommand.Append("AND (");

                sqlCommand.Append("([DisplayName]  LIKE '%' + @SearchInput + '%') ");
                sqlCommand.Append("OR ([RoleName]  LIKE '%' + @SearchInput + '%') ");

                sqlCommand.Append(")");
            }
            //else
            //{
            //    sqlCommand.Append(" AND @SearchInput = '' ");
            //}

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

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

    }
}
