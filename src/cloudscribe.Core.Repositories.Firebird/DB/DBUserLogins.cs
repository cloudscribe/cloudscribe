// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2016-01-02
// 


using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
{
    internal class DBUserLogins
    {
        internal DBUserLogins(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

        public async Task<bool> Create(
            int siteId,
            string loginProvider, 
            string providerKey, 
            string providerDisplayName,
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserLogins (");
            sqlCommand.Append("LoginProvider ,");
            sqlCommand.Append("ProviderKey, ");
            sqlCommand.Append("ProviderDisplayName, ");
            sqlCommand.Append("UserId, ");
            sqlCommand.Append("SiteId ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append("@LoginProvider, ");
            sqlCommand.Append("@ProviderKey, ");
            sqlCommand.Append("@ProviderDisplayName, ");
            sqlCommand.Append("@UserId, ");
            sqlCommand.Append("@SiteId ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter("@LoginProvider", FbDbType.VarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new FbParameter("@ProviderKey", FbDbType.VarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[2].Value = userId;

            arParams[3] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[3].Value = siteId;

            arParams[4] = new FbParameter("@ProviderDisplayName", FbDbType.VarChar, 100);
            arParams[4].Value = providerDisplayName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }


        public async Task<bool> Delete(
            int siteId,
            string loginProvider,
            string providerKey,
            string userId,
            CancellationToken cancellationToken)
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
            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@LoginProvider", FbDbType.VarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new FbParameter("@ProviderKey", FbDbType.VarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[2].Value = userId;

            arParams[3] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[3].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams, 
                cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((@SiteId = -1) OR (SiteId = @SiteId)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[1].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("SiteId = @SiteId ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<DbDataReader> Find(
            int siteId,
            string loginProvider, 
            string providerKey,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = @SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("LoginProvider = @LoginProvider ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ProviderKey = @ProviderKey ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@LoginProvider", FbDbType.VarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new FbParameter("@ProviderKey", FbDbType.VarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[2].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetByUser(
            int siteId,
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = @SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = @UserId ");
            
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[1].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

    }
}
