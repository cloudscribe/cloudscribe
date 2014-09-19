//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2014-08-29
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.pgsql;
using Npgsql;
using System;
using System.Data;
using System.Text;

namespace cloudscribe.Core.Repositories.pgsql
{
    internal static class DBSystemLog
    {
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
        public static int Create(
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
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = logDate;

            arParams[1] = new NpgsqlParameter("ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = ipAddress;

            arParams[2] = new NpgsqlParameter("culture", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = culture;

            arParams[3] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = url;

            arParams[4] = new NpgsqlParameter("shorturl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = shortUrl;

            arParams[5] = new NpgsqlParameter("thread", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = thread;

            arParams[6] = new NpgsqlParameter("loglevel", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = logLevel;

            arParams[7] = new NpgsqlParameter("logger", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = logger;

            arParams[8] = new NpgsqlParameter("message", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = message;


            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            return newID;

        }


        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        public static void DeleteAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("id = :id ");
            sqlCommand.Append(";");

            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            //arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = id;

            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

          
        }

        /// <summary>
        /// Deletes a row from the mp_SystemLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("id = :id ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool DeleteOlderThan(DateTime cutoffDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("logdate < :cutoffdate ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("cutoffdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cutoffDate;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool DeleteByLevel(string logLevel)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_systemlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("loglevel = :loglevel ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("loglevel", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = logLevel;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets a count of rows in the mp_SystemLog table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_systemlog ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageAscending(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
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

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageDescending(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
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

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }

}
