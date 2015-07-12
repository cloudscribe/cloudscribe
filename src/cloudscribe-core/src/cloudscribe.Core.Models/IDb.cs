// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using cloudscribe.Configuration;
using System;
using System.Data;
using System.Data.Common;
using System.IO;

namespace cloudscribe.Core.Models
{
    public interface IDb
    {
        void EnsureDatabase();
        bool CanAccessDatabase();
        bool CanAccessDatabase(string overrideConnectionInfo);
        bool CanAlterSchema(string overrideConnectionInfo);
        bool CanCreateTemporaryTables();
        string DBPlatform { get; }
        IVersionProviderFactory VersionProviders { get; }
        int ExecteNonQuery(string connectionString, string query);
        DbException GetConnectionError(string overrideConnectionInfo);
        DbDataReader GetReader(string connectionString, string query);
        DbDataReader GetReader(string connectionString, string tableName, string whereClause);
        //http://stackoverflow.com/questions/27900493/asp-vnext-core-5-0-datatable
        //DataTable GetTable(string connectionString, string tableName, string whereClause);
        bool RunScript(FileInfo scriptFile, string overrideConnectionInfo);
        bool RunScript(string script, string overrideConnectionInfo);
        bool TableExists(string tableName);
        bool UpdateTableField(
            string connectionString,
            string tableName,
            string keyFieldName,
            string keyFieldValue,
            string dataFieldName,
            string dataFieldValue,
            string additionalWhere);

        bool UpdateTableField(
            string tableName,
            string keyFieldName,
            string keyFieldValue,
            string dataFieldName,
            string dataFieldValue,
            string additionalWhere);

        bool SitesTableExists();
        int ExistingSiteCount();

        Guid GetOrGenerateSchemaApplicationId(string applicationName);
        Version GetSchemaVersion(Guid applicationId);

        bool AddSchemaVersion(
          Guid applicationId,
          string applicationName,
          int major,
          int minor,
          int build,
          int revision);

        bool UpdateSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision);

        bool SchemaVersionExists(Guid applicationId);

        int AddSchemaScriptHistory(
            Guid applicationId,
            string scriptFile,
            DateTime runTime,
            bool errorOccurred,
            string errorMessage,
            string scriptBody);
    }
}
