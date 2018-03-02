using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using sourceDev.WebApp.Configuration;
using cloudscribe.UserProperties.Services;
using cloudscribe.UserProperties.Models;
using IdentityServer4.AccessTokenValidation;

namespace sourceDev.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

            DisableIdentityServer = Configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            IdentityServerX509CertificateThumbprintName = Configuration.GetValue<string>("AppSettings:IdentityServerX509CertificateThumbprintName");
            if (!DisableIdentityServer && Environment.IsProduction())
            {
                if (string.IsNullOrEmpty(IdentityServerX509CertificateThumbprintName))
                {
                    DisableIdentityServer = true;
                }
            }
        }

        private IHostingEnvironment Environment;
        public IConfiguration Configuration { get; }

        public bool SslIsAvailable { get; set; }
        public bool DisableIdentityServer { get; set; }
        public string IdentityServerX509CertificateThumbprintName { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // since we are protecting some data such as social auth secrets in the db
            // we need our data protection keys to be located on disk where we can find them if 
            // we need to move to different hosting, without those key on the new host it would not be possible to decrypt
            // but it is perhaps a little risky storing these keys below the appRoot folder
            // for your own production envrionments store them outside of that if possible
            string pathToCryptoKeys = Path.Combine(Environment.ContentRootPath, "dp_keys");
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

            services.Configure<ProfilePropertySetContainer>(Configuration.GetSection("ProfilePropertySetContainer"));
            //services.AddScoped<TenantProfileOptionsResolver>();

            //services.AddSession();

            // add authorization policies 
            ConfigureAuthPolicy(services);

            services.AddOptions();
            
            services.AddCloudscribeKvpUserProperties();

            
            AddDataStorageServices(services);

            /* optional and only needed if you are using cloudscribe Logging  */
            services.AddCloudscribeLogging();

            /* these are optional and only needed if using cloudscribe Setup */
            //services.Configure<SetupOptions>(Configuration.GetSection("SetupOptions"));
            //services.AddScoped<SetupManager, SetupManager>();
            //services.AddScoped<IVersionProvider, SetupVersionProvider>();
            //services.AddScoped<IVersionProvider, CloudscribeLoggingVersionProvider>();
            /* end cloudscribe Setup */

            //services.AddScoped<cloudscribe.Core.Web.ExtensionPoints.IHandleCustomRegistration, sourceDev.WebApp.Components.CustomRegistrationHandler>();

            
            //services.AddCloudscribeCore(Configuration);
            services.AddCloudscribeCoreMvc(Configuration);

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

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    //policy.WithOrigins("http://localhost:5010", "http://localhost:5011")
                    //    .AllowAnyHeader()
                    //    .AllowAnyMethod();
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            // for production be sure to use ssl
            SslIsAvailable = Configuration.GetValue<bool>("AppSettings:UseSsl");
            if (SslIsAvailable)
            {
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                });
            }


            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddRazorOptions(options =>
                    {
                        options.AddCloudscribeViewLocationFormats();

                        options.AddCloudscribeCommonEmbeddedViews();
                        options.AddCloudscribeNavigationBootstrap3Views();
                        options.AddCloudscribeCoreBootstrap3Views();
                        options.AddCloudscribeFileManagerBootstrap3Views();
                        options.AddCloudscribeLoggingBootstrap3Views();

                        options.AddCloudscribeCoreIdentityServerIntegrationBootstrap3Views();

                        options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                        
                    })
                    ;



            services.AddAuthentication()
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:44399";

                    options.ApiName = "idserverapi";
                    options.ApiSecret = "secret";
                });



            //services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            //AddDataStorageServices(services);



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
            )
        {   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/oops/Error");
            }
            
            app.UseForwardedHeaders();

            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = GzipMappingFileProvider.OnPrepareResponse,
                FileProvider = new GzipMappingFileProvider(
                    loggerFactory,
                    true,
                    Environment.WebRootFileProvider
                    )
            });

            // we don't need session
            //app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            // this uses the policy called "default"
            app.UseCors("default");
            
            var multiTenantOptions = multiTenantOptionsAccessor.Value;
            
            app.UseCloudscribeCore(
                    loggerFactory,
                    multiTenantOptions,
                    SslIsAvailable
                    );

            if (!DisableIdentityServer)
            {
                app.UseIdentityServer();
            }
                
            UseMvc(app, multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName);


        }

        private void UseMvc(IApplicationBuilder app, bool useFolders)
        {
            app.UseMvc(routes =>
            {
                routes.AddCloudscribeFileManagerRoutes();

                if (useFolders)
                {
                    routes.MapRoute(
                       name: "foldererrorhandler",
                       template: "{sitefolder}/oops/error/{statusCode?}",
                       defaults: new { controller = "Oops", action = "Error" },
                       constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );


                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() });


                }

                routes.MapRoute(
                   name: "stateautosuggest",
                   template: "coredata/stateautosuggestjson",
                   defaults: new { controller = "CoreDaata", action = "StateAutoSuggestJson" }
                   );

                routes.MapRoute(
                   name: "errorhandler",
                   template: "oops/error/{statusCode?}",
                   defaults: new { controller = "Oops", action = "Error" }
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

                options.AddPolicy(
                    "FileManagerPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators", "Content Administrators");
                    });

                options.AddPolicy(
                    "FileManagerDeletePolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators", "Content Administrators");
                    });

                options.AddPolicy(
                    "FakePolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("fake"); // no user has this role this policy is for verifying it fails
                    });

            });

        }

        private void AddDataStorageServices(IServiceCollection services)
        {
            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = Configuration["DevOptions:DbPlatform"];
            var efProvider = Configuration["DevOptions:EFProvider"];

            switch (storage)
            {
                case "NoDb":
                    services.AddCloudscribeCoreNoDbStorage();
                    services.AddCloudscribeLoggingNoDbStorage(Configuration);
                    services.AddCloudscribeKvpNoDbStorage();

                    if (!DisableIdentityServer)
                    {
                        services.AddIdentityServerConfiguredForCloudscribe()
                            .AddCloudscribeCoreNoDbIdentityServerStorage()
                            .AddCloudscribeIdentityServerIntegrationMvc()
                            .AddDeveloperSigningCredential()
                            ;
                    }
                        

                    break;

                case "ef":
                default:

                    switch (efProvider)
                    {
                        case "sqlite":
                            var slConnection = Configuration.GetConnectionString("SQLiteEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            services.AddCloudscribeKvpEFStorageSQLite(slConnection);

                            if (!DisableIdentityServer)
                            {
                                services.AddIdentityServerConfiguredForCloudscribe()
                                    .AddCloudscribeCoreEFIdentityServerStorageSQLite(slConnection)
                                    .AddCloudscribeIdentityServerIntegrationMvc()
                                    .AddDeveloperSigningCredential()
                                ;
                            }



                            break;

                        case "pgsql":
                            var pgConnection = Configuration.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeKvpEFStoragePostgreSql(pgConnection);

                            if (!DisableIdentityServer)
                            {
                                services.AddIdentityServerConfiguredForCloudscribe()
                                    .AddCloudscribeCoreEFIdentityServerStoragePostgreSql(pgConnection)
                                    .AddCloudscribeIdentityServerIntegrationMvc()
                                    .AddDeveloperSigningCredential()
                                ;
                            }

                            

                            break;

                        case "MySql":
                            var mysqlConnection = Configuration.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            services.AddCloudscribeKvpEFStorageMySql(mysqlConnection);

                            if (!DisableIdentityServer)
                            {
                                services.AddIdentityServerConfiguredForCloudscribe()
                                    .AddCloudscribeCoreEFIdentityServerStorageMySql(mysqlConnection)
                                    .AddCloudscribeIdentityServerIntegrationMvc()
                                    .AddDeveloperSigningCredential()
                                    ;
                            }

                            

                            break;

                        case "MSSQL":
                        default:
                            var connectionString = Configuration.GetConnectionString("EntityFrameworkConnectionString");

                            // this shows all the params with default values
                            // only connectionstring is required to be passed in
                            services.AddCloudscribeCoreEFStorageMSSQL(
                                connectionString:connectionString,
                                maxConnectionRetryCount: 0,
                                maxConnectionRetryDelaySeconds: 30,
                                transientSqlErrorNumbersToAdd: null,
                                useSql2008Compatibility:false);

                            //services.AddCloudscribeCoreEFStorageMSSQL(
                            //    connectionString: connectionString,
                            //    useSql2008Compatibility: true);


                            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);
                            services.AddCloudscribeKvpEFStorageMSSQL(connectionString);

                            if (!DisableIdentityServer)
                            {
                                services.AddIdentityServerConfiguredForCloudscribe()
                                    .AddCloudscribeCoreEFIdentityServerStorageMSSQL(connectionString)
                                    .AddCloudscribeIdentityServerIntegrationMvc()
                                    .AddDeveloperSigningCredential()
                                    ;
                            }

                            

                            break;
                    }


                    break;
            }
        }
        

    }
}
