// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2016-01-02
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SqlCe
{
    internal class DBCurrency
    {
        
        internal DBCurrency(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;

            // possibly will change this later to have SqlCeProviderFactory/DbProviderFactory injected
            AdoHelper = new SqlCeHelper(SqlCeProviderFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private SqlCeHelper AdoHelper;

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
            sqlCommand.Append("INSERT INTO mp_Currency ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("Created ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("@Created ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 50);
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Code", SqlDbType.NChar, 3);
            arParams[2].Value = code;

            arParams[3] = new SqlCeParameter("@SymbolLeft", SqlDbType.NVarChar, 15);
            arParams[3].Value = symbolLeft;

            arParams[4] = new SqlCeParameter("@SymbolRight", SqlDbType.NVarChar, 15);
            arParams[4].Value = symbolRight;

            arParams[5] = new SqlCeParameter("@DecimalPointChar", SqlDbType.NChar, 1);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new SqlCeParameter("@ThousandsPointChar", SqlDbType.NChar, 1);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new SqlCeParameter("@DecimalPlaces", SqlDbType.NChar, 1);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new SqlCeParameter("@Value", SqlDbType.Decimal);
            arParams[8].Value = value;

            arParams[9] = new SqlCeParameter("@LastModified", SqlDbType.DateTime);
            arParams[9].Value = lastModified;

            arParams[10] = new SqlCeParameter("@Created", SqlDbType.DateTime);
            arParams[10].Value = created;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
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
        /// <param name="created"> created </param>
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
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Code = @Code, ");
            sqlCommand.Append("SymbolLeft = @SymbolLeft, ");
            sqlCommand.Append("SymbolRight = @SymbolRight, ");
            sqlCommand.Append("DecimalPointChar = @DecimalPointChar, ");
            sqlCommand.Append("ThousandsPointChar = @ThousandsPointChar, ");
            sqlCommand.Append("DecimalPlaces = @DecimalPlaces, ");
            sqlCommand.Append("Value = @Value, ");
            sqlCommand.Append("LastModified = @LastModified ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 50);
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Code", SqlDbType.NChar, 3);
            arParams[2].Value = code;

            arParams[3] = new SqlCeParameter("@SymbolLeft", SqlDbType.NVarChar, 15);
            arParams[3].Value = symbolLeft;

            arParams[4] = new SqlCeParameter("@SymbolRight", SqlDbType.NVarChar, 15);
            arParams[4].Value = symbolRight;

            arParams[5] = new SqlCeParameter("@DecimalPointChar", SqlDbType.NChar, 1);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new SqlCeParameter("@ThousandsPointChar", SqlDbType.NChar, 1);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new SqlCeParameter("@DecimalPlaces", SqlDbType.NChar, 1);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new SqlCeParameter("@Value", SqlDbType.Decimal);
            arParams[8].Value = value;

            arParams[9] = new SqlCeParameter("@LastModified", SqlDbType.DateTime);
            arParams[9].Value = lastModified;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Title ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }


    }
}
