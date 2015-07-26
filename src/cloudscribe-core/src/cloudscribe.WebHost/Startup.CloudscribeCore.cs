// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-07-24
// 

using System;
using System.Collections.Generic;
//using Microsoft.AspNet.Hosting;
using JetBrains.Annotations;
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
            app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(20));
            app.UseStatusCodePages();

            bool useFolderSites = config.UseFoldersInsteadOfHostnamesForMultipleSites();
            ISiteRepository siteRepo = app.ApplicationServices.GetService<ISiteRepository>();





            //// Add cookie-based authentication to the request pipeline.
            ////https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/BuilderExtensions.cs
            //app.UseIdentity();

            app.UseWhen(IsNotFolderMatch,
               branchApp =>
               {
                   branchApp.UseCookieAuthentication(options =>
                   {
                       options.LoginPath = new PathString("/Account/Login");
                       options.LogoutPath = new PathString("/Account/LogOff");
                       options.CookieName = "cloudscribe-ext";
                       options.SlidingExpiration = true;
                       //options.CookiePath = "/";
                   },
                    IdentityOptions.ExternalCookieAuthenticationScheme
                    );

                   branchApp.UseCookieAuthentication(options =>
                   {
                       options.LoginPath = new PathString("/Account/Login");
                       options.LogoutPath = new PathString("/Account/LogOff");
                       options.CookieName = "cloudscribe-tfr";
                       options.SlidingExpiration = true;
                       //options.CookiePath = "/";
                   },
                   IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme
                   );

                   branchApp.UseCookieAuthentication(options =>
                   {
                       options.LoginPath = new PathString("/Account/Login");
                       options.LogoutPath = new PathString("/Account/LogOff");
                       options.CookieName = "cloudscribe-tf";
                       options.SlidingExpiration = true;
                       //options.CookiePath = "/";
                   },
                   IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme
                   );

                   branchApp.UseCookieAuthentication(options =>
                   {
                       options.LoginPath = new PathString("/Account/Login");
                       options.LogoutPath = new PathString("/Account/LogOff");
                       options.CookieName = "cloudscribe-app";
                       options.SlidingExpiration = true;
                       //options.CookiePath = "/";
                   },
                   IdentityOptions.ApplicationCookieAuthenticationScheme
                   );

                   

               });


            if (useFolderSites)
            {
                app.UseCloudscribeCoreFolderTenantsv2(config, siteRepo);
            }
            else
            {
                app.UseCloudscribeCoreHostTenants(config, siteRepo);

            }


            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                //if you are adding custom routes you should probably put them first
                // add your routes here


                // default routes for folder site go second to last
                if (useFolderSites)
                {
                    RegisterFolderSiteDefaultRoutes(routes, siteRepo);
                }


                // the default route has to be added last
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            
            

            //app.UseCookieAuthentication(options =>
            //{
            //    options.LoginPath = new PathString("/Account/Login");
            //    options.LogoutPath = new PathString("/Account/LogOff");
            //    options.CookieName = "cloudscribereee-app";
            //    options.SlidingExpiration = true;
            //});










            return app;



            


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

        public static IApplicationBuilder UseCloudscribeCoreFolderTenants(
            this IApplicationBuilder app,
            IConfiguration config,
            ISiteRepository siteRepo)
        {

            List<SiteFolder> allFolders = siteRepo.GetAllSiteFoldersNonAsync();


            foreach (SiteFolder f in allFolders)
            {
                PathString path = new PathString("/" + f.FolderName);
                app.Map(path,
                siteApp =>
                {
                    //http://stackoverflow.com/questions/31638100/resolving-js-and-css-urls-affected-by-app-map-in-asp-net-5
                    // when we branch on a path like this it is like a virtual directory
                    // urls with ~/ are resolved as /foldername
                    // so our css and json files all get resolved with /foldername
                    // this makes it find those files even though they don't actually exist on disk there
                    siteApp.UseStaticFiles();

                    //ISiteSettings siteSettings = siteRepo.FetchNonAsync(f.SiteGuid);
                    //siteApp.UseIdentity();


                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + f.FolderName + "/Account/Login");
                        options.LogoutPath = new PathString("/" + f.FolderName + "/Account/LogOff");
                        options.CookieName = f.FolderName + "-ext";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + f.FolderName;
                    },
                    IdentityOptions.ExternalCookieAuthenticationScheme
                    );

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + f.FolderName + "/Account/Login");
                        options.LogoutPath = new PathString("/" + f.FolderName + "/Account/LogOff");
                        options.CookieName = f.FolderName + "-tfr";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + f.FolderName;
                    },
                    IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme
                    );

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + f.FolderName + "/Account/Login");
                        options.LogoutPath = new PathString("/" + f.FolderName + "/Account/LogOff");
                        options.CookieName = f.FolderName + "-tf";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + f.FolderName;
                    },
                    IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme
                    );

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + f.FolderName + "/Account/Login");
                        options.LogoutPath = new PathString("/" + f.FolderName + "/Account/LogOff");
                        options.CookieName = f.FolderName + "-app";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + f.FolderName;
                    },
                    IdentityOptions.ApplicationCookieAuthenticationScheme
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


                });
            }

            return app;
        }

        private static bool IsNotFolderMatch(HttpContext context)
        {
            if(IsFolderMatch(context)) { return false; }

            return true;
        }

        private static string firstFolderSegment = string.Empty;
        private static Guid siteGuid = Guid.Empty;
        private static ISiteRepository siteRepository = null;

        private static bool IsFolderMatch(HttpContext context)
        {
            firstFolderSegment = RequestSiteResolver.GetFirstFolderSegment(context.Request.Path);
            if (string.IsNullOrWhiteSpace(firstFolderSegment)) { return false; }

            List<SiteFolder>  allFolders = siteRepository.GetAllSiteFoldersNonAsync();
            foreach (SiteFolder folder in allFolders)
            {
                if (folder.FolderName == firstFolderSegment)
                {
                    siteGuid = folder.SiteGuid;
                    return true;
                }
            }


            return false;
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

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + firstFolderSegment + "/Account/Login");
                        options.LogoutPath = new PathString("/" + firstFolderSegment + "/Account/LogOff");
                        options.CookieName = firstFolderSegment + "-ext";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + firstFolderSegment;
                        //options.AuthenticationScheme = 
                    },
                    IdentityOptions.ExternalCookieAuthenticationScheme
                    );

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + firstFolderSegment + "/Account/Login");
                        options.LogoutPath = new PathString("/" + firstFolderSegment + "/Account/LogOff");
                        options.CookieName = firstFolderSegment + "-tfr";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + firstFolderSegment;
                    },
                    IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme
                    );

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + firstFolderSegment + "/Account/Login");
                        options.LogoutPath = new PathString("/" + firstFolderSegment + "/Account/LogOff");
                        options.CookieName = firstFolderSegment + "-tf";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + firstFolderSegment;
                    },
                    IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme
                    );

                    siteApp.UseCookieAuthentication(options =>
                    {
                        options.LoginPath = new PathString("/" + firstFolderSegment + "/Account/Login");
                        options.LogoutPath = new PathString("/" + firstFolderSegment + "/Account/LogOff");
                        options.CookieName = firstFolderSegment + "-app";
                        options.SlidingExpiration = true;
                        //options.CookiePath = "/" + firstFolderSegment;
                    },
                    IdentityOptions.ApplicationCookieAuthenticationScheme
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

                    //app.UseMicrosoftAccountAuthentication(options =>
                    //{
                    //    options.ClientId = "";
                    //    options.ClientSecret = "";
                    //});

                    //app.UseTwitterAuthentication(options =>
                    //{
                    //    options.ConsumerKey = "";
                    //    options.ConsumerSecret = "";
                    //});

                    //siteApp.UseMvc(routes =>
                    //{
                    //    //if you are adding custom routes you should probably put them first
                    //    // add your routes here

                    //    //routes.MapRoute(
                    //    //    name: "default",
                    //    //    template: f.FolderName + "/{controller}/{action}/{id?}",
                    //    //    defaults: new { controller = "Home", action = "Index" }
                    //    //    //,constraints: new { name = new SiteFolderRouteConstraint(f.FolderName)}
                    //    //    );

                    //    // the default route has to be added last
                    //    routes.MapRoute(
                    //        name: "default",
                    //        template: "{controller}/{action}/{id?}",
                    //        defaults: new { controller = "Home", action = "Index" });



                    //    // Uncomment the following line to add a route for porting Web API 2 controllers.
                    //    // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
                    //});


                });

            return app;
        }

        public static IApplicationBuilder UseCloudscribeCoreHostTenants(
            this IApplicationBuilder app,
            IConfiguration config,
            ISiteRepository siteRepo)
        {

            //TODO: implement



            return app;
        }



        //app.Use(
        //    next =>
        //    {

        //        return async ctx =>
        //        {

        //            //await ctx.Response.WriteAsync("Hello from IApplicationBuilder.Use!\n");




        //            await next(ctx);
        //        };
        //    });

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

        //    //IGreeter greeter = ctx.ApplicationServices.GetService<IGreeter>();
        //    //await ctx.Response.WriteAsync(greeter.Greet());

        //    await next();
        //});

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


        //string foundHost = string.Empty;
        //app.MapWhen(ctx => {

        //    if (ctx.Request.Headers.Get("Host").Equals("customer1.cloudservice.net"))
        //    {
        //        foundHost = "foo";
        //        return true;
        //    }

        //    return false;

        //}, app2 =>
        //{
        //    if (!string.IsNullOrEmpty(foundHost))
        //    {
        //        //app2.UseIdentity();

        //        //CookieAuthenticationOptions cookieOptions = new CookieAuthenticationOptions
        //        //{
        //        //    CookieName = "cloudscribe-app",
        //        //    CookiePath = "/",
        //        //    CookieDomain = foundHost,
        //        //    LoginPath = new PathString("/Account/Login"),
        //        //    LogoutPath = new PathString("/Account/Logout")


        //        //};

        //        //app.UseCookieAuthentication(cookieOptions);

        //        //app.UseCookieAuthentication(new CookieAuthenticationOptions
        //        //{
        //        //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //        //    LoginPath = new PathString("/Account/Login"),
        //        //    Provider = new CookieAuthenticationProvider
        //        //    {
        //        //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SiteUserManager, SiteUser>(
        //        //        validateInterval: TimeSpan.FromMinutes(30),
        //        //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //        //    },
        //        //    // here for folder sites we would like to be able to set the cookie name per tenant
        //        //    // ie based on the request, but it seems not possible except in startup
        //        //    CookieName = "cloudscribe-app"

        //        //    //http://aspnet.codeplex.com/SourceControl/latest#Samples/Katana/BranchingPipelines/Startup.cs

        //        //    //http://leastprivilege.com/2012/10/08/custom-claims-principals-in-net-4-5/
        //        //    // maybe we could add a per site claim
        //        //    // or a custom claimprincipal where we can override IsAuthenticated
        //        //    // based on something in addition to the auth cookie
        //        //    //http://msdn.microsoft.com/en-us/library/system.security.claims.claimsprincipal%28v=vs.110%29.aspx
        //        //    //http://msdn.microsoft.com/en-us/library/system.security.principal.iidentity%28v=vs.110%29.aspx
        //        //    // or custom IIdentity
        //        //    //http://msdn.microsoft.com/en-us/library/system.security.claims.claimsidentity%28v=vs.110%29.aspx

        //        //    //http://stackoverflow.com/questions/19763807/how-to-set-a-custom-claimsprincipal-in-mvc-5
        //        //});

        //    }

        //});



    }
}
