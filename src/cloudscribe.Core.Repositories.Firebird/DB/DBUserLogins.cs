// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-01-14
// 
//
// You must not remove this notice, or any other, from this software.

using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.DbHelpers.Firebird;

namespace cloudscribe.Core.Repositories.Firebird
{
	internal static class DBUserLogins
    {
        public static async Task<bool> Create(string loginProvider, string providerKey, string userId) 
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_UserLogins (");
            sqlCommand.Append("LoginProvider ,");
            sqlCommand.Append("ProviderKey, ");
            sqlCommand.Append("UserId ");
            sqlCommand.Append(") ");
			
			sqlCommand.Append("VALUES (");
            sqlCommand.Append("@LoginProvider, ");
            sqlCommand.Append("@ProviderKey, ");
            sqlCommand.Append("@UserId ");
            sqlCommand.Append(")");

			sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];
			
			arParams[0] = new FbParameter("@LoginProvider", FbDbType.VarChar, 128);
			arParams[0].Value = loginProvider;
			
			arParams[1] = new FbParameter("@ProviderKey", FbDbType.VarChar, 128);
			arParams[1].Value = providerKey;
			
			arParams[2] = new FbParameter("@UserId", FbDbType.VarChar, 128);
			arParams[2].Value = userId;
			
			int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
				ConnectionString.GetWriteConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
			
			return (rowsAffected > 0);
		}
	
	
		public static async Task<bool> Delete(
			string loginProvider, 
			string providerKey, 
			string userId) 
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_UserLogins ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("LoginProvider = @LoginProvider AND ");
			sqlCommand.Append("ProviderKey = @ProviderKey AND ");
			sqlCommand.Append("UserId = @UserId "); 
			sqlCommand.Append(";");
			FbParameter[] arParams = new FbParameter[3];
			
			arParams[0] = new FbParameter("@LoginProvider", FbDbType.VarChar, 128);
			arParams[0].Value = loginProvider;
			
			arParams[1] = new FbParameter("@ProviderKey", FbDbType.VarChar, 128);
			arParams[1].Value = providerKey;
			
			arParams[2] = new FbParameter("@UserId", FbDbType.VarChar, 128);
			arParams[2].Value = userId;
			
			int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
				ConnectionString.GetWriteConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
				
			return (rowsAffected > -1);
		}

        public static async Task<bool> DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static async Task<bool> DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = @SiteGuid) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static async Task<DbDataReader> Find(string loginProvider, string providerKey)
        {
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_UserLogins ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("LoginProvider = @LoginProvider AND ");
			sqlCommand.Append("ProviderKey = @ProviderKey ");
			
			sqlCommand.Append(";");
	
			FbParameter[] arParams = new FbParameter[2];
			
			arParams[0] = new FbParameter("@LoginProvider", FbDbType.VarChar, 128);
			arParams[0].Value = loginProvider;
			
			arParams[1] = new FbParameter("@ProviderKey", FbDbType.VarChar, 128);
			arParams[1].Value = providerKey;
			
			return await AdoHelper.ExecuteReaderAsync(
				ConnectionString.GetReadConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
				
		}

        public static async Task<DbDataReader> GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
           
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
		
	}
}
