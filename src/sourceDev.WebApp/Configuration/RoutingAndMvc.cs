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
        public static IRouteBuilder UseCustomRoutes(this IRouteBuilder routes, bool useFolders)
        {
            routes.AddCloudscribeFileManagerRoutes();

            if (useFolders)
            {
                routes.MapRoute(
                   name: "foldererrorhandler",
                   template: "{sitefolder}/oops/error/{statusCode?}",
                   defaults: new { controller = "Oops", action = "Error" },
                   constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                );

                routes.MapRoute(
                       name: "apifoldersitemap",
                       template: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapRoute(
                    name: "folderdefault-localized",
                    template: "{sitefolder}/{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) });


                routes.MapRoute(
                    name: "folderdefault",
                    template: "{sitefolder}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() });


            }

            //routes.MapRoute(
            //   name: "stateautosuggest",
            //   template: "coredata/stateautosuggestjson",
            //   defaults: new { controller = "CoreDaata", action = "StateAutoSuggestJson" }
            //   );

            routes.MapRoute(
               name: "errorhandler",
               template: "oops/error/{statusCode?}",
               defaults: new { controller = "Oops", action = "Error" }
               );

            //routes.MapRoute(
            //    name: "systlog",
            //    template: "systemlog/{action=Index}"
            //    //, defaults: new { action = "Index" }
            //    );

            //routes.MapRoute(
            //    name: "predefault",
            //    template: "{controller}/{action}"
            //    , defaults: new { action = "Index" }
            //    );

            routes.MapRoute(
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );


            routes.MapRoute(
                name: "default",
                template: "{controller}/{action}"
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
            

            var boostrapVersion = config.GetValue<int>("DevOptions:BootstrapVersion");

            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
               
            })
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddRazorOptions(options =>
                    {
                        options.AddCloudscribeViewLocationFormats();
                        options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());

                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    ;

            return services;
        }

    }
}
