// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2016-01-02
//

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Text;

namespace cloudscribe.Core.Repositories.SqlCe
{
    internal class DBUserLogins
    {
        internal DBUserLogins(
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
            sqlCommand.Append("@LoginProvider, ");
            sqlCommand.Append("@ProviderKey, ");
            sqlCommand.Append("@UserId, ");
            sqlCommand.Append("@SiteId, ");
            sqlCommand.Append("@ProviderDisplayName ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@LoginProvider", SqlDbType.NVarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new SqlCeParameter("@ProviderKey", SqlDbType.NVarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[2].Value = userId;

            arParams[3] = new SqlCeParameter("@SiteId", SqlDbType.Int);
            arParams[3].Value = siteId;

            arParams[4] = new SqlCeParameter("@ProviderDisplayName", SqlDbType.NVarChar, 100);
            arParams[4].Value = providerDisplayName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("((@SiteId = -1) OR (SiteId = @SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("LoginProvider = @LoginProvider AND ");
            sqlCommand.Append("ProviderKey = @ProviderKey AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@LoginProvider", SqlDbType.NVarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new SqlCeParameter("@ProviderKey", SqlDbType.NVarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[2].Value = userId;

            arParams[3] = new SqlCeParameter("@SiteId", SqlDbType.Int);
            arParams[3].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool DeleteByUser(int siteId, string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((@SiteId = -1) OR (SiteId = @SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@SiteId", SqlDbType.Int);
            arParams[1].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("SiteId = @SiteId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteId", SqlDbType.Int);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("SiteId = @SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("LoginProvider = @LoginProvider ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ProviderKey = @ProviderKey  ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@LoginProvider", SqlDbType.NVarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new SqlCeParameter("@ProviderKey", SqlDbType.NVarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new SqlCeParameter("@SiteId", SqlDbType.Int);
            arParams[2].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("SiteId = @SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = @UserId  ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@SiteId", SqlDbType.Int);
            arParams[1].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
