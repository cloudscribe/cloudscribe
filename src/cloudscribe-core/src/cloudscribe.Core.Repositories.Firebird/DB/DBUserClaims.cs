// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2015-06-16
// 

using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
{
    internal class DBUserClaims
    {
        internal DBUserClaims(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;


        public async Task<int> Create(
            string userId,
            string claimType,
            string claimValue)
        {

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter(":UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter(":ClaimType", FbDbType.VarChar, -1);
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter(":ClaimValue", FbDbType.VarChar, -1);
            arParams[2].Value = claimValue;

            string statement = "EXECUTE PROCEDURE mp_USERCLAIMS_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")";

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.StoredProcedure,
                statement,
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;
        }


        public async Task<bool> Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Id", FbDbType.Integer);
            arParams[0].Value = id;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               sqlCommand.ToString(),
               arParams);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteByUser(string userId, string claimType)
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
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar);
            arParams[1].Value = claimType;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               sqlCommand.ToString(),
               arParams);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteByUser(string userId, string claimType, string claimValue)
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
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar);
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter("@ClaimValue", FbDbType.VarChar, -1);
            arParams[2].Value = claimValue;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               sqlCommand.ToString(),
               arParams);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = @SiteGuid) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               sqlCommand.ToString(),
               arParams);

            return (rowsAffected > -1);

        }


        public async Task<DbDataReader> GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> GetUsersByClaim(int siteId, string claimType, string claimValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  u.* ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("JOIN mp_UserClaims uc ");
            sqlCommand.Append("ON u.UserID = uc.UserId ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteID = @SiteId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("uc.ClaimType = @ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("uc.ClaimValue = @ClaimValue ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("u.Name ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar, 1000);
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter("@ClaimValue", FbDbType.VarChar, 1000);
            arParams[2].Value = claimValue;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
