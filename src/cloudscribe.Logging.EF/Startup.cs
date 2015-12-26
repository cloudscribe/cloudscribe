// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-03
// Last Modified:			2015-12-26
// 

//http://www.jerriepelser.com/blog/moving-entity-framework-7-models-to-external-project

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;

namespace cloudscribe.Logging.EF
{
    public class Startup
    {
        
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<ILogModelMapper, SqlServerLogModelMapper>();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<LoggingDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:EF7ConnectionOptions:ConnectionString"]));
        }


        public void Configure(IApplicationBuilder app)
        {
        }

    }
}
