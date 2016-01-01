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
    internal class DBGeoCountry
    {
        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        internal DBGeoCountry(
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
        /// Inserts a row in the mp_GeoCountry table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="iSOCode2"> iSOCode2 </param>
        /// <param name="iSOCode3"> iSOCode3 </param>
        /// <returns>bool</returns>
        public async Task<bool> Create(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = name;

            arParams[2] = new NpgsqlParameter("isocode2", NpgsqlTypes.NpgsqlDbType.Text, 2);
            arParams[2].Value = iSOCode2;

            arParams[3] = new NpgsqlParameter("isocode3", NpgsqlTypes.NpgsqlDbType.Text, 3);
            arParams[3].Value = iSOCode3;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_geocountry (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("name, ");
            sqlCommand.Append("isocode2, ");
            sqlCommand.Append("isocode3 )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":name, ");
            sqlCommand.Append(":isocode2, ");
            sqlCommand.Append(":isocode3 ");
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
        /// Updates a row in the mp_GeoCountry table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="iSOCode2"> iSOCode2 </param>
        /// <param name="iSOCode3"> iSOCode3 </param>
        /// <returns>bool</returns>
        public async Task<bool> Update(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = name;

            arParams[2] = new NpgsqlParameter("isocode2", NpgsqlTypes.NpgsqlDbType.Text, 2);
            arParams[2].Value = iSOCode2;

            arParams[3] = new NpgsqlParameter("isocode3", NpgsqlTypes.NpgsqlDbType.Text, 3);
            arParams[3].Value = iSOCode3;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_geocountry ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("name = :name, ");
            sqlCommand.Append("isocode2 = :isocode2, ");
            sqlCommand.Append("isocode3 = :isocode3 ");

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
        /// Deletes a row from the mp_GeoCountry table. Returns true if row deleted.
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
            sqlCommand.Append("DELETE FROM mp_geocountry ");
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
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
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
            sqlCommand.Append("FROM	mp_geocountry ");
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
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="countryISOCode2"> countryISOCode2 </param>
        public async Task<DbDataReader> GetByISOCode2(
            string countryISOCode2,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("isocode2", NpgsqlTypes.NpgsqlDbType.Char, 2);
            arParams[0].Value = countryISOCode2;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_geocountry ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("isocode2 = :isocode2 ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

        public async Task<DbDataReader> AutoComplete(
            string query, 
            int maxRows,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("query", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[0].Value = query + "%";

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_geocountry ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append(" (name LIKE :query) ");
            sqlCommand.Append(" OR (isocode2 LIKE :query) ");

            sqlCommand.Append("ORDER BY isocode2 ");
            sqlCommand.Append("LIMIT " + maxRows.ToString());
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoCountry table.
        /// </summary>
        public async Task<DbDataReader> GetAll(CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_geocountry ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoCountry table.
        /// </summary>
        public async Task<int> GetCount(CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_geocountry ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Gets a page of data from the mp_GeoCountry table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public async Task<DbDataReader> GetPage(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_geocountry  ");
            sqlCommand.Append("ORDER BY name  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }
    }
}
