// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2015-01-04
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MSSQL;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoZone_Insert", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoZone_Update", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static async Task<bool> Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoZone_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        public static async Task<bool> DeleteByCountry(Guid countryGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoZone_DeleteByCountry", 1);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoZone_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return await sph.ExecuteReaderAsync();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetByCode(Guid countryGuid, string code)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoZone_SelectByCode", 2);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);
            return await sph.ExecuteReaderAsync();

        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoZone table.
        /// </summary>
        public static async Task<int> GetCount(Guid countryGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoZone_GetCountByCountry", 1);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result);

            
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoZone table.
        /// </summary>
        public static async Task<DbDataReader> GetByCountry(Guid countryGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoZone_SelectByCountry", 1);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            return await sph.ExecuteReaderAsync();


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
            //totalPages = 1;
            //int totalRows
            //    = GetCount(countryGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoZone_SelectPageByCountry", 3);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }

    }

}
