using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.WebApp
{
    public class DevOptions
    {
        public string DbPlatform { get; set; } = "mssql"; // or pgsql mysql sqlite sqlce firebird
    }
}
