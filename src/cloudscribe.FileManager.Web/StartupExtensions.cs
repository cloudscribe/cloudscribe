using cloudscribe.FileManager.Web.Controllers;
using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Services;
using cloudscribe.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeFileManager(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.TryAddScoped<FileManagerService>();
            services.TryAddScoped<IImageResizer, ImageSharpResizer>();
            services.TryAddScoped<IFileManagerNameRules, DefaultFileManagerNameRules>();
            //services.TryAddScoped<IFileExtensionValidationRegexBuilder, FileExtensionValidationRegexBuilder>();
            services.TryAddScoped<IMediaPathResolver, DefaultMediaPathResolver>();
            services.AddScoped<IVersionProvider, FileManagerVersionProvider>();



            // Angular's default header name for sending the XSRF token.
            //services.AddAntiforgery(options =>
            //{
            //    options.HeaderName = "X-XSRF-TOKEN";
            //    options.Cookie.
            //}
                  
            //);

            if(configuration != null)
            {
                services.Configure<FileManagerIcons>(configuration.GetSection("FileManagerIcons"));
                services.Configure<AutomaticUploadOptions>(configuration.GetSection("AutomaticUploadOptions"));
            }

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRFToken";
            });

            return services;
        }

        public static IRouteBuilder AddCloudscribeFileManagerRoutes(this IRouteBuilder routes)
        {
            routes.MapRoute(
               name: "filemanagerjs",
               template: "filemanager/js/{*slug}"
               , defaults: new { controller = "FileManager", action = "js" }
               );

            routes.MapRoute(
               name: "filemanagercss",
               template: "filemanager/css/{*slug}"
               , defaults: new { controller = "FileManager", action = "css" }
               );

            //routes.MapRoute(
            //   name: "filemanagerfonts",
            //   template: "filemanager/fonts/{*slug}"
            //   , defaults: new { controller = "FileManager", action = "fonts" }
            //   );

            return routes;
        }

        [Obsolete("AddBootstrap3EmbeddedViewsForFileManager is deprecated, please use AddCloudscribeFileManagerBootstrap3Views instead.")]
        public static RazorViewEngineOptions AddBootstrap3EmbeddedViewsForFileManager(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(FileManagerController).GetTypeInfo().Assembly,
                    "cloudscribe.FileManager.Web"
                ));

            return options;
        }

        public static RazorViewEngineOptions AddCloudscribeFileManagerBootstrap3Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(FileManagerController).GetTypeInfo().Assembly,
                    "cloudscribe.FileManager.Web"
                ));

            return options;
        }

        public static RazorViewEngineOptions AddCloudscribeFileManagerBootstrap4Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(FileManagerController).GetTypeInfo().Assembly,
                    "cloudscribe.FileManager.Web"
                ));

            return options;
        }

    }
}
