// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2016-01-01
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.pgsql
{

    internal class DBCurrency
    {
        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        internal DBCurrency(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
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
        /// <returns>bool</returns>
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[11];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter("code", NpgsqlTypes.NpgsqlDbType.Text, 3);
            arParams[2].Value = code;

            arParams[3] = new NpgsqlParameter("symbolleft", NpgsqlTypes.NpgsqlDbType.Varchar, 15);
            arParams[3].Value = symbolLeft;

            arParams[4] = new NpgsqlParameter("symbolright", NpgsqlTypes.NpgsqlDbType.Varchar, 15);
            arParams[4].Value = symbolRight;

            arParams[5] = new NpgsqlParameter("decimalpointchar", NpgsqlTypes.NpgsqlDbType.Text, 1);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new NpgsqlParameter("thousandspointchar", NpgsqlTypes.NpgsqlDbType.Text, 1);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new NpgsqlParameter("decimalplaces", NpgsqlTypes.NpgsqlDbType.Text, 1);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new NpgsqlParameter("value", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[8].Value = value;

            arParams[9] = new NpgsqlParameter("lastmodified", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Value = lastModified;

            arParams[10] = new NpgsqlParameter("created", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[10].Value = created;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_currency (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("code, ");
            sqlCommand.Append("symbolleft, ");
            sqlCommand.Append("symbolright, ");
            sqlCommand.Append("decimalpointchar, ");
            sqlCommand.Append("thousandspointchar, ");
            sqlCommand.Append("decimalplaces, ");
            sqlCommand.Append("value, ");
            sqlCommand.Append("lastmodified, ");
            sqlCommand.Append("created )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":code, ");
            sqlCommand.Append(":symbolleft, ");
            sqlCommand.Append(":symbolright, ");
            sqlCommand.Append(":decimalpointchar, ");
            sqlCommand.Append(":thousandspointchar, ");
            sqlCommand.Append(":decimalplaces, ");
            sqlCommand.Append(":value, ");
            sqlCommand.Append(":lastmodified, ");
            sqlCommand.Append(":created ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
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
        /// <param name="created"> created </param>
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[10];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter("code", NpgsqlTypes.NpgsqlDbType.Text, 3);
            arParams[2].Value = code;

            arParams[3] = new NpgsqlParameter("symbolleft", NpgsqlTypes.NpgsqlDbType.Varchar, 15);
            arParams[3].Value = symbolLeft;

            arParams[4] = new NpgsqlParameter("symbolright", NpgsqlTypes.NpgsqlDbType.Varchar, 15);
            arParams[4].Value = symbolRight;

            arParams[5] = new NpgsqlParameter("decimalpointchar", NpgsqlTypes.NpgsqlDbType.Text, 1);
            arParams[5].Value = decimalPointChar;

            arParams[6] = new NpgsqlParameter("thousandspointchar", NpgsqlTypes.NpgsqlDbType.Text, 1);
            arParams[6].Value = thousandsPointChar;

            arParams[7] = new NpgsqlParameter("decimalplaces", NpgsqlTypes.NpgsqlDbType.Text, 1);
            arParams[7].Value = decimalPlaces;

            arParams[8] = new NpgsqlParameter("value", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[8].Value = value;

            arParams[9] = new NpgsqlParameter("lastmodified", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Value = lastModified;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_currency ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("title = :title, ");
            sqlCommand.Append("code = :code, ");
            sqlCommand.Append("symbolleft = :symbolleft, ");
            sqlCommand.Append("symbolright = :symbolright, ");
            sqlCommand.Append("decimalpointchar = :decimalpointchar, ");
            sqlCommand.Append("thousandspointchar = :thousandspointchar, ");
            sqlCommand.Append("decimalplaces = :decimalplaces, ");
            sqlCommand.Append("value = :value, ");
            sqlCommand.Append("lastmodified = :lastmodified ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Currency table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public async Task<DbDataReader> GetOne(
            Guid guid,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_currency ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public async Task<DbDataReader> GetAll(CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_currency ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);


        }

    }
}
