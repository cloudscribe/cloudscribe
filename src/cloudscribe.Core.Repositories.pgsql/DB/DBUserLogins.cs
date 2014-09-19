// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-27
// 
// You must not remove this notice, or any other, from this software.

using Npgsql;
using System;
using System.Data;
using System.Text;
using cloudscribe.DbHelpers.pgsql;


namespace cloudscribe.Core.Repositories.pgsql
{
    internal static class DBUserLogins
    {

        public static bool Create(string loginProvider, string providerKey, string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userlogins (");
            sqlCommand.Append("loginprovider ,");
            sqlCommand.Append("providerkey, ");
            sqlCommand.Append("userid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(":loginprovider, ");
            sqlCommand.Append(":providerkey, ");
            sqlCommand.Append(":userid ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("loginprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = loginProvider;

            arParams[1] = new NpgsqlParameter("providerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = providerKey;

            arParams[2] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("DELETE FROM mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("loginprovider = :loginprovider AND ");
            sqlCommand.Append("providerkey = :providerkey AND ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("loginprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = loginProvider;

            arParams[1] = new NpgsqlParameter("providerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = providerKey;

            arParams[2] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlogins ");
            sqlCommand.Append("WHERE ");
           
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userlogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("userid IN (SELECT userguid FROM mp_users WHERE siteguid = :siteguid) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static IDataReader Find(string loginProvider, string providerKey)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("loginprovider = :loginprovider AND ");
            sqlCommand.Append("providerkey = :providerkey ");
           
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("loginprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = loginProvider;

            arParams[1] = new NpgsqlParameter("providerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = providerKey;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userlogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



    }	
		
	
}
