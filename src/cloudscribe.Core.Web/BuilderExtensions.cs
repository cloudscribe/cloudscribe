using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IO;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// this overload is deprecated, the ILoggerFactory is not used or needed here
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="multiTenantOptions"></param>
        /// <param name="sslIsAvailable"></param>
        /// <param name="applicationCookieSecure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCloudscribeCore(
           this IApplicationBuilder app,
           ILoggerFactory loggerFactory,
           MultiTenantOptions multiTenantOptions,
           bool sslIsAvailable = false,
           CookieSecurePolicy applicationCookieSecure = CookieSecurePolicy.SameAsRequest
           )
        {
            app.UseCloudscribeCore(multiTenantOptions, sslIsAvailable, applicationCookieSecure);

            return app;
        }

        public static IApplicationBuilder UseCloudscribeCore(
           this IApplicationBuilder app,
           CookieSecurePolicy applicationCookieSecure = CookieSecurePolicy.SameAsRequest
           )
        {
            var multiTenantOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<cloudscribe.Core.Models.MultiTenantOptions>>();
            var config = app.ApplicationServices.GetRequiredService<IConfiguration>();

            var sslIsAvailable = config.GetValue<bool>("AppSettings:UseSsl");
            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore(multiTenantOptions, sslIsAvailable, applicationCookieSecure);

            return app;
        }



        public static IApplicationBuilder UseCloudscribeCore(
           this IApplicationBuilder app,
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
                builder.UseSiteAndThemeStaticFiles(multiTenantOptions, ctx.Tenant);

                
                //builder.UseAuthentication();
                
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCloudscribeEnforceSiteRulesMiddleware();

            return app;

        }


        public static IApplicationBuilder UseSiteAndThemeStaticFiles(
           this IApplicationBuilder builder,
           MultiTenantOptions multiTenantOptions,
           SiteContext tenant
           )
        {
            var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

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
                    var themePath = Path.Combine(env.ContentRootPath,
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
                    var themePath = Path.Combine(env.ContentRootPath,
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
                // but if using related sites mode and RelatedAliasId is specified then all sites use the same 
                
                var folderExists = TryEnsureTenantWwwRoot(env, tenant, multiTenantOptions);
                string aliasId = tenant.AliasId;
                var usingSharedAlias = false;
                if(multiTenantOptions.UseRelatedSitesMode && !string.IsNullOrWhiteSpace(multiTenantOptions.RelatedSiteAliasId))
                {
                    aliasId = multiTenantOptions.RelatedSiteAliasId;
                    usingSharedAlias = true;
                }

                if (folderExists)
                {
                    var siteFilesPath = Path.Combine(env.ContentRootPath,
                        multiTenantOptions.SiteUploadFilesRootFolderName,
                        aliasId,
                        multiTenantOptions.SiteContentFolderName);

                    if (string.IsNullOrEmpty(tenantSegment) || usingSharedAlias) // root tenant or hostname tenant or shared alias
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(siteFilesPath)
                            //,RequestPath = new PathString("/files")
                        });
                    }
                    else if(!usingSharedAlias) //separate folders per folder tenant
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

        private static bool TryEnsureTenantWwwRoot(
            IWebHostEnvironment env,
            ISiteContext tenant, 
            MultiTenantOptions options)
        {
          
            var siteFilesPath = Path.Combine(env.ContentRootPath, options.SiteUploadFilesRootFolderName);
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

            string tenantFolder;

            if(options.UseRelatedSitesMode && !string.IsNullOrWhiteSpace(options.RelatedSiteAliasId))
            {
                tenantFolder = Path.Combine(siteFilesPath, options.RelatedSiteAliasId);
            }
            else
            {
                tenantFolder = Path.Combine(siteFilesPath, tenant.AliasId);
            }
            
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
