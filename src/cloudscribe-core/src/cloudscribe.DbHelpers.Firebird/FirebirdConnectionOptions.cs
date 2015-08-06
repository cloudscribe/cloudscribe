// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-06
// Last Modified:			2015-08-06
// 



namespace cloudscribe.DbHelpers.Firebird
{
    public class FirebirdConnectionOptions
    {

        private string connectionString = string.Empty;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private string readConnectionString = string.Empty;

        public string ReadConnectionString
        {
            get
            {
                if (readConnectionString.Length > 0) { return readConnectionString; }
                return connectionString;
            }
            set { readConnectionString = value; }
        }

        private string writeConnectionString = string.Empty;

        public string WriteConnectionString
        {
            get
            {
                if (writeConnectionString.Length > 0) { return writeConnectionString; }
                return connectionString;
            }
            set { writeConnectionString = value; }
        }
    }
}
