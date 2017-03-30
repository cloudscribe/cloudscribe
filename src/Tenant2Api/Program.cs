using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Tenant2Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Tenant2 API";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:5004")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
