// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-07-04
// 


using cloudscribe.Configuration;
using Microsoft.Framework.Configuration;
using System;

namespace cloudscribe.DbHelpers.Firebird
{
    public static class Extensions
    {
        public static string GetFirebirdWriteConnectionString(this IConfiguration configuration)
        {
            string connectionString = configuration.GetOrDefault("AppSettings:FirebirdWriteConnectionString",
                configuration.Get("AppSettings:FirebirdConnectionString")
                );

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:FirebirdConnectionString");
            }

            return connectionString;
        }

        public static string GetFirebirdReadConnectionString(this IConfiguration configuration)
        {
            string connectionString = configuration.GetOrDefault("AppSettings:FirebirdReadConnectionString",
                configuration.Get("AppSettings:FirebirdConnectionString")
                );

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:FirebirdConnectionString");
            }

            return connectionString;
        }
    }
}
