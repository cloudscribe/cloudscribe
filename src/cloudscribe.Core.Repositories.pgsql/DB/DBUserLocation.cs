// Author:					Joe Audette
// Created:				    2008-01-04
// Last Modified:			2015-01-15
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using cloudscribe.DbHelpers.pgsql;

namespace cloudscribe.Core.Repositories.pgsql
{
    internal static class DBUserLocation
    {
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
        public static int Create(
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

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_insert(:rowid,:userguid,:siteguid,:ipaddress,:ipaddresslong,:hostname,:longitude,:latitude,:isp,:continent,:country,:region,:city,:timezone,:capturecount,:firstcaptureutc,:lastcaptureutc)",
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
        public static bool Update(
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

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_update(:rowid,:userguid,:siteguid,:ipaddress,:ipaddresslong,:hostname,:longitude,:latitude,:isp,:continent,:country,:region,:city,:timezone,:capturecount,:lastcaptureutc)",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_UserLocation table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowID)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowID.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_delete(:rowid)",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlocation ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(Guid rowID)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("rowid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowID.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_select_one(:rowid)",
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="userguid"> userguid </param>
        /// <param name="iPAddress"> iPAddress </param>
        public static IDataReader GetOne(Guid userGuid, long iPAddressLong)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("ipaddresslong", NpgsqlTypes.NpgsqlDbType.Bigint);
            arParams[1].Value = iPAddressLong;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_select_onebyuserandip(:userguid,:ipaddresslong)",
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        public static IDataReader GetByUser(Guid userGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_select_byuser(:userguid)",
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_UserLocation table.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static IDataReader GetBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_select_bysite(:siteguid)",
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Users table which have the passed in IP Address
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static async Task<DbDataReader> GetUsersByIPAddress(Guid siteGuid, string ipv4Address)
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
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        public static int GetCountByUser(Guid userGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_countbyuser(:userguid)",
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the mp_UserLocation table.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static int GetCountBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_countbysite(:siteguid)",
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the mp_UserLocation table.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountByUser(userGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_selectpagebyuser(:userguid,:pagenumber,:pagesize)",
                arParams);

        }


        /// <summary>
        /// Gets a page of data from the mp_UserLocation table.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountBySite(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_userlocation_selectpagebyite(:siteguid,:pagenumber,:pagesize)",
                arParams);

        }


    }
}
