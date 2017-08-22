//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2015-12-03
//// Last Modified:			2016-05-18
//// 

////http://www.jerriepelser.com/blog/moving-entity-framework-7-models-to-external-project

//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;


//namespace cloudscribe.Core.Storage.EFCore.MySql
//{
//    public class CoreDcContextDesignTimeFactory : IDesignTimeDbContextFactory<CoreDbContext>
//    {
//        public CoreDbContext CreateDbContext(string[] args)
//        {
//            var builder = new DbContextOptionsBuilder<CoreDbContext>();
//            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");
            
//            return new CoreDbContext(builder.Options);
//        }
//    }

//    public class Startup
//    {
        
//        public IConfigurationRoot Configuration { get; set; }

//        public Startup(IHostingEnvironment env)
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(env.ContentRootPath)
//                .AddJsonFile("appsettings.json");

//            // this file name is ignored by gitignore
//            // so you can create it and use on your local dev machine
//            // remember last config source added wins if it has the same settings
//            builder.AddJsonFile("appsettings.dev.json", optional: true);

//            Configuration = builder.Build();
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            //services.AddEntityFrameworkSqlServer()
//            //  .AddDbContext<CoreDbContext>((serviceProvider, options) =>
//            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
//            //           .UseInternalServiceProvider(serviceProvider)
//            //           );
//            //services.AddEntityFrameworkMySQL()
//            //  .AddDbContext<CoreDbContext>((serviceProvider, options) =>
//            //    options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"))
//            //           .UseInternalServiceProvider(serviceProvider)
//            //           );
//            services.AddCloudscribeCoreEFStorageMySql(Configuration.GetConnectionString("MySqlEntityFrameworkConnectionString"));
//        }


//        public void Configure(IApplicationBuilder app)
//        {
//        }

//    }
//}
