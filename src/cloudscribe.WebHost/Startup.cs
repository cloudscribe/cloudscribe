
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web;
using Microsoft.Owin;
using cloudscribe.Core.Repositories.Caching;
using Microsoft.Owin.Security.DataProtection;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.OwinHost;
using Ninject.Web;
using Owin;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using log4net;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Loader;
using cloudscribe.WebHost.DI;
using cloudscribe.WebHost.DI.Ninject.Modules;
using cloudscribe.Configuration;

//http://www.codemag.com/Article/1405071


[assembly: OwinStartupAttribute(typeof(cloudscribe.WebHost.Startup))]
namespace cloudscribe.WebHost
{
    public partial class Startup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Startup));

        private IDataProtectionProvider dataProtectionProvider = null;

        public void Configuration(IAppBuilder app)
        {
            //http://benfoster.io/blog/how-to-write-owin-middleware-in-5-different-steps

            app.CreatePerOwinContext(GetKernel);
            app.UseNinjectMiddleware(GetKernel);


            //HttpConfiguration config = new HttpConfiguration();

            //config.DependencyResolver = new NinjectDependencyResolver(CreateKernel());

            //app.UseWebApi(config);

            // unfortunately this does not seem to work
            // it changes the Request Path to one that matches a rout but still
            // ends in 404
            //app.Use(typeof(UrlRewriterMiddleware), GetSiteRepository());
            //app.UseStageMarker(PipelineStage.Authenticate);

            // this needed by SiteUserManager so must be set on the sitecontext first
            dataProtectionProvider = app.GetDataProtectionProvider();
            app.CreatePerOwinContext(GetSiteContext);
            
            ConfigureAuth(app);
        }

        private ISiteContext GetSiteContext()
        {
            StandardKernel ninjectKernal = GetKernel();
            ISiteRepository siteRepo = ninjectKernal.Get<ISiteRepository>();   
            CachingSiteRepository siteCache = new CachingSiteRepository(siteRepo);

            IUserRepository userRepo = ninjectKernal.Get<IUserRepository>(); 
            CachingUserRepository userCache = new CachingUserRepository(userRepo);

            SiteContext siteContext
                = new SiteContext(siteCache, userCache, dataProtectionProvider);
            
            return siteContext;
        }

        //private static ISiteRepository GetSiteRepository()
        //{
            
        //    // TODO : dependency injection
        //    return SiteContext.GetSiteRepository();
            
        //}



        //private static IUserRepository GetUserRepository()
        //{
        //    // TODO : dependency injection
        //    return SiteContext.GetUserRepository();

        //} 

        private static StandardKernel _kernel = null;
        public static StandardKernel GetKernel()
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel();
                _kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);

                _kernel.Load(Assembly.GetExecutingAssembly());
                //_kernel.Load(Assembly.GetExecutingAssembly(), Assembly.Load("Super.CompositionRoot"));

                

                _kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();


                
                RegisterServices(_kernel);

                if (AppSettings.MvcSiteMapProvider_UseExternalDIContainer)
                {
                    BindSiteMapProvider(_kernel);
                }
                
            }
            return _kernel;
        }

        /// <summary>
        /// Map dependencies here
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            

            // here we could use conditional compilation to map alternate data layers
            //
            //kernel.Bind<ISiteContext>().To<cloudscribe.Core.Web.SiteContext>(); 

            

            switch(AppSettings.DbPlatform.ToLower())
            {
                case "sqlite":

                    kernel.Bind<ISiteRepository>().To<cloudscribe.Core.Repositories.SQLite.SiteRepository>();
                    kernel.Bind<IUserRepository>().To<cloudscribe.Core.Repositories.SQLite.UserRepository>();
                    kernel.Bind<IGeoRepository>().To<cloudscribe.Core.Repositories.SQLite.GeoRepository>();
                    kernel.Bind<IDb>().To<cloudscribe.DbHelpers.SQLite.Db>();

                    break;

                case "sqlce":

                    kernel.Bind<ISiteRepository>().To<cloudscribe.Core.Repositories.SqlCe.SiteRepository>();
                    kernel.Bind<IUserRepository>().To<cloudscribe.Core.Repositories.SqlCe.UserRepository>();
                    kernel.Bind<IGeoRepository>().To<cloudscribe.Core.Repositories.SqlCe.GeoRepository>();
                    kernel.Bind<IDb>().To<cloudscribe.DbHelpers.SqlCe.Db>();

                    break;

                case "pgsql":

                    kernel.Bind<ISiteRepository>().To<cloudscribe.Core.Repositories.pgsql.SiteRepository>();
                    kernel.Bind<IUserRepository>().To<cloudscribe.Core.Repositories.pgsql.UserRepository>();
                    kernel.Bind<IGeoRepository>().To<cloudscribe.Core.Repositories.pgsql.GeoRepository>();
                    kernel.Bind<IDb>().To<cloudscribe.DbHelpers.pgsql.Db>();

                    break;

                case "firebird":

                    kernel.Bind<ISiteRepository>().To<cloudscribe.Core.Repositories.Firebird.SiteRepository>();
                    kernel.Bind<IUserRepository>().To<cloudscribe.Core.Repositories.Firebird.UserRepository>();
                    kernel.Bind<IGeoRepository>().To<cloudscribe.Core.Repositories.Firebird.GeoRepository>();
                    kernel.Bind<IDb>().To<cloudscribe.DbHelpers.Firebird.Db>();

                    break;

                case "mysql":

                    kernel.Bind<ISiteRepository>().To<cloudscribe.Core.Repositories.MySql.SiteRepository>();
                    kernel.Bind<IUserRepository>().To<cloudscribe.Core.Repositories.MySql.UserRepository>();
                    kernel.Bind<IGeoRepository>().To<cloudscribe.Core.Repositories.MySql.GeoRepository>();
                    kernel.Bind<IDb>().To<cloudscribe.DbHelpers.MySql.Db>();

                    break;

                case "mssql":
                default:

                    kernel.Bind<ISiteRepository>().To<cloudscribe.Core.Repositories.MSSQL.SiteRepository>();
                    kernel.Bind<IUserRepository>().To<cloudscribe.Core.Repositories.MSSQL.UserRepository>();
                    kernel.Bind<IGeoRepository>().To<cloudscribe.Core.Repositories.MSSQL.GeoRepository>();
                    kernel.Bind<IDb>().To<cloudscribe.DbHelpers.MSSQL.Db>();

                    break;

            }

        } 

        // this is broken
        // https://github.com/maartenba/MvcSiteMapProvider/issues/288
        private static void BindSiteMapProvider(IKernel container)
        {
            // Setup configuration of DI (required)
            container.Load(new MvcSiteMapProviderModule());
            // Setup global sitemap loader (required)
            
            MvcSiteMapProvider.SiteMaps.Loader = container.Get<ISiteMapLoader>();
            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
            //var validator = container.Get<ISiteMapXmlValidator>();
            //validator.ValidateXml(System.Web.Hosting.HostingEnvironment.MapPath("~/site-static.sitemap"));
            // Register the Sitemaps routes for search engines (optional)
            //XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
            
        }
    }
}
