using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        
        public static IApplicationBuilder UseCloudscribeCore(
           this IApplicationBuilder app,
           ILoggerFactory loggerFactory,
           MultiTenantOptions multiTenantOptions,
           bool sslIsAvailable = false,
           CookieSecurePolicy applicationCookieSecure = CookieSecurePolicy.SameAsRequest
           )
        {
            

            app.UseMultitenancy<cloudscribe.Core.Models.SiteContext>();
            
            app.UsePerTenant<cloudscribe.Core.Models.SiteContext>((ctx, builder) =>
            {
               
                if (!string.IsNullOrWhiteSpace(ctx.Tenant.ForcedCulture) && !string.IsNullOrWhiteSpace(ctx.Tenant.ForcedUICulture))
                {
                    var tenantLocalization = new RequestLocalizationOptions();
                    tenantLocalization.DefaultRequestCulture = new RequestCulture(culture: ctx.Tenant.ForcedCulture, uiCulture: ctx.Tenant.ForcedUICulture);
                    tenantLocalization.SupportedCultures = new[] { new CultureInfo(ctx.Tenant.ForcedCulture) };
                    tenantLocalization.SupportedUICultures = new[] { new CultureInfo(ctx.Tenant.ForcedUICulture) };

                    builder.UseRequestLocalization(tenantLocalization);
                }
                
                // custom 404 and error page - this preserves the status code (ie 404)
                if (multiTenantOptions.Mode != cloudscribe.Core.Models.MultiTenantMode.FolderName || string.IsNullOrEmpty(ctx.Tenant.SiteFolderName))
                {
                    builder.UseStatusCodePagesWithReExecute("/oops/error/{0}");
                }
                else
                {
                    builder.UseStatusCodePagesWithReExecute("/" + ctx.Tenant.SiteFolderName + "/oops/error/{0}");
                }

                // resolve static files from wwwroot folders within themes and within sitefiles
                builder.UseSiteAndThemeStaticFiles(loggerFactory, multiTenantOptions, ctx.Tenant);

                
                //builder.UseAuthentication();
                
            });
            
            app.UseAuthentication();
            app.UseCloudscribeEnforceSiteRulesMiddleware();

            return app;

        }


        public static IApplicationBuilder UseSiteAndThemeStaticFiles(
           this IApplicationBuilder builder,
           ILoggerFactory loggerFactory,
           MultiTenantOptions multiTenantOptions,
           SiteContext tenant
           )
        {
            var tenantSegment = "";
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName && !string.IsNullOrEmpty(tenant.SiteFolderName))
            {
                tenantSegment = tenant.SiteFolderName + "/";  
            }

            var themeName = tenant.Theme;
            bool themeFound = false;
            if (multiTenantOptions.UserPerSiteThemes)
            {
                // this allows serving static files from the "wwwroot" folder beneath the theme folder
                // we don't want to serve the view files over http, but we can serve css and js etc from the static folder beneath the theme folder
                // without serving theme views
                // TODO: we could possibly use the GzipMappingFileProvider here to handle .gz
                // but probably would want a config setting to make that optional
                if (!string.IsNullOrEmpty(themeName))
                {
                    var themePath = Path.Combine(Directory.GetCurrentDirectory(),
                        multiTenantOptions.SiteFilesFolderName,
                        tenant.AliasId,
                        multiTenantOptions.SiteThemesFolderName,
                        themeName,
                        multiTenantOptions.ThemeStaticFilesFolderName);

                    if (Directory.Exists(themePath))
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(themePath),
                            RequestPath = new PathString("/" + tenantSegment + themeName)
                        });
                        themeFound = true;
                    }

                }
            }

            if(!themeFound && multiTenantOptions.UseSharedThemes)
            {
                if (!string.IsNullOrEmpty(themeName))
                {
                    var themePath = Path.Combine(Directory.GetCurrentDirectory(),
                        multiTenantOptions.SharedThemesFolderName,
                        themeName,
                        multiTenantOptions.ThemeStaticFilesFolderName);

                    if (Directory.Exists(themePath))
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(themePath)
                            ,RequestPath = new PathString("/" + tenantSegment + themeName)
                        });
                    }

                }

            }
            
            if(multiTenantOptions.UserPerSiteWwwRoot)
            {
                // this allows serving static files from /sitefiles/[aliasid]/wwwroot
                // so that files can be isolated per tenant
                
                var folderExists = TryEnsureTenantWwwRoot(tenant, multiTenantOptions);

                if (folderExists)
                {
                    var siteFilesPath = Path.Combine(Directory.GetCurrentDirectory(),
                        multiTenantOptions.SiteFilesFolderName,
                        tenant.AliasId,
                        multiTenantOptions.SiteContentFolderName);

                    if (string.IsNullOrEmpty(tenantSegment)) // root tenant or hostname tenant
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(siteFilesPath)
                            //,RequestPath = new PathString("/files")
                        });
                    }
                    else
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(siteFilesPath),
                            RequestPath = new PathString("/" + tenant.SiteFolderName)
                        });
                    }

                }
            }
            

            return builder;
        }

        private static bool TryEnsureTenantWwwRoot(ISiteContext tenant, MultiTenantOptions options)
        {
          
            var siteFilesPath = Path.Combine(Directory.GetCurrentDirectory(), options.SiteFilesFolderName);
            if (!Directory.Exists(siteFilesPath))
            {
                try
                {
                    Directory.CreateDirectory(siteFilesPath);     
                }
                catch 
                {      
                    return false;
                }
            }

            var tenantFolder = Path.Combine(siteFilesPath, tenant.AliasId);
            if (!Directory.Exists(tenantFolder))
            {
                try
                {
                    Directory.CreateDirectory(tenantFolder);
                }
                catch
                {
                    return false;
                }
            }

            var tenantWwwRoot = Path.Combine(tenantFolder, options.SiteContentFolderName);
            if (!Directory.Exists(tenantWwwRoot))
            {
                try
                {
                    Directory.CreateDirectory(tenantWwwRoot);
                }
                catch 
                {
                    return false;
                }
            }
            
            return true;
        }

       

    }
}
