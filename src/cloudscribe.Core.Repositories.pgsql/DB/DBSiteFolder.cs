// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-01
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.pgsql
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

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public async Task<bool> Add(
            Guid guid,
            Guid siteGuid,
            string folderName,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = folderName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_sitefolders (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("foldername )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":foldername ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = folderName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_sitefolders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("foldername = :foldername ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> Delete(
            Guid guid,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteFoldersBySite(
            Guid siteGuid,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public DbDataReader GetOne(Guid guid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");
            
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = folderName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("foldername = :foldername ");
            sqlCommand.Append(";");
            
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<bool> Exists(
            string folderName,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[0].Value = folderName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("foldername = :foldername ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return (count > 0);

        }


        public async Task<Guid> GetSiteGuid(
            string folderName,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = folderName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE( ");
            sqlCommand.Append("(SELECT siteguid FROM mp_sitefolders where foldername = :foldername limit 1), ");
            sqlCommand.Append("(SELECT siteguid FROM mp_sites order by siteid limit 1) ");
            sqlCommand.Append(") ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            string strGuid = result.ToString();
            if (strGuid.Length == 36)
            {
                return new Guid(strGuid);
            }
            else
            {
                return Guid.Empty;
            }


        }

        public async Task<DbDataReader> GetAll(CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("s.siteid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("sf.guid, ");
            sqlCommand.Append("sf.foldername ");

            sqlCommand.Append("FROM	mp_sitefolders sf ");

            sqlCommand.Append("JOIN	mp_sites s ");

            sqlCommand.Append("ON sf.siteguid = s.siteguid ");

            sqlCommand.Append("ORDER BY sf.foldername ");
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
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("s.siteid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("sf.guid, ");
            sqlCommand.Append("sf.foldername ");

            sqlCommand.Append("FROM	mp_sitefolders sf ");

            sqlCommand.Append("JOIN	mp_sites s ");

            sqlCommand.Append("ON sf.siteguid = s.siteguid ");

            sqlCommand.Append("ORDER BY sf.foldername ");
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
            sqlCommand.Append("FROM	mp_sitefolders ");
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
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("s.siteid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("sf.guid, ");
            sqlCommand.Append("sf.foldername ");

            sqlCommand.Append("FROM	mp_sitefolders sf  ");
            sqlCommand.Append("JOIN	mp_sites s ");

            sqlCommand.Append("ON sf.siteguid = s.siteguid ");

            sqlCommand.Append("ORDER BY sf.foldername ");

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }


    }
}
