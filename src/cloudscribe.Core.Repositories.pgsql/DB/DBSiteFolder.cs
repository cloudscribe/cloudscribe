// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-11-18
// 

using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Logging;
using cloudscribe.DbHelpers.pgsql;

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
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;

        public async Task<bool> Add(
            Guid guid,
            Guid siteGuid,
            string folderName)
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

            //int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_insert(:guid,:siteguid,:foldername)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }

        public async Task<bool> Update(
            Guid guid,
            Guid siteGuid,
            string folderName)
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

            //int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_update(:guid,:siteguid,:foldername)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public async Task<bool> Delete(Guid guid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitefolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            //int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_delete(:guid)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_select_one(:guid)",
            //    arParams);

            return AdoHelper.ExecuteReader(
               readConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               arParams);

        }

        public async Task<DbDataReader> GetOne(string folderName)
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
                arParams);

        }

        public async Task<DbDataReader> GetBySite(Guid siteGuid)
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

            //return await AdoHelper.ExecuteReaderAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_select_bysite(:siteguid)",
            //    arParams);

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<bool> Exists(string folderName)
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

            //object result = await AdoHelper.ExecuteScalarAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_exists(:foldername)",
            //    arParams);

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(result);

            return (count > 0);

        }


        public async Task<Guid> GetSiteGuid(string folderName)
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

            //object result = await AdoHelper.ExecuteScalarAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitefolders_selectsiteguid(:foldername)",
            //    arParams);

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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

        public async Task<DbDataReader> GetAll()
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
                null);
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

        public async Task<int> GetFolderCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitefolders ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetPage(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount();

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

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
                arParams);


        }


    }
}
