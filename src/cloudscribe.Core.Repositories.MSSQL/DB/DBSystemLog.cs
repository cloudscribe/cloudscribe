//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2014-08-29
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.DbHelpers.MSSQL;
using System;
using System.Data;

namespace cloudscribe.Core.Repositories.MSSQL
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SystemLog_Insert", 9);
            sph.DefineSqlParameter("@LogDate", SqlDbType.DateTime, ParameterDirection.Input, logDate);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@Culture", SqlDbType.NVarChar, 10, ParameterDirection.Input, culture);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, -1, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@ShortUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, shortUrl);
            sph.DefineSqlParameter("@Thread", SqlDbType.NVarChar, 255, ParameterDirection.Input, thread);
            sph.DefineSqlParameter("@LogLevel", SqlDbType.NVarChar, 20, ParameterDirection.Input, logLevel);
            sph.DefineSqlParameter("@Logger", SqlDbType.NVarChar, 255, ParameterDirection.Input, logger);
            sph.DefineSqlParameter("@Message", SqlDbType.NVarChar, -1, ParameterDirection.Input, message);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        public static void DeleteAll()
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SystemLog_DeleteAll", 0);
            //sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            sph.ExecuteNonQuery();
            

        }

        /// <summary>
        /// Deletes a row from the mp_SystemLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SystemLog_Delete", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool DeleteOlderThan(DateTime cutoffDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SystemLog_DeleteOlderThan", 1);
            sph.DefineSqlParameter("@CutoffDate", SqlDbType.DateTime, ParameterDirection.Input, cutoffDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool DeleteByLevel(string logLevel)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SystemLog_DeleteByLevel", 1);
            sph.DefineSqlParameter("@LogLevel", SqlDbType.NVarChar, 20, ParameterDirection.Input, logLevel);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_SystemLog table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_SystemLog_GetCount",
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SystemLog_SelectPageAscending", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SystemLog_SelectPageDescending", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


    }
}
