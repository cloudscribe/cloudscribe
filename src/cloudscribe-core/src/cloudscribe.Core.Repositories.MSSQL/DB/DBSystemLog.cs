// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2015-08-18
// 


using cloudscribe.DbHelpers.MSSQL;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{
    internal class DBSystemLog
    {
        internal DBSystemLog(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SystemLog_Insert", 
                9);

            sph.DefineSqlParameter("@LogDate", SqlDbType.DateTime, ParameterDirection.Input, logDate);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@Culture", SqlDbType.NVarChar, 10, ParameterDirection.Input, culture);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, -1, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@ShortUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, shortUrl);
            sph.DefineSqlParameter("@Thread", SqlDbType.NVarChar, 255, ParameterDirection.Input, thread);
            sph.DefineSqlParameter("@LogLevel", SqlDbType.NVarChar, 20, ParameterDirection.Input, logLevel);
            sph.DefineSqlParameter("@Logger", SqlDbType.NVarChar, 255, ParameterDirection.Input, logger);
            sph.DefineSqlParameter("@Message", SqlDbType.NVarChar, -1, ParameterDirection.Input, message);
            int newID = Convert.ToInt32(sph.ExecuteScalarAsync());
            return newID;
        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        public async Task<bool> DeleteAll()
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SystemLog_DeleteAll", 
                0);

            //sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);


        }

        /// <summary>
        /// Deletes a row from the mp_SystemLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public async Task<bool> Delete(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SystemLog_Delete", 
                1);

            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteOlderThan(DateTime cutoffDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SystemLog_DeleteOlderThan", 
                1);

            sph.DefineSqlParameter("@CutoffDate", SqlDbType.DateTime, ParameterDirection.Input, cutoffDate);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteByLevel(string logLevel)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SystemLog_DeleteByLevel", 
                1);

            sph.DefineSqlParameter("@LogLevel", SqlDbType.NVarChar, 20, ParameterDirection.Input, logLevel);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_SystemLog table.
        /// </summary>
        public async Task<int> GetCount()
        {
            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_SystemLog_GetCount",
                null);

            return Convert.ToInt32(result);

        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public async Task<DbDataReader> GetPageAscending(
            int pageNumber,
            int pageSize)
        {
            //totalPages = 1;
            //int totalRows = GetCount();

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SystemLog_SelectPageAscending", 
                2);

            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public async Task<DbDataReader> GetPageDescending(
            int pageNumber,
            int pageSize)
        {
            //totalPages = 1;
            //int totalRows = GetCount();

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SystemLog_SelectPageDescending", 
                2);

            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }


    }
}
