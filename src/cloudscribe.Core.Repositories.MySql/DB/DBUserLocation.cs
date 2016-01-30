// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-01-04
// Last Modified:			2016-01-30
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{

    internal class DBUserLocation
    {
        internal DBUserLocation(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have MySqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(MySqlClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;


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
        public async Task<bool> Create(
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
            DateTime lastCaptureUTC,
            CancellationToken cancellationToken
            )
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
            sqlCommand.Append("?RowID, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?IPAddress, ");
            sqlCommand.Append("?IPAddressLong, ");
            sqlCommand.Append("?Hostname, ");
            sqlCommand.Append("?Longitude, ");
            sqlCommand.Append("?Latitude, ");
            sqlCommand.Append("?ISP, ");
            sqlCommand.Append("?Continent, ");
            sqlCommand.Append("?Country, ");
            sqlCommand.Append("?Region, ");
            sqlCommand.Append("?City, ");
            sqlCommand.Append("?TimeZone, ");
            sqlCommand.Append("?CaptureCount, ");
            sqlCommand.Append("?FirstCaptureUTC, ");
            sqlCommand.Append("?LastCaptureUTC )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[17];

            arParams[0] = new MySqlParameter("?RowID", MySqlDbType.VarChar, 36);
            arParams[0].Value = rowID.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?IPAddress", MySqlDbType.VarChar, 50);
            arParams[3].Value = iPAddress;

            arParams[4] = new MySqlParameter("?IPAddressLong", MySqlDbType.Int64);
            arParams[4].Value = iPAddressLong;

            arParams[5] = new MySqlParameter("?Hostname", MySqlDbType.VarChar, 255);
            arParams[5].Value = hostname;

            arParams[6] = new MySqlParameter("?Longitude", MySqlDbType.Float);
            arParams[6].Value = longitude;

            arParams[7] = new MySqlParameter("?Latitude", MySqlDbType.Float);
            arParams[7].Value = latitude;

            arParams[8] = new MySqlParameter("?ISP", MySqlDbType.VarChar, 255);
            arParams[8].Value = iSP;

            arParams[9] = new MySqlParameter("?Continent", MySqlDbType.VarChar, 255);
            arParams[9].Value = continent;

            arParams[10] = new MySqlParameter("?Country", MySqlDbType.VarChar, 255);
            arParams[10].Value = country;

            arParams[11] = new MySqlParameter("?Region", MySqlDbType.VarChar, 255);
            arParams[11].Value = region;

            arParams[12] = new MySqlParameter("?City", MySqlDbType.VarChar, 255);
            arParams[12].Value = city;

            arParams[13] = new MySqlParameter("?TimeZone", MySqlDbType.VarChar, 255);
            arParams[13].Value = timeZone;

            arParams[14] = new MySqlParameter("?CaptureCount", MySqlDbType.Int32);
            arParams[14].Value = captureCount;

            arParams[15] = new MySqlParameter("?FirstCaptureUTC", MySqlDbType.DateTime);
            arParams[15].Value = firstCaptureUTC;

            arParams[16] = new MySqlParameter("?LastCaptureUTC", MySqlDbType.DateTime);
            arParams[16].Value = lastCaptureUTC;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return rowsAffected > 0;

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
        public async Task<bool> Update(
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
            DateTime lastCaptureUTC,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_UserLocation ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("SiteGuid = ?SiteGuid, ");
            sqlCommand.Append("IPAddress = ?IPAddress, ");
            sqlCommand.Append("IPAddressLong = ?IPAddressLong, ");
            sqlCommand.Append("Hostname = ?Hostname, ");
            sqlCommand.Append("Longitude = ?Longitude, ");
            sqlCommand.Append("Latitude = ?Latitude, ");
            sqlCommand.Append("ISP = ?ISP, ");
            sqlCommand.Append("Continent = ?Continent, ");
            sqlCommand.Append("Country = ?Country, ");
            sqlCommand.Append("Region = ?Region, ");
            sqlCommand.Append("City = ?City, ");
            sqlCommand.Append("TimeZone = ?TimeZone, ");
            sqlCommand.Append("CaptureCount = ?CaptureCount, ");
            sqlCommand.Append("LastCaptureUTC = ?LastCaptureUTC ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowID = ?RowID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[16];

            arParams[0] = new MySqlParameter("?RowID", MySqlDbType.VarChar, 36);
            arParams[0].Value = rowID.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?IPAddress", MySqlDbType.VarChar, 50);
            arParams[3].Value = iPAddress;

            arParams[4] = new MySqlParameter("?IPAddressLong", MySqlDbType.Int64);
            arParams[4].Value = iPAddressLong;

            arParams[5] = new MySqlParameter("?Hostname", MySqlDbType.VarChar, 255);
            arParams[5].Value = hostname;

            arParams[6] = new MySqlParameter("?Longitude", MySqlDbType.Float);
            arParams[6].Value = longitude;

            arParams[7] = new MySqlParameter("?Latitude", MySqlDbType.Float);
            arParams[7].Value = latitude;

            arParams[8] = new MySqlParameter("?ISP", MySqlDbType.VarChar, 255);
            arParams[8].Value = iSP;

            arParams[9] = new MySqlParameter("?Continent", MySqlDbType.VarChar, 255);
            arParams[9].Value = continent;

            arParams[10] = new MySqlParameter("?Country", MySqlDbType.VarChar, 255);
            arParams[10].Value = country;

            arParams[11] = new MySqlParameter("?Region", MySqlDbType.VarChar, 255);
            arParams[11].Value = region;

            arParams[12] = new MySqlParameter("?City", MySqlDbType.VarChar, 255);
            arParams[12].Value = city;

            arParams[13] = new MySqlParameter("?TimeZone", MySqlDbType.VarChar, 255);
            arParams[13].Value = timeZone;

            arParams[14] = new MySqlParameter("?CaptureCount", MySqlDbType.Int32);
            arParams[14].Value = captureCount;

            arParams[15] = new MySqlParameter("?LastCaptureUTC", MySqlDbType.DateTime);
            arParams[15].Value = lastCaptureUTC;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_UserLocation table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public async Task<bool> Delete(
            Guid rowID,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = ?RowID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowID", MySqlDbType.VarChar, 36);
            arParams[0].Value = rowID.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteByUser(
            Guid userGuid,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
            return (rowsAffected > 0);
        }

        public async Task<bool> DeleteBySite(
            Guid siteGuid,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
            return (rowsAffected > 0);
        }

        ///// <summary>
        ///// Gets an IDataReader with one row from the mp_UserLocation table.
        ///// </summary>
        ///// <param name="rowID"> rowID </param>
        //public DbDataReader GetOne(Guid rowID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserLocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("RowID = ?RowID ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?RowID", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = rowID.ToString();

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="userguid"> userguid </param>
        /// <param name="iPAddress"> iPAddress </param>
        public async Task<DbDataReader> GetOne(
            Guid userGuid, 
            long iPAddressLong,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Userguid = ?Userguid ");
            sqlCommand.Append("AND IPAddressLong = ?IPAddressLong ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?Userguid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?IPAddressLong", MySqlDbType.Int64);
            arParams[1].Value = iPAddressLong;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        ///// <summary>
        ///// Gets an IDataReader with one row from the mp_UserLocation table.
        ///// </summary>
        ///// <param name="userGuid"> userGuid </param>
        //public DbDataReader GetByUser(Guid userGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserLocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("UserGuid = ?UserGuid ");
        //    sqlCommand.Append("ORDER BY LastCaptureUTC DESC ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        ///// <summary>
        ///// Gets an IDataReader with one row from the mp_UserLocation table.
        ///// </summary>
        ///// <param name="siteGuid"> siteGuid </param>
        //public DbDataReader GetBySite(Guid siteGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserLocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteGuid = ?SiteGuid ");
        //    sqlCommand.Append("ORDER BY IPAddressLong ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Users table which have the passed in IP Address
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public async Task<DbDataReader> GetUsersByIPAddress(
            Guid siteGuid, 
            string ipv4Address,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  u.* ");
            sqlCommand.Append("FROM	mp_UserLocation ul ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON ul.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("(u.SiteGuid = ?SiteGuid OR ?SiteGuid = '00000000-0000-0000-0000-000000000000') ");
            sqlCommand.Append("AND ul.IPAddress = ?IPAddress ");

            sqlCommand.Append("ORDER BY ul.LastCaptureUTC DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?IPAddress", MySqlDbType.VarChar, 50);
            arParams[1].Value = ipv4Address;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams, 
                cancellationToken);

        }

        /// <summary>
        /// Gets a count of rows in the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        public async Task<int> GetCountByUser(
            Guid userGuid,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_UserLocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        ///// <summary>
        ///// Gets a count of rows in the mp_UserLocation table.
        ///// </summary>
        ///// <param name="siteGuid"> siteGuid </param>
        //public int GetCountBySite(Guid siteGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_UserLocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteGuid = ?SiteGuid ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    return Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        readConnectionString,
        //        sqlCommand.ToString(),
        //        arParams));
        //}

        /// <summary>
        /// Gets a page of data from the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public async Task<DbDataReader> GetPageByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken
            )
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_UserLocation  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("IPAddressLong  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

        ///// <summary>
        ///// Gets a page of data from the mp_UserLocation table.
        ///// </summary>
        ///// <param name="siteGuid"> siteGuid </param>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public DbDataReader GetPageBySite(
        //    Guid siteGuid,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT	* ");
        //    sqlCommand.Append("FROM	mp_UserLocation  ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("SiteGuid = ?SiteGuid ");
        //    sqlCommand.Append("ORDER BY  ");
        //    sqlCommand.Append("IPAddressLong  ");
        //    sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
        //    arParams[1].Value = pageSize;

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        sqlCommand.ToString(),
        //        arParams);
        //}


    }
}
