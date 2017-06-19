using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Components;
using cloudscribe.Web.Common.Helpers;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using NodaTime;
using NodaTime.TimeZones;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeCommmon(
            this IServiceCollection services,
            IConfigurationRoot configuration = null)
        {
            services.TryAddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));
            services.TryAddScoped<ITimeZoneHelper, TimeZoneHelper>();
            services.TryAddScoped<IResourceHelper, ResourceHelper>();

            services.TryAddScoped<ITimeZoneIdResolver, GmtTimeZoneIdResolver>();
            services.TryAddScoped<ICkeditorOptionsResolver, DefaultCkeditorOptionsResolver>();

            

            if (configuration != null)
            {
                services.Configure<CkeditorOptions>(configuration.GetSection("CkeditorOptions"));
            }
            else
            {
                
                services.Configure<CkeditorOptions>(c =>
                {
                    // not doing anything just configuring the default
                });
            }
            

            return services;
        }

        public static RazorViewEngineOptions AddCloudscribeCommonEmbeddedViews(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                   typeof(CloudscribeCommonResources).GetTypeInfo().Assembly,
                   "cloudscribe.Web.Common"
               ));

            return options;
        }

        //public static IRouteBuilder AddRoutesForCloudscribeCommonResources(this IRouteBuilder routes)
        //{
            
        //    routes.MapRoute(
        //       name: "crjs",
        //       template: "cr/js/{*slug}"
        //       , defaults: new { controller = "cr", action = "js" }
        //       );

        //    routes.MapRoute(
        //       name: "crcss",
        //       template: "cr/css/{*slug}"
        //       , defaults: new { controller = "cr", action = "css" }
        //       );

        //    routes.MapRoute(
        //       name: "crfonts",
        //       template: "cr/fonts/{*slug}"
        //       , defaults: new { controller = "cr", action = "fonts" }
        //       );

        //    routes.MapRoute(
        //       name: "crimages",
        //       template: "cr/images/{*slug}"
        //       , defaults: new { controller = "cr", action = "images" }
        //       );

        //    return routes;
        //}

        public static IApplicationBuilder UseCloudscribeCommonStaticFiles(this IApplicationBuilder builder)
        {

            builder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new EmbeddedFileResolvingProvider(typeof(CloudscribeCommonResources).GetTypeInfo().Assembly,"cloudscribe.Web.Common")
                , RequestPath = new PathString("/cr")
            });

            return builder;
        }
    }
}
