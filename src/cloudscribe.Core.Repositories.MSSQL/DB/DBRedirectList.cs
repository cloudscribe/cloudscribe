// Author:					Joe Audette
// Created:				    2008-11-19
// Last Modified:			2014-08-29
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.DbHelpers.MSSQL;
using System;
using System.Data;


namespace cloudscribe.Core.Repositories.MSSQL
{
    internal static class DBRedirectList
    {
       
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
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            int siteID,
            string oldUrl,
            string newUrl,
            DateTime createdUtc,
            DateTime expireUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RedirectList_Insert", 7);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@OldUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, oldUrl);
            sph.DefineSqlParameter("@NewUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, newUrl);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@ExpireUtc", SqlDbType.DateTime, ParameterDirection.Input, expireUtc);
            int rowsAffected = sph.ExecuteNonQuery();
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
        public static bool Update(
            Guid rowGuid, 
            string oldUrl,
            string newUrl,
            DateTime expireUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RedirectList_Update", 4);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);  
            sph.DefineSqlParameter("@OldUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, oldUrl);
            sph.DefineSqlParameter("@NewUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, newUrl); 
            sph.DefineSqlParameter("@ExpireUtc", SqlDbType.DateTime, ParameterDirection.Input, expireUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }



        /// <summary>
        /// Deletes a row from the mp_RedirectList table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RedirectList_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne( Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RedirectList_SelectOne", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            return sph.ExecuteReader();

        }


        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetBySiteAndUrl(int siteId, string oldUrl)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RedirectList_SelectBySiteAndUrl", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@OldUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, oldUrl);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// returns true if the record exists
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static bool Exists(int siteId, string oldUrl)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RedirectList_Exists", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@OldUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, oldUrl);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static int GetCount(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RedirectList_GetCount", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the mp_RedirectList table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(siteId);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RedirectList_SelectPage", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }



    }
}
