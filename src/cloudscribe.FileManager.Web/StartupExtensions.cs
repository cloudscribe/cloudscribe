﻿using cloudscribe.FileManager.Web.Controllers;
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
            services.TryAddScoped<IMediaPathResolver, DefaultMediaPathResolver>();
            services.AddScoped<IVersionProvider, FileManagerVersionProvider>();

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

            

            return routes;
        }

        public static IEndpointRouteBuilder AddCloudscribeFileManagerRoutes(this IEndpointRouteBuilder routes)
        {
            routes.MapControllerRoute(
               name: "filemanagerjs",
               pattern: "filemanager/js/{*slug}"
               , defaults: new { controller = "FileManager", action = "js" }
               );

            routes.MapControllerRoute(
               name: "filemanagercss",
               pattern: "filemanager/css/{*slug}"
               , defaults: new { controller = "FileManager", action = "css" }
               );

            return routes;
        }
    }
}
