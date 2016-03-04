// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-12-26
// 

using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Session;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.Localization;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.AspNet.Routing;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Models.Setup;
using cloudscribe.Logging.Web;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Navigation;
//using cloudscribe.Core.Web.Razor;
using cloudscribe.Setup.Web;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Messaging;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using cloudscribe.Web.Pagination;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Repositories.EF;


namespace example.WebApp
{
    public static class CloudscribeCoreServiceCollectionExtensions
    {

        /// <summary>
        /// Setup dependency injection for cloudscribe components
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCloudscribeCore(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddCaching();
            services.AddSession();
            //services.ConfigureSession(o =>
            //{
            //    o.IdleTimeout = TimeSpan.FromSeconds(10);
            //});
            
            services.Configure<MultiTenantOptions>(configuration.GetSection("MultiTenantOptions"));
            services.Configure<SiteConfigOptions>(configuration.GetSection("SiteConfigOptions"));
            services.Configure<SetupOptions>(configuration.GetSection("SetupOptions"));
            services.Configure<UIOptions>(configuration.GetSection("UIOptions"));
            services.Configure<cloudscribe.Core.Web.Components.Editor.CkeditorOptions>(configuration.GetSection("CkeditorOptions"));


            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();
            //services.AddScoped<cloudscribe.Core.Models.Setup.ISetupStep, cloudscribe.Core.Web.Components.EnsureSiteSetupStep2>();


            //*** Database platform ****************************************************************
            // here is where you could change to use one of the other db platforms
            // we have support for MSSQL MySql, PostgreSql, Firebird, SQLite, and SqlCe
            // as of 2015-11-12 mssql, sqlite, pgsql work in dnxcore50 
            // to use one of the others you would have to remove the dnxcore50 from the project.json in this project
            // add a reference for one of the other cloudscribe.Core.Repositories.dbplatform 
            // and cloudscribe.DbHelpers.dbplatform packages

            // this DevOptions can override from appsettings.localoverides.json
            // making it easy to change the db platform you are working with
            DevOptions devOptions = configuration.Get<DevOptions>("DevOptions");

            switch (devOptions.DbPlatform)
            {



                // the commented ones don't work yet in dnxcore50 so we can't add them to project.json
                // without dropping dnxcore50

                //case "sqlce":

                //    services.Configure<cloudscribe.DbHelpers.SqlCe.SqlCeConnectionOptions>(configuration.GetSection("Data:SqlCeConnectionOptions"));
                //    services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.SqlCe.SiteRepository>();
                //    services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.SqlCe.UserRepository>();
                //    services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.SqlCe.GeoRepository>();
                //    services.TryAddScoped<IDbSetup, cloudscribe.Setup.SqlCe.DbSetup>();
                //    services.TryAddScoped<IDataPlatformInfo, cloudscribe.Setup.SqlCe.DbSetup>();
                //    services.TryAddScoped<ILogRepository, cloudscribe.Logging.SqlCe.LogRepository>();
                //    services.TryAddScoped<cloudscribe.DbHelpers.SqlCe.SqlCeConnectionStringResolver, cloudscribe.DbHelpers.SqlCe.SqlCeConnectionStringResolver>();


                //    break;

                //case "firebird":

                //    services.Configure<cloudscribe.DbHelpers.ConnectionStringOptions>(configuration.GetSection("Data:Firebird:ConnectionStringOptions"));

                //    services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.Firebird.SiteRepository>();
                //    services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.Firebird.UserRepository>();
                //    services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.Firebird.GeoRepository>();
                //    services.TryAddScoped<IDbSetup, cloudscribe.Setup.Firebird.DbSetup>();
                //    services.TryAddScoped<IDataPlatformInfo, cloudscribe.Setup.Firebird.DbSetup>();
                //    services.TryAddScoped<ILogRepository, cloudscribe.Logging.Firebird.LogRepository>();

                //    break;


                //case "mysql":

                //    services.Configure<cloudscribe.DbHelpers.ConnectionStringOptions>(configuration.GetSection("Data:MySql:ConnectionStringOptions"));

                //    services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.MySql.SiteRepository>();
                //    services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.MySql.UserRepository>();
                //    services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.MySql.GeoRepository>();
                //    services.TryAddScoped<IDbSetup, cloudscribe.Setup.MySql.DbSetup>();
                //    services.TryAddScoped<IDataPlatformInfo, cloudscribe.Setup.MySql.DbSetup>();
                //    services.TryAddScoped<ILogRepository, cloudscribe.Logging.MySql.LogRepository>();

                //    break;

                //case "sqlite":

                //    services.Configure<cloudscribe.DbHelpers.SQLite.SqliteConnectionOptions>(configuration.GetSection("Data:SqliteConnectionOptions"));


                //    services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.SQLite.SiteRepository>();
                //    services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.SQLite.UserRepository>();
                //    services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.SQLite.GeoRepository>();
                //    services.TryAddScoped<IDbSetup, cloudscribe.Setup.Sqlite.DbSetup>();
                //    services.TryAddScoped<IDataPlatformInfo, cloudscribe.Setup.Sqlite.DbSetup>();
                //    services.TryAddScoped<ILogRepository, cloudscribe.Logging.Sqlite.LogRepository>();
                //    services.TryAddScoped<cloudscribe.DbHelpers.SQLite.SqliteConnectionstringResolver, cloudscribe.DbHelpers.SQLite.SqliteConnectionstringResolver>();

                //    break;


                //case "pgsql":

                //    services.Configure<cloudscribe.DbHelpers.ConnectionStringOptions>(configuration.GetSection("Data:PostgreSql:ConnectionStringOptions"));

                //    services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.pgsql.SiteRepository>();
                //    services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.pgsql.UserRepository>();
                //    services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.pgsql.GeoRepository>();
                //    services.TryAddScoped<IDbSetup, cloudscribe.Setup.pgsql.DbSetup>();
                //    services.TryAddScoped<IDataPlatformInfo, cloudscribe.Setup.pgsql.DbSetup>();
                //    services.TryAddScoped<ILogRepository, cloudscribe.Logging.pgsql.LogRepository>();

                //    break;

                //case "mssql":

                //services.Configure<cloudscribe.DbHelpers.MSSQLConnectionOptions>(configuration.GetSection("Data:MSSQLConnectionOptions"));
                //services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.MSSQL.SiteRepository>();
                //services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.MSSQL.UserRepository>();
                //services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.MSSQL.GeoRepository>();
                //services.TryAddScoped<IDbSetup, cloudscribe.Setup.MSSQL.DbSetup>();
                //services.TryAddScoped<IDataPlatformInfo, cloudscribe.Setup.MSSQL.DbSetup>();
                //services.TryAddScoped<ILogRepository, cloudscribe.Logging.MSSQL.LogRepository>();

                case "ef7":

                    services.TryAddScoped<ICoreModelMapper, SqlServerCoreModelMapper>();
                    services.TryAddScoped<cloudscribe.Logging.EF.ILogModelMapper, cloudscribe.Logging.EF.SqlServerLogModelMapper>();

                    services.AddEntityFramework()
                    .AddSqlServer()
                    .AddDbContext<CoreDbContext>(options =>
                        options.UseSqlServer(configuration["Data:EF7ConnectionOptions:ConnectionString"])
                    // this is needed if you use sqlserver 2008
                    // .UseRowNumberForPaging()

                    )
                    .AddDbContext<cloudscribe.Logging.EF.LoggingDbContext>(options =>
                    options.UseSqlServer(configuration["Data:EF7ConnectionOptions:ConnectionString"])
                    );

                    services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.EF.SiteRepository>();
                    services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.EF.UserRepository>();
                    services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.EF.GeoRepository>();
                    services.TryAddScoped<IDataPlatformInfo, cloudscribe.Core.Repositories.EF.DataPlatformInfo>();

                    services.TryAddScoped<ILogRepository, cloudscribe.Logging.EF.LogRepository>();


                    break;
            }

            
            //**************************************************************************************

            // RequestSiteResolver resolves ISiteSettings based on the request to support multi tenancy based on either host name or first folder depending on configuration
            //services.TryAddScoped<ISiteResolver, RequestSiteResolver>();
            services.TryAddScoped<ITimeZoneResolver, RequestTimeZoneResolver>();

            //services.AddMultitenancy<SiteSettings, SiteResolver>();
            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();


            services.TryAddScoped<SiteManager, SiteManager>();
            services.TryAddScoped<SetupManager, SetupManager>();
            services.TryAddScoped<GeoDataManager, GeoDataManager>();
            services.TryAddScoped<SystemInfoManager, SystemInfoManager>();
            services.TryAddScoped<IpAddressTracker, IpAddressTracker>();

            //

            services.TryAddScoped<SiteDataProtector, SiteDataProtector>();

            // VersionProviders are used by the Setup controller to determine what install and upgrade scripts to run
            services.AddScoped<IVersionProvider, SetupVersionProvider>();
            services.AddScoped<IVersionProvider, CloudscribeCoreVersionProvider>();
            services.AddScoped<IVersionProvider, cloudscribe.Logging.Web.CloudscribeLoggingVersionProvider>();

            // the factory will provide access to the previously registered IVersionProviders
            services.TryAddScoped<IVersionProviderFactory, VersionProviderFactory>();

            services.AddCloudscribeIdentity<SiteUser, SiteRole>()
                .AddDefaultTokenProviders();

            // you can use either json or xml to maintain your navigation map we provide examples of each navigation.xml and 
            // navigation.json in the root of this project
            // you can override the name of the file used with AppSettings:NavigationXmlFileName or AppSettings:NavigationJsonFileName in config.json
            // the file must live in the root of the web project code not in wwwroot

            // it is arguable which is easier for humans to read and maintain, myself I think for something like a navigation tree
            // that could get large xml is easier to work with and not make mistakes. in json one missing or extra comma can break it
            // granted xml can be broken by typos too but the end tags make it easier to keep track of where you are imho (JA)
            //services.TryAddScoped<INavigationTreeBuilder, JsonNavigationTreeBuilder>();
            //services.TryAddScoped<INavigationTreeBuilder, HardCodedNavigationTreeBuilder>();
            services.TryAddScoped<ITreeCache, MemoryTreeCache>();
            services.AddScoped<INavigationTreeBuilder, XmlNavigationTreeBuilder>();
            services.AddScoped<NavigationTreeBuilderService, NavigationTreeBuilderService>();
            services.TryAddScoped<INodeUrlPrefixProvider, FolderTenantNodeUrlPrefixProvider>(); 
            services.TryAddScoped<INavigationNodePermissionResolver, NavigationNodePermissionResolver>();
            services.Configure<NavigationOptions>(configuration.GetSection("NavigationOptions"));

            
            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();

            
            //services.AddTransient<IEmailTemplateService, HardCodedEmailTemplateService>();
            services.AddTransient<ISiteMessageEmailSender, SiteEmailMessageSender>();
            services.AddTransient<ISmsSender, SiteSmsSender>();
            
            services.TryAddSingleton<IThemeListBuilder, SiteThemeListBuilder>();
            

            services.TryAddSingleton<IRazorViewEngine, CoreViewEngine>();
            // cloudscribe.Core.Web.CoreViewEngine adds /Views/Sys as the last place to search for views
            // cloudscribe views are all under Views/Sys
            // to modify a view just copy it to a higher priority location
            // ie copy /Views/Sys/Manage/*.cshtml up to /Views/Manage/ and that one will have higher priority
            // and you can modify it however you like
            // upgrading to newer versions of cloudscribe could modify or add views below /Views/Sys
            // so you may need to compare your custom views to the originals again after upgrades

            // Add MVC services to the services container.
            services.AddMvc(options =>
            {
               
                //options.Filters.Add(...);
            })
            .AddViewLocalization(options => options.ResourcesPath = "AppResources")
            //.AddDataAnnotationsLocalization()
            //.AddXmlDataContractSerializerFormatters()
            .AddViewOptions(options =>
             {
                 
                 
             });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new SiteViewLocationExpander());
            });




            //https://github.com/aspnet/Announcements/issues/71

            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // establish AppId and AppSecret here https://developers.facebook.com/apps
            // https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookAuthenticationOptions.cs
            //services.Configure<FacebookAuthenticationOptions>(options =>
            //{
            //    // options here are only used if not specified in site settings
            //    options.AppId = configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            //    //options.AuthenticationScheme = AuthenticationScheme.External;

            //});

            //// get clientid and secret here https://account.live.com/developers/applications/index
            //services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            //{
            //    options.ClientId = configuration["Authentication:MicrosoftAccount:ClientId"];
            //    options.ClientSecret = configuration["Authentication:MicrosoftAccount:ClientSecret"];
            //});

            ////https://auth0.com/docs/connections/social/google
            //// get clientid and secret here https://console.developers.google.com/
            //services.Configure<GoogleAuthenticationOptions>(options =>
            //{
            //    options.ClientId = configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            //});


            //// get consumerkey and secret here https://apps.twitter.com/
            //// unlike the other providers twitter does not allow creating localhost apps for testing
            //// but you can use an url shortener to mask the localhost urls
            //services.Configure<TwitterAuthenticationOptions>(options =>
            //{
            //    options.ConsumerKey = configuration["Authentication:Twitter:ConsumerKey"];
            //    options.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"];
            //});

            return services;
        }


        public static IdentityBuilder AddCloudscribeIdentity<TUser, TRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            // Services used by identity
            services.AddOptions();
            services.AddAuthentication(options =>
            {
                // This is the Default value for ExternalCookieAuthenticationScheme
                //options.SignInScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
                options.SignInScheme = AuthenticationScheme.External;
            });

            services.TryAddSingleton<IdentityMarkerService>();

            //****** cloudscribe implementation of AspNet.Identity****************************************************

            // I think this is creating multiple instances of UserStore when we only want 1
            //services.TryAddScoped<IUserStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserPasswordStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserEmailStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserLoginStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserRoleStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserClaimStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserPhoneNumberStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserLockoutStore<SiteUser>, UserStore<SiteUser>>();
            //services.TryAddScoped<IUserTwoFactorStore<SiteUser>, UserStore<SiteUser>>();

            services.TryAddScoped<IUserStore<SiteUser>, UserStore<SiteUser>>();


            services.TryAddScoped<IRoleStore<SiteRole>, RoleStore<SiteRole>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            // the DNX451 desktop version of SitePasswordHasher can validate against existing hashed or encrypted passwords from mojoportal users
            // so to use existing users from mojoportal you would have to run on the desktop version at least until all users update their passwords
            // then you could migrate to dnxcore50
            // it also alllows us to create a default admin@admin.com user with administrator role with a cleartext password which would be updated 
            // to the default identity hash as soon as you change the password from its default "admin"
            services.TryAddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();

            services.TryAddScoped<SiteUserManager<SiteUser>, SiteUserManager<SiteUser>>();
            services.TryAddScoped<SiteRoleManager<SiteRole>, SiteRoleManager<SiteRole>>();
            services.TryAddScoped<SiteSignInManager<SiteUser>, SiteSignInManager<SiteUser>>();
            //services.TryAddScoped<SignInManager<SiteUser>, SignInManager<SiteUser>>();

            
            services.TryAddScoped<SiteAuthCookieValidator, SiteAuthCookieValidator>();
            services.TryAddScoped<SiteCookieAuthenticationEvents, SiteCookieAuthenticationEvents>();
            //********************************************************************************************************

            // most of the below code was borrowed from here:
            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/IdentityServiceCollectionExtensions.cs


            

            // Identity services
            //services.TryAddSingleton<IdentityMarkerService>();
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
            services.TryAddScoped<UserManager<TUser>, UserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>, SignInManager<TUser>>();
            services.TryAddScoped<RoleManager<TRole>, RoleManager<TRole>>();

            //http://docs.asp.net/en/latest/security/2fa.html
            //services.TryAddScoped<IdentityOptions>();
            services.Configure<IdentityOptions>(options =>
            {
                // some other options are overridden by ste settings
                // but in all cases we do want to require each account to have a unique email
                // it is problematic to not require that when a site is created
                // then accounts with duplicate email can be created and later
                // if the configuration is changed to now require unique email
                // there already exist problem records
                // the default setting for Identity is false but we want it to be true for these reasons
                options.User.RequireUniqueEmail = true;
                options.Cookies.TwoFactorUserIdCookieAuthenticationScheme = AuthenticationScheme.TwoFactorUserId;
            });


            //services.Configure<SharedAuthenticationOptions>(options =>
            //{
            //    //options.SignInScheme = IdentityOptions.ExternalCookieAuthenticationScheme;
            //    options.SignInScheme = AuthenticationScheme.External;

            //});

            // Configure all of the cookie middlewares
            //services.ConfigureIdentityApplicationCookie(options =>
            //{
            //    options.AuthenticationScheme = IdentityOptions.ApplicationCookieAuthenticationScheme;
            //    options.AutomaticAuthentication = true;
            //    options.LoginPath = new PathString("/Account/Login");
            //    options.Notifications = new CookieAuthenticationNotifications
            //    {
            //        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            //    };
            //});

            //https://github.com/aspnet/Announcements/issues/71

            //services.ConfigureCookieAuthentication(options =>
            //{
            //    options.AuthenticationScheme = AuthenticationScheme.Application;
            //    options.CookieName = AuthenticationScheme.Application;
            //    options.AutomaticAuthentication = true;
            //    options.LoginPath = new PathString("/Account/Login");
            //    options.Notifications = new CookieAuthenticationNotifications
            //    {
            //        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            //    };
            //}
            //, AuthenticationScheme.Application
            //);

            //services.ConfigureCookieAuthentication(options =>
            //{
            //    options.AuthenticationScheme = AuthenticationScheme.External;
            //    options.CookieName = AuthenticationScheme.External;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //}
            //, AuthenticationScheme.External);

            //services.ConfigureCookieAuthentication(options =>
            //{
            //    options.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe;
            //    options.CookieName = AuthenticationScheme.TwoFactorRememberMe;
            //}
            //, AuthenticationScheme.TwoFactorRememberMe);

            //services.ConfigureCookieAuthentication(options =>
            //{
            //    options.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId;
            //    options.CookieName = AuthenticationScheme.TwoFactorUserId;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //}
            //, AuthenticationScheme.TwoFactorUserId);



            IdentityBuilder builder = new IdentityBuilder(typeof(TUser), typeof(TRole), services);
            //builder.AddUserStore(UserStore<SiteUser>>();


            return builder;
        }

    }
}
