// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-11-19
// Last Modified:			2015-06-14
// 

using cloudscribe.DbHelpers.Sqlite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Framework.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBRedirectList
    {
        internal DBRedirectList(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;

        /// <summary>
        /// Inserts a row in the mp_RedirectList table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="oldUrl"> oldUrl </param>
        /// <param name="newUrl"> newUrl </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="expireUtc"> expireUtc </param>
        /// <returns>int</returns>
        public int Create(
            Guid rowGuid,
            Guid siteGuid,
            int siteID,
            string oldUrl,
            string newUrl,
            DateTime createdUtc,
            DateTime expireUtc)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_RedirectList (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("OldUrl, ");
            sqlCommand.Append("NewUrl, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("ExpireUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":SiteID, ");
            sqlCommand.Append(":OldUrl, ");
            sqlCommand.Append(":NewUrl, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":ExpireUtc )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String);
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[2].Value = siteID;

            arParams[3] = new SqliteParameter(":OldUrl", DbType.String);
            arParams[3].Value = oldUrl;

            arParams[4] = new SqliteParameter(":NewUrl", DbType.String);
            arParams[4].Value = newUrl;

            arParams[5] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[5].Value = createdUtc;

            arParams[6] = new SqliteParameter(":ExpireUtc", DbType.DateTime);
            arParams[6].Value = expireUtc;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_RedirectList table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="oldUrl"> oldUrl </param>
        /// <param name="newUrl"> newUrl </param>
        /// <param name="expireUtc"> expireUtc </param>
        /// <returns>bool</returns>
        public bool Update(
            Guid rowGuid,
            string oldUrl,
            string newUrl,
            DateTime expireUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_RedirectList ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("OldUrl = :OldUrl, ");
            sqlCommand.Append("NewUrl = :NewUrl, ");
            sqlCommand.Append("ExpireUtc = :ExpireUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String);
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":OldUrl", DbType.String);
            arParams[1].Value = oldUrl;

            arParams[2] = new SqliteParameter(":NewUrl", DbType.String);
            arParams[2].Value = newUrl;

            arParams[3] = new SqliteParameter(":ExpireUtc", DbType.DateTime);
            arParams[3].Value = expireUtc;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_RedirectList table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RedirectList ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String);
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public DbDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_RedirectList ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String);
            arParams[0].Value = rowGuid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public DbDataReader GetBySiteAndUrl(int siteId, string oldUrl)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_RedirectList ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID ");
            sqlCommand.Append("AND OldUrl = :OldUrl ");
            sqlCommand.Append("AND ExpireUtc < :CurrentTime ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":OldUrl", DbType.String);
            arParams[1].Value = oldUrl;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Value = DateTime.UtcNow;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// returns true if the record exists
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public bool Exists(int siteId, string oldUrl)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_RedirectList ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID ");
            sqlCommand.Append("AND OldUrl = :OldUrl ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":OldUrl", DbType.String);
            arParams[1].Value = oldUrl;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }


        /// <summary>
        /// Gets a count of rows in the mp_RedirectList table.
        /// </summary>
        public int GetCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_RedirectList ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }


        /// <summary>
        /// Gets a page of data from the mp_RedirectList table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount(siteId);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_RedirectList  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID ");
            sqlCommand.Append("ORDER BY OldUrl ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

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
