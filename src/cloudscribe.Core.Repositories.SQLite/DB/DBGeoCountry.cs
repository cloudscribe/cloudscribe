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
    internal class DBGeoCountry
    {
        internal DBGeoCountry(
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
        /// Inserts a row in the mp_GeoCountry table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="iSOCode2"> iSOCode2 </param>
        /// <param name="iSOCode3"> iSOCode3 </param>
        /// <returns>bool</returns>
        public bool Create(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GeoCountry (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("ISOCode2, ");
            sqlCommand.Append("ISOCode3 )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":Name, ");
            sqlCommand.Append(":ISOCode2, ");
            sqlCommand.Append(":ISOCode3 )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":Name", DbType.String);
            arParams[1].Value = name;

            arParams[2] = new SqliteParameter(":ISOCode2", DbType.String);
            arParams[2].Value = iSOCode2;

            arParams[3] = new SqliteParameter(":ISOCode3", DbType.String);
            arParams[3].Value = iSOCode3;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

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
        public bool Update(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_GeoCountry ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Name = :Name, ");
            sqlCommand.Append("ISOCode2 = :ISOCode2, ");
            sqlCommand.Append("ISOCode3 = :ISOCode3 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":Name", DbType.String);
            arParams[1].Value = name;

            arParams[2] = new SqliteParameter(":ISOCode2", DbType.String);
            arParams[2].Value = iSOCode2;

            arParams[3] = new SqliteParameter(":ISOCode3", DbType.String);
            arParams[3].Value = iSOCode3;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoCountry table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoCountry ");
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
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
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
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="countryISOCode2"> countryISOCode2 </param>
        public DbDataReader GetByISOCode2(string countryISOCode2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ISOCode2 = :ISOCode2 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ISOCode2", DbType.String);
            arParams[0].Value = countryISOCode2;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader AutoComplete(string query, int maxRows)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("(Name LIKE :Query) ");
            sqlCommand.Append("OR (ISOCode2 LIKE :Query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY ISOCode2 ");
            sqlCommand.Append("LIMIT " + maxRows.ToString());
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Query", DbType.String);
            arParams[0].Value = query + "%";

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoCountry table.
        /// </summary>
        public DbDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoCountry table.
        /// </summary>
        public int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a page of data from the mp_GeoCountry table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public DbDataReader GetPage(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_GeoCountry  ");
            sqlCommand.Append("ORDER BY [Name]  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[1].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }
    }
}
