// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-07-31
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

            bool useFolderSites = config.UseFoldersInsteadOfHostnamesForMultipleSites();
            bool addFolderRoutesToMainApp = useFolderSites;
            ISiteRepository siteRepo = app.ApplicationServices.GetService<ISiteRepository>();

            
            //// Add cookie-based authentication to the request pipeline.
            ////https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/BuilderExtensions.cs
            //app.UseIdentity();

            app.UseWhen(IsNotFolderMatch, //request does not match any folder tenant so it is root site
               branchApp =>
               {
                   string loginPath = "/Account/Login";
                   string logoutPath = "/Account/LogOff";
                   string cookieAuthSchemeSuffix = "-cloudscribeApp";
                   string cookiePath = string.Empty;
                   string cookieNamePrefix = "cloudscribe";
                   
                   ConfigureAppCookieOptions(
                       branchApp,
                       loginPath,
                       logoutPath,
                       cookieAuthSchemeSuffix,
                       cookieNamePrefix,
                       cookiePath
                       );
               });


            // this is ugly that we have to wireup cookie middleware per site
            // it will work ok with a reasonable number of sites
            // but will get worse as the number of sites goes up we end up with too much middleware

            bool useMapBranching = true;

            try
            {
                // errors expected here if db has not yet been initilaized and populated
                if (useFolderSites)
                {

                    if (useMapBranching)
                    {
                        // this one uses app.Map(/folderName
                        addFolderRoutesToMainApp = false; // in this case we have to add folder routes to the branch not the main app
                        app.UseCloudscribeCoreFolderTenants(config, siteRepo);
                    }
                    else
                    {
                        // this one uses app.UseWhen(IsFolderMatch
                        app.UseCloudscribeCoreFolderTenantsv2(config, siteRepo);
                    }



                }
                else
                {
                    app.UseCloudscribeCoreHostTenants(config, siteRepo);

                }
            }
            catch { }
            

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                //if you are adding custom routes you should probably put them first
                // add your routes here


                // default routes for folder site go second to last
                try
                {
                    // exceptions expected here on new install until db has been initialized
                    if (useFolderSites)
                    {
                        RegisterFolderSiteDefaultRoutes(routes, siteRepo);
                    }
                }
                catch { }
                


                // the default route has to be added last
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            
            return app;
            
        }

        

        public static IApplicationBuilder UseCloudscribeCoreFolderTenants(
            this IApplicationBuilder app,
            IConfiguration config,
            ISiteRepository siteRepo)
        {
            siteRepository = siteRepo;

            List<SiteFolder> allFolders = siteRepo.GetAllSiteFoldersNonAsync();

            foreach (SiteFolder f in allFolders)
            {
                PathString path = new PathString("/" + f.FolderName);
                app.Map(path,
                siteApp =>
                {
                    bool useStaticFiles = true;
                    bool addRoutes = true;
                    bool adjustCookiePath = false;

                    ConfigureFolderApp(siteApp, f.FolderName, addRoutes, useStaticFiles, adjustCookiePath);

                });
            }

            return app;
        }

        

        public static IApplicationBuilder UseCloudscribeCoreFolderTenantsv2(
            this IApplicationBuilder app,
            IConfiguration config,
            ISiteRepository siteRepo)
        {
            siteRepository = siteRepo;

            app.UseWhen(IsFolderMatch,
                siteApp =>
                {
                    //ISiteSettings siteSettings = siteRepo.FetchNonAsync(siteGuid);
                    bool useStaticFiles = false;
                    bool addRoutes = false;
                    bool adjustCookiePath = false;

                    ConfigureFolderApp(siteApp, firstFolderSegment, addRoutes, useStaticFiles, adjustCookiePath);

                });

            return app;
        }

        private static void ConfigureFolderApp(
            IApplicationBuilder siteApp, 
            string folderSegment,
            bool addRoutes,
            bool useStaticFiles,
            bool adjustCookiePath)
        {
            //http://stackoverflow.com/questions/31638100/resolving-js-and-css-urls-affected-by-app-map-in-asp-net-5
            // when we branch on a path like this it is like a virtual directory
            // urls with ~/ are resolved as /foldername
            // so our css and json files all get resolved with /foldername
            // this makes it find those files even though they don't actually exist on disk there
            if (useStaticFiles) { siteApp.UseStaticFiles(); }

            //ISiteSettings siteSettings = siteRepo.FetchNonAsync(f.SiteGuid);
            string loginPath = "/" + folderSegment + "/Account/Login";
            string logoutPath = "/" + folderSegment + "/Account/LogOff";
            string cookieAuthSchemeSuffix = "-" + folderSegment;
            string cookiePath = string.Empty;
            string cookieNamePrefix = folderSegment;
            if (adjustCookiePath) { cookiePath = "/" + folderSegment; }

            ConfigureAppCookieOptions(
                siteApp,
                loginPath,
                logoutPath,
                cookieAuthSchemeSuffix,
                cookieNamePrefix,
                cookiePath
                );

            
            
            //TODO: the things could come from site settings

            //siteApp.UseFacebookAuthentication(options =>
            //{
            //    options.AppId = "";
            //    options.AppSecret = "";
            //});

            //siteApp.UseGoogleAuthentication(options =>
            //{
            //    options.ClientId = "";
            //    options.ClientSecret = "";
            //});

            //siteApp.UseMicrosoftAccountAuthentication(options =>
            //{
            //    options.ClientId = "";
            //    options.ClientSecret = "";
            //});

            //siteApp.UseTwitterAuthentication(options =>
            //{
            //    options.ConsumerKey = "";
            //    options.ConsumerSecret = "";
            //});
            if(addRoutes)
            {
                siteApp.UseMvc(routes =>
                {
                    //if you are adding custom routes you should probably put them first
                    // add your routes here

                    //routes.MapRoute(
                    //    name: "default",
                    //    template: f.FolderName + "/{controller}/{action}/{id?}",
                    //    defaults: new { controller = "Home", action = "Index" }
                    //    //,constraints: new { name = new SiteFolderRouteConstraint(f.FolderName)}
                    //    );

                    // the default route has to be added last
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" });



                    // Uncomment the following line to add a route for porting Web API 2 controllers.
                    // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
                });
            }
            

        }

        private static void ConfigureAppCookieOptions(
            IApplicationBuilder siteApp,
            string loginPath,
            string logoutPath,
            string cookieAuthSchemeSuffix,
            string cookieNamePrefix,
            string cookiePath)
        {
            MultiTenantCookieAuthenticationNotifications cookieNotifications 
                = siteApp.ApplicationServices.GetService<MultiTenantCookieAuthenticationNotifications>();

            string externalAuthScheme = IdentityOptions.ExternalCookieAuthenticationScheme;

            if (!string.IsNullOrWhiteSpace(cookieAuthSchemeSuffix))
            {
                externalAuthScheme = "External" + cookieAuthSchemeSuffix;
            }

            siteApp.UseMultiTenantCookieAuthentication(options =>
            {
               
                options.AuthenticationScheme = externalAuthScheme;

                options.LoginPath = new PathString(loginPath);
                options.LogoutPath = new PathString(logoutPath);
                options.CookieName = cookieNamePrefix + "-ext";
                options.SlidingExpiration = true;
                if (cookiePath.Length > 0) { options.CookiePath = cookiePath; }
                options.Notifications = cookieNotifications;
            },
            externalAuthScheme
            );

            string twoFactorRememberMeAuthScheme = IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme;

            if (!string.IsNullOrWhiteSpace(cookieAuthSchemeSuffix))
            {
                twoFactorRememberMeAuthScheme = "TwoFactorRememberMe" + cookieAuthSchemeSuffix;
            }

            siteApp.UseMultiTenantCookieAuthentication(options =>
            {
                options.AuthenticationScheme = twoFactorRememberMeAuthScheme;

                options.LoginPath = new PathString(loginPath);
                options.LogoutPath = new PathString(logoutPath);
                options.CookieName = cookieNamePrefix + "-tfr";
                options.SlidingExpiration = true;
                if (cookiePath.Length > 0) { options.CookiePath = cookiePath; }
                options.Notifications = cookieNotifications;
            },
            twoFactorRememberMeAuthScheme
            );


            string twoFactorUserIdAuthScheme = IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme;

            if (!string.IsNullOrWhiteSpace(cookieAuthSchemeSuffix))
            {
                twoFactorUserIdAuthScheme = "TwoFactorUserId" + cookieAuthSchemeSuffix;
            }

            siteApp.UseMultiTenantCookieAuthentication(options =>
            {
                options.AuthenticationScheme = twoFactorUserIdAuthScheme;

                options.LoginPath = new PathString(loginPath);
                options.LogoutPath = new PathString(logoutPath);
                options.CookieName = cookieNamePrefix + "-tf";
                options.SlidingExpiration = true;
                if (cookiePath.Length > 0) { options.CookiePath = cookiePath; }
                options.Notifications = cookieNotifications;
            },
            twoFactorUserIdAuthScheme
            );

            string applicationAuthScheme = IdentityOptions.ApplicationCookieAuthenticationScheme;

            if (!string.IsNullOrWhiteSpace(cookieAuthSchemeSuffix))
            {
                applicationAuthScheme = "Application" + cookieAuthSchemeSuffix;
            }

            siteApp.UseMultiTenantCookieAuthentication(options =>
            {
                // changing this breaks login causes exceptions
                // trying to overcome it by implementing a custom SignInManager
                options.AuthenticationScheme = applicationAuthScheme;

                options.LoginPath = new PathString(loginPath);
                options.LogoutPath = new PathString(logoutPath);
                options.CookieName = cookieNamePrefix + "-app";
                options.SlidingExpiration = true;
                if (cookiePath.Length > 0) { options.CookiePath = cookiePath; }

                options.Notifications = cookieNotifications;
            },
            applicationAuthScheme
            
            //IdentityOptions.ApplicationCookieAuthenticationScheme
            );
 
        }

        private static string firstFolderSegment = string.Empty;
        private static Guid siteGuid = Guid.Empty;
        private static ISiteRepository siteRepository = null;

        // this is kind of funky our test is also setting these static variables above 
        // because if it is a match we need the foldername and siteGuid to configure the app
        // that is we need information from the HttpContext to configure the app
        // but we don't get access to it after the test
        // seems like in multi threaded with lots of requests these vars could step on each other
        // need a better way than using side effects of our test

        private static bool IsFolderMatch(HttpContext context)
        {
            firstFolderSegment = RequestSiteResolver.GetFirstFolderSegment(context.Request.Path);
            if (string.IsNullOrWhiteSpace(firstFolderSegment)) { return false; }
            try
            {
                List<SiteFolder> allFolders = siteRepository.GetAllSiteFoldersNonAsync();
                foreach (SiteFolder folder in allFolders)
                {
                    if (folder.FolderName == firstFolderSegment)
                    {
                        siteGuid = folder.SiteGuid;
                        return true;
                    }
                }
            }
            catch { }
            


            return false;
        }

        private static bool IsNotFolderMatch(HttpContext context)
        {
            if (IsFolderMatch(context)) { return false; }

            return true;
        }

        private static void RegisterFolderSiteDefaultRoutes(IRouteBuilder routes, ISiteRepository siteRepo)
        {
            List<SiteFolder> allFolders = siteRepo.GetAllSiteFoldersNonAsync();
            foreach (SiteFolder f in allFolders)
            {
                // if you need to make your custom routes "folder aware"
                // you can add them here appending the folder to the front of your template
                // and giving them each a unique name that does not clash with other routes
                // your routes should have more specific template than the default route

                // go ahead add your own routes here


                // the default route for a folder site should be last
                routes.MapRoute(
                name: f.FolderName + "Default",
                template: f.FolderName + "/{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { name = new SiteFolderRouteConstraint(f.FolderName) }
                );

            }
        }

        public static IApplicationBuilder UseCloudscribeCoreHostTenants(
            this IApplicationBuilder app,
            IConfiguration config,
            ISiteRepository siteRepo)
        {

            //TODO: implement



            return app;
        }



        

        //app.Use(async (ctx, next) =>
        //{
        //    //ctx.Items.Add("foo", "foo");
        //    //ISiteRepository siteRepository = ctx.ApplicationServices.GetService<ISiteRepository>();
        //    //if(siteRepository != null)
        //    //{
        //    //    ISiteResolver siteResolver = new RequestSiteResolver(siteRepository,
        //    //        ctx.Request.Host.Value,
        //    //        ctx.Request.Path.Value);

        //    //    // adding to httpcontext.items
        //    //    // would rather add it to the container (ApplicationServices)
        //    //    ctx.Items.Add("ISiteResolver", siteResolver);
        //    //    // and now how can we get this as a dependency
        //    //}


        //    await next();
        //});

        
    }
}
