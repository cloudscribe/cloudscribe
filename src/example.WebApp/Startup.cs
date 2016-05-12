using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Models.Setup;
using cloudscribe.Core.Storage.EF;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Navigation;
using cloudscribe.Logging.EF;
using cloudscribe.Logging.Web;
using cloudscribe.Setup.Web;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using example.WebApp;
using Glimpse;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;


namespace example.WebApp
{
    public class Startup
    {        
        private string appBasePath;

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

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
            appBasePath = appEnv.ApplicationBasePath;
        }

        public IConfigurationRoot Configuration { get; set; }

        private void ConfigureAuthPolicy(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ServerAdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins");
                    });

                options.AddPolicy(
                    "CoreDataPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins");
                    });

                options.AddPolicy(
                    "AdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins", "Admins");
                    });

                options.AddPolicy(
                    "UserManagementPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins", "Admins");
                    });

                options.AddPolicy(
                    "RoleAdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Role Admins", "Admins");
                    });

                options.AddPolicy(
                    "SystemLogPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins");
                    });

                options.AddPolicy(
                    "SetupSystemPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins, Admins");
                    });

            });

        }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDataProtection(configure =>
            {
                string pathToCryptoKeys = appBasePath + Path.DirectorySeparatorChar + "dp_keys" + Path.DirectorySeparatorChar;
                configure.PersistKeysToFileSystem(new DirectoryInfo(pathToCryptoKeys));
            });
            
            bool enableGlimpse = Configuration.Get("DiagnosticOptions:EnableGlimpse", false);

            if (enableGlimpse)
            {
                services.AddGlimpse();
            }

            services.AddCaching();
            // we currently only use session for alerts, so we can fire an alert on the next request
            // if session is disabled this feature fails quietly with no errors
            services.AddSession();

            ConfigureAuthPolicy(services);

            services.AddOptions();

            /* optional and only needed if you are using cloudscribe Logging  */
            services.AddScoped<LogManager>();

            /* these are optional and only needed if using cloudscribe Setup */
            services.Configure<SetupOptions>(Configuration.GetSection("SetupOptions"));
            services.AddScoped<SetupManager, SetupManager>();
            services.AddScoped<IVersionProvider, CloudscribeCoreVersionProvider>();
            services.AddScoped<IVersionProviderFactory, VersionProviderFactory>();
            services.AddScoped<IVersionProvider, SetupVersionProvider>();
            services.AddScoped<IVersionProvider, CloudscribeLoggingVersionProvider>();
            /* end cloudscribe Setup */
            
            services.AddCloudscribeCore(Configuration);
            
            services.AddMvc()
                    .AddViewLocalization(options =>
                    {
                        options.ResourcesPath = "AppResources";
                    })
                    .AddRazorOptions(options =>
                    {
                        options.ViewLocationExpanders.Add(new SiteViewLocationExpander());
                    });
            
            ConfigureDatabase(services, Configuration);

            
            var container = new Container();
            container.Populate(services);
            
            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<IdentityOptions> identityOptionsAccessor,
            IServiceProvider serviceProvider,
            ILogRepository logRepository)
        {
            var enableGlimpse = Configuration.Get("DiagnosticOptions:EnableGlimpse", false);

            if (enableGlimpse)
            {
                app.UseGlimpse();
            }

            /* optional and only needed if you are using cloudscribe Logging  */
            ConfigureLogging(loggerFactory, serviceProvider, logRepository);
            
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {                
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler(options =>
            {
                options.AuthenticationDescriptions.Clear();
            });

            app.UseStaticFiles();

            app.UseSession();

            app.UseMultitenancy<SiteSettings>();

            //app.UseTenantContainers<SiteSettings>();
            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UsePerTenant<SiteSettings>((ctx, builder) =>
            {
                //var tenantIdentityOptionsProvider = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>();
                //var cookieOptions = tenantIdentityOptionsProvider.Value.Cookies;
                var cookieOptions = identityOptionsAccessor.Value.Cookies;


                var shouldUseFolder = !multiTenantOptions.UseRelatedSitesMode 
                                        && multiTenantOptions.Mode == MultiTenantMode.FolderName 
                                        && ctx.Tenant.SiteFolderName.Length > 0;

                builder.UseCookieAuthentication(cookieOptions.ExternalCookie);
                builder.UseCookieAuthentication(cookieOptions.TwoFactorRememberMeCookie);
                builder.UseCookieAuthentication(cookieOptions.TwoFactorUserIdCookie);
                builder.UseCookieAuthentication(cookieOptions.ApplicationCookie);

                builder.UseSocialAuth(ctx.Tenant, cookieOptions, shouldUseFolder);
            });

            UseMvc(app, multiTenantOptions.Mode == MultiTenantMode.FolderName);

            var devOptions = Configuration.Get<DevOptions>("DevOptions");
            if (devOptions.DbPlatform == "ef7")
            {
                // this creates ensures the database is created and initial data
                StartupDataUtils.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                // this one is only needed if using cloudscribe Logging with EF as the logging storage
                LoggingDbInitializer.InitializeDatabaseAsync(app.ApplicationServices).Wait();
            }
        }
        
        private void UseMvc(IApplicationBuilder app, bool useFolders)
        {
            app.UseMvc(routes =>
            {
                if(useFolders)
                {
                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new SiteFolderRouteConstraint() });
                }
                
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
        
        private void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISetupTask, EnsureInitialDataSetupTask>();

            var devOptions = configuration.Get<DevOptions>("DevOptions");

            switch (devOptions.DbPlatform)
            {
                case "ef7":
                    var connectionString = configuration["Data:EF7ConnectionOptions:ConnectionString"];
                    services.AddCloudscribeCoreEFStorage(connectionString);

                    // only needed if using cloudscribe logging with EF storage
                    services.AddCloudscribeLoggingEFStorage(connectionString);
                    

                    break;
            }
        }

        private void ConfigureLogging(ILoggerFactory loggerFactory, IServiceProvider serviceProvider, ILogRepository logRepository)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);

            // a customizable filter for logging
            LogLevel minimumLevel = LogLevel.Warning;

            // add exclusions to remove noise in the logs
            var excludedLoggers = new List<string>
            {
                "Microsoft.Data.Entity.Storage.Internal.RelationalCommandBuilderFactory",
                "Microsoft.Data.Entity.Query.Internal.QueryCompiler",
                "Microsoft.Data.Entity.DbContext",
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

            loggerFactory.AddDbLogger(serviceProvider, logRepository, logFilter);
        }


        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
