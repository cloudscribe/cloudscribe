using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerIntegration
    {
        public static IServiceCollection SetupIdentityServerIntegrationAndCORSPolicy(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment environment,
            //ILogger log,
            bool sslIsAvailable,
            bool disableIdentityServer,
            out bool didSetupIdServer
            )
        {
            didSetupIdServer = !disableIdentityServer;
            if(disableIdentityServer)
            {
                return services;
            }

            var storage = config["DevOptions:DbPlatform"];
            var efProvider = config["DevOptions:EFProvider"];

            var tmpKeyPath = Path.Combine(environment.ContentRootPath, "tempkey.rsa");

            switch (storage)
            {
                case "NoDb":
                    services.AddIdentityServerConfiguredForCloudscribe()
                        .AddCloudscribeCoreNoDbIdentityServerStorage()
                        .AddCloudscribeIdentityServerIntegrationMvc()
                        .AddDeveloperSigningCredential(true, tmpKeyPath)
                        ;
                   
                    break;

                case "ef":
                default:

                    switch (efProvider)
                    {
                        case "sqlite":
                            //var slConnection = config.GetConnectionString("SQLiteEntityFrameworkConnectionString");
                            var dbName = config.GetConnectionString("SQLiteDbName");
                            var dbPath = Path.Combine(environment.ContentRootPath, dbName);
                            var slConnection = $"Data Source={dbPath}";

                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoreEFIdentityServerStorageSQLite(slConnection)
                                .AddCloudscribeIdentityServerIntegrationMvc()
                                .AddDeveloperSigningCredential(true, tmpKeyPath)
                            ;
                            
                            break;

                        case "pgsql-old":
                            var pgConnection = config.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            
                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoreEFIdentityServerStoragePostgreSql(pgConnection)
                                .AddCloudscribeIdentityServerIntegrationMvc()
                                .AddDeveloperSigningCredential(true, tmpKeyPath)
                            ;
                            
                            break;

                        case "pgsql":
                            var pgsConnection = config.GetConnectionString("PostgreSqlConnectionString");

                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoredentityServerPostgreSqlStorage(pgsConnection)
                                .AddCloudscribeIdentityServerIntegrationMvc()
                                .AddDeveloperSigningCredential(true, tmpKeyPath)
                            ;

                            break;

                        case "MySql":
                            var mysqlConnection = config.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            
                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoreEFIdentityServerStorageMySql(mysqlConnection)
                                .AddCloudscribeIdentityServerIntegrationMvc()
                                .AddDeveloperSigningCredential(true, tmpKeyPath)
                                ;
                           
                            break;

                        case "MSSQL":
                        default:
                            var connectionString = config.GetConnectionString("EntityFrameworkConnectionString");
                            
                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoreEFIdentityServerStorageMSSQL(connectionString)
                                .AddCloudscribeIdentityServerIntegrationMvc()
                                .AddDeveloperSigningCredential(true, tmpKeyPath)
                                ;
                            
                            break;
                    }
                    
                    break;
            }

            

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

            return services;
        }

        public static IServiceCollection SetupIdentityServerApiAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:44399";

                    options.ApiName = "idserverapi";
                    options.ApiSecret = "secret";
                    options.RequireHttpsMetadata = false;
                });

            return services;
        }

    }
}
