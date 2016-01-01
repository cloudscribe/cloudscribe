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

    internal class DBGeoZone
    {
        internal DBGeoZone(
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

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;


        /// <summary>
        /// Inserts a row in the mp_GeoZone table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="countryGuid"> countryGuid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <returns>bool</returns>
        public async Task<bool> Create(
            Guid guid,
            Guid countryGuid,
            string name,
            string code,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = name;

            arParams[3] = new NpgsqlParameter("code", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Value = code;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_geozone (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("countryguid, ");
            sqlCommand.Append("name, ");
            sqlCommand.Append("code )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":countryguid, ");
            sqlCommand.Append(":name, ");
            sqlCommand.Append(":code ");
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
        /// Updates a row in the mp_GeoZone table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="countryGuid"> countryGuid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <returns>bool</returns>
        public async Task<bool> Update(
            Guid guid,
            Guid countryGuid,
            string name,
            string code,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = name;

            arParams[3] = new NpgsqlParameter("code", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Value = code;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_geozone ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("countryguid = :countryguid, ");
            sqlCommand.Append("name = :name, ");
            sqlCommand.Append("code = :code ");

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
        /// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
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
            sqlCommand.Append("DELETE FROM mp_geozone ");
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

        public async Task<bool> DeleteByCountry(
            Guid countryGuid,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_geozone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("countryguid = :countryguid ");
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
        /// Gets an IDataReader with one row from the mp_GeoZone table.
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
            sqlCommand.Append("FROM	mp_geozone ");
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
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public async Task<DbDataReader> GetByCode(
            Guid countryGuid, 
            string code,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new NpgsqlParameter("code", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = code;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_geozone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("countryguid = :countryguid ");
            sqlCommand.Append("AND code = :code ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }

        public async Task<DbDataReader> AutoComplete(
            Guid countryGuid, 
            string query, 
            int maxRows,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new NpgsqlParameter("query", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = query + "%";

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_geozone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("countryguid = :countryguid ");

            sqlCommand.Append("AND (  ");
            sqlCommand.Append(" (name LIKE :query) ");
            sqlCommand.Append(" OR (code LIKE :query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY code ");
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
        /// Gets an IDataReader with all rows in the mp_GeoZone table.
        /// </summary>
        public async Task<DbDataReader> GetByCountry(
            Guid countryGuid,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_geozone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("countryguid = :countryguid ");
            sqlCommand.Append("ORDER BY name ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoZone table.
        /// </summary>
        public async Task<int> GetCount(
            Guid countryGuid,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_geozone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("countryguid = :countryguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);


        }

        /// <summary>
        /// Gets a page of data from the mp_GeoZone table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public async Task<DbDataReader> GetPage(
            Guid countryGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("countryguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	gz.*, ");
            sqlCommand.Append("gc.name As countryname ");
            sqlCommand.Append("FROM	mp_geozone gz  ");
            sqlCommand.Append("JOIN mp_geocountry gc ");
            sqlCommand.Append("ON gz.countryguid = gc.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("gz.countryguid = :countryguid ");
            sqlCommand.Append("ORDER BY name ");
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
