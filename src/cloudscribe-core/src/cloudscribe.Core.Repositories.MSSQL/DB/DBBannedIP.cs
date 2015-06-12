// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-06-09
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MSSQL;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;

namespace cloudscribe.Core.Repositories.MSSQL
{

    internal class DBBannedIP
    {
        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;

        internal DBBannedIP(
            string dbReadConnectionString, 
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

        }

        /// <summary>
        /// Inserts a row in the mp_BannedIPAddresses table. Returns new integer id.
        /// </summary>
        /// <param name="bannedIP"> bannedIP </param>
        /// <param name="bannedUTC"> bannedUTC </param>
        /// <param name="bannedReason"> bannedReason </param>
        /// <returns>int</returns>
        public int Add(
            string bannedIP,
            DateTime bannedUtc,
            string bannedReason)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_BannedIPAddresses_Insert", 
                3);

            sph.DefineSqlParameter("@BannedIP", SqlDbType.NVarChar, 50, ParameterDirection.Input, bannedIP);
            sph.DefineSqlParameter("@BannedUTC", SqlDbType.DateTime, ParameterDirection.Input, bannedUtc);
            sph.DefineSqlParameter("@BannedReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, bannedReason);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the mp_BannedIPAddresses table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="bannedIP"> bannedIP </param>
        /// <param name="bannedUTC"> bannedUTC </param>
        /// <param name="bannedReason"> bannedReason </param>
        /// <returns>bool</returns>
        public bool Update(
            int rowId,
            string bannedIP,
            DateTime bannedUtc,
            string bannedReason)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_BannedIPAddresses_Update", 
                4);

            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@BannedIP", SqlDbType.NVarChar, 50, ParameterDirection.Input, bannedIP);
            sph.DefineSqlParameter("@BannedUTC", SqlDbType.DateTime, ParameterDirection.Input, bannedUtc);
            sph.DefineSqlParameter("@BannedReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, bannedReason);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_BannedIPAddresses table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public bool Delete(int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_BannedIPAddresses_Delete", 
                1);

            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public DbDataReader GetOne(int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_BannedIPAddresses_SelectOne", 
                1);

            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="ipAddress"> ipAddress </param>
        public DbDataReader GeByIpAddress(string ipAddress)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_BannedIPAddresses_SelectByIP", 
                1);

            sph.DefineSqlParameter("@BannedIP", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Returns true if the passed in address is banned
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public bool IsBanned(string ipAddress)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_BannedIPAddresses_Exists", 
                1);

            sph.DefineSqlParameter("@BannedIP", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            int foundRows = Convert.ToInt32(sph.ExecuteScalar());
            return (foundRows > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_BannedIPAddresses table.
        /// </summary>
        public int GetCount()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_BannedIPAddresses_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_BannedIPAddresses table.
        /// </summary>
        public DbDataReader GetAll()
        {

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_BannedIPAddresses_SelectAll",
                null);

        }

        ///// <summary>
        ///// Gets a page of data from the mp_BannedIPAddresses table.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public DbDataReader GetPage(
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    totalPages = 1;
        //    int totalRows
        //        = GetCount();

        //    if (pageSize > 0) totalPages = totalRows / pageSize;

        //    if (totalRows <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else
        //    {
        //        int remainder;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        readConnectionString, 
        //        "mp_BannedIPAddresses_SelectPage", 
        //        2);

        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();

        //}


    }
}
