// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Setup;

namespace cloudscribe.Setup.Web
{
    

    public interface IDbSetup : IDataPlatformInfo
    {
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

        Task<DbDataReader> SchemaVersionGetAll(CancellationToken cancellationToken);


    }
}
