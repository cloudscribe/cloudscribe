// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2014-08-29
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
   
    internal static class DBCurrency
    {
       
        private static string GetConnectionString()
        {
            return ConnectionString.GetConnectionString();
        }


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
        /// <returns>int</returns>
        public static int Create(
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
            #region Bit Conversion


            #endregion

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
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":Title, ");
            sqlCommand.Append(":Code, ");
            sqlCommand.Append(":SymbolLeft, ");
            sqlCommand.Append(":SymbolRight, ");
            sqlCommand.Append(":DecimalPointChar, ");
            sqlCommand.Append(":ThousandsPointChar, ");
            sqlCommand.Append(":DecimalPlaces, ");
            sqlCommand.Append(":Value, ");
            sqlCommand.Append(":LastModified, ");
            sqlCommand.Append(":Created )");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[11];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SQLiteParameter(":Title", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SQLiteParameter(":Code", DbType.String, 3);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = code;

            arParams[3] = new SQLiteParameter(":SymbolLeft", DbType.String, 15);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = symbolLeft;

            arParams[4] = new SQLiteParameter(":SymbolRight", DbType.String, 15);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = symbolRight;

            arParams[5] = new SQLiteParameter(":DecimalPointChar", DbType.String, 1);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = decimalPointChar;

            arParams[6] = new SQLiteParameter(":ThousandsPointChar", DbType.String, 1);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new SQLiteParameter(":DecimalPlaces", DbType.String, 1);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = decimalPlaces;

            arParams[8] = new SQLiteParameter(":Value", DbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = value;

            arParams[9] = new SQLiteParameter(":LastModified", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            arParams[10] = new SQLiteParameter(":Created", DbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = created;


            int rowsAffected = 0;
            rowsAffected = AdoHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return rowsAffected;

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
        public static bool Update(
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
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_Currency ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = :Title, ");
            sqlCommand.Append("Code = :Code, ");
            sqlCommand.Append("SymbolLeft = :SymbolLeft, ");
            sqlCommand.Append("SymbolRight = :SymbolRight, ");
            sqlCommand.Append("DecimalPointChar = :DecimalPointChar, ");
            sqlCommand.Append("ThousandsPointChar = :ThousandsPointChar, ");
            sqlCommand.Append("DecimalPlaces = :DecimalPlaces, ");
            sqlCommand.Append("Value = :Value, ");
            sqlCommand.Append("LastModified = :LastModified ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[10];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SQLiteParameter(":Title", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SQLiteParameter(":Code", DbType.String, 3);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = code;

            arParams[3] = new SQLiteParameter(":SymbolLeft", DbType.String, 15);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = symbolLeft;

            arParams[4] = new SQLiteParameter(":SymbolRight", DbType.String, 15);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = symbolRight;

            arParams[5] = new SQLiteParameter(":DecimalPointChar", DbType.String, 1);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = decimalPointChar;

            arParams[6] = new SQLiteParameter(":ThousandsPointChar", DbType.String, 1);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new SQLiteParameter(":DecimalPlaces", DbType.String, 1);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = decimalPlaces;

            arParams[8] = new SQLiteParameter(":Value", DbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = value;

            arParams[9] = new SQLiteParameter(":LastModified", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

          
            int rowsAffected = AdoHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Currency table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = AdoHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Currency table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
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
        //        GetConnectionString(),
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
        //    sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
        //    sqlCommand.Append(";");

        //    SQLiteParameter[] arParams = new SQLiteParameter[1];

        //    arParams[0] = new SQLiteParameter(":PageSize", DbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = pageSize;

        //    return AdoHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);
        //}


    }

}
