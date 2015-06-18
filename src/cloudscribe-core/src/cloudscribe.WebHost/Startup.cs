using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Routing;
using Microsoft.Data.Entity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using Microsoft.Framework.Runtime;
using cloudscribe.WebHost.Models;

namespace cloudscribe.WebHost
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            var configuration = new Microsoft.Framework.ConfigurationModel.Configuration()
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                configuration.AddUserSecrets();
            }
            configuration.AddEnvironmentVariables();
            Configuration = configuration;

            //env.MapPath
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Application settings to the services container.
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));

            // Add EF services to the services container.
            //https://github.com/aspnet/EntityFramework/blob/dev/src/EntityFramework.Core/Extensions/EntityFrameworkServiceCollectionExtensions.cs
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // Add Identity services to the services container.
            // https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/IdentityServiceCollectionExtensions.cs
            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity.EntityFramework/IdentityRole.cs

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            services.Configure<FacebookAuthenticationOptions>(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });

            // Add MVC services to the services container.
            services.AddMvc();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.

            // Add the console logger.
            loggerfactory.AddConsole(minLevel: LogLevel.Warning);

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/BuilderExtensions.cs
            app.UseIdentity();

            // some examples from http://stackoverflow.com/questions/24422903/setup-owin-dynamically-by-domain

            //app.MapWhen(ctx => ctx.Request.Headers.Get("Host").Equals("customer1.cloudservice.net"), app2 =>
            //{
            //    app2.UseIdentity();
            //});
            //app.MapWhen(ctx => ctx.Request.Headers.Get("Host").Equals("customer2.cloudservice.net"), app2 =>
            //{
            //    app2.UseGoogleAuthentication(...);
            //});

            //app.MapWhen()

            string foundHost = string.Empty;
            app.MapWhen(ctx => {

                if( ctx.Request.Headers.Get("Host").Equals("customer1.cloudservice.net"))
                {
                    foundHost = "foo";
                    return true;
                }

                return false;

            }, app2 =>
            {
                if(!string.IsNullOrEmpty(foundHost))
                {
                    //app2.UseIdentity();

                    //CookieAuthenticationOptions cookieOptions = new CookieAuthenticationOptions
                    //{
                    //    CookieName = "cloudscribe-app",
                    //    CookiePath = "/",
                    //    CookieDomain = foundHost,
                    //    LoginPath = new PathString("/Account/Login"),
                    //    LogoutPath = new PathString("/Account/Logout")
                         
                         
                    //};

                    //app.UseCookieAuthentication(cookieOptions);

                    //app.UseCookieAuthentication(new CookieAuthenticationOptions
                    //{
                    //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    //    LoginPath = new PathString("/Account/Login"),
                    //    Provider = new CookieAuthenticationProvider
                    //    {
                    //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SiteUserManager, SiteUser>(
                    //        validateInterval: TimeSpan.FromMinutes(30),
                    //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                    //    },
                    //    // here for folder sites we would like to be able to set the cookie name per tenant
                    //    // ie based on the request, but it seems not possible except in startup
                    //    CookieName = "cloudscribe-app"

                    //    //http://aspnet.codeplex.com/SourceControl/latest#Samples/Katana/BranchingPipelines/Startup.cs

                    //    //http://leastprivilege.com/2012/10/08/custom-claims-principals-in-net-4-5/
                    //    // maybe we could add a per site claim
                    //    // or a custom claimprincipal where we can override IsAuthenticated
                    //    // based on something in addition to the auth cookie
                    //    //http://msdn.microsoft.com/en-us/library/system.security.claims.claimsprincipal%28v=vs.110%29.aspx
                    //    //http://msdn.microsoft.com/en-us/library/system.security.principal.iidentity%28v=vs.110%29.aspx
                    //    // or custom IIdentity
                    //    //http://msdn.microsoft.com/en-us/library/system.security.claims.claimsidentity%28v=vs.110%29.aspx

                    //    //http://stackoverflow.com/questions/19763807/how-to-set-a-custom-claimsprincipal-in-mvc-5
                    //});

                }
                
            });

            // Add authentication middleware to the request pipeline. You can configure options such as Id and Secret in the ConfigureServices method.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // app.UseFacebookAuthentication();
            // app.UseGoogleAuthentication();
            // app.UseMicrosoftAccountAuthentication();
            // app.UseTwitterAuthentication();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
