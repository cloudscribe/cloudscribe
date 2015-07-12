// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-21
// Last Modified:			2015-07-04
// 


using cloudscribe.Configuration;
using Microsoft.Framework.Configuration;
using System;

namespace cloudscribe.DbHelpers.pgsql
{
    public static class Extensions
    {
        public static string GetPgsqlWriteConnectionString(this IConfiguration configuration)
        {
            string connectionString =  configuration.GetOrDefault("AppSettings:PostgreSQLWriteConnectionString",
                configuration.Get("AppSettings:PostgreSQLConnectionString")
                );

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:PostgreSQLConnectionString");
            }

            return connectionString;
        }

        public static string GetPgsqlReadConnectionString(this IConfiguration configuration)
        {
            string connectionString = configuration.GetOrDefault("AppSettings:PostgreSQLReadConnectionString",
                configuration.Get("AppSettings:PostgreSQLConnectionString")
                );

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:PostgreSQLConnectionString");
            }

            return connectionString;
        }
    }
}
