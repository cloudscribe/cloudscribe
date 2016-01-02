// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-11
// Last Modified:			2016-01-02
// 

using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
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

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

        public async Task<int> Create(
            int siteId,
            string userId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken)
        {

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter(":UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter(":ClaimType", FbDbType.VarChar, -1);
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter(":ClaimValue", FbDbType.VarChar, -1);
            arParams[2].Value = claimValue;

            arParams[3] = new FbParameter(":SiteId", FbDbType.Integer);
            arParams[3].Value = siteId;

            string statement = "EXECUTE PROCEDURE mp_USERCLAIMS_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")";

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.StoredProcedure,
                statement,
                true,
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;
        }


        public async Task<bool> Delete(
            int id,
            CancellationToken cancellationToken)
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
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((@SiteId = -1) OR (SiteId = @SiteId)) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[1].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               true,
               arParams,
               cancellationToken);

            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId, 
            string claimType,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((@SiteId = -1) OR (SiteId = @SiteId)) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = @ClaimType ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar);
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[2].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               true,
               arParams,
               cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId, 
            string claimType, 
            string claimValue,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((@SiteId = -1) OR (SiteId = @SiteId)) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimType = @ClaimType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ClaimValue = @ClaimValue ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@ClaimType", FbDbType.VarChar);
            arParams[1].Value = claimType;

            arParams[2] = new FbParameter("@ClaimValue", FbDbType.VarChar, -1);
            arParams[2].Value = claimValue;

            arParams[3] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[3].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               true,
               arParams,
               cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId  = @SiteId ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
               writeConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               true,
               arParams,
               cancellationToken);

            return (rowsAffected > -1);

        }


        public async Task<DbDataReader> GetByUser(
            int siteId, 
            string userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserClaims ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId  = @SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserId", FbDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[1].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetUsersByClaim(
            int siteId, 
            string claimType, 
            string claimValue,
            CancellationToken cancellationToken)
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
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

    }
}
