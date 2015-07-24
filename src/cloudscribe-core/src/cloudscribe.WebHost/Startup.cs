using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
//using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.Data.Entity;
using Microsoft.Framework.Caching;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Runtime;
//using cloudscribe.WebHost.Models;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Repositories.MSSQL;
using cloudscribe.Core.Web.Components;
using Autofac;
using Autofac.Framework.DependencyInjection;

namespace cloudscribe.WebHost
{
    public class Startup
    {
        

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var configuration = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            //appEnv.
            

            if (env.IsEnvironment("Development"))
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                configuration.AddUserSecrets();
            }

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            configuration.AddJsonFile("config.local.overrides.json", optional: true);

            // most common use of environment variables would be in azure hosting
            // since it is added last anything in env vars would trump the same setting in previous config sources
            // so no risk of messing up settings if deploying a new version to azure
            configuration.AddEnvironmentVariables();
            Configuration = configuration.Build();

            //env.MapPath
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add Application settings to the services container.
            services.Configure<AppSettings>(Configuration.GetConfigurationSection("AppSettings"));


            // Add EF services to the services container.
            //https://github.com/aspnet/EntityFramework/blob/dev/src/EntityFramework.Core/Extensions/EntityFrameworkServiceCollectionExtensions.cs
            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));


            // Add Identity services to the services container.
            // https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/IdentityServiceCollectionExtensions.cs
            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity.EntityFramework/IdentityRole.cs

            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity.EntityFramework/IdentityEntityFrameworkBuilderExtensions.cs
            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity.EntityFramework/IdentityEntityFrameworkServices.cs


            
            // Setup dependencies for cloudscribe Identity, Roles and and Site Administration
            // this is in Startup.CloudscribeCore.cs
            services.ConfigureCloudscribeCore(Configuration);

            // previous entity framework dependencies
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            
            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            services.Configure<FacebookAuthenticationOptions>(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });

            // we are adding this from Startup.CloudscribeCore.cs so it is not needed here
            // Add MVC services to the services container.
            //services.AddMvc();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            //Autofac config
            var builder = new ContainerBuilder();

            ////Populate the container with services that were previously registered
            //// it seems this depends on beta4
            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();

        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure the HTTP request pipeline.

            // Add the console logger.
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                //app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                //app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
                //app.UseErrorPage(ErrorPageOptions.ShowAll);
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();
            
            // this is in Startup.CloudscribeCore.cs
            app.UseCloudscribeCore(Configuration);

            
        }
    }
}
