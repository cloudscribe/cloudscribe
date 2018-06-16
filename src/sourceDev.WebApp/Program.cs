using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using sourceDev.WebApp.Configuration;

namespace sourceDev.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateWebHostBuilder(args);
            var host = hostBuilder.Build();

            var config = host.Services.GetRequiredService<IConfiguration>();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                try
                {
                    EnsureDataStorageIsReady(config, services);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            var env = host.Services.GetRequiredService<IHostingEnvironment>();
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            ConfigureLogging(env, loggerFactory, host.Services);

            host.Run();
        }

        private static void EnsureDataStorageIsReady(IConfiguration config, IServiceProvider services)
        {
            var storage = config["DevOptions:DbPlatform"];

            switch (storage)
            {
                case "NoDb":
                    CoreNoDbStartup.InitializeDataAsync(services).Wait();

                    // you can use this hack to add clients and scopes into the db during startup if needed
                    // I used this before we implemented the UI for adding them
                    // you should not use this on the first run that actually creates the initial cloudscribe data
                    // you must wait until after that and then you can get the needed siteid from the database
                    // this will only run at startup time and only add data if no data exists for the given site.
                    // if you pass in an invalid siteid it will not fail, you will get data with a bad siteid
                    // make note of your siteid, don't use these, these are from my NoDb storage
                    // site1 05301194-da1d-43a8-9aa4-6c5f8959f37b
                    // site2 a9e2c249-90b4-4770-9e99-9702d89f73b6
                    // replace null with your siteid and run the app, then change it back to null since it can only be a one time task
                    string sId = null;

                    CloudscribeIdentityServerIntegrationNoDbStorage.InitializeDatabaseAsync(
                        services,
                        sId,
                        IdServerClients.Get(),
                        IdServerResources.GetApiResources(),
                        IdServerResources.GetIdentityResources()
                        ).Wait();

                    break;

                case "ef":
                default:
                    // this creates ensures the database is created and initial data
                    CoreEFStartup.InitializeDatabaseAsync(services).Wait();

                    // this one is only needed if using cloudscribe Logging with EF as the logging storage
                    LoggingEFStartup.InitializeDatabaseAsync(services).Wait();

                    KvpEFCoreStartup.InitializeDatabaseAsync(services).Wait();

                    // you can use this hack to add clients and scopes into the db during startup if needed
                    // I used this before we implemented the UI for adding them
                    // you should not use this on the first run that actually creates the initial cloudscribe data
                    // you must wait until after that and then you can get the needed siteid from the database
                    // this will only run at startup time and only add data if no data exists for the given site.
                    // if you pass in an invalid siteid it will not fail, you will get data with a bad siteid
                    // make note of your siteid, don't use these, these are from my db
                    // site1 8f54733c-3f3a-4971-bb1f-8950cea42f1a
                    // site2 7c111db3-e270-497a-9a12-aed436c764c6
                    // replace null with your siteid and run the app, then change it back to null since it can only be a one time task
                    string siteId = null;

                    CloudscribeIdentityServerIntegrationEFCoreStorage.InitializeDatabaseAsync(
                        services,
                        siteId,
                        IdServerClients.Get(),
                        IdServerResources.GetApiResources(),
                        IdServerResources.GetIdentityResources()
                        ).Wait();

                    break;
            }
        }

        private static void ConfigureLogging(
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
            LogLevel minimumLevel;
            if (env.IsProduction())
            {
                minimumLevel = LogLevel.Warning;
            }
            else
            {
                minimumLevel = LogLevel.Information;
            }

            //minimumLevel = LogLevel.Debug;

            // a customizable filter for logging
            // add exclusions to remove noise in the logs
            var excludedLoggers = new List<string>
            {
                "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
                "Microsoft.AspNetCore.Hosting.Internal.WebHost",
            };

            Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
            {
                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (excludedLoggers.Contains(loggerName))
                {
                    return false;
                }

                return true;
            };

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }


        //https://joonasw.net/view/aspnet-core-2-configuration-changes

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddJsonFile("app-userproperties.json", optional: true, reloadOnChange: true);
            })
                .UseStartup<Startup>()
                ;


        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();


        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    return new WebHostBuilder()
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .ConfigureAppConfiguration((hostingContext, config) =>
        //        {
        //            var env = hostingContext.HostingEnvironment;

        //            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //                    .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
        //                    ;


        //            if (env.IsDevelopment())
        //            {
        //                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
        //                if (appAssembly != null)
        //                {
        //                    config.AddUserSecrets(appAssembly, optional: true);
        //                }
        //            }

        //            config.AddEnvironmentVariables();

        //            if (args != null)
        //            {
        //                config.AddCommandLine(args);
        //            }
        //        })
        //        .ConfigureLogging((hostingContext, logging) =>
        //        {
        //            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //            logging.AddConsole();
        //            logging.AddDebug();
        //        })
        //        .UseIISIntegration()
        //        .UseDefaultServiceProvider((context, options) =>
        //        {
        //            options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        //        })
        //        .UseStartup<Startup>()
        //        .Build();
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //         .ConfigureAppConfiguration((builderContext, config) =>
        //         {
        //             config.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true);
        //         })
        //        .UseStartup<Startup>()
        //        .Build();

    }
}
