// Author:					Joe Audette
// Created:				    2008-09-12
// Last Modified:			2015-01-15
// 
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using cloudscribe.DbHelpers.MySql;

namespace cloudscribe.Core.Repositories.MySql
{
    internal static class DBSiteSettingsEx
    {
        public static IDataReader GetSiteSettingsExList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  e.* ");

            sqlCommand.Append("FROM	mp_SiteSettingsEx e ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_SiteSettingsExDef d ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.KeyName = d.KeyName ");
            sqlCommand.Append("AND e.GroupName = d.GroupName ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.SiteID = ?SiteID ");

            sqlCommand.Append("ORDER BY d.GroupName, d.SortOrder ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void EnsureSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteSettingsEx");
            sqlCommand.Append("( ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("KeyName, ");
            sqlCommand.Append("KeyValue, ");
            sqlCommand.Append("GroupName ");
            sqlCommand.Append(")");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("t.SiteID, ");
            sqlCommand.Append("t.SiteGuid, ");
            sqlCommand.Append("t.KeyName, ");
            sqlCommand.Append("t.DefaultValue, ");
            sqlCommand.Append("t.GroupName  ");

            sqlCommand.Append("FROM ");

            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("d.KeyName, ");
            sqlCommand.Append("d.DefaultValue, ");
            sqlCommand.Append("d.GroupName ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Sites s, ");
            sqlCommand.Append("mp_SiteSettingsExDef d ");
            sqlCommand.Append(") t ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_SiteSettingsEx e ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.SiteID = t.SiteID ");
            sqlCommand.Append("AND e.KeyName = t.KeyName ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.SiteID IS NULL ");
            sqlCommand.Append("; ");
            
            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static bool SaveExpandoProperty(
           int siteId,
           Guid siteGuid,
           string groupName,
           string keyName,
           string keyValue)
        {
            int count = GetCount(siteId, keyName);
            if (count > 0)
            {
                return Update(siteId, keyName, keyValue);

            }
            else
            {
                return Create(siteId, siteGuid, keyName, keyValue, groupName);

            }

        }

        public static bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SiteSettingsEx ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("KeyValue = ?KeyValue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID <> ?SiteID AND ");
            sqlCommand.Append("KeyName = ?KeyName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?KeyName", MySqlDbType.VarChar, 128);
            arParams[1].Value = keyName;

            arParams[2] = new MySqlParameter("?KeyValue", MySqlDbType.Text);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static bool Create(
            int siteId,
            Guid siteGuid,
            string keyName,
            string keyValue,
            string groupName)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteSettingsEx (");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("KeyName, ");
            sqlCommand.Append("KeyValue, ");
            sqlCommand.Append("GroupName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?SiteId, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?KeyName, ");
            sqlCommand.Append("?KeyValue, ");
            sqlCommand.Append("?GroupName )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?SiteId", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?KeyName", MySqlDbType.VarChar, 128);
            arParams[2].Value = keyName;

            arParams[3] = new MySqlParameter("?KeyValue", MySqlDbType.Text);
            arParams[3].Value = keyValue;

            arParams[4] = new MySqlParameter("?GroupName", MySqlDbType.VarChar, 128);
            arParams[4].Value = groupName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        private static bool Update(
            int siteID,
            string keyName,
            string keyValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SiteSettingsEx ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("KeyValue = ?KeyValue ");
   
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = ?SiteID AND ");
            sqlCommand.Append("KeyName = ?KeyName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteID;

            arParams[1] = new MySqlParameter("?KeyName", MySqlDbType.VarChar, 128);
            arParams[1].Value = keyName;

            arParams[2] = new MySqlParameter("?KeyValue", MySqlDbType.Text);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static int GetCount(
            int siteID,
            string keyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteSettingsEx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = ?SiteID AND ");
            sqlCommand.Append("KeyName = ?KeyName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteID;

            arParams[1] = new MySqlParameter("?KeyName", MySqlDbType.VarChar, 128);
            arParams[1].Value = keyName;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetDefaultExpandoSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteSettingsExDef ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }
        

    }
}
