// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-06-22
// Last Modified:			2015-08-07
// 

using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Runtime;
using System;
using System.IO;

namespace cloudscribe.DbHelpers.SQLite
{
    public class SQLiteConnectionstringResolver
    {
        public SQLiteConnectionstringResolver(
            IApplicationEnvironment appEnv,
            IOptions<SqliteConnectionOptions> configuration)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }

            appBasePath = appEnv.ApplicationBasePath;
            options = configuration.Options;

        }

        private SqliteConnectionOptions options;
        private string appBasePath;

        public string Resolve()
        {
            if(options.ConnectionString.Length > 0) { return options.ConnectionString; }

            string pathToDbFile = appBasePath + "/config/sqlitedb/".Replace("/", Path.DirectorySeparatorChar.ToString()) + options.DbFileName;
            string connectionString = "data source=" + pathToDbFile + ";version=3;";
            return connectionString;
        }
    }
}
