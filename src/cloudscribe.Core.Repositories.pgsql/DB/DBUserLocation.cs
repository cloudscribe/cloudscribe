// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-01-04
// Last Modified:			2016-01-30
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

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
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
        /// <returns>bool</returns>
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[17];

            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowID.ToString();

            arParams[1] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new NpgsqlParameter("ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[3].Value = iPAddress;

            arParams[4] = new NpgsqlParameter("ipaddresslong", NpgsqlTypes.NpgsqlDbType.Bigint);
            arParams[4].Value = iPAddressLong;

            arParams[5] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Value = hostname;

            arParams[6] = new NpgsqlParameter("longitude", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[6].Value = longitude;

            arParams[7] = new NpgsqlParameter("latitude", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[7].Value = latitude;

            arParams[8] = new NpgsqlParameter("isp", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Value = iSP;

            arParams[9] = new NpgsqlParameter("continent", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Value = continent;

            arParams[10] = new NpgsqlParameter("country", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Value = country;

            arParams[11] = new NpgsqlParameter("region", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = region;

            arParams[12] = new NpgsqlParameter("city", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = city;

            arParams[13] = new NpgsqlParameter("timezone", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Value = timeZone;

            arParams[14] = new NpgsqlParameter("capturecount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[14].Value = captureCount;

            arParams[15] = new NpgsqlParameter("firstcaptureutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[15].Value = firstCaptureUTC;

            arParams[16] = new NpgsqlParameter("lastcaptureutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[16].Value = lastCaptureUTC;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userlocation (");
            sqlCommand.Append("rowid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("ipaddress, ");
            sqlCommand.Append("ipaddresslong, ");
            sqlCommand.Append("hostname, ");
            sqlCommand.Append("longitude, ");
            sqlCommand.Append("latitude, ");
            sqlCommand.Append("isp, ");
            sqlCommand.Append("continent, ");
            sqlCommand.Append("country, ");
            sqlCommand.Append("region, ");
            sqlCommand.Append("city, ");
            sqlCommand.Append("timezone, ");
            sqlCommand.Append("capturecount, ");
            sqlCommand.Append("firstcaptureutc, ");
            sqlCommand.Append("lastcaptureutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":ipaddress, ");
            sqlCommand.Append(":ipaddresslong, ");
            sqlCommand.Append(":hostname, ");
            sqlCommand.Append(":longitude, ");
            sqlCommand.Append(":latitude, ");
            sqlCommand.Append(":isp, ");
            sqlCommand.Append(":continent, ");
            sqlCommand.Append(":country, ");
            sqlCommand.Append(":region, ");
            sqlCommand.Append(":city, ");
            sqlCommand.Append(":timezone, ");
            sqlCommand.Append(":capturecount, ");
            sqlCommand.Append(":firstcaptureutc, ");
            sqlCommand.Append(":lastcaptureutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            //int rowsAffected = AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userlocation_insert(:rowid,:userguid,:siteguid,:ipaddress,:ipaddresslong,:hostname,:longitude,:latitude,:isp,:continent,:country,:region,:city,:timezone,:capturecount,:firstcaptureutc,:lastcaptureutc)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[16];

            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowID.ToString();

            arParams[1] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new NpgsqlParameter("ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[3].Value = iPAddress;

            arParams[4] = new NpgsqlParameter("ipaddresslong", NpgsqlTypes.NpgsqlDbType.Bigint);
            arParams[4].Value = iPAddressLong;

            arParams[5] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Value = hostname;

            arParams[6] = new NpgsqlParameter("longitude", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[6].Value = longitude;

            arParams[7] = new NpgsqlParameter("latitude", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[7].Value = latitude;

            arParams[8] = new NpgsqlParameter("isp", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Value = iSP;

            arParams[9] = new NpgsqlParameter("continent", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Value = continent;

            arParams[10] = new NpgsqlParameter("country", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Value = country;

            arParams[11] = new NpgsqlParameter("region", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = region;

            arParams[12] = new NpgsqlParameter("city", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = city;

            arParams[13] = new NpgsqlParameter("timezone", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Value = timeZone;

            arParams[14] = new NpgsqlParameter("capturecount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[14].Value = captureCount;

            arParams[15] = new NpgsqlParameter("lastcaptureutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[15].Value = lastCaptureUTC;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_userlocation ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("userguid = :userguid, ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("ipaddress = :ipaddress, ");
            sqlCommand.Append("ipaddresslong = :ipaddresslong, ");
            sqlCommand.Append("hostname = :hostname, ");
            sqlCommand.Append("longitude = :longitude, ");
            sqlCommand.Append("latitude = :latitude, ");
            sqlCommand.Append("isp = :isp, ");
            sqlCommand.Append("continent = :continent, ");
            sqlCommand.Append("country = :country, ");
            sqlCommand.Append("region = :region, ");
            sqlCommand.Append("city = :city, ");
            sqlCommand.Append("timezone = :timezone, ");
            sqlCommand.Append("capturecount = :capturecount, ");
            sqlCommand.Append("lastcaptureutc = :lastcaptureutc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("rowid = :rowid ");
            sqlCommand.Append(";");

            //int rowsAffected = AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userlocation_update(:rowid,:userguid,:siteguid,:ipaddress,:ipaddresslong,:hostname,:longitude,:latitude,:isp,:continent,:country,:region,:city,:timezone,:capturecount,:lastcaptureutc)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowID.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowid = :rowid ");
            sqlCommand.Append(";");

            //int rowsAffected = AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userlocation_delete(:rowid)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteByUser(
            Guid userGuid,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteBySite(
            Guid siteGuid,
            CancellationToken cancellationToken
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        ///// <summary>
        ///// Gets an IDataReader with one row from the mp_UserLocation table.
        ///// </summary>
        ///// <param name="rowID"> rowID </param>
        //public DbDataReader GetOne(Guid rowID)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Char, 36);
        //    arParams[0].Value = rowID.ToString();

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_userlocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("rowid = :rowid ");
        //    sqlCommand.Append(";");

        //    //return AdoHelper.ExecuteReader(
        //    //    readConnectionString,
        //    //    CommandType.StoredProcedure,
        //    //    "mp_userlocation_select_one(:rowid)",
        //    //    arParams);

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("ipaddresslong", NpgsqlTypes.NpgsqlDbType.Bigint);
            arParams[1].Value = iPAddressLong;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userlocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ipaddresslong = :ipaddresslong ");
            sqlCommand.Append(";");

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userlocation_select_onebyuserandip(:userguid,:ipaddresslong)",
            //    arParams);

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
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
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_userlocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("userguid = :userguid ");
        //    sqlCommand.Append("ORDER BY lastcaptureutc DESC ");
        //    sqlCommand.Append(";");

        //    //return AdoHelper.ExecuteReader(
        //    //    readConnectionString,
        //    //    CommandType.StoredProcedure,
        //    //    "mp_userlocation_select_byuser(:userguid)",
        //    //    arParams);

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        ///// <summary>
        ///// Gets an IDataReader with one row from the mp_UserLocation table.
        ///// </summary>
        ///// <param name="siteGuid"> siteGuid </param>
        //public DbDataReader GetBySite(Guid siteGuid)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_userlocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("siteguid = :siteguid ");
        //    sqlCommand.Append(";");

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
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
            sqlCommand.Append("FROM	mp_userlocation ul ");

            sqlCommand.Append("JOIN	mp_users u ");
            sqlCommand.Append("ON ul.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("(u.siteguid = :siteguid OR :siteguid = '00000000-0000-0000-0000-000000000000') ");
            sqlCommand.Append("AND ul.ipaddress = :ipaddress ");

            sqlCommand.Append("ORDER BY ul.lastcaptureutc DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = ipv4Address;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_userlocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            //return Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userlocation_countbyuser(:userguid)",
            //    arParams));

            object obj = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(obj);

        }

        ///// <summary>
        ///// Gets a count of rows in the mp_UserLocation table.
        ///// </summary>
        ///// <param name="siteGuid"> siteGuid </param>
        //public int GetCountBySite(Guid siteGuid)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_userlocation ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("siteguid = :siteguid ");
        //    sqlCommand.Append(";");

        //    object obj = AdoHelper.ExecuteScalar(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return Convert.ToInt32(obj);

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
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_userlocation  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("ipaddresslong  ");
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

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userlocation_selectpagebyuser(:userguid,:pagenumber,:pagesize)",
            //    arParams);

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
        //    //totalPages = 1;
        //    //int totalRows
        //    //    = GetCountBySite(siteGuid);

        //    //if (pageSize > 0) totalPages = totalRows / pageSize;

        //    //if (totalRows <= pageSize)
        //    //{
        //    //    totalPages = 1;
        //    //}
        //    //else
        //    //{
        //    //    int remainder;
        //    //    Math.DivRem(totalRows, pageSize, out remainder);
        //    //    if (remainder > 0)
        //    //    {
        //    //        totalPages += 1;
        //    //    }
        //    //}

        //    NpgsqlParameter[] arParams = new NpgsqlParameter[3];

        //    arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[1].Value = pageNumber;

        //    arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[2].Value = pageSize;

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.StoredProcedure,
        //        "mp_userlocation_selectpagebyite(:siteguid,:pagenumber,:pagesize)",
        //        arParams);

        //}


    }
}
