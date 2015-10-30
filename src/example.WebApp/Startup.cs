using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.IISPlatformHandler;
using Microsoft.AspNet.Server.Features;
using Microsoft.AspNet.Server.Kestrel.Infrastructure;
using Microsoft.AspNet.Diagnostics;
//using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Core;
//using Microsoft.Data.Entity;
using Microsoft.Framework.Caching;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Extensions;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using Microsoft.Framework.OptionsModel;
using Microsoft.Dnx.Runtime;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Logging;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Repositories.MSSQL;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Logging;
//using Autofac;
//using Autofac.Framework.DependencyInjection;

namespace example.WebApp
{
    public class Startup
    {
        

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //appEnv.
            //env.EnvironmentName = "Development";

            if (env.IsEnvironment("Development"))
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);

            // most common use of environment variables would be in azure hosting
            // since it is added last anything in env vars would trump the same setting in previous config sources
            // so no risk of messing up settings if deploying a new version to azure
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            //env.MapPath
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "AppResources");

            // we may need this on linux/mac as urls are case sensitive by default
            //services.Configure<RouteOptions>(routeOptions => routeOptions.LowercaseUrls = true);

            // Setup dependencies for cloudscribe Identity, Roles and and Site Administration
            // this is in Startup.CloudscribeCore.DI.MS.cs
            services.ConfigureCloudscribeCore(Configuration);
            

            services.Configure<MvcOptions>(options =>
            {
                // forces https
                // note that the home or root is still accessible non securely
                // only enable this if you have an ssl certificate installed and working
                //options.Filters.Add(new RequireHttpsAttribute());

                //options.ModelValidatorProviders.Add()

            });

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength
            //});


                // we are adding this from Startup.CloudscribeCore.cs so it is not needed here
                // Add MVC services to the services container.
                //services.AddMvc();

                // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
                // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
                // services.AddWebApiConventions();

                //Autofac config
                // var builder = new ContainerBuilder();

                ////Populate the container with services that were previously registered
                //// it seems this depends on beta4
                //builder.Populate(services);

                //var container = builder.Build();

                //return container.Resolve<IServiceProvider>();

            }

        // Configure is called after ConfigureServices is called.
        // you can change this method signature to include any dependencies that need to be injected into this method
        // you can see we added the dependency for IOptions<MultiTenantOptions>
        // so basically if you need any service in this method that was previously setup in ConfigureServices
        // you can just add it to the method signature and it will be provided by dependency injection
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IServiceProvider serviceProvider,
            ILogRepository logRepository)
        {
            // Configure the HTTP request pipeline.

            // LogLevels
            //Debug = 1,
            //Verbose = 2,
            //Information = 3,
            //Warning = 4,
            //Error = 5,
            //Critical = 6,
            loggerFactory.MinimumLevel = LogLevel.Information;
            // Add the console logger.
            loggerFactory.AddConsole();
            // Add cloudscribe db logging
            loggerFactory.AddDbLogger(serviceProvider, logRepository);

            //app.UseCultureReplacer();

            // localization from .resx files is not really working in beta8
            // will have to wait till next release

            var localizationOptions = new RequestLocalizationOptions
            {
                // Set options here to change middleware behavior
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("it")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("it")
                }
            };

            // Optionally create an app-specific provider with just a delegate, e.g. look up user preference from DB.
            // Inserting it as position 0 ensures it has priority over any of the default providers.
            //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            //{

            //}));

            app.UseRequestLocalization(localizationOptions);



            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                //app.UseBrowserLink();

                app.UseDeveloperExceptionPage();
                
                //app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
                //app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                //app.UseErrorHandler("/Home/Error");
                //app.UseErrorPage(ErrorPageOptions.ShowAll);

                // handle 404 and other non success
                //app.UseStatusCodePages();

                // app.UseStatusCodePages(context => context.HttpContext.Response.SendAsync("Handler, status code: " + context.HttpContext.Response.StatusCode, "text/plain"));
                // app.UseStatusCodePages("text/plain", "Response, status code: {0}");
                // app.UseStatusCodePagesWithRedirects("~/errors/{0}"); // PathBase relative
                // app.UseStatusCodePagesWithRedirects("/base/errors/{0}"); // Absolute
                // app.UseStatusCodePages(builder => builder.UseWelcomePage());
                //app.UseStatusCodePagesWithReExecute("/errors/{0}");
                //app.UseStatusCodePagesWithReExecute("/error/{0}");
            }

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            //app.UseRuntimeInfoPage("/info");

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // the only thing we are using session for is Alerts
            app.UseSession();
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(20));
            

            // this is in Startup.CloudscribeCore.cs
            app.UseCloudscribeCore(multiTenantOptions,Configuration);

            // it is very important that all authentication configuration be set before configuring mvc
            // ie if app.UseFacebookAuthentication(); was below app.UseMvc the facebook login button will not be shown

            
            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                // Understanding ASP.NET Routing:

                // it is very important that routes are registered in the correct order. more specific routes must be registered first and 
                // less specific routes must be registered later. a request may match more than one route.

                // When a request comes in it is compared to routes in the route table and the first route it matches is used no matter if a 
                // better match exists. therefore if a less specific route is registered first it will catch requests that would have a better 
                // match with a more specific route that was registered later.

                // ie the default route is usually the least specific route and must be registered last

                // something like a route for a cms would likely need to be the default route added last especially if you are not going to use 
                // a controller segment in the route because without a controller segment the route is less specific


                // default route for folder sites must be second to last
            if (multiTenantOptions.Value.Mode == MultiTenantMode.FolderName)
                {
                    routes.MapRoute(
                    name: "folderdefault",
                    template: "{sitefolder}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { name = new SiteFolderRouteConstraint() }
                    );
                }


                // the default route has to be added last
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }

                    );

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            })
            ;

            // https://github.com/aspnet/Announcements/issues/54
            // if you want to run the IIS pipeline for requests not handled 
            // ie you won't see the IIS 404 page without this.
            // if nothing else handles the 404 then you get a blank white page in the browser
            //app.RunIISPipeline();

            //app.Run(context =>
            //{
            //    context.Response.StatusCode = 404;
            //    return Task.FromResult(0);
            //});



        }
    }
}
