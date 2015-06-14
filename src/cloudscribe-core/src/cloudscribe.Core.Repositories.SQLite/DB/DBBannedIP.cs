// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-06-14
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using Microsoft.Data.SQLite;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
//using System.Data.SQLite;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBBannedIP
    {

        internal DBBannedIP(
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
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_BannedIPAddresses (");
            sqlCommand.Append("BannedIP, ");
            sqlCommand.Append("BannedUTC, ");
            sqlCommand.Append("BannedReason )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":BannedIP, ");
            sqlCommand.Append(":BannedUTC, ");
            sqlCommand.Append(":BannedReason );");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":BannedIP", DbType.String);
            arParams[0].Value = bannedIP;

            arParams[1] = new SQLiteParameter(":BannedUTC", DbType.DateTime);
            arParams[1].Value = bannedUtc;

            arParams[2] = new SQLiteParameter(":BannedReason", DbType.String);
            arParams[2].Value = bannedReason;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

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
            
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_BannedIPAddresses ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("BannedIP = :BannedIP, ");
            sqlCommand.Append("BannedUTC = :BannedUTC, ");
            sqlCommand.Append("BannedReason = :BannedReason ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowID = :RowID ;");

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":RowID", DbType.Int32);
            arParams[0].Value = rowId;

            arParams[1] = new SQLiteParameter(":BannedIP", DbType.String);
            arParams[1].Value = bannedIP;

            arParams[2] = new SQLiteParameter(":BannedUTC", DbType.DateTime);
            arParams[2].Value = bannedUtc;

            arParams[3] = new SQLiteParameter(":BannedReason", DbType.String);
            arParams[3].Value = bannedReason;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_BannedIPAddresses table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public bool Delete(int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BannedIPAddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = :RowID ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":RowID", DbType.Int32);
            arParams[0].Value = rowId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public DbDataReader GetOne(int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = :RowID ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":RowID", DbType.Int32);
            arParams[0].Value = rowId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="ipAddress"> ipAddress </param>
        public DbDataReader GeByIpAddress(string ipAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("BannedIP = :BannedIP ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":BannedIP", DbType.String);
            arParams[0].Value = ipAddress;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_BannedIPAddresses table.
        /// </summary>
        public DbDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ;");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Returns true if the passed in address is banned
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public bool IsBanned(string ipAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("BannedIP = :BannedIP ;");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":BannedIP", DbType.String);
            arParams[0].Value = ipAddress;

            int foundRows = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return (foundRows > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_BannedIPAddresses table.
        /// </summary>
        public int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ;");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a page of data from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPage(
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
            sqlCommand.Append("FROM	mp_BannedIPAddresses  ");
            sqlCommand.Append("ORDER BY  BannedIP ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize  ; ");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":PageNumber", DbType.Int32);
            arParams[0].Value = pageNumber;

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

    }
}
