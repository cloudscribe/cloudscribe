// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-01-14
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using cloudscribe.DbHelpers.MSSQL;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{
	
	internal static class DBUserLogins
    {


		/// <summary>
		/// Inserts a row in the mp_UserLogins table. Returns new integer id.
		/// </summary>
		/// <returns>int</returns>
        public static async Task<bool> Create(string loginProvider, string providerKey, string userId) 
		{
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_Insert", 3);
            sph.DefineSqlParameter("@LoginProvider", SqlDbType.NVarChar, 128, ParameterDirection.Input, loginProvider);
			sph.DefineSqlParameter("@ProviderKey", SqlDbType.NVarChar, 128, ParameterDirection.Input, providerKey);
			sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);

            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);
		}


		/// <summary>
		/// Deletes a row from the mp_UserLogins table. Returns true if row deleted.
		/// </summary>
		/// <param name="loginProvider"> loginProvider </param>
		/// <param name="providerKey"> providerKey </param>
		/// <param name="userId"> userId </param>
		/// <returns>bool</returns>
		public static async Task<bool> Delete(string loginProvider, string providerKey, string userId) 
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_Delete", 3);
			sph.DefineSqlParameter("@LoginProvider", SqlDbType.NVarChar, 128, ParameterDirection.Input, loginProvider);
			sph.DefineSqlParameter("@ProviderKey", SqlDbType.NVarChar, 128, ParameterDirection.Input, providerKey);
			sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
			int rowsAffected = await sph.ExecuteNonQueryAsync();
			return (rowsAffected > 0);
			
		}

        public static async Task<bool> DeleteByUser(string userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        public static async Task<bool> DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        public static async Task<DbDataReader> Find(string loginProvider, string providerKey)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserLogins_Find", 2);
            sph.DefineSqlParameter("@LoginProvider", SqlDbType.NVarChar, 128, ParameterDirection.Input, loginProvider);
            sph.DefineSqlParameter("@ProviderKey", SqlDbType.NVarChar, 128, ParameterDirection.Input, providerKey);
            return await sph.ExecuteReaderAsync();

        }
		
		
		public static async Task<DbDataReader> GetByUser(string  userId)  
		{
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserLogins_SelectByUser", 1);
			sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
			return await sph.ExecuteReaderAsync();
			
		}
		
        
	
	}

}
