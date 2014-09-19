// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-27
// 
//
// You must not remove this notice, or any other, from this software.

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using cloudscribe.DbHelpers.MySql;

namespace cloudscribe.Core.Repositories.MySql
{
	
	internal static class DBUserLogins
    {
	
		
        public static bool Create(string loginProvider, string providerKey, string userId) 
		{
		
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_UserLogins (");
            sqlCommand.Append("LoginProvider ,");
            sqlCommand.Append("ProviderKey, ");
            sqlCommand.Append("UserId ");
            sqlCommand.Append(") ");
			
			sqlCommand.Append("VALUES (");
            sqlCommand.Append("?LoginProvider, ");
            sqlCommand.Append("?ProviderKey, ");
            sqlCommand.Append("?UserId ");
            sqlCommand.Append(")");

			sqlCommand.Append(";");
			
			MySqlParameter[] arParams = new MySqlParameter[3];
			
			arParams[0] = new MySqlParameter("?LoginProvider", MySqlDbType.VarChar, 128);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = loginProvider;
			
			arParams[1] = new MySqlParameter("?ProviderKey", MySqlDbType.VarChar, 128);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = providerKey;
			
			arParams[2] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = userId;
			
			int rowsAffected = AdoHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
				
			return (rowsAffected > -1);
			
		}
	
	
		
		public static bool Delete(
			string loginProvider, 
			string providerKey, 
			string userId) 
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_UserLogins ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("LoginProvider = ?LoginProvider AND ");
			sqlCommand.Append("ProviderKey = ?ProviderKey AND ");
			sqlCommand.Append("UserId = ?UserId "); 
			sqlCommand.Append(";");
	
			MySqlParameter[] arParams = new MySqlParameter[3];
			
			arParams[0] = new MySqlParameter("?LoginProvider", MySqlDbType.VarChar, 128);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = loginProvider;
			
			arParams[1] = new MySqlParameter("?ProviderKey", MySqlDbType.VarChar, 128);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = providerKey;
			
			arParams[2] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = userId;
			
			int rowsAffected = AdoHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(), 
				sqlCommand.ToString(), 
				arParams);	
			return (rowsAffected > 0);
			
		}

        public static bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            
            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = ?SiteGuid) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);


        }
		
		public static IDataReader Find(string loginProvider, string providerKey)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_UserLogins ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("LoginProvider = ?LoginProvider AND ");
			sqlCommand.Append("ProviderKey = ?ProviderKey ");
			
			sqlCommand.Append(";");
			
			MySqlParameter[] arParams = new MySqlParameter[2];
			
			arParams[0] = new MySqlParameter("?LoginProvider", MySqlDbType.VarChar, 128);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = loginProvider;
				
			arParams[1] = new MySqlParameter("?ProviderKey", MySqlDbType.VarChar, 128);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = providerKey;
				
			return AdoHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
				
		}

        public static IDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = ?UserId  ");
           
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
		
		
		
		
		
	}
}
