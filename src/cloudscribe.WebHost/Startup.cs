
using Autofac;
using Autofac.Integration.Mvc;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Repositories.Caching;
using cloudscribe.Core.Web;
using log4net;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using MvcSiteMapProvider.Loader;
using Owin;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(cloudscribe.WebHost.Startup))]
namespace cloudscribe.WebHost
{
    public partial class Startup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Startup));

        private IDataProtectionProvider dataProtectionProvider = null;
        private IContainer container = null;
        
        public void Configuration(IAppBuilder app)
        {
            //http://benfoster.io/blog/how-to-write-owin-middleware-in-5-different-steps

            var builder = new ContainerBuilder();
            RegisterServices(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterControllers(typeof(cloudscribe.Core.Web.SiteContext).Assembly).InstancePerRequest();

            builder.RegisterModule(new cloudscribe.WebHost.DI.Autofac.Modules.MvcSiteMapProviderModule()); // Required
            builder.RegisterModule(new cloudscribe.WebHost.DI.Autofac.Modules.MvcModule());

            container = builder.Build();
            
            // this needed by SiteUserManager so must be set on the sitecontext first
            dataProtectionProvider = app.GetDataProtectionProvider();

            ISiteContext siteContext = GetSiteContext();

            var newBuilder = new ContainerBuilder();
            newBuilder.RegisterInstance(siteContext).As<ISiteContext>();
            newBuilder.Update(container);

            

            app.CreatePerOwinContext(GetSiteContext);

            app.UseAutofacMiddleware(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            HttpConfiguration config = new HttpConfiguration();
            app.UseAutofacWebApi(GlobalConfiguration.Configuration);

            // Setup global sitemap loader (required)
            MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            ConfigureAuth(app);
        }

        private ISiteContext GetSiteContext()
        {
           
            ISiteRepository siteRepo = container.Resolve<ISiteRepository>();
            CachingSiteRepository siteCache = new CachingSiteRepository(siteRepo);

            IUserRepository userRepo = container.Resolve<IUserRepository>(); 
            CachingUserRepository userCache = new CachingUserRepository(userRepo);

            SiteContext siteContext
                = new SiteContext(siteCache, userCache, dataProtectionProvider);
            
            return siteContext;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            
            switch(AppSettings.DbPlatform.ToLower())
            {

                //case "sqlite":

                //    builder.RegisterType<cloudscribe.Core.Repositories.SQLite.SiteRepository>().As<ISiteRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.SQLite.UserRepository>().As<IUserRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.SQLite.GeoRepository>().As<IGeoRepository>();
                //    builder.RegisterType<cloudscribe.DbHelpers.SQLite.Db>().As<IDb>();
                    
                //    break;

                case "sqlce":

                    builder.RegisterType<cloudscribe.Core.Repositories.SqlCe.SiteRepository>().As<ISiteRepository>();
                    builder.RegisterType<cloudscribe.Core.Repositories.SqlCe.UserRepository>().As<IUserRepository>();
                    builder.RegisterType<cloudscribe.Core.Repositories.SqlCe.GeoRepository>().As<IGeoRepository>();
                    builder.RegisterType<cloudscribe.DbHelpers.SqlCe.Db>().As<IDb>();

                    break;

                //case "pgsql":

                //    builder.RegisterType<cloudscribe.Core.Repositories.pgsql.SiteRepository>().As<ISiteRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.pgsql.UserRepository>().As<IUserRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.pgsql.GeoRepository>().As<IGeoRepository>();
                //    builder.RegisterType<cloudscribe.DbHelpers.pgsql.Db>().As<IDb>();

                //    break;

                //case "firebird":
 
                //    builder.RegisterType<cloudscribe.Core.Repositories.Firebird.SiteRepository>().As<ISiteRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.Firebird.UserRepository>().As<IUserRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.Firebird.GeoRepository>().As<IGeoRepository>();
                //    builder.RegisterType<cloudscribe.DbHelpers.Firebird.Db>().As<IDb>();

                //    break;

                //case "mysql":

                //    builder.RegisterType<cloudscribe.Core.Repositories.MySql.SiteRepository>().As<ISiteRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.MySql.UserRepository>().As<IUserRepository>();
                //    builder.RegisterType<cloudscribe.Core.Repositories.MySql.GeoRepository>().As<IGeoRepository>();
                //    builder.RegisterType<cloudscribe.DbHelpers.MySql.Db>().As<IDb>();

                //    break;

                case "mssql":
                default:

                    builder.RegisterType<cloudscribe.Core.Repositories.MSSQL.SiteRepository>().As<ISiteRepository>();
                    builder.RegisterType<cloudscribe.Core.Repositories.MSSQL.UserRepository>().As<IUserRepository>();
                    builder.RegisterType<cloudscribe.Core.Repositories.MSSQL.GeoRepository>().As<IGeoRepository>();
                    builder.RegisterType<cloudscribe.DbHelpers.MSSQL.Db>().As<IDb>();

                    break;

            }

            builder.RegisterType<cloudscribe.Core.Web.Components.WebConfigStartupTrigger>()
                .As<cloudscribe.Core.Models.Site.ITriggerStartup>();

            //

        } 

        
    }
}
