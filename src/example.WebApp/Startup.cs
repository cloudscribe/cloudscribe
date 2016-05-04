using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Models.Setup;
using cloudscribe.Core.Repositories.EF;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Editor;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.Navigation;
using cloudscribe.Logging.EF;
using cloudscribe.Logging.Web;
using cloudscribe.Setup.Web;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using cloudscribe.Web.Pagination;
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
using InitialData = cloudscribe.Core.Repositories.EF.InitialData;

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

            ConfigureAuthPolicy(services);

            ConfigureCloudscribeCore(services, Configuration);

            services.AddScoped<LogManager>();

            var container = new Container();

            container.Populate(services);

            container.ConfigureTenants<SiteSettings>(c =>
            {
            });

            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IServiceProvider serviceProvider,
            ILogRepository logRepository)
        {
            var enableGlimpse = Configuration.Get("DiagnosticOptions:EnableGlimpse", false);

            if (enableGlimpse)
            {
                app.UseGlimpse();
            }

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

            app.UseTenantContainers<SiteSettings>();

            app.UsePerTenant<SiteSettings>((ctx, builder) =>
            {
                var tenantIdentityOptionsProvider = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>();
                var cookieOptions = tenantIdentityOptionsProvider.Value.Cookies;

                builder.UseCookieAuthentication(cookieOptions.ExternalCookie);
                builder.UseCookieAuthentication(cookieOptions.TwoFactorRememberMeCookie);
                builder.UseCookieAuthentication(cookieOptions.TwoFactorUserIdCookie);
                builder.UseCookieAuthentication(cookieOptions.ApplicationCookie);
            });

            AddMvc(app);

            var devOptions = Configuration.Get<DevOptions>("DevOptions");
            if (devOptions.DbPlatform == "ef7")
            {
                InitialData.InitializeDatabaseAsync(app.ApplicationServices).Wait();
                DbInitializer.InitializeDatabaseAsync(app.ApplicationServices).Wait();
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
              
        private void AddSocialAuth(
            IApplicationBuilder app, 
            SiteSettings site, 
            IdentityOptions identityOptions,
            bool shouldUseFolder)
        {
            // TODO: will this require a restart if the options are updated in the ui?
            // no just need to clear the tenant cache after updating the settings
            if (!string.IsNullOrWhiteSpace(site.GoogleClientId))
            {
                app.UseGoogleAuthentication(options =>
                {
                    options.AuthenticationScheme = "Google";
                    options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;

                    options.ClientId = site.GoogleClientId;
                    options.ClientSecret = site.GoogleClientSecret;

                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-google";
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(site.FacebookAppId))
            {
                app.UseFacebookAuthentication(options =>
                {
                    options.AuthenticationScheme = "Facebook";
                    options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;
                    options.AppId = site.FacebookAppId;
                    options.AppSecret = site.FacebookAppSecret;

                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-facebook";
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(site.MicrosoftClientId))
            {
                app.UseMicrosoftAccountAuthentication(options =>
                {
                    options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;
                    options.ClientId = site.MicrosoftClientId;
                    options.ClientSecret = site.MicrosoftClientSecret;
                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-microsoft";
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(site.TwitterConsumerKey))
            {
                app.UseTwitterAuthentication(options =>
                {
                    options.SignInScheme = identityOptions.Cookies.ExternalCookie.AuthenticationScheme;
                    options.ConsumerKey = site.TwitterConsumerKey;
                    options.ConsumerSecret = site.TwitterConsumerSecret;

                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-twitter";
                    }
                });
            }
        }

        private void AddMvc(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "folderdefault",
                    template: "{sitefolder}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { name = new SiteFolderRouteConstraint() });
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        public IServiceCollection ConfigureCloudscribeCore(IServiceCollection services, IConfigurationRoot configuration)
        {
            ConfigureOptions(services, configuration);

            ConfigureDatabase(services, configuration);

            services.AddCaching();

            services.AddSession();

            services.AddScoped<ITimeZoneResolver, RequestTimeZoneResolver>();

            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();

            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsProvider>();

            services.AddScoped<SiteManager, SiteManager>();
            services.AddScoped<SetupManager, SetupManager>();
            services.AddScoped<GeoDataManager, GeoDataManager>();
            services.AddScoped<SystemInfoManager, SystemInfoManager>();
            services.AddScoped<IpAddressTracker, IpAddressTracker>();

            services.AddScoped<SiteDataProtector>();

            services.AddScoped<IVersionProvider, SetupVersionProvider>();
            services.AddScoped<IVersionProvider, CloudscribeCoreVersionProvider>();
            services.AddScoped<IVersionProvider, CloudscribeLoggingVersionProvider>();
            services.AddScoped<IVersionProviderFactory, VersionProviderFactory>();

            services.AddIdentity<SiteUser, SiteRole>()
                .AddUserStore<UserStore<SiteUser>>()
                .AddRoleStore<RoleStore<SiteRole>>()
                .AddUserManager<SiteUserManager<SiteUser>>()
                .AddRoleManager<SiteRoleManager<SiteRole>>();

            AddCloudscribeIdentity<SiteUser, SiteRole>(services);

            AddCloudscribeNavigation(services);

            services.AddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();

            AddCloudscribeMessaging(services);

            services.AddSingleton<IThemeListBuilder, SiteThemeListBuilder>();

            #region MVC
            {
                services.AddSingleton<IRazorViewEngine, CoreViewEngine>();

                services
                    .AddMvc()
                    .AddViewLocalization(options =>
                    {
                        options.ResourcesPath = "AppResources";
                    })
                    .AddRazorOptions(options =>
                    {
                        options.ViewLocationExpanders.Add(new SiteViewLocationExpander());
                    });
            }
            #endregion

            services.AddSingleton<IAntiforgeryTokenStore, SiteAntiforgeryTokenStore>();

            return services;
        }

        public IServiceCollection AddCloudscribeIdentity<TUser, TRole>(IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsProvider>();

            services.AddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            services.AddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();
            services.AddScoped<SiteSignInManager<SiteUser>, SiteSignInManager<SiteUser>>();
            services.AddScoped<SiteAuthCookieValidator, SiteAuthCookieValidator>();
            services.AddScoped<SiteCookieAuthenticationEvents, SiteCookieAuthenticationEvents>();

            return services;
        }

        private void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<MultiTenantOptions>(configuration.GetSection("MultiTenantOptions"));
            services.Configure<SiteConfigOptions>(configuration.GetSection("SiteConfigOptions"));
            services.Configure<SetupOptions>(configuration.GetSection("SetupOptions"));
            services.Configure<UIOptions>(configuration.GetSection("UIOptions"));
            services.Configure<CkeditorOptions>(configuration.GetSection("CkeditorOptions"));
            services.Configure<NavigationOptions>(configuration.GetSection("NavigationOptions"));
        }

        private void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISetupTask, EnsureInitialDataSetupTask>();

            var devOptions = configuration.Get<DevOptions>("DevOptions");

            switch (devOptions.DbPlatform)
            {
                case "ef7":

                    services
                        .AddEntityFramework()
                        .AddSqlServer()
                        .AddDbContext<CoreDbContext>(options =>
                        {
                            options.UseSqlServer(configuration["Data:EF7ConnectionOptions:ConnectionString"]);
                        })
                        .AddDbContext<LoggingDbContext>(options =>
                        {
                            options.UseSqlServer(configuration["Data:EF7ConnectionOptions:ConnectionString"]);
                        });

                    services.AddScoped<ICoreModelMapper, SqlServerCoreModelMapper>();
                    services.AddScoped<ILogModelMapper, SqlServerLogModelMapper>();

                    services.AddScoped<ISiteRepository, SiteRepository>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IGeoRepository, GeoRepository>();
                    services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
                    services.AddScoped<ILogRepository, LogRepository>();

                    break;
            }
        }

        private void AddCloudscribeNavigation(IServiceCollection services)
        {
            services.AddScoped<ITreeCache, MemoryTreeCache>();
            services.AddScoped<INavigationTreeBuilder, XmlNavigationTreeBuilder>();
            services.AddScoped<NavigationTreeBuilderService, NavigationTreeBuilderService>();
            services.AddScoped<INodeUrlPrefixProvider, FolderTenantNodeUrlPrefixProvider>();
            services.AddScoped<INavigationNodePermissionResolver, NavigationNodePermissionResolver>();
        }

        private void AddCloudscribeMessaging(IServiceCollection services)
        {
            services.AddTransient<IEmailTemplateService, HardCodedEmailTemplateService>();
            services.AddTransient<ISiteMessageEmailSender, SiteEmailMessageSender>();
            services.AddTransient<ISmsSender, SiteSmsSender>();
        }
    }
}
