// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-07
// Last Modified:			2015-08-07
// 


namespace cloudscribe.DbHelpers.SQLite
{
    public class SqliteConnectionOptions
    {

        public string DbFileName { get; set; } = "cloudscribe.sqlite";

        public string ConnectionString { get; set; } = string.Empty;

    }
}
