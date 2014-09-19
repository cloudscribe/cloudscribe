// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-28
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.SQLite;
using System.Text;


namespace cloudscribe.Core.Repositories.SQLite
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
            sqlCommand.Append(":LoginProvider, ");
            sqlCommand.Append(":ProviderKey, ");
            sqlCommand.Append(":UserId ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":LoginProvider", DbType.String, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = loginProvider;

            arParams[1] = new SQLiteParameter(":ProviderKey", DbType.String, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = providerKey;

            arParams[2] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
			sqlCommand.Append("LoginProvider = :LoginProvider AND ");
			sqlCommand.Append("ProviderKey = :ProviderKey AND ");
			sqlCommand.Append("UserId = :UserId "); 
			sqlCommand.Append(";");
	
			SQLiteParameter[] arParams = new SQLiteParameter[3];
			
			arParams[0] = new SQLiteParameter(":LoginProvider", DbType.String, 128);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = loginProvider;
			
			arParams[1] = new SQLiteParameter(":ProviderKey", DbType.String, 128);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = providerKey;
			
			arParams[2] = new SQLiteParameter(":UserId", DbType.String, 128);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = userId;
			
			
			int rowsAffected = AdoHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
				
			return (rowsAffected > 0);
			
		}

        public static bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = :SiteGuid) ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
			sqlCommand.Append("LoginProvider = :LoginProvider AND ");
			sqlCommand.Append("ProviderKey = :ProviderKey ");
			sqlCommand.Append(";");
	
			SQLiteParameter[] arParams = new SQLiteParameter[2];
			
			arParams[0] = new SQLiteParameter(":LoginProvider", DbType.String, 128);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = loginProvider;
			
			arParams[1] = new SQLiteParameter(":ProviderKey", DbType.String, 128);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = providerKey;
			
			return AdoHelper.ExecuteReader(
				ConnectionString.GetConnectionString(), 
				sqlCommand.ToString(), 
				arParams);
				
		}

        public static IDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
		
		
		
		
	}
}
