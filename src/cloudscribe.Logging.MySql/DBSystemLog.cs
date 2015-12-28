// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2015-12-27
// 

using cloudscribe.DbHelpers.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.MySql
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
            sqlCommand.Append("INSERT INTO mp_SystemLog (");
            sqlCommand.Append("LogDate, ");
            sqlCommand.Append("IpAddress, ");
            sqlCommand.Append("Culture, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("ShortUrl, ");
            sqlCommand.Append("Thread, ");
            sqlCommand.Append("LogLevel, ");
            sqlCommand.Append("Logger, ");
            sqlCommand.Append("Message )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?LogDate, ");
            sqlCommand.Append("?IpAddress, ");
            sqlCommand.Append("?Culture, ");
            sqlCommand.Append("?Url, ");
            sqlCommand.Append("?ShortUrl, ");
            sqlCommand.Append("?Thread, ");
            sqlCommand.Append("?LogLevel, ");
            sqlCommand.Append("?Logger, ");
            sqlCommand.Append("?Message )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?LogDate", MySqlDbType.DateTime);
            arParams[0].Value = logDate;

            arParams[1] = new MySqlParameter("?IpAddress", MySqlDbType.VarChar, 50);
            arParams[1].Value = ipAddress;

            arParams[2] = new MySqlParameter("?Culture", MySqlDbType.VarChar, 10);
            arParams[2].Value = culture;

            arParams[3] = new MySqlParameter("?Url", MySqlDbType.Text);
            arParams[3].Value = url;

            arParams[4] = new MySqlParameter("?ShortUrl", MySqlDbType.VarChar, 255);
            arParams[4].Value = shortUrl;

            arParams[5] = new MySqlParameter("?Thread", MySqlDbType.VarChar, 255);
            arParams[5].Value = thread;

            arParams[6] = new MySqlParameter("?LogLevel", MySqlDbType.VarChar, 20);
            arParams[6].Value = logLevel;

            arParams[7] = new MySqlParameter("?Logger", MySqlDbType.VarChar, 255);
            arParams[7].Value = logger;

            arParams[8] = new MySqlParameter("?Message", MySqlDbType.Text);
            arParams[8].Value = message;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }

        
        public async Task<bool> DeleteAll(CancellationToken cancellationToken)
        {
            //TODO: using TRUNCATE Table might be more efficient but possibly will cuase errors in some installations
            //http://dev.mysql.com/doc/refman/5.1/en/truncate-table.html

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("ID = ?ID ");
            sqlCommand.Append(";");

            //MySqlParameter[] arParams = new MySqlParameter[1];

            //arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = id;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return (rowsAffected > 0);

        }

        
        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = ?ID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
            arParams[0].Value = id;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        
        public async Task<bool> DeleteOlderThan(DateTime cutoffDate, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogDate < ?CutoffDate ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CutoffDate", MySqlDbType.DateTime);
            arParams[0].Value = cutoffDate;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        
        public async Task<bool> DeleteByLevel(string logLevel, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogLevel = ?LogLevel ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LogLevel", MySqlDbType.Int32);
            arParams[0].Value = logLevel;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        
        public async Task<int> GetCount(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SystemLog ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
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
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY ID  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[1].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
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
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY ID DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[1].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


    }
}
