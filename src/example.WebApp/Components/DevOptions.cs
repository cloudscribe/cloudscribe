using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.WebApp
{
    public class DevOptions
    {
        public string DbPlatform { get; set; } = "ef"; // or NoDb mssql pgsql mysql sqlite sqlce firebird
    }
}
