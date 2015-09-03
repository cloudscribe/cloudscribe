// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-09-03
// 

using cloudscribe.DbHelpers.MSSQL;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
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
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;

        public async Task<int> Create(
            int siteId,
            string userId,
            string claimType,
            string claimValue)
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

            object result = await sph.ExecuteScalarAsync();
            int newID = Convert.ToInt32(result);
            return newID;
        }




        public async Task<bool> Delete(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_Delete", 
                1);

            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteByUser(int siteId, string userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_DeleteByUser", 
                2);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteByUser(int siteId, string userId, string claimType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_DeleteByUserByType", 
                2);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
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

        public async Task<bool> DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserClaims_DeleteBySite", 
                1);

            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > 0);

        }


        public async Task<DbDataReader> GetByUser(
            int siteId,
            string userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserClaims_SelectByUser", 
                2);

            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync();

        }

        public async Task<DbDataReader> GetUsersByClaim(int siteId, string claimType, string claimValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_SelectAllByClaim",
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@ClaimValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimValue);
            return await sph.ExecuteReaderAsync();

        }



    }

}
