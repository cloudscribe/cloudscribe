// Author:					Joe Audette
// Created:				    2009-09-10
// Last Modified:			2015-06-09
//                         
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MSSQL;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
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
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;

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
