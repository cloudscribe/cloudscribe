// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2014-08-28
// 
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Text;

namespace cloudscribe.Core.Repositories.Firebird
{
    internal static class DBUserClaims
    {

        public static int Create(
            string userId,
            string claimType,
            string claimValue)
        {
            
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter(":UserId", FbDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter(":ClaimType", FbDbType.VarChar, -1);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter(":ClaimValue", FbDbType.VarChar, -1);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = claimValue;

            string statement = "EXECUTE PROCEDURE mp_USERCLAIMS_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")";

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                statement,
                arParams));

            return newID;
        }


        public static bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Id", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
               ConnectionString.GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = claimType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
               ConnectionString.GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter("@ClaimValue", FbDbType.VarChar, -1);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = claimValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
               ConnectionString.GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
               ConnectionString.GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        
        
    }
}
