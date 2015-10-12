// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-10-12
// 

using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Session;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Extensions;
using Microsoft.Framework.Caching;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.Configuration;
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
using cloudscribe.Core.Models.Logging;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Navigation;
using cloudscribe.Core.Web.Razor;
using cloudscribe.Messaging;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Pagination;
using cloudscribe.Core.Identity;

namespace cloudscribe.WebHost
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


            //services.AddInstance<IConfiguration>(configuration);
            //services.TryAddScoped<ConfigHelper, ConfigHelper>();
            services.Configure<MultiTenantOptions>(configuration.GetSection("MultiTenantOptions"));
            services.Configure<SetupOptions>(configuration.GetSection("SetupOptions"));
            services.Configure<UIOptions>(configuration.GetSection("UIOptions"));
            services.Configure<UIOptions>(configuration.GetSection("CkeditorOptions"));

            
            



            //*** Database platform ****************************************************************
            // here is where you could change to use one of the other db platforms
            // we have support for MySql, PostgreSql, Firebird, SQLite, and SqlCe
            // as of 2015-06-24 those can only be used in the full desktop framework (there are not yet ado.net drivers that support dnxcore50 but they should be available at some point)
            // so you would have to remove the dnxcore50 from the project.json in this project
            // add a nuget for one of the other cloudscribe.Core.Repositories.dbplatform 
            // and cloudscribe.DbHelpers.dbplatform packages
            services.Configure<cloudscribe.DbHelpers.MSSQL.MSSQLConnectionOptions>(configuration.GetSection("Data:MSSQLConnectionOptions"));
            services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.MSSQL.SiteRepository>();
            services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.MSSQL.UserRepository>();
            services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.MSSQL.GeoRepository>();
            services.TryAddScoped<IDb, cloudscribe.DbHelpers.MSSQL.Db>();
            services.TryAddScoped<ILogRepository, cloudscribe.Core.Repositories.MSSQL.LogRepository>();

            //services.Configure<cloudscribe.DbHelpers.MySql.MySqlConnectionOptions>(configuration.GetSection("Data:MySqlConnectionOptions"));
            //services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.MySql.SiteRepository>();
            //services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.MySql.UserRepository>();
            //services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.MySql.GeoRepository>();
            //services.TryAddScoped<IDb, cloudscribe.DbHelpers.MySql.Db>();

            //services.Configure<cloudscribe.DbHelpers.pgsql.PostgreSqlConnectionOptions>(configuration.GetSection("Data:PostgreSqlConnectionOptions"));
            //services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.pgsql.SiteRepository>();
            //services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.pgsql.UserRepository>();
            //services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.pgsql.GeoRepository>();
            //services.TryAddScoped<IDb, cloudscribe.DbHelpers.pgsql.Db>();

            //services.Configure<cloudscribe.DbHelpers.Firebird.FirebirdConnectionOptions>(configuration.GetSection("Data:FirebirdConnectionOptions"));
            //services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.Firebird.SiteRepository>();
            //services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.Firebird.UserRepository>();
            //services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.Firebird.GeoRepository>();
            //services.TryAddScoped<IDb, cloudscribe.DbHelpers.Firebird.Db>();

            //services.Configure<cloudscribe.DbHelpers.Sqlite.SqliteConnectionOptions>(configuration.GetSection("Data:SqliteConnectionOptions"));
            //services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.SQLite.SiteRepository>();
            //services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.SQLite.UserRepository>();
            //services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.SQLite.GeoRepository>();
            //services.TryAddScoped<IDb, cloudscribe.DbHelpers.Sqlite.Db>();

            //services.Configure<cloudscribe.DbHelpers.SqlCe.SqliteConnectionOptions>(configuration.GetSection("Data:SqlCeConnectionOptions"));
            //services.TryAddScoped<ISiteRepository, cloudscribe.Core.Repositories.SqlCe.SiteRepository>();
            //services.TryAddScoped<IUserRepository, cloudscribe.Core.Repositories.SqlCe.UserRepository>();
            //services.TryAddScoped<IGeoRepository, cloudscribe.Core.Repositories.SqlCe.GeoRepository>();
            //services.TryAddScoped<IDb, cloudscribe.DbHelpers.SqlCe.Db>();


            //**************************************************************************************

            // RequestSiteResolver resolves ISiteSettings based on the request to support multi tenancy based on either host name or first folder depending on configuration
            services.TryAddScoped<ISiteResolver, RequestSiteResolver>();
            services.TryAddScoped<ITimeZoneResolver, RequestTimeZoneResolver>();
            services.TryAddScoped<SiteManager, SiteManager>();
            services.TryAddScoped<SetupManager, SetupManager>();
            services.TryAddScoped<GeoDataManager, GeoDataManager>();
            services.TryAddScoped<SystemInfoManager, SystemInfoManager>();

            

            // VersionProviders are used by the Setup controller to determine what install and upgrade scripts to run
            services.TryAddScoped<IVersionProviderFactory, ConfigVersionProviderFactory>();

            services.AddCloudscribeIdentity<SiteUser, SiteRole>();
            
            // you can use either json or xml to maintain your navigation map we provide examples of each navigation.xml and 
            // navigation.json in the root of this project
            // you can override the name of the file used with AppSettings:NavigationXmlFileName or AppSettings:NavigationJsonFileName in config.json
            // the file must live in the root of the web project code not in wwwroot

            // it is arguable which is easier for humans to read and maintain, myself I think for something like a navigation tree
            // that could get large xml is easier to work with and not make mistakes. in json one missing or extra comma can break it
            // granted xml can be broken by typos too but the end tags make it easier to keep track of where you are imho (JA)
            //services.TryAddScoped<INavigationTreeBuilder, JsonNavigationTreeBuilder>();
            //services.TryAddScoped<INavigationTreeBuilder, HardCodedNavigationTreeBuilder>();
            services.TryAddScoped<INavigationTreeBuilder, XmlNavigationTreeBuilder>();
            services.TryAddScoped<INodeUrlPrefixProvider, FolderTenantNodeUrlPrefixProvider>(); 
            services.TryAddScoped<INavigationNodePermissionResolver, NavigationNodePermissionResolver>();
            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();
            services.Configure<NavigationOptions>(configuration.GetSection("NavigationOptions"));
            

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // TODO: implement TenantLayoutSelector
            services.Configure<LayoutSelectorOptions>(configuration.GetSection("LayoutSelectorOptions"));
            services.TryAddSingleton<ILayoutSelector, TenantLayoutSelector>();
            services.TryAddSingleton<ILayoutFileListBuilder, LayoutFileListBuilder>();
            services.TryAddSingleton<ILayoutFileDisplayNameFilter, TenantLayoutFileDisplayNameFilter>();
            



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
            .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
            //.AddXmlDataContractSerializerFormatters()
            .AddViewOptions(options =>
             {
                 
                 
             });

            
                


            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // establish AppId and AppSecret here https://developers.facebook.com/apps
            // https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookAuthenticationOptions.cs
            services.Configure<FacebookAuthenticationOptions>(options =>
            {
                // options here are only used if not specified in site settings
                options.AppId = configuration["Authentication:Facebook:AppId"];
                options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                //options.AuthenticationScheme = AuthenticationScheme.External;

            });

            // get clientid and secret here https://account.live.com/developers/applications/index
            services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            {
                options.ClientId = configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });

            //https://auth0.com/docs/connections/social/google
            // get clientid and secret here https://console.developers.google.com/
            services.Configure<GoogleAuthenticationOptions>(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });


            // get consumerkey and secret here https://apps.twitter.com/
            // unlike the other providers twitter does not allow creating localhost apps for testing
            // but you can use an url shortener to mask the localhost urls
            services.Configure<TwitterAuthenticationOptions>(options =>
            {
                options.ConsumerKey = configuration["Authentication:Twitter:ConsumerKey"];
                options.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"];
            });

            return services;
        }


        public static IdentityBuilder AddCloudscribeIdentity<TUser, TRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            //****** cloudscribe implementation of AspNet.Identity****************************************************
            services.TryAddScoped<IUserStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserPasswordStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserEmailStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserLoginStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserRoleStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserClaimStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserPhoneNumberStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserLockoutStore<SiteUser>, UserStore<SiteUser>>();
            services.TryAddScoped<IUserTwoFactorStore<SiteUser>, UserStore<SiteUser>>();
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

            //services.TryAddScoped<ICookieAuthenticationSchemeSet, DefaultCookieAuthenticationSchemeSet>();
            //services.TryAddScoped<ICookieAuthenticationSchemeSet, FolderTenantCookieAuthSchemeResolver>();

            services.TryAddScoped<MultiTenantCookieOptionsResolver, MultiTenantCookieOptionsResolver>();
            services.TryAddScoped<MultiTenantCookieOptionsResolverFactory, MultiTenantCookieOptionsResolverFactory>();
            services.TryAddScoped<MultiTenantAuthCookieValidator, MultiTenantAuthCookieValidator>();
            services.TryAddScoped<MultiTenantCookieAuthenticationNotifications, MultiTenantCookieAuthenticationNotifications>();
            //********************************************************************************************************

            // most of the below code was borrowed from here:
            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/IdentityServiceCollectionExtensions.cs


            // Services used by identity
            services.AddOptions();
            services.AddAuthentication();

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

            services.Configure<IdentityOptions>(options =>
            {
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                //options.Lockout.MaxFailedAccessAttempts = 10;
            });

            
            services.Configure<SharedAuthenticationOptions>(options =>
            {
                //options.SignInScheme = IdentityOptions.ExternalCookieAuthenticationScheme;
                options.SignInScheme = AuthenticationScheme.External;
                
            });

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


            services.ConfigureCookieAuthentication(options =>
            {
                options.AuthenticationScheme = AuthenticationScheme.Application;
                options.CookieName = AuthenticationScheme.Application;
                options.AutomaticAuthentication = true;
                options.LoginPath = new PathString("/Account/Login");
                options.Notifications = new CookieAuthenticationNotifications
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                };
            }
            , AuthenticationScheme.Application
            );

            services.ConfigureCookieAuthentication(options =>
            {
                options.AuthenticationScheme = AuthenticationScheme.External;
                options.CookieName = AuthenticationScheme.External;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            }
            , AuthenticationScheme.External);

            services.ConfigureCookieAuthentication(options =>
            {
                options.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe;
                options.CookieName = AuthenticationScheme.TwoFactorRememberMe;
            }
            , AuthenticationScheme.TwoFactorRememberMe);

            services.ConfigureCookieAuthentication(options =>
            {
                options.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId;
                options.CookieName = AuthenticationScheme.TwoFactorUserId;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            }
            , AuthenticationScheme.TwoFactorUserId);

            

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }

    }
}
