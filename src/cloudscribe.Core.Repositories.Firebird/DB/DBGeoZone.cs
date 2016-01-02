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
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
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

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

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

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new FbParameter("@Name", FbDbType.VarChar, 255);
            arParams[2].Value = name;

            arParams[3] = new FbParameter("@Code", FbDbType.VarChar, 255);
            arParams[3].Value = code;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GeoZone (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("CountryGuid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Code )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@CountryGuid, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@Code )");
            sqlCommand.Append(";");

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_GeoZone ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CountryGuid = @CountryGuid, ");
            sqlCommand.Append("Name = @Name, ");
            sqlCommand.Append("Code = @Code ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append("OR Guid = UPPER(@Guid) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new FbParameter("@Name", FbDbType.VarChar, 255);
            arParams[2].Value = name;

            arParams[3] = new FbParameter("@Code", FbDbType.VarChar, 255);
            arParams[3].Value = code;

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
        /// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public async Task<bool> Delete(
            Guid guid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
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

        public async Task<bool> DeleteByCountry(
            Guid countryGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = @Guid ");
            sqlCommand.Append("OR CountryGuid = UPPER(@Guid) ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

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
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public async Task<DbDataReader> GetOne(
            Guid guid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetByCode(Guid countryGuid, string code)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("(CountryGuid = @CountryGuid ");
            sqlCommand.Append("OR CountryGuid = UPPER(@CountryGuid)) ");
            sqlCommand.Append("AND Code = @Code ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new FbParameter("@Code", FbDbType.VarChar, 255);
            arParams[1].Value = code;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> AutoComplete(
            Guid countryGuid, 
            string query, 
            int maxRows,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT First " + maxRows.ToString());
            sqlCommand.Append(" * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("(CountryGuid = @CountryGuid ");
            sqlCommand.Append("OR CountryGuid = UPPER(@CountryGuid)) ");
            sqlCommand.Append("AND (");
            sqlCommand.Append("(Name LIKE @Query) ");
            sqlCommand.Append("OR (Code LIKE @Query) ");
            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY Code ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new FbParameter("@Query", FbDbType.VarChar, 255);
            arParams[1].Value = query + "%";

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
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE CountryGuid = @CountryGuid  ");
            sqlCommand.Append("OR CountryGuid = UPPER(@CountryGuid) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
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
        /// Gets an IDataReader with all rows in the mp_GeoZone table.
        /// </summary>
        public async Task<DbDataReader> GetByCountry(
            Guid countryGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE CountryGuid = @CountryGuid  ");
            sqlCommand.Append("OR CountryGuid = UPPER(@CountryGuid) ");
            sqlCommand.Append("ORDER BY Name ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


        /// <summary>
        /// Gets a page of data from the mp_GeoZone table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public async Task<DbDataReader> GetPage(
            Guid countryGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	gz.*, ");
            sqlCommand.Append("gc.Name As CountryName ");
            sqlCommand.Append("FROM	mp_GeoZone gz  ");
            sqlCommand.Append("JOIN mp_GeoCountry gc ");
            sqlCommand.Append("ON gz.CountryGuid = gc.Guid ");
            sqlCommand.Append("OR gc.Guid = UPPER(@CountryGuid) ");

            sqlCommand.Append("WHERE gz.CountryGuid = @CountryGuid  ");
            sqlCommand.Append("OR gz.CountryGuid = UPPER(@CountryGuid) ");
            sqlCommand.Append("ORDER BY gz.Name  ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            arParams[0].Value = countryGuid.ToString();
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }
    }
}
