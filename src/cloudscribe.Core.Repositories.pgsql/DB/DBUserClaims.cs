// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2015-01-14
// 
//
// You must not remove this notice, or any other, from this software.

using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.DbHelpers.pgsql;


namespace cloudscribe.Core.Repositories.pgsql
{
    internal static class DBUserClaims
    {

        public static async Task<int> Create(
            string userId,
            string claimType,
            string claimValue)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userclaims (");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("claimtype, ");
            sqlCommand.Append("claimvalue )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":claimtype, ");
            sqlCommand.Append(":claimvalue )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_userclaimsid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new NpgsqlParameter("claimvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = claimValue;

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public static async Task<bool> Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("id = :id ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = id;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static async Task<bool> DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static async Task<bool> DeleteByUser(string userId, string claimType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("claimtype = :claimtype ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static async Task<bool> DeleteByUser(string userId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("claimtype = :claimtype ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("claimvalue = :claimvalue ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("claimtype", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[1].Value = claimType;

            arParams[2] = new NpgsqlParameter("claimvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = claimValue;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static async Task<bool> DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid IN (SELECT userguid FROM mp_users WHERE siteguid = :siteguid) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static async Task<DbDataReader> GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userclaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[0].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        

        

        
    }
}
