// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2015-01-20
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal static class DBUserClaims
    {
        public static int Create(
            string userId,
            string claimType,
            string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserClaims (");
            sqlCommand.Append("UserId, ");
            sqlCommand.Append("ClaimType, ");
            sqlCommand.Append("ClaimValue )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":UserId, ");
            sqlCommand.Append(":ClaimType, ");
            sqlCommand.Append(":ClaimValue )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Value = userId;

            arParams[1] = new SQLiteParameter(":ClaimType", DbType.Object);
            arParams[1].Value = claimType;

            arParams[2] = new SQLiteParameter(":ClaimValue", DbType.Object);
            arParams[2].Value = claimValue;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = :Id ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":Id", DbType.Int32);
            arParams[0].Value = id;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(string userId, string claimType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = :ClaimType ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Value = userId;

            arParams[1] = new SQLiteParameter(":ClaimType", DbType.Object);
            arParams[1].Value = claimType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(string userId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = :ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimValue = :ClaimValue ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Value = userId;

            arParams[1] = new SQLiteParameter(":ClaimType", DbType.Object);
            arParams[1].Value = claimType;

            arParams[2] = new SQLiteParameter(":ClaimValue", DbType.Object);
            arParams[2].Value = claimValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = :SiteGuid) ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":SiteGuid", DbType.String, 128);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = :UserId ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":UserId", DbType.String, 128);
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
    }
}
