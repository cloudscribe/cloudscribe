// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
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
    internal class DBUserLogins
    {
        internal DBUserLogins(
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


        public bool Create(
            int siteId,
            string loginProvider, 
            string providerKey,
            string providerDisplayName,
            string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserLogins (");
            sqlCommand.Append("LoginProvider ,");
            sqlCommand.Append("ProviderKey, ");
            sqlCommand.Append("UserId, ");
            sqlCommand.Append("SiteId, ");
            sqlCommand.Append("ProviderDisplayName ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(":LoginProvider, ");
            sqlCommand.Append(":ProviderKey, ");
            sqlCommand.Append(":UserId, ");
            sqlCommand.Append(":SiteId, ");
            sqlCommand.Append(":ProviderDisplayName ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":LoginProvider", DbType.String);
            arParams[0].Value = loginProvider;

            arParams[1] = new SqliteParameter(":ProviderKey", DbType.String);
            arParams[1].Value = providerKey;

            arParams[2] = new SqliteParameter(":UserId", DbType.String);
            arParams[2].Value = userId;

            arParams[3] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[3].Value = siteId;

            arParams[4] = new SqliteParameter(":ProviderDisplayName", DbType.String);
            arParams[4].Value = providerDisplayName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public bool Delete(
            int siteId,
            string loginProvider,
            string providerKey,
            string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:SiteId = -1) OR (SiteId = :SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("LoginProvider = :LoginProvider AND ");
            sqlCommand.Append("ProviderKey = :ProviderKey AND ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":LoginProvider", DbType.String);
            arParams[0].Value = loginProvider;

            arParams[1] = new SqliteParameter(":ProviderKey", DbType.String);
            arParams[1].Value = providerKey;

            arParams[2] = new SqliteParameter(":UserId", DbType.String);
            arParams[2].Value = userId;

            arParams[3] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[3].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteByUser(int siteId, string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
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

        public bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("SiteId = :SiteId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public DbDataReader Find(
            int siteId,
            string loginProvider, 
            string providerKey)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = :SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("LoginProvider = :LoginProvider ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ProviderKey = :ProviderKey ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":LoginProvider", DbType.String);
            arParams[0].Value = loginProvider;

            arParams[1] = new SqliteParameter(":ProviderKey", DbType.String);
            arParams[1].Value = providerKey;

            arParams[2] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[2].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetByUser(
            int siteId,
            string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
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
    }
}
