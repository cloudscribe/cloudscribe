// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-06-22
// Last Modified:			2015-07-04
// 

using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using System;

namespace cloudscribe.DbHelpers.SQLite
{
    public class SQLiteConnectionstringResolver
    {
        public SQLiteConnectionstringResolver(
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (hostingEnvironment == null) { throw new ArgumentNullException(nameof(hostingEnvironment)); }

            env = hostingEnvironment;
            config = configuration;

        }

        private IHostingEnvironment env;
        private IConfiguration config;

        public string Resolve()
        {
            return config.GetSQLiteConnectionString(env);
        }
    }
}
