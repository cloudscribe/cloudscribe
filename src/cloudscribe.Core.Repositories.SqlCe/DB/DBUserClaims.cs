// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2014-08-27
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Data.SqlServerCe;
using cloudscribe.DbHelpers.SqlCe;

namespace cloudscribe.Core.Repositories.SqlCe
{
    internal static class DBUserClaims
    {

        public static int Create(
            string userId,
            string claimType,
            string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserClaims ");
            sqlCommand.Append("(");
            sqlCommand.Append("UserId, ");
            sqlCommand.Append("ClaimType, ");
            sqlCommand.Append("ClaimValue ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@UserId, ");
            sqlCommand.Append("@ClaimType, ");
            sqlCommand.Append("@ClaimValue ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@ClaimType", SqlDbType.NText);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = claimType;

            arParams[2] = new SqlCeParameter("@ClaimValue", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = claimValue;


            int newId = Convert.ToInt32(AdoHelper.DoInsertGetIdentitiy(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

        }



        
        public static bool Update(
            int id,
            string userId,
            string claimType,
            string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_UserClaims ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserId = @UserId, ");
            sqlCommand.Append("ClaimType = @ClaimType, ");
            sqlCommand.Append("ClaimValue = @ClaimValue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@Id", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqlCeParameter("@ClaimType", SqlDbType.NVarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = claimType;

            arParams[3] = new SqlCeParameter("@ClaimValue", SqlDbType.NVarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = claimValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Id", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteByUser(string userId, string claimType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = @ClaimType ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@ClaimType", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = claimType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByUser(string userId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = @ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimValue = @ClaimValue ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@ClaimType", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = claimType;

            arParams[2] = new SqlCeParameter("@ClaimValue", SqlDbType.NVarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = claimValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = @SiteGuid) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

      
        
    }
}
