// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-08-01
// 

using System;
using System.Collections.Generic;
//using Microsoft.AspNet.Hosting;
//using JetBrains.Annotations;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Session;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Caching;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Routing;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Site;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Messaging;
using cloudscribe.Web.Navigation;
using cloudscribe.Core.Identity;

namespace cloudscribe.WebHost
{
    /// <summary>
    /// Setup dependency injection and application configuration for cloudscribe core
    /// 
    /// this file is part of cloudscribe.Core.Integration nuget package
    /// there are only a few lines of cloudscribe specific code in main Startup.cs and those reference methods in this file
    /// you are allowed to modify this file if needed but beware that if you upgrade the nuget package it would overwrite this file
    /// so you should probably make a copy of your changes somewhere first so you could restore them after upgrading
    /// </summary>
    public static class CloudscribeCoreApplicationBuilderExtensions
    {
        

        /// <summary>
        /// application configuration for cloudscribe core
        /// here is where we will need to do some magic for mutli tenants by folder if configured for that as by default
        /// we would also plug in any custom OWIN middleware components here
        /// things that would historically be implemented as HttpModules would now be implemented as OWIN middleware components
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCloudscribeCore(this IApplicationBuilder app, IConfiguration config)
        {

            // the only thing we are using session for is Alerts
            app.UseSession();
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(20));
            app.UseStatusCodePages();

            IOptions<MultiTenantOptions> multiTenantOptions = app.ApplicationServices.GetService<IOptions<MultiTenantOptions>>();

            //bool useFolderSites = config.UseFoldersInsteadOfHostnamesForMultipleSites();
            //bool addFolderRoutesToMainApp = useFolderSites;
            ISiteRepository siteRepo = app.ApplicationServices.GetService<ISiteRepository>();


            //// Add cookie-based authentication to the request pipeline.
            ////https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/BuilderExtensions.cs
            //app.UseIdentity();
            app.UseCloudscribeIdentity();
            
            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                //if you are adding custom routes you should probably put them first
                // add your routes here


                // default routes for folder site go second to last
                try
                {
                    
                    if (multiTenantOptions.Options.Mode == MultiTenantMode.FolderName)
                    {
                        //RegisterFolderSiteDefaultRoutes(routes, siteRepo);

                        routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new SiteFolderRouteConstraint() }
                        );
                    }
                }
                catch { }
                


                // the default route has to be added last
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                    
                    );

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            
            return app;
            
        }

        

        //private static void RegisterFolderSiteDefaultRoutes(IRouteBuilder routes, ISiteRepository siteRepo)
        //{
        //    // exceptions expected here on new install until db has been initialized

        //    List<SiteFolder> allFolders = siteRepo.GetAllSiteFoldersNonAsync();
        //    foreach (SiteFolder f in allFolders)
        //    {
        //        // if you need to make your custom routes "folder aware"
        //        // you can add them here appending the folder to the front of your template
        //        // and giving them each a unique name that does not clash with other routes
        //        // your routes should have more specific template than the default route

        //        // go ahead add your own routes here


        //        // the default route for a folder site should be last
        //        routes.MapRoute(
        //        name: f.FolderName + "Default",
        //        template: f.FolderName + "/{controller}/{action}/{id?}",
        //        defaults: new { controller = "Home", action = "Index" },
        //        constraints: new { name = new SiteFolderRouteConstraint(f.FolderName) }
        //        );

        //    }
        //}

        
        
        
    }
}
