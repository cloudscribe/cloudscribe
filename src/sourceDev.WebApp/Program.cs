using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sourceDev.WebApp.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sourceDev.WebApp
{
    public class Program
    {
        // Changed to async Task<int> to avoid blocking .Wait() calls during startup (FR-4)
        // See: https://aka.ms/aspnet/async-guidance
        public static async Task<int> Main(string[] args) => await StartWebServerAsync(args);

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

        public static async Task<int> StartWebServerAsync(string[] args)
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
                        await EnsureDataStorageIsReady(config, services);
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

                await host.RunAsync();

                return 0;
            }
            catch(Exception ex)
            {
                //errors in appsettings.json would cause an exception here so let's at least display the exception!
                Console.WriteLine("Host terminated unexpectedly.");
                Console.WriteLine(ex.ToString());
                return -1;
            }
            //finally
            //{
            //    Log.CloseAndFlush();
            //}



        }


        // FR-4: Changed to async to avoid blocking .Wait() calls during database initialization
        // This prevents thread pool starvation and potential deadlocks during startup
        private static async Task EnsureDataStorageIsReady(IConfiguration config, IServiceProvider services)
        {
            var storage = config["DevOptions:DbPlatform"];

            switch (storage)
            {
                case "nodb":
                    await CoreNoDbStartup.InitializeDataAsync(services);

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

                    await CloudscribeIdentityServerIntegrationNoDbStorage.InitializeDatabaseAsync(
                        services,
                        sId,
                        IdServerClients.Get(),
                        IdServerResources.GetApiResources(),
                        IdServerResources.GetIdentityResources()
                        );

                    break;

                case "ef":
                default:

                    // this one is only needed if using cloudscribe Logging with EF as the logging storage

                    // Note - temporarily disabled as part of net10 upgrade because it throws whilst Logger still at net8 - jk
                    // await LoggingEFStartup.InitializeDatabaseAsync(services);

                    // this creates ensures the database is created and initial data
                    await CoreEFStartup.InitializeDatabaseAsync(services);

                    // query tool
                    await QueryToolStartup.InitializeDatabaseAsync(services);


                    //await KvpEFCoreStartup.InitializeDatabaseAsync(services);

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

                    await CloudscribeIdentityServerIntegrationEFCoreStorage.InitializeDatabaseAsync(
                        services,
                        siteId,
                        IdServerClients.Get(),
                        IdServerResources.GetApiResources(),
                        IdServerResources.GetIdentityResources()
                        );

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


    }
}
