// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2016-01-01
// 

using cloudscribe.DbHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{

    internal class DBCurrency
    {
        internal DBCurrency(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;

            // possibly will change this later to have SqliteFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqliteFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private AdoHelper AdoHelper;

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
        public bool Create(
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

            SqliteParameter[] arParams = new SqliteParameter[11];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":Title", DbType.String);
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Code", DbType.String);
            arParams[2].Value = code;

            arParams[3] = new SqliteParameter(":SymbolLeft", DbType.String);
            arParams[3].Value = symbolLeft;

            arParams[4] = new SqliteParameter(":SymbolRight", DbType.String);
            arParams[4].Value = symbolRight;

            arParams[5] = new SqliteParameter(":DecimalPointChar", DbType.String);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new SqliteParameter(":ThousandsPointChar", DbType.String);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new SqliteParameter(":DecimalPlaces", DbType.String);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new SqliteParameter(":Value", DbType.Decimal);
            arParams[8].Value = value;

            arParams[9] = new SqliteParameter(":LastModified", DbType.DateTime);
            arParams[9].Value = lastModified;

            arParams[10] = new SqliteParameter(":Created", DbType.DateTime);
            arParams[10].Value = created;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
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
        public bool Update(
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

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":Title", DbType.String);
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Code", DbType.String);
            arParams[2].Value = code;

            arParams[3] = new SqliteParameter(":SymbolLeft", DbType.String);
            arParams[3].Value = symbolLeft;

            arParams[4] = new SqliteParameter(":SymbolRight", DbType.String);
            arParams[4].Value = symbolRight;

            arParams[5] = new SqliteParameter(":DecimalPointChar", DbType.String);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new SqliteParameter(":ThousandsPointChar", DbType.String);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new SqliteParameter(":DecimalPlaces", DbType.String);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new SqliteParameter(":Value", DbType.Decimal);
            arParams[8].Value = value;

            arParams[9] = new SqliteParameter(":LastModified", DbType.DateTime);
            arParams[9].Value = lastModified;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Currency table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Currency table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public DbDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
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

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":PageSize", DbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = pageSize;

        //    return AdoHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);
        //}


    }

}
