// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2016-01-01
// 

using cloudscribe.DbHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBUserClaims
    {
        internal DBUserClaims(
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

        public int Create(
            int siteId,
            string userId,
            string claimType,
            string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserClaims (");
            sqlCommand.Append("SiteId, ");
            sqlCommand.Append("UserId, ");
            sqlCommand.Append("ClaimType, ");
            sqlCommand.Append("ClaimValue )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":SiteId, ");
            sqlCommand.Append(":UserId, ");
            sqlCommand.Append(":ClaimType, ");
            sqlCommand.Append(":ClaimValue )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":ClaimType", DbType.Object);
            arParams[1].Value = claimType;

            arParams[2] = new SqliteParameter(":ClaimValue", DbType.Object);
            arParams[2].Value = claimValue;

            arParams[3] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[3].Value = siteId;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = :Id ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Id", DbType.Int32);
            arParams[0].Value = id;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteByUser(int siteId, string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:SiteId = -1) OR (SiteId = :SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[1].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteByUser(int siteId, string userId, string claimType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:SiteId = -1) OR (SiteId = :SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = :ClaimType ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":ClaimType", DbType.Object);
            arParams[1].Value = claimType;

            arParams[2] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[2].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteByUser(int siteId, string userId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:SiteId = -1) OR (SiteId = :SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = :ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimValue = :ClaimValue ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":ClaimType", DbType.Object);
            arParams[1].Value = claimType;

            arParams[2] = new SqliteParameter(":ClaimValue", DbType.Object);
            arParams[2].Value = claimValue;

            arParams[3] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[3].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = :SiteId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public DbDataReader GetByUser(
            int siteId,
            string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = :SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[1].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetUsersByClaim(int siteId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  u.* ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserClaims uc ");
            sqlCommand.Append("ON u.UserID = uc.UserId ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = :SiteId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("uc.ClaimType = :ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("uc.ClaimValue = :ClaimValue ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("u.Name ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":UserId", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":ClaimType", DbType.String);
            arParams[1].Value = claimType;

            arParams[2] = new SqliteParameter(":ClaimValue", DbType.String);
            arParams[2].Value = claimValue;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
