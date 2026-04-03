using cloudscribe.Core.IdentityServerIntegration;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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


            services.AddTransient<IRedirectUriValidator, IdServerRedirectValidator>();

            switch (storage.ToLower())
            {
                case "nodb":
                    services.AddIdentityServerConfiguredForCloudscribe()
                        .AddCloudscribeCoreNoDbIdentityServerStorage()
                        .AddCloudscribeIdentityServerIntegrationMvc()
                        .AddDeveloperSigningCredential(true, tmpKeyPath)
                        ;
                   
                    break;

                case "ef":
                default:

                    switch (efProvider.ToLower())
                    {
                        case "sqlite":
                            var slConnection = config.GetConnectionString("SQLiteEntityFrameworkConnectionString");

                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoreEFIdentityServerStorageSQLite(slConnection)
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

                        case "mysql":
                            var mysqlConnection = config.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            
                            services.AddIdentityServerConfiguredForCloudscribe()
                                .AddCloudscribeCoreEFIdentityServerStorageMySql(mysqlConnection)
                                .AddCloudscribeIdentityServerIntegrationMvc()
                                .AddDeveloperSigningCredential(true, tmpKeyPath)
                                ;
                           
                            break;

                        case "mssql":
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:44399";
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = "https://localhost:44399",
                        ValidateLifetime = true,
                        ClockSkew = System.TimeSpan.FromMinutes(5)
                    };

                    // Handle key refresh for modern package compatibility
                    options.RefreshOnIssuerKeyNotFound = true;
                });

            return services;
        }

    }
}
