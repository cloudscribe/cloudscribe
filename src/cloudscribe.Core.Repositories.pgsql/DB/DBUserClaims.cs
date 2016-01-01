// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-11
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
    internal class DBUserClaims
    {
        internal DBUserClaims(
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


        public async Task<int> Create(
            int siteId,
            string userId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userclaims (");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("claimtype, ");
            sqlCommand.Append("claimvalue )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":claimtype, ");
            sqlCommand.Append(":claimvalue )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_userclaimsid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new NpgsqlParameter("claimvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = claimValue;

            arParams[3] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public async Task<bool> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("id = :id ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = id;

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
            sqlCommand.Append("DELETE FROM mp_userclaims ");
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

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId, 
            string claimType,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:siteid = -1) OR (siteid = :siteid)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("claimtype = :claimtype ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = siteId;

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
            string claimType, 
            string claimValue,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((:siteid = -1) OR (siteid = :siteid)) ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("claimtype = :claimtype ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("claimvalue = :claimvalue ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new NpgsqlParameter("claimvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = claimValue;

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

        public async Task<bool> DeleteBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
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

        public async Task<DbDataReader> GetByUser(
            int siteId, 
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userclaims ");
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

        public async Task<DbDataReader> GetUsersByClaim(
            int siteId, 
            string claimType, 
            string claimValue,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  u.* ");

            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("JOIN mp_userclaims uc ");
            sqlCommand.Append("ON u.userid = uc.userid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.siteid = :siteid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("uc.claimtype = :claimtype ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("uc.claimvalue = :claimvalue ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("u.name ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new NpgsqlParameter("claimvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = claimValue;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


    }
}
