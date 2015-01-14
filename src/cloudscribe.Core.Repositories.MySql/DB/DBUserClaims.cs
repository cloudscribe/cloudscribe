// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2015-01-14
// 
// You must not remove this notice, or any other, from this software.

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.DbHelpers.MySql;

namespace cloudscribe.Core.Repositories.MySql
{

    internal static class DBUserClaims
    {

        public static async Task<int> Create(
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
            sqlCommand.Append("?UserId, ");
            sqlCommand.Append("?ClaimType, ");
            sqlCommand.Append("?ClaimValue )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?ClaimType", MySqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new MySqlParameter("?ClaimValue", MySqlDbType.Text);
            arParams[2].Value = claimValue;

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;

        }


        public static async Task<bool> Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = ?Id ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Id", MySqlDbType.Int32);
            arParams[0].Value = id;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> DeleteByUser(string userId, string claimType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = ?ClaimType ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?ClaimType", MySqlDbType.Text);
            arParams[1].Value = claimType;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }

        public static async Task<bool> DeleteByUser(string userId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = ?ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimValue = ?ClaimValue ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?ClaimType", MySqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new MySqlParameter("?ClaimValue", MySqlDbType.Text);
            arParams[2].Value = claimValue;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<bool> DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = ?SiteGuid) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 128);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static async Task<DbDataReader> GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        

        

        
    }
}
