// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2016-01-01
// 


using cloudscribe.DbHelpers;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBGeoZone
    {
        internal DBGeoZone(
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
        /// Inserts a row in the mp_GeoZone table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="countryGuid"> countryGuid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <returns>bool</returns>
        public bool Create(
            Guid guid,
            Guid countryGuid,
            string name,
            string code)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GeoZone (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("CountryGuid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Code )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":CountryGuid, ");
            sqlCommand.Append(":Name, ");
            sqlCommand.Append(":Code )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new SqliteParameter(":Name", DbType.String);
            arParams[2].Value = name;

            arParams[3] = new SqliteParameter(":Code", DbType.String);
            arParams[3].Value = code;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

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
        public bool Update(
            Guid guid,
            Guid countryGuid,
            string name,
            string code)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_GeoZone ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CountryGuid = :CountryGuid, ");
            sqlCommand.Append("Name = :Name, ");
            sqlCommand.Append("Code = :Code ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new SqliteParameter(":Name", DbType.String);
            arParams[2].Value = name;

            arParams[3] = new SqliteParameter(":Code", DbType.String);
            arParams[3].Value = code;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
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

        public bool DeleteByCountry(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[0].Value = countryGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
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
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetByCode(Guid countryGuid, string code)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append("AND Code = :Code ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new SqliteParameter(":Code", DbType.String);
            arParams[1].Value = code;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader AutoComplete(Guid countryGuid, string query, int maxRows)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");

            sqlCommand.Append("AND ( ");
            sqlCommand.Append("(Name LIKE :Query) ");
            sqlCommand.Append("OR (Code LIKE :Query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY Code ");
            sqlCommand.Append("LIMIT " + maxRows.ToString());
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new SqliteParameter(":Query", DbType.String);
            arParams[1].Value = query + "%";

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoZone table.
        /// </summary>
        public DbDataReader GetByCountry(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append("ORDER BY Name ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[0].Value = countryGuid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoZone table.
        /// </summary>
        public int GetCount(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[0].Value = countryGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the mp_GeoZone table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public DbDataReader GetPage(
            Guid countryGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	gz.*, ");
            sqlCommand.Append("gc.Name As CountryName ");
            sqlCommand.Append("FROM	mp_GeoZone gz ");
            sqlCommand.Append("JOIN mp_GeoCountry gc ");
            sqlCommand.Append("ON gz.CountryGuid = gc.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("gz.CountryGuid = :CountryGuid ");
            sqlCommand.Append("ORDER BY gz.Name ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":CountryGuid", DbType.String);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }
    }
}
