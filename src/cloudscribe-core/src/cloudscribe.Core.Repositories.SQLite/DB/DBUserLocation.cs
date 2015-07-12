// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-01-04
// Last Modified:			2015-06-14
//

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SQLite;
using Microsoft.Framework.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBUserLocation
    {
        internal DBUserLocation(
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
        /// Inserts a row in the mp_UserLocation table. Returns rows affected count.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="iPAddress"> iPAddress </param>
        /// <param name="iPAddressLong"> iPAddressLong </param>
        /// <param name="hostname"> hostname </param>
        /// <param name="longitude"> longitude </param>
        /// <param name="latitude"> latitude </param>
        /// <param name="iSP"> iSP </param>
        /// <param name="continent"> continent </param>
        /// <param name="country"> country </param>
        /// <param name="region"> region </param>
        /// <param name="city"> city </param>
        /// <param name="timeZone"> timeZone </param>
        /// <param name="captureCount"> captureCount </param>
        /// <param name="firstCaptureUTC"> firstCaptureUTC </param>
        /// <param name="lastCaptureUTC"> lastCaptureUTC </param>
        /// <returns>int</returns>
        public int Create(
            Guid rowID,
            Guid userGuid,
            Guid siteGuid,
            string iPAddress,
            long iPAddressLong,
            string hostname,
            double longitude,
            double latitude,
            string iSP,
            string continent,
            string country,
            string region,
            string city,
            string timeZone,
            int captureCount,
            DateTime firstCaptureUTC,
            DateTime lastCaptureUTC)
        {
            

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserLocation (");
            sqlCommand.Append("RowID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("IPAddress, ");
            sqlCommand.Append("IPAddressLong, ");
            sqlCommand.Append("Hostname, ");
            sqlCommand.Append("Longitude, ");
            sqlCommand.Append("Latitude, ");
            sqlCommand.Append("ISP, ");
            sqlCommand.Append("Continent, ");
            sqlCommand.Append("Country, ");
            sqlCommand.Append("Region, ");
            sqlCommand.Append("City, ");
            sqlCommand.Append("TimeZone, ");
            sqlCommand.Append("CaptureCount, ");
            sqlCommand.Append("FirstCaptureUTC, ");
            sqlCommand.Append("LastCaptureUTC )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowID, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":IPAddress, ");
            sqlCommand.Append(":IPAddressLong, ");
            sqlCommand.Append(":Hostname, ");
            sqlCommand.Append(":Longitude, ");
            sqlCommand.Append(":Latitude, ");
            sqlCommand.Append(":ISP, ");
            sqlCommand.Append(":Continent, ");
            sqlCommand.Append(":Country, ");
            sqlCommand.Append(":Region, ");
            sqlCommand.Append(":City, ");
            sqlCommand.Append(":TimeZone, ");
            sqlCommand.Append(":CaptureCount, ");
            sqlCommand.Append(":FirstCaptureUTC, ");
            sqlCommand.Append(":LastCaptureUTC )");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[17];

            arParams[0] = new SQLiteParameter(":RowID", DbType.String);
            arParams[0].Value = rowID.ToString();

            arParams[1] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new SQLiteParameter(":IPAddress", DbType.String);
            arParams[3].Value = iPAddress;

            arParams[4] = new SQLiteParameter(":IPAddressLong", DbType.Int64);
            arParams[4].Value = iPAddressLong;

            arParams[5] = new SQLiteParameter(":Hostname", DbType.String);
            arParams[5].Value = hostname;

            arParams[6] = new SQLiteParameter(":Longitude", DbType.Double);
            arParams[6].Value = longitude;

            arParams[7] = new SQLiteParameter(":Latitude", DbType.Double);
            arParams[7].Value = latitude;

            arParams[8] = new SQLiteParameter(":ISP", DbType.String);
            arParams[8].Value = iSP;

            arParams[9] = new SQLiteParameter(":Continent", DbType.String);
            arParams[9].Value = continent;

            arParams[10] = new SQLiteParameter(":Country", DbType.String);
            arParams[10].Value = country;

            arParams[11] = new SQLiteParameter(":Region", DbType.String);
            arParams[11].Value = region;

            arParams[12] = new SQLiteParameter(":City", DbType.String);
            arParams[12].Value = city;

            arParams[13] = new SQLiteParameter(":TimeZone", DbType.String);
            arParams[13].Value = timeZone;

            arParams[14] = new SQLiteParameter(":CaptureCount", DbType.Int32);
            arParams[14].Value = captureCount;

            arParams[15] = new SQLiteParameter(":FirstCaptureUTC", DbType.DateTime);
            arParams[15].Value = firstCaptureUTC;

            arParams[16] = new SQLiteParameter(":LastCaptureUTC", DbType.DateTime);
            arParams[16].Value = lastCaptureUTC;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_UserLocation table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="iPAddress"> iPAddress </param>
        /// <param name="iPAddressLong"> iPAddressLong </param>
        /// <param name="hostname"> hostname </param>
        /// <param name="longitude"> longitude </param>
        /// <param name="latitude"> latitude </param>
        /// <param name="iSP"> iSP </param>
        /// <param name="continent"> continent </param>
        /// <param name="country"> country </param>
        /// <param name="region"> region </param>
        /// <param name="city"> city </param>
        /// <param name="timeZone"> timeZone </param>
        /// <param name="captureCount"> captureCount </param>
        /// <param name="lastCaptureUTC"> lastCaptureUTC </param>
        /// <returns>bool</returns>
        public bool Update(
            Guid rowID,
            Guid userGuid,
            Guid siteGuid,
            string iPAddress,
            long iPAddressLong,
            string hostname,
            double longitude,
            double latitude,
            string iSP,
            string continent,
            string country,
            string region,
            string city,
            string timeZone,
            int captureCount,
            DateTime lastCaptureUTC)
        {
            
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_UserLocation ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = :UserGuid, ");
            sqlCommand.Append("SiteGuid = :SiteGuid, ");
            sqlCommand.Append("IPAddress = :IPAddress, ");
            sqlCommand.Append("IPAddressLong = :IPAddressLong, ");
            sqlCommand.Append("Hostname = :Hostname, ");
            sqlCommand.Append("Longitude = :Longitude, ");
            sqlCommand.Append("Latitude = :Latitude, ");
            sqlCommand.Append("ISP = :ISP, ");
            sqlCommand.Append("Continent = :Continent, ");
            sqlCommand.Append("Country = :Country, ");
            sqlCommand.Append("Region = :Region, ");
            sqlCommand.Append("City = :City, ");
            sqlCommand.Append("TimeZone = :TimeZone, ");
            sqlCommand.Append("CaptureCount = :CaptureCount, ");
            sqlCommand.Append("LastCaptureUTC = :LastCaptureUTC ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowID = :RowID ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[16];

            arParams[0] = new SQLiteParameter(":RowID", DbType.String);
            arParams[0].Value = rowID.ToString();

            arParams[1] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new SQLiteParameter(":IPAddress", DbType.String);
            arParams[3].Value = iPAddress;

            arParams[4] = new SQLiteParameter(":IPAddressLong", DbType.Int64);
            arParams[4].Value = iPAddressLong;

            arParams[5] = new SQLiteParameter(":Hostname", DbType.String);
            arParams[5].Value = hostname;

            arParams[6] = new SQLiteParameter(":Longitude", DbType.Double);
            arParams[6].Value = longitude;

            arParams[7] = new SQLiteParameter(":Latitude", DbType.Double);
            arParams[7].Value = latitude;

            arParams[8] = new SQLiteParameter(":ISP", DbType.String);
            arParams[8].Value = iSP;

            arParams[9] = new SQLiteParameter(":Continent", DbType.String);
            arParams[9].Value = continent;

            arParams[10] = new SQLiteParameter(":Country", DbType.String);
            arParams[10].Value = country;

            arParams[11] = new SQLiteParameter(":Region", DbType.String);
            arParams[11].Value = region;

            arParams[12] = new SQLiteParameter(":City", DbType.String);
            arParams[12].Value = city;

            arParams[13] = new SQLiteParameter(":TimeZone", DbType.String);
            arParams[13].Value = timeZone;

            arParams[14] = new SQLiteParameter(":CaptureCount", DbType.Int32);
            arParams[14].Value = captureCount;

            arParams[15] = new SQLiteParameter(":LastCaptureUTC", DbType.DateTime);
            arParams[15].Value = lastCaptureUTC;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_UserLocation table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public bool Delete(Guid rowID)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = :RowID ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":RowID", DbType.String);
            arParams[0].Value = rowID.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public DbDataReader GetOne(Guid rowID)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = :RowID ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":RowID", DbType.String);
            arParams[0].Value = rowID.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="userguid"> userguid </param>
        /// <param name="iPAddress"> iPAddress </param>
        public DbDataReader GetOne(Guid userguid, long iPAddressLong)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append("AND IPAddressLong = :IPAddressLong ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userguid.ToString();

            arParams[1] = new SQLiteParameter(":IPAddressLong", DbType.Int64);
            arParams[1].Value = iPAddressLong;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="userguid"> userguid </param>
        public DbDataReader GetByUser(Guid userguid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append("ORDER BY LastCaptureUTC DESC ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userguid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public DbDataReader GetBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY IPAddressLong ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Users table which have the passed in IP Address
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public DbDataReader GetUsersByIPAddress(Guid siteGuid, string ipv4Address)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  u.* ");
            sqlCommand.Append("FROM	mp_UserLocation ul ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON ul.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("(u.SiteGuid = :SiteGuid OR :SiteGuid = '00000000-0000-0000-0000-000000000000') ");
            sqlCommand.Append("AND ul.IPAddress = :IPAddress ");

            sqlCommand.Append("ORDER BY ul.LastCaptureUTC DESC ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SQLiteParameter(":IPAddress", DbType.String);
            arParams[1].Value = ipv4Address;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        public int GetCountByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));
        }


        /// <summary>
        /// Gets a count of rows in the mp_UserLocation table.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public int GetCountBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPageByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountByUser(userGuid);

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
            sqlCommand.Append("FROM	mp_UserLocation  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("IPAddressLong  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserGuid", DbType.String);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a page of data from the mp_UserLocation table.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountBySite(siteGuid);

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
            sqlCommand.Append("FROM	mp_UserLocation  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("IPAddressLong  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

    }
}
