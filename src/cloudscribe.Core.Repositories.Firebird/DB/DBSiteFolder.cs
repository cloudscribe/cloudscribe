// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-02
// 

using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Firebird
{
    internal class DBSiteFolder
    {
        internal DBSiteFolder(
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

        public async Task<bool> Add(
            Guid guid,
            Guid siteGuid,
            string folderName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteFolders (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FolderName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FolderName );");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[2].Value = folderName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return rowsAffected > 0;

        }


        public async Task<bool> Update(
            Guid guid,
            Guid siteGuid,
            string folderName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteFolders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("FolderName = @FolderName ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[2].Value = folderName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }


        public async Task<bool> Delete(
            Guid guid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteBySite(
            Guid siteGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }


        public DbDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> GetOne(
            string folderName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FolderName = @FolderName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Value = folderName;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetBySite(
            Guid siteGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<Guid> GetSiteGuid(
            string folderName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Value = folderName;

            Guid siteGuid = Guid.Empty;

            sqlCommand.Append("SELECT SiteGuid ");
            sqlCommand.Append("FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = @FolderName ;");

            using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken))
            {
                if (reader.Read())
                {
                    siteGuid = new Guid(reader["SiteGuid"].ToString());
                }
            }

            if (siteGuid == Guid.Empty)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT FIRST 1 SiteGuid ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                    readConnectionString,
                    CommandType.Text,
                    sqlCommand.ToString(),
                    null,
                    cancellationToken))
                {
                    if (reader.Read())
                    {
                        siteGuid = new Guid(reader["SiteGuid"].ToString());
                    }
                }

            }

            return siteGuid;

        }

        public async Task<bool> Exists(
            string folderName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = @FolderName ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Value = folderName;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return (count > 0);

        }

        public async Task<DbDataReader> GetAll(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("sf.Guid, ");
            sqlCommand.Append("sf.FolderName ");

            sqlCommand.Append("FROM	mp_SiteFolders sf ");

            sqlCommand.Append("JOIN	mp_Sites s ");

            sqlCommand.Append("ON sf.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("ORDER BY sf.FolderName ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }

        public DbDataReader GetAllNonAsync()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("sf.Guid, ");
            sqlCommand.Append("sf.FolderName ");

            sqlCommand.Append("FROM	mp_SiteFolders sf ");

            sqlCommand.Append("JOIN	mp_Sites s ");

            sqlCommand.Append("ON sf.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("ORDER BY sf.FolderName ");

            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        public async Task<int> GetFolderCount(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetPage(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            //sqlCommand.Append("	* ");

            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("sf.Guid, ");
            sqlCommand.Append("sf.FolderName ");

            sqlCommand.Append("FROM	mp_SiteFolders sf ");


            sqlCommand.Append("JOIN	mp_Sites s ");

            sqlCommand.Append("ON sf.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("ORDER BY sf.FolderName ");

            sqlCommand.Append("	; ");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }


    }
}
