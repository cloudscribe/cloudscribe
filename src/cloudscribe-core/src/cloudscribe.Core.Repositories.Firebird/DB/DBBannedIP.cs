// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-06-12
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;

namespace cloudscribe.Core.Repositories.Firebird
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

            #region Bit Conversion

            #endregion

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter(":BannedIP", FbDbType.VarChar, 50);
            arParams[0].Value = bannedIP;

            arParams[1] = new FbParameter(":BannedUTC", FbDbType.TimeStamp);
            arParams[1].Value = bannedUtc;

            arParams[2] = new FbParameter(":BannedReason", FbDbType.VarChar, 255);
            arParams[2].Value = bannedReason;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BANNEDIPADDRESSES_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                arParams));

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
            #region Bit Conversion


            #endregion

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Value = rowId;

            arParams[1] = new FbParameter("@BannedIP", FbDbType.VarChar, 50);
            arParams[1].Value = bannedIP;

            arParams[2] = new FbParameter("@BannedUTC", FbDbType.TimeStamp);
            arParams[2].Value = bannedUtc;

            arParams[3] = new FbParameter("@BannedReason", FbDbType.VarChar, 255);
            arParams[3].Value = bannedReason;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BANNEDIPADDRESSES_UPDATE ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Value = rowId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BANNEDIPADDRESSES_DELETE ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                arParams);

            return (rowsAffected > -1);
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
            sqlCommand.Append("RowID = @RowID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Value = rowId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
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
            sqlCommand.Append("BannedIP = @BannedIP ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@BannedIP", FbDbType.VarChar, 50);
            arParams[0].Value = ipAddress;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

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
            sqlCommand.Append("BannedIP = @BannedIP ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@BannedIP", FbDbType.VarChar, 50);
            arParams[0].Value = ipAddress;

            int foundRows = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
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
                readConnectionString,
                sqlCommand.ToString(),
                null));

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
                readConnectionString,
                sqlCommand.ToString(),
                null);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses  ");
            sqlCommand.Append("ORDER BY  BannedIP ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PageNumber", FbDbType.Integer);
            arParams[0].Value = pageNumber;

            arParams[1] = new FbParameter("@PageSize", FbDbType.Integer);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
