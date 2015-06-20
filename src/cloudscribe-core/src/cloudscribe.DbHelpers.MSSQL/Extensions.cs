// Author:					Joe Audette
// Created:					2015-06-16
// Last Modified:			2015-06-20
// 


using cloudscribe.Configuration;
using Microsoft.Framework.ConfigurationModel;
using System.Data.SqlClient;

namespace cloudscribe.DbHelpers.MSSQL
{
    public static class Extensions
    {
        public static string GetMSSQLWriteConnectionString(this IConfiguration configuration)
        {
            return configuration.GetOrDefault("AppSettings:MSSQLWriteConnectionString",
                configuration.Get("AppSettings:MSSQLConnectionString")
                );
        }

        public static string GetMSSQLReadConnectionString(this IConfiguration configuration)
        {
            return configuration.GetOrDefault("AppSettings:MSSQLReadConnectionString",
                configuration.Get("AppSettings:MSSQLConnectionString")
                );
        }


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
