// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2015-12-27
// 

using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Logging.Firebird
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
            FbParameter[] arParams = new FbParameter[9];

            arParams[0] = new FbParameter(":LogDate", FbDbType.TimeStamp);
            arParams[0].Value = logDate;

            arParams[1] = new FbParameter(":IpAddress", FbDbType.VarChar, 50);
            arParams[1].Value = ipAddress;

            arParams[2] = new FbParameter(":Culture", FbDbType.VarChar, 10);
            arParams[2].Value = culture;

            arParams[3] = new FbParameter(":Url", FbDbType.VarChar);
            arParams[3].Value = url;

            arParams[4] = new FbParameter(":ShortUrl", FbDbType.VarChar, 255);
            arParams[4].Value = shortUrl;

            arParams[5] = new FbParameter(":Thread", FbDbType.VarChar, 255);
            arParams[5].Value = thread;

            arParams[6] = new FbParameter(":LogLevel", FbDbType.VarChar, 20);
            arParams[6].Value = logLevel;

            arParams[7] = new FbParameter(":Logger", FbDbType.VarChar, 255);
            arParams[7].Value = logger;

            arParams[8] = new FbParameter(":Message", FbDbType.VarChar);
            arParams[8].Value = message;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SYSTEMLOG_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

        }

       
        public async Task<bool> DeleteAll(CancellationToken cancellationToken)
        {

            //TODO: does firebird support truncate table?
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@ID", FbDbType.Integer);
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
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Value = id;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        
        public async Task<bool> DeleteOlderThan(DateTime cutoffDate, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogDate < @CutoffDate ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CutoffDate", FbDbType.TimeStamp);
            arParams[0].Value = cutoffDate;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

       
        public async Task<bool> DeleteByLevel(string logLevel, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogLevel = @LogLevel ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LogLevel", FbDbType.VarChar, 20);
            arParams[0].Value = logLevel;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY ID  ");
            sqlCommand.Append("	; ");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }

        
        public async Task<DbDataReader> GetPageDescending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY ID DESC  ");
            sqlCommand.Append("	; ");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }

    }
}
