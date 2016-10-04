using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.DbContexts;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class Startup
    {

        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<IConfigurationModelMapper, SqlServerConfigurationModelMapper>();
            services.TryAddScoped<IPersistedGrantModelMapper, SqlServerPersistedGrantModelMapper>();

            services.AddEntityFrameworkSqlServer()
              .AddDbContext<ConfigurationDbContext>((serviceProvider, options) =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                       .UseInternalServiceProvider(serviceProvider)
                       );

            services.AddEntityFrameworkSqlServer()
              .AddDbContext<PersistedGrantDbContext>((serviceProvider, options) =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                       .UseInternalServiceProvider(serviceProvider)
                       );

            //services.AddDbContext<CoreDbContext>(options =>
            //        options.UseSqlServer(Configuration["Data:EF7ConnectionOptions:ConnectionString"])
            //        );
        }


        public void Configure(IApplicationBuilder app)
        {
        }

    }
}
