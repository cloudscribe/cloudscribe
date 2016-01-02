// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2008-09-12
// Last Modified:			2016-01-02
// 

using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
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

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

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
            sqlCommand.Append("e.SiteID = @SiteID ");

            sqlCommand.Append("ORDER BY d.GroupName, d.SortOrder ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<bool> EnsureSettings()
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

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                null);

            return rowsAffected > 0;

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
            sqlCommand.Append("KeyValue = @KeyValue ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID <> @SiteID ");
            sqlCommand.Append("AND KeyName = @KeyName ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@KeyName", FbDbType.VarChar, 128);
            arParams[1].Value = keyName;

            arParams[2] = new FbParameter("@KeyValue", FbDbType.VarChar);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("SiteId, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("KeyName, ");
            sqlCommand.Append("KeyValue, ");
            sqlCommand.Append("GroupName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@SiteId, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@KeyName, ");
            sqlCommand.Append("@KeyValue, ");
            sqlCommand.Append("@GroupName )");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter("@SiteId", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@KeyName", FbDbType.VarChar, 128);
            arParams[2].Value = keyName;

            arParams[3] = new FbParameter("@KeyValue", FbDbType.VarChar);
            arParams[3].Value = keyValue;

            arParams[4] = new FbParameter("@GroupName", FbDbType.VarChar, 128);
            arParams[4].Value = groupName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("KeyValue = @KeyValue ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND KeyName = @KeyName ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteID;

            arParams[1] = new FbParameter("@KeyName", FbDbType.VarChar, 128);
            arParams[1].Value = keyName;

            arParams[2] = new FbParameter("@KeyValue", FbDbType.VarChar);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("SiteID = @SiteID AND ");
            sqlCommand.Append("KeyName = @KeyName ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteID;

            arParams[1] = new FbParameter("@KeyName", FbDbType.VarChar, 128);
            arParams[1].Value = keyName;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
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
                readConnectionString,
                sqlCommand.ToString(),
                null);
        }


    }
}
