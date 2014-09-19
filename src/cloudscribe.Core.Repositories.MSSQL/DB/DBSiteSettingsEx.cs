/// Author:					Joe Audette
/// Created:				2009-09-10
/// Last Modified:			2014-08-22 
///                         
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using cloudscribe.DbHelpers.MSSQL;

namespace cloudscribe.Core.Repositories.MSSQL
{
   
    internal static class DBSiteSettingsEx
    {
       

        public static IDataReader GetSiteSettingsExList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteSettingsEx_SelectAll", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

      
        public static void EnsureSettings()
        {
            new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteSettingsEx_EnsureDefinitions", 0).ExecuteNonQuery();
        }

        public static bool SaveExpandoProperty(
            int siteId,
            Guid siteGuid,
            string groupName,
            string keyName,
            string keyValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteSettingsEx_Save", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
            sph.DefineSqlParameter("@KeyValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, keyValue);
            sph.DefineSqlParameter("@GroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyName);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteSettingsEx_UpdateRelated", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
            sph.DefineSqlParameter("@KeyValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, keyValue);
            

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static IDataReader GetDefaultExpandoSettings()
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteSettingsExDef_SelectAll", 0);
            return sph.ExecuteReader();
        }

       
    }
}
