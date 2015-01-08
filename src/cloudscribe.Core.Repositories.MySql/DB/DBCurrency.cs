// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2015-01-08
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{
    internal static class DBCurrency
    {
       
        /// <summary>
        /// Inserts a row in the mp_Currency table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="title"> title </param>
        /// <param name="code"> code </param>
        /// <param name="symbolLeft"> symbolLeft </param>
        /// <param name="symbolRight"> symbolRight </param>
        /// <param name="decimalPointChar"> decimalPointChar </param>
        /// <param name="thousandsPointChar"> thousandsPointChar </param>
        /// <param name="decimalPlaces"> decimalPlaces </param>
        /// <param name="value"> value </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>bool</returns>
        public static async Task<bool> Create(
            Guid guid,
            string title,
            string code,
            string symbolLeft,
            string symbolRight,
            string decimalPointChar,
            string thousandsPointChar,
            string decimalPlaces,
            decimal value,
            DateTime lastModified,
            DateTime created)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Currency (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Code, ");
            sqlCommand.Append("SymbolLeft, ");
            sqlCommand.Append("SymbolRight, ");
            sqlCommand.Append("DecimalPointChar, ");
            sqlCommand.Append("ThousandsPointChar, ");
            sqlCommand.Append("DecimalPlaces, ");
            sqlCommand.Append("Value, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("Created )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Code, ");
            sqlCommand.Append("?SymbolLeft, ");
            sqlCommand.Append("?SymbolRight, ");
            sqlCommand.Append("?DecimalPointChar, ");
            sqlCommand.Append("?ThousandsPointChar, ");
            sqlCommand.Append("?DecimalPlaces, ");
            sqlCommand.Append("?Value, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?Created )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[11];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Code", MySqlDbType.VarChar, 3);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = code;

            arParams[3] = new MySqlParameter("?SymbolLeft", MySqlDbType.VarChar, 15);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = symbolLeft;

            arParams[4] = new MySqlParameter("?SymbolRight", MySqlDbType.VarChar, 15);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = symbolRight;

            arParams[5] = new MySqlParameter("?DecimalPointChar", MySqlDbType.VarChar, 1);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = decimalPointChar;

            arParams[6] = new MySqlParameter("?ThousandsPointChar", MySqlDbType.VarChar, 1);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new MySqlParameter("?DecimalPlaces", MySqlDbType.VarChar, 1);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = decimalPlaces;

            arParams[8] = new MySqlParameter("?Value", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = value;

            arParams[9] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            arParams[10] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = created;

            int rowsAffected = await  AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }


        /// <summary>
        /// Updates a row in the mp_Currency table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="title"> title </param>
        /// <param name="code"> code </param>
        /// <param name="symbolLeft"> symbolLeft </param>
        /// <param name="symbolRight"> symbolRight </param>
        /// <param name="decimalPointChar"> decimalPointChar </param>
        /// <param name="thousandsPointChar"> thousandsPointChar </param>
        /// <param name="decimalPlaces"> decimalPlaces </param>
        /// <param name="value"> value </param>
        /// <param name="lastModified"> lastModified </param>
        /// <returns>bool</returns>
        public static async Task<bool> Update(
            Guid guid,
            string title,
            string code,
            string symbolLeft,
            string symbolRight,
            string decimalPointChar,
            string thousandsPointChar,
            string decimalPlaces,
            decimal value,
            DateTime lastModified)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Currency ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Code = ?Code, ");
            sqlCommand.Append("SymbolLeft = ?SymbolLeft, ");
            sqlCommand.Append("SymbolRight = ?SymbolRight, ");
            sqlCommand.Append("DecimalPointChar = ?DecimalPointChar, ");
            sqlCommand.Append("ThousandsPointChar = ?ThousandsPointChar, ");
            sqlCommand.Append("DecimalPlaces = ?DecimalPlaces, ");
            sqlCommand.Append("Value = ?Value, ");
            sqlCommand.Append("LastModified = ?LastModified ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[10];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Code", MySqlDbType.VarChar, 3);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = code;

            arParams[3] = new MySqlParameter("?SymbolLeft", MySqlDbType.VarChar, 15);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = symbolLeft;

            arParams[4] = new MySqlParameter("?SymbolRight", MySqlDbType.VarChar, 15);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = symbolRight;

            arParams[5] = new MySqlParameter("?DecimalPointChar", MySqlDbType.VarChar, 1);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = decimalPointChar;

            arParams[6] = new MySqlParameter("?ThousandsPointChar", MySqlDbType.VarChar, 1);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new MySqlParameter("?DecimalPlaces", MySqlDbType.VarChar, 1);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = decimalPlaces;

            arParams[8] = new MySqlParameter("?Value", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = value;

            arParams[9] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Currency table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static async Task<bool> Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Currency table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public static async Task<DbDataReader> GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        ///// <summary>
        ///// Gets a count of rows in the mp_Currency table.
        ///// </summary>
        //public static int GetCount()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_Currency ");
        //    sqlCommand.Append(";");

        //    return Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        null));
        //}

        ///// <summary>
        ///// Gets a page of data from the mp_Currency table.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static IDataReader GetPage(
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    int pageLowerBound = (pageSize * pageNumber) - pageSize;
        //    totalPages = 1;
        //    int totalRows = GetCount();

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

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT	* ");
        //    sqlCommand.Append("FROM	mp_Currency  ");
        //    //sqlCommand.Append("ORDER BY  ");
        //    //sqlCommand.Append("  ");
        //    sqlCommand.Append("LIMIT ?Offset, ?PageSize ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?Offset", MySqlDbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = pageLowerBound;

        //    arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = pageSize;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);
        //}

    }
}
