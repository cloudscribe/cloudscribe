// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
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

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public async Task<bool> Create(
            int siteId,
            string loginProvider, 
            string providerKey,
            string providerDisplayName,
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userlogins (");
            sqlCommand.Append("loginprovider ,");
            sqlCommand.Append("providerkey, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("providerdisplayname ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(":loginprovider, ");
            sqlCommand.Append(":providerkey, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":providerdisplayname ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("loginprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new NpgsqlParameter("providerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[2].Value = userId;

            arParams[3] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = siteId;

            arParams[4] = new NpgsqlParameter("providerdisplayname", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[4].Value = providerDisplayName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }


        public async Task<bool> Delete(
            int siteId,
            string loginProvider,
            string providerKey,
            string userId,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:siteid = -1) OR (siteid = :siteid)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("loginprovider = :loginprovider AND ");
            sqlCommand.Append("providerkey = :providerkey AND ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("loginprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new NpgsqlParameter("providerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[2].Value = userId;

            arParams[3] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
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
            sqlCommand.Append("DELETE FROM mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:siteid = -1) OR (siteid = :siteid)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }


        public async Task<bool> DeleteBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("siteid = :siteid ");
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

        public async Task<DbDataReader> Find(
            int siteId,
            string loginProvider, 
            string providerKey,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("loginprovider = :loginprovider ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("providerkey = :providerkey ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("loginprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new NpgsqlParameter("providerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
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
            sqlCommand.Append("FROM	mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("userid = :userid ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
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
