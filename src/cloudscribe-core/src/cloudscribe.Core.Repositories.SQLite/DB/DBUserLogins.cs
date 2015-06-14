// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-06-14
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using Microsoft.Data.SQLite;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBUserLogins
    {
        internal DBUserLogins(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;



        public bool Create(string loginProvider, string providerKey, string userId)
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

            arParams[0] = new SQLiteParameter(":LoginProvider", DbType.String);
            arParams[0].Value = loginProvider;

            arParams[1] = new SQLiteParameter(":ProviderKey", DbType.String);
            arParams[1].Value = providerKey;

            arParams[2] = new SQLiteParameter(":UserId", DbType.String);
            arParams[2].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public bool Delete(
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

            arParams[0] = new SQLiteParameter(":LoginProvider", DbType.String);
            arParams[0].Value = loginProvider;

            arParams[1] = new SQLiteParameter(":ProviderKey", DbType.String);
            arParams[1].Value = providerKey;

            arParams[2] = new SQLiteParameter(":UserId", DbType.String);
            arParams[2].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = :SiteGuid) ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public DbDataReader Find(string loginProvider, string providerKey)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LoginProvider = :LoginProvider AND ");
            sqlCommand.Append("ProviderKey = :ProviderKey ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":LoginProvider", DbType.String);
            arParams[0].Value = loginProvider;

            arParams[1] = new SQLiteParameter(":ProviderKey", DbType.String);
            arParams[1].Value = providerKey;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String);
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }
    }
}
