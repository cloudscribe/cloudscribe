// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-09-12
// Last Modified:			2016-01-01
//

using cloudscribe.DbHelpers;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    public class DBSiteSettingsEx
    {
        internal DBSiteSettingsEx(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;

            // possibly will change this later to have SqliteFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqliteFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private AdoHelper AdoHelper;

        public DbDataReader GetSiteSettingsExList(int siteId)
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
            sqlCommand.Append("e.SiteID = :SiteID ");

            sqlCommand.Append("ORDER BY d.GroupName, d.SortOrder ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public void EnsureSettings()
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
            sqlCommand.Append("s.SiteID As SiteID, ");
            sqlCommand.Append("s.SiteGuid As SiteGuid, ");
            sqlCommand.Append("d.KeyName As KeyName, ");
            sqlCommand.Append("d.DefaultValue As DefaultValue, ");
            sqlCommand.Append("d.GroupName As GroupName ");
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
                connectionString,
                sqlCommand.ToString(),
                null);

        }

        public bool SaveExpandoProperty(
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

        public bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteSettingsEx ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("KeyValue = :KeyValue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID <> :SiteID AND ");
            sqlCommand.Append("KeyName = :KeyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":KeyName", DbType.String);
            arParams[1].Value = keyName;

            arParams[2] = new SqliteParameter(":KeyValue", DbType.Object);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



        private bool Create(
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
            sqlCommand.Append(":SiteId, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":KeyName, ");
            sqlCommand.Append(":KeyValue, ");
            sqlCommand.Append(":GroupName )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":KeyName", DbType.String);
            arParams[2].Value = keyName;

            arParams[3] = new SqliteParameter(":KeyValue", DbType.Object);
            arParams[3].Value = keyValue;

            arParams[4] = new SqliteParameter(":GroupName", DbType.String);
            arParams[4].Value = groupName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        private bool Update(
            int siteID,
            string keyName,
            string keyValue)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteSettingsEx ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("KeyValue = :KeyValue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID AND ");
            sqlCommand.Append("KeyName = :KeyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteID;

            arParams[1] = new SqliteParameter(":KeyName", DbType.String);
            arParams[1].Value = keyName;

            arParams[2] = new SqliteParameter(":KeyValue", DbType.Object);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        private int GetCount(
            int siteID,
            string keyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteSettingsEx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID AND ");
            sqlCommand.Append("KeyName = :KeyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteID;

            arParams[1] = new SqliteParameter(":KeyName", DbType.String);
            arParams[1].Value = keyName;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));
        }

        public DbDataReader GetDefaultExpandoSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteSettingsExDef ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }

    }
}
