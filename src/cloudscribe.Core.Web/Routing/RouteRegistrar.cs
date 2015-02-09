using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using log4net;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;

namespace cloudscribe.Web.Routing
{
    public class RouteRegistrar
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RouteRegistrar));

        public static void RegisterRoutes(RouteCollection routes)
        {
            // it may be better to do this in the webhost or custom web project 
            // so their is more flexible control
            //if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites && AppSettings.RegisterDefaultRoutesForFolderSites)
            //{
            //    try
            //    {
            //        RegisterFolderSiteDefaultRoutes(routes);
            //    }
            //    catch(Exception ex)
            //    {
            //        log.Error(ex);
            //    }
            //}
           
            //routes.MapRoute(
            //    name: "Test",
            //    url: "test/{action}/{id}",
            //    defaults: new { controller = "Test", action = "Index", id = UrlParameter.Optional }
            //);


            try
            {
                RoutesConfig registrarConfig = RoutesConfig.GetConfig();

                foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
                {
                    try
                    {
                        registrar.RegisterRoutes(routes);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

           
            
        }

        //public static void RegisterFolderSiteDefaultRoutes(RouteCollection routes)
        //{
        //    // TODO: dependency injection here? how?
        //    ISiteRepository siteRepo = cloudscribe.Core.Web.SiteContext.GetSiteRepository();

        //    List<SiteFolder> allFolders = siteRepo.GetAllSiteFolders();
        //    foreach (SiteFolder f in allFolders)
        //    {
        //        routes.MapRoute(
        //        name: f.FolderName + "Default",
        //        url: f.FolderName + "/{controller}/{action}/{id}",
        //        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        //        constraints: new { name = new SiteFolderRouteConstraint(f.FolderName) }
        //        );

        //    }

        //}

        public static void RegisterApiRoutes(HttpConfiguration config)
        {

            try
            {
                RoutesConfig registrarConfig = RoutesConfig.GetConfig();

                foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
                {
                    try
                    {
                        registrar.Register(config);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
 
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            
            try
            {
                RoutesConfig registrarConfig = RoutesConfig.GetConfig();

                foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
                {
                    try
                    {
                        registrar.RegisterGlobalFilters(filters);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }


        public static void AddDefaultRouteForNewSiteFolder(string siteFolder)
        {
            RouteCollection routes = RouteTable.Routes;
            // first we have to remove the current default route
            // which should always be the last route in the collection
            // otherwise it matches before the folder routes during route resolution
            Route defaultRoute = null;
            foreach(Route r in routes)
            {
                if(r.Url.ToString() == "{controller}/{action}/{id}")
                {
                    defaultRoute = r;
                }
            }
            if(defaultRoute != null)
            {
                routes.Remove(defaultRoute);
            }
                
            routes.MapRoute(
            name: siteFolder + "Default",
            url: siteFolder + "/{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            constraints: new { name = new cloudscribe.Web.Routing.SiteFolderRouteConstraint(siteFolder) }
            );

            // re-create the default route, it has to be the last one
            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }


    }
}