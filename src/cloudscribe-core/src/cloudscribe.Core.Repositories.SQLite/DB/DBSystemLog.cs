//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2015-06-14
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SQLite;
using Microsoft.Framework.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBSystemLog
    {
        internal DBSystemLog(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;

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
            sqlCommand.Append(":LogDate, ");
            sqlCommand.Append(":IpAddress, ");
            sqlCommand.Append(":Culture, ");
            sqlCommand.Append(":Url, ");
            sqlCommand.Append(":ShortUrl, ");
            sqlCommand.Append(":Thread, ");
            sqlCommand.Append(":LogLevel, ");
            sqlCommand.Append(":Logger, ");
            sqlCommand.Append(":Message )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SQLiteParameter[] arParams = new SQLiteParameter[9];

            arParams[0] = new SQLiteParameter(":LogDate", DbType.DateTime);
            arParams[0].Value = logDate;

            arParams[1] = new SQLiteParameter(":IpAddress", DbType.String);
            arParams[1].Value = ipAddress;

            arParams[2] = new SQLiteParameter(":Culture", DbType.String);
            arParams[2].Value = culture;

            arParams[3] = new SQLiteParameter(":Url", DbType.Object);
            arParams[3].Value = url;

            arParams[4] = new SQLiteParameter(":ShortUrl", DbType.String);
            arParams[4].Value = shortUrl;

            arParams[5] = new SQLiteParameter(":Thread", DbType.String);
            arParams[5].Value = thread;

            arParams[6] = new SQLiteParameter(":LogLevel", DbType.String);
            arParams[6].Value = logLevel;

            arParams[7] = new SQLiteParameter(":Logger", DbType.String);
            arParams[7].Value = logger;

            arParams[8] = new SQLiteParameter(":Message", DbType.Object);
            arParams[8].Value = message;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        public void DeleteAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("ID = :ID ");
            sqlCommand.Append(";");

            //SQLiteParameter[] arParams = new SQLiteParameter[1];

            //arParams[0] = new SQLiteParameter(":ID", DbType.Int32);
            //arParams[0].Value = id;

            AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Deletes a row from the mp_SystemLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = :ID ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":ID", DbType.Int32);
            arParams[0].Value = id;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public bool DeleteOlderThan(DateTime cutoffDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogDate < :CutoffDate ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":CutoffDate", DbType.DateTime);
            arParams[0].Value = cutoffDate;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public bool DeleteByLevel(string logLevel)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogLevel = :LogLevel ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":LogLevel", DbType.String);
            arParams[0].Value = logLevel;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_SystemLog table.
        /// </summary>
        public int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SystemLog ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPageAscending(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY ID  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new SQLiteParameter(":OffsetRows", DbType.Int32);
            arParams[1].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPageDescending(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY ID DESC ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new SQLiteParameter(":OffsetRows", DbType.Int32);
            arParams[1].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
