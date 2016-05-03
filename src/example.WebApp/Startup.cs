using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNet.Authentication.Cookies;
//using Microsoft.AspNet.Authentication.Facebook;
//using Microsoft.AspNet.Authentication.Google;
//using Microsoft.AspNet.Authentication.MicrosoftAccount;
//using Microsoft.AspNet.Authentication.Twitter;
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
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using Glimpse;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Web.Components;
using cloudscribe.Logging.Web;
using cloudscribe.Core.Web.Middleware;
using SaasKit.Multitenancy;
using StructureMap;
using SaasKit.Multitenancy.StructureMap;
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

        private string appBasePath;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        //public void ConfigureServices(IServiceCollection services)
        {

            //http://docs.asp.net/en/latest/security/data-protection/configuration/overview.html
            //If you change the key persistence location, the system will no longer automatically encrypt keys 
            // at rest since it doesn’t know whether DPAPI is an appropriate encryption mechanism.
            services.ConfigureDataProtection(configure =>
            {
                string pathToCryptoKeys = appBasePath + Path.DirectorySeparatorChar
                + "dp_keys" + Path.DirectorySeparatorChar;

                // these keys are not encrypted at rest
                // since we have specified a non default location
                // that also makes the key portable so they will still work if we migrate to 
                // a new machine (will they work on different OS? I think so)
                // this is a similar server migration issue as the old machinekey
                // where we specified a machinekey in web.config so it would not change if we 
                // migrate to a new server
                configure.PersistKeysToFileSystem(
                    new DirectoryInfo(pathToCryptoKeys)
                    );

                //configure.ProtectKeysWithCertificate("thumbprint");
                //configure.SetDefaultKeyLifetime(TimeSpan.FromDays(14));
                ///configure.
            });


            //services.TryAddScoped<IConfigurationRoot, Configuration>();

            //http://blog.getglimpse.com/2015/11/19/installing-glimpse-v2-beta1/
            bool enableGlimpse = Configuration.Get<bool>("DiagnosticOptions:EnableGlimpse", false);
            if (enableGlimpse)
            {
                services.AddGlimpse();
            }

            //services.AddLocalization(options => options.ResourcesPath = "AppResources");

            ConfigureAuthPolicy(services);


            // we may need this on linux/mac as urls are case sensitive by default
            //services.Configure<RouteOptions>(routeOptions => routeOptions.LowercaseUrls = true);

            // Setup dependencies for cloudscribe Identity, Roles and and Site Administration
            // this is in Startup.CloudscribeCore.DI.MS.cs
            services.ConfigureCloudscribeCore(Configuration);

            services.TryAddScoped<LogManager, LogManager>();

            var container = new Container();

            container.Populate(services);

            container.Configure(c =>
            {
                // Application Services
                c.For<ITenantContainerBuilder<SiteSettings>>().Use(() => new SiteTenantContainerBuilder(container));
            });

            container.ConfigureTenants<SiteSettings>(c =>
            {
                // Tenant Scoped Services
                //c.For<IMessageService>().Singleton().Use<MessageService>();

            });

            return container.GetInstance<IServiceProvider>();


            //services.Configure<MvcOptions>(options =>
            //{
            //    // forces https
            //    // note that the home or root is still accessible non securely
            //    // only enable this if you have an ssl certificate installed and working
            //    //options.Filters.Add(new RequireHttpsAttribute());

            //    //options.ModelValidatorProviders.Add()

            //});

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

            //http://blog.getglimpse.com/2015/11/19/installing-glimpse-v2-beta1/
            bool enableGlimpse = Configuration.Get<bool>("DiagnosticOptions:EnableGlimpse", false);
            if (enableGlimpse)
            {
                app.UseGlimpse();
            }


            // LogLevels
            //Debug = 1,
            //Trace = 2,
            //Information = 3,
            //Warning = 4,
            //Error = 5,
            //Critical = 6,

            // Add the console logger.
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);
            
            // a customizable filter for logging
            LogLevel minimumLevel = LogLevel.Warning;
            List<string> excludedLoggers = new List<string>();
            // add exclusions to remove noise in the logs

            // we need to filter out EF otherwise each time we persist a log item to the db more logs are generated
            // so it can become an infinite loop that keeps creating data
            // you can add any loggers that you want to exclude to reduce noise in the log
            excludedLoggers.Add("Microsoft.Data.Entity.Storage.Internal.RelationalCommandBuilderFactory");
            excludedLoggers.Add("Microsoft.Data.Entity.Query.Internal.QueryCompiler");
            excludedLoggers.Add("Microsoft.Data.Entity.DbContext");

            Func<string, LogLevel, bool> logFilter = delegate (string loggerName, LogLevel logLevel)
            {
                if (logLevel < minimumLevel) { return false; }
                if(excludedLoggers.Contains(loggerName)) { return false; }

                return true;
            };
            // Add cloudscribe db logging
            loggerFactory.AddDbLogger(serviceProvider, logRepository, logFilter);
            


            //app.UseCultureReplacer();

            // localization from .resx files is not really working in beta8
            // will have to wait till next release

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("it"),
                new CultureInfo("fr-FR")
            };

            var locOptions = new RequestLocalizationOptions
            {
                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures

            };
            // You can change which providers are configured to determine the culture for requests, or even add a custom
            // provider with your own logic. The providers will be asked in order to provide a culture for each request,
            // and the first to provide a non-null result that is in the configured supported cultures list will be used.
            // By default, the following built-in providers are configured:
            // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
            // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
            // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
            //locOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            //{
            //  // My custom request culture logic
            //  return new ProviderCultureResult("en");
            //}));
            //app.UseRequestLocalization(locOptions, 
            //    defaultRequestCulture: new RequestCulture(culture: "en-US", uiCulture: "en-US"));

            
            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                //app.UseBrowserLink();

                app.UseDeveloperExceptionPage();
                
                //app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
                //app.UseStatusCodePagesWithReExecute("/error/{0}");
                //app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
            else
            {
                
                app.UseExceptionHandler("/Home/Error");

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

            //app.UseCommonExceptionHandler();

            // Add the platform handler to the request pipeline.
            //app.UseIISPlatformHandler();
            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            //app.UseRuntimeInfoPage("/info");

            // Add static files to the request pipeline.
            app.UseStaticFiles();
            
            // the only thing we are using session for is Alerts
            
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(20));



            // this is in Startup.CloudscribeCore.cs
            // app.UseCloudscribeCore(multiTenantOptions,Configuration);
            app.UseMultitenancy<SiteSettings>();
            app.UseTenantContainers<SiteSettings>();

            // https://github.com/saaskit/saaskit/blob/master/src/SaasKit.Multitenancy/MultitenancyApplicationBuilderExtensions.cs
            // should custom builder extension use Microsoft.AspNet.Builder as their own namespace?
            // I can see the merits but wondered is that conventional
            app.UsePerTenant<SiteSettings>((ctx, builder) =>
            {
                //app.UseSession();

                var folderPath = ctx.Tenant.SiteFolderName;
                
                var singletonIdentityOptions = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
                var resolver = app.ApplicationServices.GetRequiredService<IIdentityOptionsResolver>();
                var identityOptions = resolver.ResolveOptions(singletonIdentityOptions);
                //if (identityOptions == null) { throw new ArgumentException("failed to get identity options"); }
                //if (identityOptions.Cookies.ApplicationCookie == null) { throw new ArgumentException("failed to get identity application cookie options"); }


                //var shouldUseFolder = true;
                ////var shouldUseFolder = (
                ////(multiTenantOptions.Value.Mode == MultiTenantMode.FolderName)
                ////&& (!multiTenantOptions.Value.UseRelatedSitesMode)
                ////&& (ctx.Tenant.SiteFolderName.Length > 0)
                ////);

                var cookieEvents = app.ApplicationServices.GetService<SiteCookieAuthenticationEvents>();
                identityOptions.Cookies.ExternalCookie.Events = cookieEvents;
                identityOptions.Cookies.TwoFactorRememberMeCookie.Events = cookieEvents;
                identityOptions.Cookies.TwoFactorUserIdCookie.Events = cookieEvents;
                identityOptions.Cookies.ApplicationCookie.Events = cookieEvents;

                builder.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
                builder.UseCookieAuthentication(identityOptions.Cookies.TwoFactorRememberMeCookie);
                builder.UseCookieAuthentication(identityOptions.Cookies.TwoFactorUserIdCookie);
                builder.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);


                if (string.IsNullOrEmpty(folderPath))
                {
                    //builder.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
                    //builder.UseCookieAuthentication(identityOptions.Cookies.TwoFactorRememberMeCookie);
                    //builder.UseCookieAuthentication(identityOptions.Cookies.TwoFactorUserIdCookie);
                    //builder.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);
                    
                }
                else
                {
                    // When Map is used, the matched path segment(s) are removed from HttpRequest.Path 
                    // and appended to HttpRequest.PathBase for each request.
                    // maybe should try .MapWhen instead to avoid that
                    //builder.MapWhen(context => {
                    //    return context.Request.Path.StartsWithSegments(new PathString("/" + folderPath));
                    //}, folderTenant =>
                    //{
                    //    var singletonOptions = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
                    //    var resolv = app.ApplicationServices.GetRequiredService<IIdentityOptionsResolver>();
                    //    var identityOption = resolver.ResolveOptions(singletonIdentityOptions);

                    //    var cookieEvent = app.ApplicationServices.GetService<SiteCookieAuthenticationEvents>();
                    //    identityOption.Cookies.ExternalCookie.Events = cookieEvent;
                    //    identityOption.Cookies.TwoFactorRememberMeCookie.Events = cookieEvent;
                    //    identityOption.Cookies.TwoFactorUserIdCookie.Events = cookieEvent;
                    //    identityOption.Cookies.ApplicationCookie.Events = cookieEvent;

                    //    folderTenant.UseCookieAuthentication(identityOption.Cookies.ExternalCookie);
                    //    folderTenant.UseCookieAuthentication(identityOption.Cookies.TwoFactorRememberMeCookie);
                    //    folderTenant.UseCookieAuthentication(identityOption.Cookies.TwoFactorUserIdCookie);
                    //    folderTenant.UseCookieAuthentication(identityOption.Cookies.ApplicationCookie);

                    //    //AddSocialAuth(folderTenant, ctx.Tenant, identityOption, true);

                    //    // it is very important that all authentication configuration be set before configuring mvc
                    //    AddMvc(folderTenant, false);
                    //});

                    //builder.Map("/" + folderPath, folderTenant =>
                    //{

                    //    folderTenant.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
                    //    folderTenant.UseCookieAuthentication(identityOptions.Cookies.TwoFactorRememberMeCookie);
                    //    folderTenant.UseCookieAuthentication(identityOptions.Cookies.TwoFactorUserIdCookie);
                    //    folderTenant.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);

                    //    //AddSocialAuth(folderTenant, ctx.Tenant, identityOptions, true);

                    //    // it is very important that all authentication configuration be set before configuring mvc
                    //    AddMvc(folderTenant, false);


                    //});
                }

                //AddSocialAuth(builder, ctx.Tenant, identityOptions, false);

                //AddMvc(builder, true);



            });

            // it is very important that all authentication configuration be set before configuring mvc
            AddMvc(app, true);

            

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

            DevOptions devOptions = Configuration.Get<DevOptions>("DevOptions");
            if (devOptions.DbPlatform == "ef7")
            {
                cloudscribe.Core.Repositories.EF.InitialData.InitializeDatabaseAsync(app.ApplicationServices).Wait();
                // this is using EF for the logger but using EF for Core does not mean you must use EF for logging
                // one should be able to use the MSSQL Logger while still using EF for the core repos
                // one problem with EF logging is that EF logs a lot of information stuff and if we use EF for logigng
                // then every time a log item is inserted to the db it generates more logging events
                // and thus a continuous creation of data can result so the EF logger is designed to leave out
                // EF components from logging
                // by using the mssql logger instead then no extra log items will be created and you can more easily allow
                // EF to log things of its own
                // however the mssql logger depends on the setup system which is not used by EF components
                // this dependency is more practical than technical though, you could run the db setup script for mssql logging
                // manually instead of using the setup system.
                cloudscribe.Logging.EF.DbInitializer.InitializeDatabaseAsync(app.ApplicationServices).Wait();
            }



        }

        

        private void AddSocialAuth(
            IApplicationBuilder app, 
            SiteSettings site, 
            IdentityOptions identityOptions,
            bool shouldUseFolder
            )
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

        private void AddMvc(IApplicationBuilder app, bool includeFolderRoutes)
        {
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


                //// default route for folder sites must be second to last
                if (includeFolderRoutes)
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
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

    }
}
