// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-07-11
// 

using System;
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
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
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
    public static class CloudscribeCoreServiceCollectionExtensions
    {
        /// <summary>
        /// Setup dependency injection for cloudscribe components
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCloudscribeCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInstance<IConfiguration>(configuration);
            services.AddCaching();
            
            
            //*** Database platform ****************************************************************
            // here is where you could change to use one of the other db platforms
            // we have support for MySql, PostgreSql, Firebird, SQLite, and SqlCe
            // as of 2015-06-24 those can only be used in the full desktop framework (there are not yet ado.net drivers that support dnxcore50 but they should be available at some point)
            // so you would have to remove the dnxcore50 from the project.json in this project
            // add a nuget for one of the other cloudscribe.Core.Repositories.dbplatform 
            // and cloudscribe.DbHelpers.dbplatform packages
            services.TryAdd(ServiceDescriptor.Scoped<ISiteRepository, cloudscribe.Core.Repositories.MSSQL.SiteRepository>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserRepository, cloudscribe.Core.Repositories.MSSQL.UserRepository>());
            services.TryAdd(ServiceDescriptor.Scoped<IGeoRepository, cloudscribe.Core.Repositories.MSSQL.GeoRepository>());
            services.TryAdd(ServiceDescriptor.Scoped<IDb, cloudscribe.DbHelpers.MSSQL.Db>());
            //**************************************************************************************

            // RequestSiteResolver resolves ISiteSettings based on the request to support multi tenancy based on either host name or first folder depending on configuration
            services.TryAdd(ServiceDescriptor.Scoped<ISiteResolver, RequestSiteResolver>());

            // VersionProviders are used by the Setup controller to determine what install and upgrade scripts to run
            services.TryAdd(ServiceDescriptor.Scoped<IVersionProviderFactory, ConfigVersionProviderFactory>());

            //****** cloudscribe implementation of AspNet.Identity****************************************************
            services.TryAdd(ServiceDescriptor.Scoped<IUserStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserPasswordStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserEmailStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserLoginStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserRoleStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserClaimStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserPhoneNumberStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserLockoutStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserTwoFactorStore<SiteUser>, UserStore<SiteUser>>());
            services.TryAdd(ServiceDescriptor.Scoped<IRoleStore<SiteRole>, RoleStore<SiteRole>>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>());
            // the DNX451 desktop version of SitePasswordHasher can validate against existing hashed or encrypted passwords from mojoportal users
            // so to use existing users from mojoportal you would have to run on the desktop version at least until all users update their passwords
            // then you could migrate to dnxcore50
            // it also alllows us to create a default admin@admin.com user with administrator role with a cleartext password which would be updated 
            // to the default identity hash as soon as you change the password from its default "admin"
            services.TryAdd(ServiceDescriptor.Transient<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>());
            services.AddIdentity<SiteUser, SiteRole>();
            //********************************************************************************************************

            // you can use either json or xml to maintain your navigation map we provide examples of each navigation.xml and 
            // navigation.json in the root of this project
            // you can override the name of the file used with AppSettings:NavigationXmlFileName or AppSettings:NavigationJsonFileName in config.json
            // the file must live in the root of the web project code not in wwwroot

            // it is arguable which is easier for humans to read and maintain, myself I think for something like a navigation tree
            // that could get large xml is easier to work with and not make mistakes. in json one missing or extra comma can break it
            // granted xml can be broken by typos too but the end tags make it easier to keep track of where you are imho (JA)
            //services.TryAdd(ServiceDescriptor.Scoped<INavigationTreeBuilder, JsonNavigationTreeBuilder>());
            services.TryAdd(ServiceDescriptor.Scoped<INavigationTreeBuilder, XmlNavigationTreeBuilder>());

            

            services.TryAdd(ServiceDescriptor.Scoped<INavigationNodePermissionResolver, NavigationNodePermissionResolver>());
            services.TryAdd(ServiceDescriptor.Transient<IBuildPaginationLinks, PaginationLinkBuilder>());


            // Add MVC services to the services container.
            services.AddMvc().Configure<MvcOptions>(options =>
            {
                options.ViewEngines.Clear();
                // cloudscribe.Core.Web.CoreViewEngine adds /Views/Sys as the last place to search for views
                // cloudscribe views are all under Views/Sys
                // to modify a view just copy it to a higher priority location
                // ie copy /Views/Sys/Manage/*.cshtml up to /Views/Manage/ and that one will have higher priority
                // and you can modify it however you like
                // upgrading to newer versions of cloudscribe could modify or add views below /Views/Sys
                // so you may need to compare your custom views to the originals again after upgrades
                options.ViewEngines.Add(typeof(CoreViewEngine));
            });

            return services;
        }

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


            return app;
        }

    }
}
