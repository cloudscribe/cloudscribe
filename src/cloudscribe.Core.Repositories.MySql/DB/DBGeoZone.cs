// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2015-01-15
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Text;

namespace cloudscribe.Core.Repositories.MySql
{
    internal static class DBGeoZone
    {
        /// <summary>
        /// Inserts a row in the mp_GeoZone table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="countryGuid"> countryGuid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <returns>int</returns>
        public static async Task<bool> Create(
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
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?CountryGuid, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?Code )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[2].Value = name;

            arParams[3] = new MySqlParameter("?Code", MySqlDbType.VarChar, 255);
            arParams[3].Value = code;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
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
        public static async Task<bool> Update(
            Guid guid,
            Guid countryGuid,
            string name,
            string code)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_GeoZone ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CountryGuid = ?CountryGuid, ");
            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("Code = ?Code ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[2].Value = name;

            arParams[3] = new MySqlParameter("?Code", MySqlDbType.VarChar, 255);
            arParams[3].Value = code;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static async Task<bool> Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> DeleteByCountry(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = ?CountryGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = countryGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetByCode(Guid countryGuid, string code)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = ?CountryGuid ");
            sqlCommand.Append("AND Code = ?Code ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new MySqlParameter("?Code", MySqlDbType.VarChar, 255);
            arParams[1].Value = code;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoZone table.
        /// </summary>
        public static async Task<DbDataReader> GetByCountry(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = ?CountryGuid ");
            sqlCommand.Append("ORDER BY Name ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoZone table.
        /// </summary>
        public static async Task<int> GetCount(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = ?CountryGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = countryGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Gets a page of data from the mp_GeoZone table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static async Task<DbDataReader> GetPage(
            Guid countryGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount(countryGuid);

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
            sqlCommand.Append("SELECT	gz.*, ");
            sqlCommand.Append("gc.Name As CountryName ");
            sqlCommand.Append("FROM	mp_GeoZone gz ");
            sqlCommand.Append("JOIN mp_GeoCountry gc ");
            sqlCommand.Append("ON gz.CountryGuid = gc.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("gz.CountryGuid = ?CountryGuid ");
            sqlCommand.Append("ORDER BY gz.Name ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
