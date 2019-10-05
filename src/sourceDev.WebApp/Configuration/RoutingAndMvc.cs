using cloudscribe.Core.Web.Components;
using cloudscribe.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {
        public static IEndpointRouteBuilder UseCustomRoutes(this IEndpointRouteBuilder routes, bool useFolders)
        {
            routes.AddCloudscribeFileManagerRoutes();

            if (useFolders)
            {
                routes.MapControllerRoute(
                   name: "foldererrorhandler",
                   pattern: "{sitefolder}/oops/error/{statusCode?}",
                   defaults: new { controller = "Oops", action = "Error" },
                   constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                );

                routes.MapControllerRoute(
                       name: "apifoldersitemap",
                       pattern: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapControllerRoute(
                    name: "folderdefault-localized",
                    pattern: "{sitefolder}/{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) });


                routes.MapControllerRoute(
                    name: "folderdefault",
                    pattern: "{sitefolder}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() });


            }

            //routes.MapControllerRoute(
            //   name: "stateautosuggest",
            //   pattern: "coredata/stateautosuggestjson",
            //   defaults: new { controller = "CoreDaata", action = "StateAutoSuggestJson" }
            //   );

            routes.MapControllerRoute(
               name: "errorhandler",
               pattern: "oops/error/{statusCode?}",
               defaults: new { controller = "Oops", action = "Error" }
               );

            //routes.MapControllerRoute(
            //    name: "systlog",
            //    pattern: "systemlog/{action=Index}"
            //    //, defaults: new { action = "Index" }
            //    );

            //routes.MapControllerRoute(
            //    name: "predefault",
            //    pattern: "{controller}/{action}"
            //    , defaults: new { action = "Index" }
            //    );

            routes.MapControllerRoute(
                    name: "default-localized",
                    pattern: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );


            routes.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}"
                ,defaults: new { controller = "Home", action = "Index" }
                );


            return routes;
        }

        public static IServiceCollection SetupMvc(
            this IServiceCollection services,
            IConfiguration config,
            bool sslIsAvailable
            )
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
               // options.ConstraintMap.Add("sitefolder", typeof(SiteFolderRouteConstraint));
            });


            services.Configure<MvcOptions>(options =>
            {
                if (sslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                options.CacheProfiles.Add("SiteMapCacheProfile",
                    new CacheProfile
                    {
                        Duration = 30
                    });

            });
            

           

            services.AddMvc(options => {
                //options.EnableEndpointRouting = false;
               
            })
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddRazorOptions(options =>
                    {
                        options.AddCloudscribeViewLocationFormats();
                        options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());

                    })
                    //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    ;

            return services;
        }

    }
}
