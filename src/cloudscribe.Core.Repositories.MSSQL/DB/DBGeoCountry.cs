// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2015-01-08
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MSSQL;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{
   
    internal static class DBGeoCountry
    {
        
        /// <summary>
        /// Inserts a row in the mp_GeoCountry table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="iSOCode2"> iSOCode2 </param>
        /// <param name="iSOCode3"> iSOCode3 </param>
        /// <returns>int</returns>
        public static async Task<bool> Create(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoCountry_Insert", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@ISOCode2", SqlDbType.NChar, 2, ParameterDirection.Input, iSOCode2);
            sph.DefineSqlParameter("@ISOCode3", SqlDbType.NChar, 3, ParameterDirection.Input, iSOCode3);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
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
        public static async Task<bool> Update(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoCountry_Update", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@ISOCode2", SqlDbType.NChar, 2, ParameterDirection.Input, iSOCode2);
            sph.DefineSqlParameter("@ISOCode3", SqlDbType.NChar, 3, ParameterDirection.Input, iSOCode3);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoCountry table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static async Task<bool> Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GeoCountry_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoCountry_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return await sph.ExecuteReaderAsync();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="countryISOCode2"> countryISOCode2 </param>
        public static async Task<DbDataReader> GetByISOCode2(string countryISOCode2)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoCountry_SelectByISOCode2", 1);
            sph.DefineSqlParameter("@ISOCode2", SqlDbType.NChar, 2, ParameterDirection.Input, countryISOCode2);
            return await sph.ExecuteReaderAsync();

        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoCountry table.
        /// </summary>
        public static async Task<int> GetCount()
        {
            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_GeoCountry_GetCount",
                null);

            return Convert.ToInt32(result);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoCountry table.
        /// </summary>
        public static async Task<DbDataReader> GetAll()
        {

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_GeoCountry_SelectAll",
                null);

        }

        /// <summary>
        /// Gets a page of data from the mp_GeoCountry table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static async Task<DbDataReader> GetPage(
            int pageNumber,
            int pageSize)
        {
            //totalPages = 1;
            //int totalRows
            //    = GetCount();

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GeoCountry_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }

    }

}
