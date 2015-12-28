// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2015-12-27
// 

using cloudscribe.DbHelpers.pgsql;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.pgsql
{
    internal class DBSystemLog
    {
        internal DBSystemLog(
            string dbReadConnectionString,
            string dbWriteConnectionString)
        {
            
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;
        }

        
        private string readConnectionString;
        private string writeConnectionString;

        /// <summary>
        /// Inserts a row in the mp_SystemLog table. Returns new integer id.
        /// </summary>
        /// <param name="logDate"> logDate </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <param name="culture"> culture </param>
        /// <param name="url"> url </param>
        /// <param name="shortUrl"> shortUrl </param>
        /// <param name="thread"> thread </param>
        /// <param name="logLevel"> logLevel </param>
        /// <param name="logger"> logger </param>
        /// <param name="message"> message </param>
        /// <returns>int</returns>
        public int Create(
            DateTime logDate,
            string ipAddress,
            string culture,
            string url,
            string shortUrl,
            string thread,
            string logLevel,
            string logger,
            string message)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_systemlog (");
            sqlCommand.Append("logdate, ");
            sqlCommand.Append("ipaddress, ");
            sqlCommand.Append("culture, ");
            sqlCommand.Append("url, ");
            sqlCommand.Append("shorturl, ");
            sqlCommand.Append("thread, ");
            sqlCommand.Append("loglevel, ");
            sqlCommand.Append("logger, ");
            sqlCommand.Append("message )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":logdate, ");
            sqlCommand.Append(":ipaddress, ");
            sqlCommand.Append(":culture, ");
            sqlCommand.Append(":url, ");
            sqlCommand.Append(":shorturl, ");
            sqlCommand.Append(":thread, ");
            sqlCommand.Append(":loglevel, ");
            sqlCommand.Append(":logger, ");
            sqlCommand.Append(":message )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_systemlogid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[9];
            arParams[0] = new NpgsqlParameter("logdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[0].Value = logDate;

            arParams[1] = new NpgsqlParameter("ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = ipAddress;

            arParams[2] = new NpgsqlParameter("culture", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[2].Value = culture;

            arParams[3] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Value = url;

            arParams[4] = new NpgsqlParameter("shorturl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Value = shortUrl;

            arParams[5] = new NpgsqlParameter("thread", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Value = thread;

            arParams[6] = new NpgsqlParameter("loglevel", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[6].Value = logLevel;

            arParams[7] = new NpgsqlParameter("logger", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Value = logger;

            arParams[8] = new NpgsqlParameter("message", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Value = message;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newID;

        }


        
        public async Task<bool> DeleteAll(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");

            sqlCommand.Append(";");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return (rowsAffected > 0);

        }

        
        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
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

       
        public async Task<bool> DeleteOlderThan(DateTime cutoffDate, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("logdate < :cutoffdate ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("cutoffdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[0].Value = cutoffDate;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        
        public async Task<bool> DeleteByLevel(string logLevel, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("loglevel = :loglevel ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("loglevel", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = logLevel;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams, 
                cancellationToken);

            return (rowsAffected > -1);

        }

        
        public async Task<int> GetCount(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_systemlog ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return Convert.ToInt32(result);
        }

       
        public async Task<DbDataReader> GetPageAscending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_systemlog  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY id  ");
            //sqlCommand.Append("  ");
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

        
        public async Task<DbDataReader> GetPageDescending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_systemlog  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY id DESC  ");
            //sqlCommand.Append("  ");
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
