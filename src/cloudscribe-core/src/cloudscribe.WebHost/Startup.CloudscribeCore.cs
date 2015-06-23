// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-06-23
// 

using System;
//using Microsoft.AspNet.Hosting;
using JetBrains.Annotations;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Builder;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.Components;
using cloudscribe.AspNet.Identity;

namespace cloudscribe.WebHost
{
    public static class CloudscribeCoreServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureCloudscribeCore(this IServiceCollection services)
        {

            services.TryAdd(ServiceDescriptor.Scoped<ISiteRepository, cloudscribe.Core.Repositories.MSSQL.SiteRepository>());
            services.TryAdd(ServiceDescriptor.Scoped<IUserRepository, cloudscribe.Core.Repositories.MSSQL.UserRepository>());
            services.TryAdd(ServiceDescriptor.Scoped<IGeoRepository, cloudscribe.Core.Repositories.MSSQL.GeoRepository>());
            services.TryAdd(ServiceDescriptor.Scoped<IDb, cloudscribe.DbHelpers.MSSQL.Db>());

            services.TryAdd(ServiceDescriptor.Scoped<ISiteResolver, RequestSiteResolver>());
            services.TryAdd(ServiceDescriptor.Scoped<IVersionProviderFactory, ConfigVersionProviderFactory>());

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

            services.AddIdentity<SiteUser, SiteRole>();

            return services;
        }

        public static IApplicationBuilder UseCloudscribeCore(this IApplicationBuilder app, IConfiguration config)
        {

            app.Use(
                next =>
                {

                    return async ctx =>
                    {

                        //await ctx.Response.WriteAsync("Hello from IApplicationBuilder.Use!\n");




                        await next(ctx);
                    };
                });

            app.Use(async (ctx, next) =>
            {
                //ctx.Items.Add("foo", "foo");
                //ISiteRepository siteRepository = ctx.ApplicationServices.GetService<ISiteRepository>();
                //if(siteRepository != null)
                //{
                //    ISiteResolver siteResolver = new RequestSiteResolver(siteRepository,
                //        ctx.Request.Host.Value,
                //        ctx.Request.Path.Value);

                //    // adding to httpcontext.items
                //    // would rather add it to the container (ApplicationServices)
                //    ctx.Items.Add("ISiteResolver", siteResolver);
                //    // and now how can we get this as a dependency
                //}

                //IGreeter greeter = ctx.ApplicationServices.GetService<IGreeter>();
                //await ctx.Response.WriteAsync(greeter.Greet());

                await next();
            });

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

                if (ctx.Request.Headers.Get("Host").Equals("customer1.cloudservice.net"))
                {
                    foundHost = "foo";
                    return true;
                }

                return false;

            }, app2 =>
            {
                if (!string.IsNullOrEmpty(foundHost))
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


            return app;
        }

    }
}
