// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2014-08-29
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.pgsql;
using Npgsql;
using System;
using System.Data;
using System.Text;


namespace cloudscribe.Core.Repositories.pgsql
{
   
    internal static class DBBannedIP
    {
        
        /// <summary>
        /// Inserts a row in the mp_BannedIPAddresses table. Returns new integer id.
        /// </summary>
        /// <param name="bannedIP"> bannedIP </param>
        /// <param name="bannedUTC"> bannedUTC </param>
        /// <param name="bannedReason"> bannedReason </param>
        /// <returns>int</returns>
        public static int Add(
            string bannedIP,
            DateTime bannedUtc,
            string bannedReason)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("bannedip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = bannedIP;

            arParams[1] = new NpgsqlParameter("bannedutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = bannedUtc;

            arParams[2] = new NpgsqlParameter("bannedreason", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = bannedReason;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_insert(:bannedip,:bannedutc,:bannedreason)",
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
        public static bool Update(
            int rowId,
            string bannedIP,
            DateTime bannedUtc,
            string bannedReason)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];
            
            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            arParams[1] = new NpgsqlParameter("bannedip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = bannedIP;

            arParams[2] = new NpgsqlParameter("bannedutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = bannedUtc;

            arParams[3] = new NpgsqlParameter("bannedreason", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = bannedReason;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_update(:rowid,:bannedip,:bannedutc,:bannedreason)",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_BannedIPAddresses table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(int rowId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_delete(:rowid)",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(
            int rowId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_select_one(:rowid)",
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="ipAddress"> ipAddress </param>
        public static IDataReader GeByIpAddress(string ipAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_bannedipaddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bannedip = :bannedip ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("bannedip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = ipAddress;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_BannedIPAddresses table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_select_all()",
                null);

        }

        /// <summary>
        /// Gets a count of rows in the mp_BannedIPAddresses table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_count()",
                null));

        }

        /// <summary>
        /// Returns true if the passed in address is banned
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool IsBanned(string ipAddress)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("bannedip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = ipAddress;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_bannedipaddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bannedip = :bannedip "); 
            sqlCommand.Append(";");

            int foundRows = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (foundRows > 0);

        }

        /// <summary>
        /// Gets a page of data from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount();

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
            
            arParams[0] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_bannedipaddresses_selectpage(:pagenumber,:pagesize)",
                arParams);

        }

    }
}
