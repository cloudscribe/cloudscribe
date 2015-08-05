// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-05
// Last Modified:			2015-08-05
// 

namespace cloudscribe.DbHelpers.MSSQL
{
    public class MSSQLConnectionOptions
    {
        public string ReadConnectionString { get; set; } = string.Empty;
        public string WriteConnectionString { get; set; } = string.Empty;
        public string OwnerPrefix { get; set; } = "[dbo].";
    }
}
