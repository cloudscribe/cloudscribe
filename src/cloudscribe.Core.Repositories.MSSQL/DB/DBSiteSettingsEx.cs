// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2009-09-10
// Last Modified:			2016-01-03
//                         

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{

    internal class DBSiteSettingsEx
    {
        internal DBSiteSettingsEx(
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

        public DbDataReader GetSiteSettingsExList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteSettingsEx_SelectAll", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }


        public async Task<bool> EnsureSettings()
        {
            int rowsAffected = await new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_SiteSettingsEx_EnsureDefinitions", 0).ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public bool SaveExpandoProperty(
            int siteId,
            Guid siteGuid,
            string groupName,
            string keyName,
            string keyValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SiteSettingsEx_Save", 
                5);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
            sph.DefineSqlParameter("@KeyValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, keyValue);
            sph.DefineSqlParameter("@GroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyName);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SiteSettingsEx_UpdateRelated", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
            sph.DefineSqlParameter("@KeyValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, keyValue);


            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public DbDataReader GetDefaultExpandoSettings()
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteSettingsExDef_SelectAll", 
                0);

            return sph.ExecuteReader();
        }


    }
}
