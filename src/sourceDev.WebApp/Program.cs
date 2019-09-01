using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sourceDev.WebApp.Configuration;
using System;
using System.Linq;

namespace sourceDev.WebApp
{
    public class Program
    {
        public static int Main(string[] args) => StartWebServer(args);

        //public static void Main(string[] args)
        //{
        //    var hostBuilder = CreateWebHostBuilder(args);
        //    var host = hostBuilder.Build();

        //    var config = host.Services.GetRequiredService<IConfiguration>();

        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;

        //        try
        //        {
        //            EnsureDataStorageIsReady(config, services);

        //        }
        //        catch (Exception ex)
        //        {
        //            var logger = services.GetRequiredService<ILogger<Program>>();
        //            logger.LogError(ex, "An error occurred while migrating the database.");
        //        }
        //    }

        //    var env = host.Services.GetRequiredService<IHostingEnvironment>();
        //    var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
        //    ConfigureLogging(env, loggerFactory, host.Services, config);

        //    host.Run();
        //}

        public static int StartWebServer(string[] args)
        {

            //Log.Logger =
            //new LoggerConfiguration()
            //    .MinimumLevel.Warning()
            //    .Enrich.WithProperty("Application", "MyApplicationName")
            //    .WriteTo.Console()
            //    .CreateLogger();
            try
            {
                var hostBuilder = CreateHostBuilder(args);
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

                var env = host.Services.GetRequiredService<IWebHostEnvironment>();
                var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
                ConfigureLogging(env, loggerFactory, host.Services, config);

                host.Run();

                return 0;
            }
            catch(Exception ex)
            {
                //Log.Fatal(ex, "Host terminated unexpectedly.");
                return -1;
            }
            //finally
            //{
            //    Log.CloseAndFlush();
            //}



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

                    // this one is only needed if using cloudscribe Logging with EF as the logging storage
                    LoggingEFStartup.InitializeDatabaseAsync(services).Wait();

                    // this creates ensures the database is created and initial data
                    CoreEFStartup.InitializeDatabaseAsync(services).Wait();

                    

                    //KvpEFCoreStartup.InitializeDatabaseAsync(services).Wait();

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
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
            IConfiguration config
            )
        {
            var dbLoggerConfig = config.GetSection("DbLoggerConfig").Get<cloudscribe.Logging.Models.DbLoggerConfig>();
            LogLevel minimumLevel;
            string levelConfig;
            if (env.IsProduction())
            {
                levelConfig = dbLoggerConfig.ProductionLogLevel;
            }
            else
            {
                levelConfig = dbLoggerConfig.DevLogLevel;
            }
            switch (levelConfig)
            {
                case "Debug":
                    minimumLevel = LogLevel.Debug;
                    break;

                case "Information":
                    minimumLevel = LogLevel.Information;
                    break;

                case "Trace":
                    minimumLevel = LogLevel.Trace;
                    break;

                default:
                    minimumLevel = LogLevel.Warning;
                    break;
            }

            // a customizable filter for logging
            // add exclusions in appsettings.json to remove noise in the logs
            bool logFilter(string loggerName, LogLevel logLevel)
            {
                if (dbLoggerConfig.ExcludedNamesSpaces.Any(f => loggerName.StartsWith(f)))
                {
                    return false;
                }

                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (dbLoggerConfig.BelowWarningExcludedNamesSpaces.Any(f => loggerName.StartsWith(f)) && logLevel < LogLevel.Warning)
                {
                    return false;
                }
                return true;
            }

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        //https://joonasw.net/view/aspnet-core-2-configuration-changes

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //    .ConfigureAppConfiguration((builderContext, config) =>
        //    {
        //        //config.AddJsonFile("app-userproperties.json", optional: true, reloadOnChange: true);
        //    })
        //    .UseStartup<Startup>()
        //    //.ConfigureKestrel((context, options) =>
        //    //{
        //    //    // Set properties and call methods on options
        //    //})
        //        ;


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
