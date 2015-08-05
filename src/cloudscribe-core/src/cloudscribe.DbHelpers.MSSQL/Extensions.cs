﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-16
// Last Modified:			2015-08-05
// 



using System.Data.SqlClient;

namespace cloudscribe.DbHelpers.MSSQL
{
    public static class Extensions
    {
        //public static string GetMSSQLWriteConnectionString(this IConfiguration configuration)
        //{
        //    string connectionString = configuration.GetOrDefault("AppSettings:MSSQLWriteConnectionString",
        //        configuration.Get("AppSettings:MSSQLConnectionString")
        //        );

        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        throw new ArgumentException("could not find connection string AppSettings:MSSQLConnectionString");
        //    }

        //    return connectionString;
        //}

        //public static string GetMSSQLReadConnectionString(this IConfiguration configuration)
        //{
        //    string connectionString = configuration.GetOrDefault("AppSettings:MSSQLReadConnectionString",
        //        configuration.Get("AppSettings:MSSQLConnectionString")
        //        );

        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        throw new ArgumentException("could not find connection string AppSettings:MSSQLConnectionString");
        //    }

        //    return connectionString;
        //}

        //public static string MSSQLOwnerPrefix(this IConfiguration configuration)
        //{
        //    return configuration.GetOrDefault("AppSettings:MSSQLOwnerPrefix", "[dbo].");
        //}


        public static SqlParameter Copy(this SqlParameter origParam)
        {
            SqlParameter newParam = new SqlParameter();
            newParam.DbType = origParam.DbType;
            newParam.Direction = origParam.Direction;
            newParam.ParameterName = origParam.ParameterName;
            newParam.Size = origParam.Size;
            newParam.Precision = origParam.Precision;
            return newParam;
        }

    }
}
