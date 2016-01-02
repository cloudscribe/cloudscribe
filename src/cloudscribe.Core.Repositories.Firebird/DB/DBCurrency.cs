// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2016-01-02
//

using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Core.Repositories.Firebird
{

    internal class DBCurrency
    {
        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

        internal DBCurrency(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
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
        public async Task<bool> Create(
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
            DateTime created,
            CancellationToken cancellationToken)
        {

            FbParameter[] arParams = new FbParameter[11];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@Title", FbDbType.VarChar, 50);
            arParams[1].Value = title;

            arParams[2] = new FbParameter("@Code", FbDbType.Char, 3);
            arParams[2].Value = code;

            arParams[3] = new FbParameter("@SymbolLeft", FbDbType.VarChar, 15);
            arParams[3].Value = symbolLeft;

            arParams[4] = new FbParameter("@SymbolRight", FbDbType.VarChar, 15);
            arParams[4].Value = symbolRight;

            arParams[5] = new FbParameter("@DecimalPointChar", FbDbType.Char, 1);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new FbParameter("@ThousandsPointChar", FbDbType.Char, 1);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new FbParameter("@DecimalPlaces", FbDbType.Char, 1);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new FbParameter("@Value", FbDbType.Decimal);
            arParams[8].Value = value;

            arParams[9] = new FbParameter("@LastModified", FbDbType.TimeStamp);
            arParams[9].Value = lastModified;

            arParams[10] = new FbParameter("@Created", FbDbType.TimeStamp);
            arParams[10].Value = created;

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
            sqlCommand.Append("\"Value\", ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("Created )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Code, ");
            sqlCommand.Append("@SymbolLeft, ");
            sqlCommand.Append("@SymbolRight, ");
            sqlCommand.Append("@DecimalPointChar, ");
            sqlCommand.Append("@ThousandsPointChar, ");
            sqlCommand.Append("@DecimalPlaces, ");
            sqlCommand.Append("@Value, ");
            sqlCommand.Append("@LastModified, ");
            sqlCommand.Append("@Created )");
            sqlCommand.Append(";");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

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
        public async Task<bool> Update(
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
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Currency ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Code = @Code, ");
            sqlCommand.Append("SymbolLeft = @SymbolLeft, ");
            sqlCommand.Append("SymbolRight = @SymbolRight, ");
            sqlCommand.Append("DecimalPointChar = @DecimalPointChar, ");
            sqlCommand.Append("ThousandsPointChar = @ThousandsPointChar, ");
            sqlCommand.Append("DecimalPlaces = @DecimalPlaces, ");
            sqlCommand.Append("\"Value\" = @Value, ");
            sqlCommand.Append("LastModified = @LastModified ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append("OR Guid = UPPER(@Guid) ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[10];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@Title", FbDbType.VarChar, 50);
            arParams[1].Value = title;

            arParams[2] = new FbParameter("@Code", FbDbType.Char, 3);
            arParams[2].Value = code;

            arParams[3] = new FbParameter("@SymbolLeft", FbDbType.VarChar, 15);
            arParams[3].Value = symbolLeft;

            arParams[4] = new FbParameter("@SymbolRight", FbDbType.VarChar, 15);
            arParams[4].Value = symbolRight;

            arParams[5] = new FbParameter("@DecimalPointChar", FbDbType.Char, 1);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new FbParameter("@ThousandsPointChar", FbDbType.Char, 1);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new FbParameter("@DecimalPlaces", FbDbType.Char, 1);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new FbParameter("@Value", FbDbType.Decimal);
            arParams[8].Value = value;

            arParams[9] = new FbParameter("@LastModified", FbDbType.TimeStamp);
            arParams[9].Value = lastModified;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_Currency table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public async Task<bool> Delete(
            Guid guid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append("OR Guid = UPPER(@Guid) ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Currency table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public async Task<DbDataReader> GetOne(
            Guid guid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append("OR Guid = UPPER(@Guid) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

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

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public async Task<DbDataReader> GetAll(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Currency ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }


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
        //    sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
        //    sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
        //    sqlCommand.Append("	* ");
        //    sqlCommand.Append("FROM	mp_Currency  ");
        //    //sqlCommand.Append("ORDER BY  ");
        //    //sqlCommand.Append("  ");
        //    sqlCommand.Append("	; ");


        //    return AdoHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //}
    }
}
