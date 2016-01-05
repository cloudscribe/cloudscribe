// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using System.IO;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Setup;

namespace cloudscribe.Setup.Web
{
    

    public interface IDbSetup : IDataPlatformInfo
    {
        //TODO split any of these methods that could/should also be implemented in EF
        // into a different interface ie CanAccessDatabase, ExistingSiteCount, etc
        // then what remains can be used from a non ef implementation while still using ef for some repositories
        // ie I want to decouple the setup system in a way that it can still be used for additional feature installation even if 
        // the core features are using ef
        // probably also should add a model class for SchemaVersion so EF can populate it to avoid
        // a clash if the sql core scripts exist on disk - we don't want to run them if using EF for core repos
        // but we still want to be able to let other scripts run if they are not related to core

        IVersionProviderFactory VersionProviders { get; }
        void EnsureDatabase();
        bool CanAccessDatabase();
        bool CanAccessDatabase(string overrideConnectionInfo);
        bool CanAlterSchema(string overrideConnectionInfo);
        bool CanCreateTemporaryTables();
        
        DbException GetConnectionError(string overrideConnectionInfo);
        
        bool RunScript(FileInfo scriptFile, string overrideConnectionInfo);
        bool RunScript(string script, string overrideConnectionInfo);
        bool TableExists(string tableName);
        
        bool SchemaTableExists();
       
        Guid GetOrGenerateSchemaApplicationId(string applicationName);
        Version GetSchemaVersion(Guid applicationId);
        bool SchemaVersionExists(Guid applicationId);

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

        

        //int AddSchemaScriptHistory(
        //    Guid applicationId,
        //    string scriptFile,
        //    DateTime runTime,
        //    bool errorOccurred,
        //    string errorMessage,
        //    string scriptBody);

        //int ExecteNonQuery(string connectionString, string query);

        //bool UpdateTableField(
        //    string connectionString,
        //    string tableName,
        //    string keyFieldName,
        //    string keyFieldValue,
        //    string dataFieldName,
        //    string dataFieldValue,
        //    string additionalWhere);

        //bool UpdateTableField(
        //    string tableName,
        //    string keyFieldName,
        //    string keyFieldValue,
        //    string dataFieldName,
        //    string dataFieldValue,
        //    string additionalWhere);

        //DbDataReader GetReader(string connectionString, string query);
        //DbDataReader GetReader(string connectionString, string tableName, string whereClause);
        //http://stackoverflow.com/questions/27900493/asp-vnext-core-5-0-datatable
        //DataTable GetTable(string connectionString, string tableName, string whereClause);

    }
}
