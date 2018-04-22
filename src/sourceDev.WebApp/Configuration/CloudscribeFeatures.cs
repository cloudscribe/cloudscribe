using cloudscribe.UserProperties.Models;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = config["DevOptions:DbPlatform"];
            var efProvider = config["DevOptions:EFProvider"];

            switch (storage)
            {
                case "NoDb":
                    services.AddCloudscribeCoreNoDbStorage();
                    services.AddCloudscribeLoggingNoDbStorage(config);
                    services.AddCloudscribeKvpNoDbStorage();
                    
                    break;

                case "ef":
                default:

                    switch (efProvider)
                    {
                        case "sqlite":
                            var slConnection = config.GetConnectionString("SQLiteEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            services.AddCloudscribeKvpEFStorageSQLite(slConnection);

                            break;

                        case "pgsql":
                            var pgConnection = config.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeKvpEFStoragePostgreSql(pgConnection);

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


            //services.AddCloudscribeCore(Configuration);
            services.AddCloudscribeCoreMvc(config);



            return services;
        }

    }
}
