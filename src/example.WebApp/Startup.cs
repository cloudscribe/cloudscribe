using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

namespace example.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            appBasePath = env.ContentRootPath;

            if (env.IsDevelopment())
            {
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
        }

        private string appBasePath;
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // since we are protecting some data such as social auth secrets in the db
            // we need our data protection keys to be located on disk where we can find them if 
            // we need to move to different hosting, without those key on the new host it would not be possible to decrypt
            // but it is perhaps a little risky storing these keys below the appRoot folder
            // for your own production envrionments store them outside of that if possible
            string pathToCryptoKeys = appBasePath + System.IO.Path.DirectorySeparatorChar + "dp_keys" + System.IO.Path.DirectorySeparatorChar;
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(pathToCryptoKeys));
            



            // waiting for rc2 compatible glimpse
            //bool enableGlimpse = Configuration.GetValue("DiagnosticOptions:EnableGlimpse", false);

            //if (enableGlimpse)
            //{
            //    services.AddGlimpse();
            //}

            services.AddMemoryCache();
            // we currently only use session for alerts, so we can fire an alert on the next request
            // if session is disabled this feature fails quietly with no errors
            services.AddSession();
            
            // add authorization policies 
            ConfigureAuthPolicy(services);

            services.AddOptions();

            /* optional and only needed if you are using cloudscribe Logging  */
            services.AddScoped<cloudscribe.Logging.Web.LogManager>();

            /* these are optional and only needed if using cloudscribe Setup */
            //services.Configure<SetupOptions>(Configuration.GetSection("SetupOptions"));
            //services.AddScoped<SetupManager, SetupManager>();
            //services.AddScoped<IVersionProvider, SetupVersionProvider>();
            //services.AddScoped<IVersionProvider, CloudscribeLoggingVersionProvider>();
            /* end cloudscribe Setup */
            
            services.AddCloudscribeCore(Configuration);

            services.AddCloudscribeIdentity(options => {

                options.Cookies.ApplicationCookie.AuthenticationScheme 
                    = cloudscribe.Core.Identity.AuthenticationScheme.Application;
                
                options.Cookies.ApplicationCookie.CookieName 
                    = cloudscribe.Core.Identity.AuthenticationScheme.Application;

                //options.Cookies.ApplicationCookie.DataProtectionProvider = 
                //DataProtectionProvider.Create(new DirectoryInfo("C:\\Github\\Identity\\artifacts"));
            });

            services.Configure<GlobalResourceOptions>(Configuration.GetSection("GlobalResourceOptions"));
            services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();
            
            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("fr"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //  // My custom request culture logic
                //  return new ProviderCultureResult("en");
                //}));
            });

            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddRazorOptions(options =>
                    {
                        options.AddEmbeddedViewsForNavigation();
                        options.AddEmbeddedViewsForCloudscribeCore();
                        options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                    });

            ConfigureDataStorage(services);

            //var container = new Container();
            //container.Populate(services);

            //return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IServiceProvider serviceProvider
            )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            var storage = Configuration["DevOptions:DbPlatform"];
            if(storage != "NoDb")
            {   
                ConfigureLogging(loggerFactory, serviceProvider);
            }
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseMultitenancy<cloudscribe.Core.Models.SiteSettings>();

            //app.UseTenantContainers<SiteSettings>();
            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UsePerTenant<cloudscribe.Core.Models.SiteSettings>((ctx, builder) =>
            {
                var tenant = ctx.Tenant;

                var shouldUseFolder = !multiTenantOptions.UseRelatedSitesMode
                                        && multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && tenant.SiteFolderName.Length > 0;
                
                var externalCookieOptions = SetupOtherCookies(cloudscribe.Core.Identity.AuthenticationScheme.External, tenant);
                builder.UseCookieAuthentication(externalCookieOptions);

                var twoFactorRememberMeCookieOptions = SetupOtherCookies(cloudscribe.Core.Identity.AuthenticationScheme.TwoFactorRememberMe, tenant);
                builder.UseCookieAuthentication(twoFactorRememberMeCookieOptions);

                var twoFactorUserIdCookie = SetupOtherCookies(cloudscribe.Core.Identity.AuthenticationScheme.TwoFactorUserId, tenant);
                builder.UseCookieAuthentication(twoFactorUserIdCookie);

                var cookieEvents = new CookieAuthenticationEvents();
                var logger = loggerFactory.CreateLogger<cloudscribe.Core.Identity.SiteAuthCookieValidator>();
                var cookieValidator = new cloudscribe.Core.Identity.SiteAuthCookieValidator(logger);
                var appCookieOptions = SetupAppCookie(
                    cookieEvents,
                    cookieValidator,
                    cloudscribe.Core.Identity.AuthenticationScheme.Application,
                    tenant
                    );
                builder.UseCookieAuthentication(appCookieOptions);

                // known issue here is if a site is updated to populate the
                // social auth keys, it currently requires a restart so that the middleware gets registered
                // in order for it to work or for the social auth buttons to appear 
                builder.UseSocialAuth(ctx.Tenant, externalCookieOptions, shouldUseFolder);

            });


            UseMvc(app, multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName);
            
            

            switch (storage)
            {
                case "NoDb":
                    CoreNoDbStartup.InitializeDataAsync(app.ApplicationServices).Wait();
                    break;

                case "ef":
                default:
                    // this creates ensures the database is created and initial data
                    CoreEFStartup.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    // this one is only needed if using cloudscribe Logging with EF as the logging storage
                    LoggingDbInitializer.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    break;
            }

           
        }

        private CookieAuthenticationOptions SetupAppCookie(
           // CookieAuthenticationOptions options,
            CookieAuthenticationEvents cookieEvents,
            cloudscribe.Core.Identity.SiteAuthCookieValidator siteValidator,
            string scheme, 
            cloudscribe.Core.Models.SiteSettings tenant
            )
        {
            var options = new CookieAuthenticationOptions();
            options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
            options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
            options.CookiePath = "/" + tenant.SiteFolderName;

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";

            cookieEvents.OnValidatePrincipal = siteValidator.ValidatePrincipal;
            options.Events = cookieEvents;

            options.AutomaticAuthenticate = true;
            options.AutomaticChallenge = true;

            return options;
        }

        private CookieAuthenticationOptions SetupOtherCookies(
            //CookieAuthenticationOptions options, 
            string scheme, 
            cloudscribe.Core.Models.SiteSettings tenant
            )
        {
            var options = new CookieAuthenticationOptions();
            //var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
            //    ? PathString.Empty
            //    : new PathString("/" + tenant.SiteFolderName);

            options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
            options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
            options.CookiePath = "/" + tenant.SiteFolderName;

            return options;

        }

        private void UseMvc(IApplicationBuilder app, bool useFolders)
        {
            app.UseMvc(routes =>
            {
                if (useFolders)
                {
                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() });
                }


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    //,defaults: new { controller = "Home", action = "Index" }
                    );
            });
        }


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
                        authBuilder.RequireRole("ServerAdmins", "Administrators");
                    });

                options.AddPolicy(
                    "UserManagementPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins", "Administrators");
                    });

                options.AddPolicy(
                    "RoleAdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Role Administrators", "Administrators");
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
                        authBuilder.RequireRole("ServerAdmins, Administrators");
                    });

            });

        }

        private void ConfigureDataStorage(IServiceCollection services)
        {
            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();
            
            var storage = Configuration["DevOptions:DbPlatform"];

            switch (storage)
            {
                case "NoDb":
                    services.AddCloudscribeCoreNoDbStorage();
                    break;

                case "ef":
                default:
                    var connectionString = Configuration.GetConnectionString("EntityFrameworkConnectionString");
                    services.AddCloudscribeCoreEFStorage(connectionString);

                    // only needed if using cloudscribe logging with EF storage
                    services.AddCloudscribeLoggingEFStorage(connectionString);


                    break;
            }
        }

        private void ConfigureLogging(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            //var logRepository = serviceProvider.GetService<cloudscribe.Logging.Web.ILogRepository>();

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
            
            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }
    }
}
