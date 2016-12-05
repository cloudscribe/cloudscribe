using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using IdentityServer4.Models;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Core.Web.Components;

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
            
            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            
            // most common use of environment variables would be in azure hosting
            // since it is added last anything in env vars would trump the same setting in previous config sources
            // so no risk of messing up settings if deploying a new version to azure
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            
            environment = env;
            
        }

        private IHostingEnvironment environment;
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // since we are protecting some data such as social auth secrets in the db
            // we need our data protection keys to be located on disk where we can find them if 
            // we need to move to different hosting, without those key on the new host it would not be possible to decrypt
            // but it is perhaps a little risky storing these keys below the appRoot folder
            // for your own production envrionments store them outside of that if possible
            string pathToCryptoKeys = Path.Combine(environment.ContentRootPath, "dp_keys");
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(pathToCryptoKeys));

            // waiting for RTM compatible glimpse
            //bool enableGlimpse = Configuration.GetValue("DiagnosticOptions:EnableGlimpse", false);

            //if (enableGlimpse)
            //{
            //    services.AddGlimpse();
            //}

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.AddMemoryCache();
            // we currently only use session for alerts, so we can fire an alert on the next request
            // if session is disabled this feature fails quietly with no errors
            services.AddSession();
            
            // add authorization policies 
            ConfigureAuthPolicy(services);

            services.AddOptions();

            /* optional and only needed if you are using cloudscribe Logging  */
            //services.AddCloudscribeLoggingNoDbStorage(Configuration);
            services.AddCloudscribeLogging();

            /* these are optional and only needed if using cloudscribe Setup */
            //services.Configure<SetupOptions>(Configuration.GetSection("SetupOptions"));
            //services.AddScoped<SetupManager, SetupManager>();
            //services.AddScoped<IVersionProvider, SetupVersionProvider>();
            //services.AddScoped<IVersionProvider, CloudscribeLoggingVersionProvider>();
            /* end cloudscribe Setup */

            //services.AddSingleton<IThemeListBuilder, SharedThemeListBuilder>();
            services.AddCloudscribeCore(Configuration);
            
            
            services.Configure<GlobalResourceOptions>(Configuration.GetSection("GlobalResourceOptions"));
            services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();
            
            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("cy-GB"),
                    new CultureInfo("cy"),
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

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddRazorOptions(options =>
                    {
                        options.AddCloudscribeViewLocationFormats();

                        options.AddEmbeddedViewsForNavigation();
                        options.AddEmbeddedBootstrap3ViewsForCloudscribeCore();
                        options.AddEmbeddedViewsForCloudscribeLogging();

                        options.AddEmbeddedViewsForCloudscribeIdentityServerIntegration();

                        options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                        //options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SharedThemesViewLocationExpander());
                    })
                    ;

            ConfigureDataStorage(services);

           

            //var container = new Container();
            //container.Populate(services);

            //return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // you can add things to this method signature and they will be injected as long as they were registered during 
        // ConfigureServices
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IServiceProvider serviceProvider,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            ,cloudscribe.Logging.Web.ILogRepository logRepo
            )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            ConfigureLogging(loggerFactory, serviceProvider, logRepo);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            var storage = Configuration["DevOptions:DbPlatform"];
            
            switch (storage)
            {
                case "NoDb":
                    CoreNoDbStartup.InitializeDataAsync(app.ApplicationServices).Wait();

                    // you can use this hack to add clients and scopes into the db since
                    // there is currently no ui to do it
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
                        app.ApplicationServices,
                        sId,
                        GetClients(),
                        GetScopes()
                        ).Wait();

                    break;

                case "ef":
                default:
                    // this creates ensures the database is created and initial data
                    CoreEFStartup.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    // this one is only needed if using cloudscribe Logging with EF as the logging storage
                    LoggingEFStartup.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    // you can use this hack to add clients and scopes into the db since
                    // there is currently no ui to do it
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
                        app.ApplicationServices,
                        siteId,
                        GetClients(),
                        GetScopes()
                        ).Wait();
                    
                    break;
            }

            app.UseForwardedHeaders();

            app.UseStaticFiles();
            
            app.UseSession();
            
            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            app.UseMultitenancy<cloudscribe.Core.Models.SiteContext>();

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UsePerTenant<cloudscribe.Core.Models.SiteContext>((ctx, builder) =>
            {
                // custom 404 and error page - this preserves the status code (ie 404)
                if(string.IsNullOrEmpty(ctx.Tenant.SiteFolderName))
                {
                    builder.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                }
                else
                {
                    builder.UseStatusCodePagesWithReExecute("/" + ctx.Tenant.SiteFolderName + "/Home/Error/{0}");
                }
                

                builder.UseCloudscribeCoreDefaultAuthentication(
                    loggerFactory,
                    multiTenantOptions,
                    ctx.Tenant);

                // to make this multi tenant for folders
                // using a fork of IdentityServer4 and hoping to get changes so we don't need a fork
                // https://github.com/IdentityServer/IdentityServer4/issues/19

                builder.UseIdentityServer();

                // this sets up the authentication for apis within this application endpoint
                // ie apis that are hosted in the same web app endpoint with the authority server
                // this is not needed here if you are only using separate api endpoints
                // it is needed in the startup of those separate endpoints
                builder.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
                {
                    Authority = "https://localhost:44399",
                    // using the site aliasid as the scope so each tenant has a different scope
                    // you can view the aliasid from site settings
                    // clients must be configured with the scope to have access to the apis for the tenant
                    ApiName = ctx.Tenant.AliasId, 

                    RequireHttpsMetadata = true
                });


            });

            

            UseMvc(app, multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName);

            
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
                   name: "errorhandler",
                   template: "Home/Error/{statusCode}", 
                   defaults: new { controller = "Home", action = "Error" }
                   );

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
                options.AddCloudscribeCoreDefaultPolicies();
                options.AddCloudscribeLoggingDefaultPolicy();

                options.AddPolicy(
                    "IdentityServerAdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators");
                    });

            });

        }

        private void ConfigureDataStorage(IServiceCollection services)
        {
            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();
            
            var storage = Configuration["DevOptions:DbPlatform"];
            var efProvider = Configuration["DevOptions:EFProvider"];

            switch (storage)
            {
                case "NoDb":
                    services.AddCloudscribeCoreNoDbStorage();
                    services.AddCloudscribeLoggingNoDbStorage(Configuration);

                    services.AddIdentityServer()
                        .AddCloudscribeCoreNoDbIdentityServerStorage()
                        .AddCloudscribeIdentityServerIntegration()
                        .AddTemporarySigningCredential()
                        ;

                    break;

                case "ef":
                default:

                    switch(efProvider)
                    {
                        case "pgsql":
                            var pgConnection = Configuration.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);

                            services.AddIdentityServer()
                                .AddCloudscribeCoreEFIdentityServerStoragePostgreSql(pgConnection)
                                .AddCloudscribeIdentityServerIntegration()
                                .AddTemporarySigningCredential()
                                ;

                            break;

                        case "MySql":
                            var mysqlConnection = Configuration.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);

                            services.AddIdentityServer()
                                .AddCloudscribeCoreEFIdentityServerStorageMySql(mysqlConnection)
                                .AddCloudscribeIdentityServerIntegration()
                                .AddTemporarySigningCredential()
                                ;
                            
                            break;

                        case "MSSQL":
                        default:
                            var connectionString = Configuration.GetConnectionString("EntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMSSQL(connectionString);
                            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);

                            services.AddIdentityServer()
                                .AddCloudscribeCoreEFIdentityServerStorageMSSQL(connectionString)
                                .AddCloudscribeIdentityServerIntegration()
                                .AddTemporarySigningCredential()
                                ;

                            break;
                    }
                    

                    break;
            }
        }

        private void ConfigureLogging(
            ILoggerFactory loggerFactory, 
            IServiceProvider serviceProvider
            , cloudscribe.Logging.Web.ILogRepository logRepo
            )
        {
            
            // a customizable filter for logging
            LogLevel minimumLevel = LogLevel.Information;

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
            
            loggerFactory.AddDbLogger(serviceProvider, logFilter, logRepo);
        }

        // test data for identityserver integration

        // scopes define the resources in your system
        private IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,

                new Scope
                {
                    Name = "s2",
                    DisplayName = "Site 2 api access",
                    Description = "Site2 APIs"
                }
            };
        }

        private IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = new List<IdentityServer4.Models.Secret>
                    {
                        new IdentityServer4.Models.Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "s2"
                    }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "Resource Owner Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = new List<IdentityServer4.Models.Secret>
                    {
                        new IdentityServer4.Models.Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "s2"
                    }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                // this would be for letting a different mvc app authenticate
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets = new List<IdentityServer4.Models.Secret>
                    {
                        new IdentityServer4.Models.Secret("secret".Sha256())
                    },

                    RedirectUris = new List<string>
                    {
                        "https://localhost:44399/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:5002"
                    },

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.OfflineAccess.Name,
                        "s1"
                    }
                },

                // JavaScript Client
                // you can test this client at /app.html
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = new List<string>
                    {
                        "https://localhost:44399/callback.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44399/app.html"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:5003"
                    },

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        // this client is allowed access to apis of 2 tenants
                        "s1", // site 1 aliasid
                        "s2" //site 2 aliasid
                    }
                }
            };
        }

    }
}
