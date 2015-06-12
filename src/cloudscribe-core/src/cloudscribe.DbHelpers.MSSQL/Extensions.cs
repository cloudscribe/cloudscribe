using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace cloudscribe.DbHelpers.MSSQL
{
    public static class Extensions
    {

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
