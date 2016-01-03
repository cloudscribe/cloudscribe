// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:				Joe Audette
// Created:			    2007-06-22
// Last Modified:		2016-01-03
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
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

            // possibly will change this later to have SqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqlClientFactory.Instance);
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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Currency_Insert", 
                11);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 50, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Code", SqlDbType.NChar, 3, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@SymbolLeft", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolLeft);
            sph.DefineSqlParameter("@SymbolRight", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolRight);
            sph.DefineSqlParameter("@DecimalPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPointChar);
            sph.DefineSqlParameter("@ThousandsPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, thousandsPointChar);
            sph.DefineSqlParameter("@DecimalPlaces", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPlaces);
            sph.DefineSqlParameter("@Value", SqlDbType.Decimal, ParameterDirection.Input, value);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Currency_Update", 
                10);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 50, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Code", SqlDbType.NChar, 3, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@SymbolLeft", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolLeft);
            sph.DefineSqlParameter("@SymbolRight", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolRight);
            sph.DefineSqlParameter("@DecimalPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPointChar);
            sph.DefineSqlParameter("@ThousandsPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, thousandsPointChar);
            sph.DefineSqlParameter("@DecimalPlaces", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPlaces);
            sph.DefineSqlParameter("@Value", SqlDbType.Decimal, ParameterDirection.Input, value);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Currency_Delete", 
                1);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Currency_SelectOne", 
                1);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public async Task<DbDataReader> GetAll(CancellationToken cancellationToken)
        {
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_Currency_SelectAll",
                null,
                cancellationToken);

        }


    }
}
