using cloudscribe.Core.Models;
using cloudscribe.UserProperties.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using sourceDev.WebApp.Components;
using System;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config,
            IHostingEnvironment env
            )
        {
            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = config["DevOptions:DbPlatform"];
            var efProvider = config["DevOptions:EFProvider"];
            var useMiniProfiler = config.GetValue<bool>("DevOptions:EnableMiniProfiler");

            switch (storage)
            {
                case "NoDb":
                    var useSingletons = true;
                    services.AddCloudscribeCoreNoDbStorage(useSingletons);
                    services.AddCloudscribeLoggingNoDbStorage(config);
                    services.AddCloudscribeKvpNoDbStorage();

                    if(useMiniProfiler)
                    {
                        services.AddMiniProfiler();
                    }
                    
                    
                    break;

                case "ef":
                default:

                    if (useMiniProfiler)
                    {
                        services.AddMiniProfiler()
                            .AddEntityFramework();
                    }

                    switch (efProvider)
                    {
                        case "sqlite":

                            var dbName = config.GetConnectionString("SQLiteDbName");
                            var dbPath = Path.Combine(env.ContentRootPath, dbName);
                            var slConnection = $"Data Source={dbPath}";

                            //var slConnection = config.GetConnectionString("SQLiteEntityFrameworkConnectionString");
                            //Data Source=cloudscribe.dev2.db


                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            services.AddCloudscribeKvpEFStorageSQLite(slConnection);

                            break;

                        case "pgsql-old":
                            var pgConnection = config.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeKvpEFStoragePostgreSql(pgConnection);

                            break;

                        case "pgsql":
                            var pgsConnection = config.GetConnectionString("PostgreSqlConnectionString");
                            services.AddCloudscribeCorePostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeLoggingPostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeKvpPostgreSqlStorage(pgsConnection);

                            break;


                        case "MySql":
                            var mysqlConnection = config.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            services.AddCloudscribeKvpEFStorageMySql(mysqlConnection);

                            break;

                        case "MSSQL":
                        default:
                            var connectionString = config.GetConnectionString("EntityFrameworkConnectionString");

                            // this shows all the params with default values
                            // only connectionstring is required to be passed in
                            services.AddCloudscribeCoreEFStorageMSSQL(
                                connectionString: connectionString,
                                maxConnectionRetryCount: 0,
                                maxConnectionRetryDelaySeconds: 30,
                                transientSqlErrorNumbersToAdd: null,
                                useSql2008Compatibility: false);

                            //services.AddCloudscribeCoreEFStorageMSSQL(
                            //    connectionString: connectionString,
                            //    useSql2008Compatibility: true);


                            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);
                            services.AddCloudscribeKvpEFStorageMSSQL(connectionString);
                            
                            break;
                    }


                    break;
            }

            return services;
        }

        public static IServiceCollection SetupCloudscribeFeatures(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            services.AddScoped<cloudscribe.Versioning.IVersionProvider, cloudscribe.Web.StaticFiles.VersionProvider>();
            /* optional and only needed if you are using cloudscribe Logging  */
            services.AddCloudscribeLogging();

            services.Configure<ProfilePropertySetContainer>(config.GetSection("ProfilePropertySetContainer"));
            services.AddCloudscribeKvpUserProperties();

            /* these are optional and only needed if using cloudscribe Setup */
            //services.Configure<SetupOptions>(Configuration.GetSection("SetupOptions"));
            //services.AddScoped<SetupManager, SetupManager>();
            //services.AddScoped<IVersionProvider, SetupVersionProvider>();
            //services.AddScoped<IVersionProvider, CloudscribeLoggingVersionProvider>();
            /* end cloudscribe Setup */

            //services.AddScoped<cloudscribe.Core.Web.ExtensionPoints.IHandleCustomRegistration, sourceDev.WebApp.Components.CustomRegistrationHandler>();

            // this will capture the jwt from oidc and add it as an access_token claim
            // however that doubles the cookie size https://hajekj.net/2017/03/20/cookie-size-and-cookie-authentication-in-asp-net-core/
            //services.AddSingleton<cloudscribe.Core.Identity.ICaptureOidcTokens, cloudscribe.Core.Identity.SiteOidcTokenCapture>();

            //services.AddTransient<cloudscribe.Core.Identity.SiteOidcHybridFlowHelperAuthCookieEvents>();
            //services.AddTransient<cloudscribe.Core.Identity.ISiteAuthCookieEvents, cloudscribe.Core.Identity.SiteOidcHybridFlowHelperAuthCookieEvents>();

            // this stores the auth ticket in distributed cache and only the sessionid is kept in the auth cookie making it very small
            // however the default IDistributedCache is really a memory cache and rebuilding the app or recycle app pool loses the data
            // so user is not logged in. That can be solved by injecting a real distributed cache such as Redis
            //services.AddSingleton<cloudscribe.Core.Identity.ICookieAuthTicketStoreProvider, cloudscribe.Core.Identity.CookieAuthDistributedCacheTicketStoreProvider>();

           
            //services.AddMojoPortalPasswordMigration();

            //services.AddCloudscribeCore(Configuration);
            services.AddCloudscribeCoreMvc(config);

            services.AddScoped<IGuardNeededRoles, DemoRoleGuard>();

            var useWindowsCompatLdap = config.GetValue<bool>("DevOptions:UseWindowsCompatLdap");
            if(useWindowsCompatLdap)
            {
                services.AddCloudscribeLdapWindowsSupport(config);
            }
            else
            {
                services.AddCloudscribeLdapSupport(config);
            }
            

            // this was just for testing expired password reset token
            //services.Configure<DataProtectionTokenProviderOptions>(options =>
            //         options.TokenLifespan = TimeSpan.FromMinutes(3));


            return services;
        }

    }
}
