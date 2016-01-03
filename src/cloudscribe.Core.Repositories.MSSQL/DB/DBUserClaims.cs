// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2016-01-03
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
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

            // possibly will change this later to have SqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqlClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public async Task<int> Create(
            int siteId,
            string userId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_Insert", 
                4);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@ClaimValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimValue);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);

            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int newID = Convert.ToInt32(result);
            return newID;
        }




        public async Task<bool> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_Delete", 
                1);

            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_DeleteByUser", 
                2);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteByUser(
            int siteId, 
            string userId, 
            string claimType,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_DeleteByUserByType", 
                2);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > 0);

        }

        //public async Task<bool> DeleteByUser(string userId, string claimType, string claimValue)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_UserClaims_DeleteExactByUser", 
        //        3);

        //    sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
        //    sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
        //    sph.DefineSqlParameter("@ClaimValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimValue);
        //    int rowsAffected = await sph.ExecuteNonQueryAsync();
        //    return (rowsAffected > 0);

        //}

        public async Task<bool> DeleteBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_DeleteBySite", 
                1);

            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > 0);

        }


        public async Task<DbDataReader> GetByUser(
            int siteId,
            string userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserClaims_SelectByUser", 
                2);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public async Task<DbDataReader> GetUsersByClaim(
            int siteId, 
            string claimType, 
            string claimValue,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_SelectAllByClaim",
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@ClaimValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimValue);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }



    }

}
