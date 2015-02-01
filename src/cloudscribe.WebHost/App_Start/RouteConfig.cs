using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using cloudscribe.Configuration;
using cloudscribe.Web.Routing;
using cloudscribe.Core.Models;
using log4net;
using Autofac;


//http://www.c-sharpcorner.com/UploadFile/ff2f08/custom-route-constraints-in-Asp-Net-mvc-5/

namespace cloudscribe.WebHost
{
    public class RouteConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RouteConfig));

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // call the cloudscribe RouteRegister
            // which in turn calls feature sepcific
            // route registrars that implement IRegisterRoutes
            // and configured under /Config/RouteRegistrars
            RouteRegistrar.RegisterRoutes(routes);

            // add local project or other custom routes here

            if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites 
                && AppSettings.RegisterDefaultRoutesForFolderSites)
            {
                try
                {
                    // errors are expected here for new installations until the database
                    // has been setup
                    RegisterFolderSiteDefaultRoutes(routes);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
           
            
            
            // create the default route which handles most common case
            // and uses home if no other controller matches
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
        }

        private static void RegisterFolderSiteDefaultRoutes(RouteCollection routes)
        {
            
            //StandardKernel kernel = Startup.GetKernel();
            //ISiteRepository siteRepo = kernel.Get<ISiteRepository>();
            ISiteRepository siteRepo = DependencyResolver.Current.GetService<ISiteRepository>();
            //IContainer container = Startup.GetContainer();
            //ISiteRepository siteRepo = container.Resolve<ISiteRepository>();

            List<SiteFolder> allFolders = siteRepo.GetAllSiteFoldersNonAsync();
            foreach (SiteFolder f in allFolders)
            {
                routes.MapRoute(
                name: f.FolderName + "Default",
                url: f.FolderName + "/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { name = new SiteFolderRouteConstraint(f.FolderName) }
                );

            }

        }
    }
}
