

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {

        public static IApplicationBuilder UseCloudscribeCore(
           this IApplicationBuilder app,
           ILoggerFactory loggerFactory,
           MultiTenantOptions multiTenantOptions,
           bool sslIsAvailable = false,
           Func<IApplicationBuilder, ISiteContext, bool> identityServerInvoker = null,
           CookieSecurePolicy applicationCookieSecure = CookieSecurePolicy.SameAsRequest
           )
        {
            app.UseCloudscribeCommonStaticFiles();

            app.UseMultitenancy<cloudscribe.Core.Models.SiteContext>();



            app.UsePerTenant<cloudscribe.Core.Models.SiteContext>((ctx, builder) =>
            {
                //var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
                

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

                builder.UseCloudscribeCoreDefaultAuthentication(
                    loggerFactory,
                    multiTenantOptions,
                    ctx.Tenant,
                    sslIsAvailable);

                // to make this multi tenant for folders we are
                // using a fork of IdentityServer4 and hoping to get changes so we don't need a fork
                // https://github.com/IdentityServer/IdentityServer4/issues/19

                if(identityServerInvoker != null)
                {
                    var addedIdentityServer = identityServerInvoker(builder, ctx.Tenant);
                }
                //builder.UseIdentityServer();

                //// this sets up the authentication for apis within this application endpoint
                //// ie apis that are hosted in the same web app endpoint with the authority server
                //// this is not needed here if you are only using separate api endpoints
                //// it is needed in the startup of those separate endpoints
                //// note that with both cookie auth and jwt auth middleware the principal is merged from both the cookie and the jwt token if it is passed
                //builder.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
                //{
                //    Authority = "https://localhost:44399",
                //    // using the site aliasid as the scope so each tenant has a different scope
                //    // you can view the aliasid from site settings
                //    // clients must be configured with the scope to have access to the apis for the tenant
                //    ApiName = ctx.Tenant.AliasId,
                //    //RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                //    //AuthenticationScheme = AuthenticationScheme.Application,

                //    RequireHttpsMetadata = true
                //});



            });

            app.UseCloudscribeEnforceSiteRulesMiddleware();

            return app;

        }

        public static IApplicationBuilder UseCloudscribeCoreDefaultAuthentication(
           this IApplicationBuilder builder,
           ILoggerFactory loggerFactory,
           MultiTenantOptions multiTenantOptions,
           SiteContext tenant,
           bool sslIsAvailable = true,
           CookieSecurePolicy applicationCookieSecure = CookieSecurePolicy.SameAsRequest
           )
        {

            var useFolder = !multiTenantOptions.UseRelatedSitesMode
                                        && multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && tenant.SiteFolderName.Length > 0;

            var externalCookieOptions = builder.SetupOtherCookies(
                    AuthenticationScheme.External,
                    multiTenantOptions.UseRelatedSitesMode,
                    tenant);
            builder.UseCookieAuthentication(externalCookieOptions);

            var twoFactorRememberMeCookieOptions = builder.SetupOtherCookies(
                AuthenticationScheme.TwoFactorRememberMe,
                multiTenantOptions.UseRelatedSitesMode,
                tenant);
            builder.UseCookieAuthentication(twoFactorRememberMeCookieOptions);

            var twoFactorUserIdCookie = builder.SetupOtherCookies(
                AuthenticationScheme.TwoFactorUserId,
                multiTenantOptions.UseRelatedSitesMode,
                tenant);
            builder.UseCookieAuthentication(twoFactorUserIdCookie);

            //var cookieEvents = new CookieAuthenticationEvents();
            var logger = loggerFactory.CreateLogger<SiteAuthCookieValidator>();
            var cookieValidator = new SiteAuthCookieValidator(logger);
            var appCookieOptions = builder.SetupAppCookie(
                cookieValidator,
                AuthenticationScheme.Application,
                multiTenantOptions.UseRelatedSitesMode,
                tenant,
                applicationCookieSecure
                );
            builder.UseCookieAuthentication(appCookieOptions);

            
            builder.UseSocialAuth(tenant, externalCookieOptions, useFolder, sslIsAvailable);


            return builder;
        }

        public static IApplicationBuilder UseSocialAuth(
            this IApplicationBuilder app,
            SiteContext site,
            CookieAuthenticationOptions externalCookieOptions,
            bool shouldUseFolder,
            bool sslIsAvailable = true
            )
        {
            // TODO: will this require a restart if the options are updated in the ui?
            // no just need to clear the tenant cache after updating the settings
            if (!string.IsNullOrWhiteSpace(site.GoogleClientId))
            {
                var googleOptions = new GoogleOptions();
                googleOptions.AuthenticationScheme = "Google";
                googleOptions.SignInScheme = externalCookieOptions.AuthenticationScheme;
                googleOptions.ClientId = site.GoogleClientId;
                googleOptions.ClientSecret = site.GoogleClientSecret;
                if (shouldUseFolder)
                {
                    googleOptions.CallbackPath = "/" + site.SiteFolderName + "/signin-google";
                }

                app.UseGoogleAuthentication(googleOptions);
            }

            if (!string.IsNullOrWhiteSpace(site.FacebookAppId))
            {
                var facebookOptions = new FacebookOptions();
                facebookOptions.AuthenticationScheme = "Facebook";
                facebookOptions.SignInScheme = externalCookieOptions.AuthenticationScheme;
                facebookOptions.AppId = site.FacebookAppId;
                facebookOptions.AppSecret = site.FacebookAppSecret;

                if (shouldUseFolder)
                {
                    facebookOptions.CallbackPath = "/" + site.SiteFolderName + "/signin-facebook";
                }

                app.UseFacebookAuthentication(facebookOptions);
            }

            if (!string.IsNullOrWhiteSpace(site.MicrosoftClientId))
            {
                var microsoftOptions = new MicrosoftAccountOptions();
                microsoftOptions.SignInScheme = externalCookieOptions.AuthenticationScheme;
                microsoftOptions.ClientId = site.MicrosoftClientId;
                microsoftOptions.ClientSecret = site.MicrosoftClientSecret;
                if (shouldUseFolder)
                {
                    microsoftOptions.CallbackPath = "/" + site.SiteFolderName + "/signin-microsoft";
                }

                app.UseMicrosoftAccountAuthentication(microsoftOptions);
            }

            
            if (!string.IsNullOrWhiteSpace(site.TwitterConsumerKey))
            {
                var twitterOptions = new TwitterOptions();
                twitterOptions.SignInScheme = externalCookieOptions.AuthenticationScheme;
                twitterOptions.ConsumerKey = site.TwitterConsumerKey;
                twitterOptions.ConsumerSecret = site.TwitterConsumerSecret;

                if (shouldUseFolder)
                {
                    twitterOptions.CallbackPath = "/" + site.SiteFolderName + "/signin-twitter";
                }

                app.UseTwitterAuthentication(twitterOptions);
            }

            if(!string.IsNullOrWhiteSpace(site.OidConnectAuthority)
                && !string.IsNullOrWhiteSpace(site.OidConnectAppId)
               // && !string.IsNullOrWhiteSpace(site.OidConnectAppSecret)
                )
            {
                var displayName = "ExternalOIDC";
                if(!string.IsNullOrWhiteSpace(site.OidConnectDisplayName))
                {
                    displayName = site.OidConnectDisplayName;
                }
                var oidOptions = new OpenIdConnectOptions();
                oidOptions.AuthenticationScheme = "ExternalOIDC";
                oidOptions.SignInScheme = externalCookieOptions.AuthenticationScheme;
                oidOptions.Authority = site.OidConnectAuthority;
                oidOptions.ClientId = site.OidConnectAppId;
                oidOptions.ClientSecret = site.OidConnectAppSecret;
                oidOptions.GetClaimsFromUserInfoEndpoint = true;
                oidOptions.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                oidOptions.RequireHttpsMetadata = sslIsAvailable;
                oidOptions.SaveTokens = true;
                oidOptions.DisplayName = displayName;

                if (shouldUseFolder)
                {
                    oidOptions.CallbackPath = "/" + site.SiteFolderName + "/signin-oidc";
                    oidOptions.SignedOutCallbackPath = "/" + site.SiteFolderName + "/signout-callback-oidc";
                    oidOptions.RemoteSignOutPath = "/" + site.SiteFolderName + "/signout-oidc";
                }
                
                //oidOptions.Events = new OpenIdConnectEvents()
                //{
                //    OnAuthenticationFailed = c =>
                //    {
                //        c.HandleResponse();

                //        c.Response.StatusCode = 500;
                //        c.Response.ContentType = "text/plain";

                //        return c.Response.WriteAsync("An error occurred processing your authentication.");
                //    }
                //};
                app.UseOpenIdConnectAuthentication(oidOptions);
               

            }

            



            return app;
        }

        public static CookieAuthenticationOptions SetupAppCookie(
            this IApplicationBuilder app,
           SiteAuthCookieValidator siteValidator,
           string scheme,
           bool useRelatedSitesMode,
           SiteContext tenant,
           CookieSecurePolicy cookieSecure = CookieSecurePolicy.SameAsRequest
           )
        {
            var cookieEvents = new CookieAuthenticationEvents();
            var options = new CookieAuthenticationOptions();
            if (useRelatedSitesMode)
            {
                options.AuthenticationScheme = scheme;
                options.CookieName = scheme;
                options.CookiePath = "/";
            }
            else
            {
                //options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
                options.AuthenticationScheme = scheme;
                options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
                options.CookiePath = "/" + tenant.SiteFolderName;
                cookieEvents.OnValidatePrincipal = siteValidator.ValidatePrincipal;
            }

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.AccessDeniedPath = tenantPathBase + "/account/accessdenied";

            options.Events = cookieEvents;

            options.AutomaticAuthenticate = true;
            options.AutomaticChallenge = false;

            options.CookieSecure = cookieSecure;

            return options;
        }

        public static CookieAuthenticationOptions SetupOtherCookies(
            this IApplicationBuilder app,
            string scheme,
            bool useRelatedSitesMode,
            SiteContext tenant,
            CookieSecurePolicy cookieSecure = CookieSecurePolicy.None
            )
        {
            var options = new CookieAuthenticationOptions();
            if (useRelatedSitesMode)
            {
                options.AuthenticationScheme = scheme;
                options.CookieName = scheme;
                options.CookiePath = "/";
            }
            else
            {
                //options.AuthenticationScheme = $"{scheme}-{tenant.SiteFolderName}";
                options.AuthenticationScheme = scheme;
                options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
                options.CookiePath = "/" + tenant.SiteFolderName;
            }

            options.AutomaticAuthenticate = false;

            if (cookieSecure != CookieSecurePolicy.None)
            {
                options.CookieSecure = cookieSecure;
            }

            return options;

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

            // would this work? I am doubtful, need to try it sometime
            //var embeddedFiles = new EmbeddedFileProvider(
            //        typeof(cloudscribe.Core.Web.Components.AccountService).GetTypeInfo().Assembly,
            //        "your.assembly.name"
            //    );

            //builder.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = embeddedFiles
            //    //,RequestPath = new PathString("/files")
            //});



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

        //public static IApplicationBuilder UseWhen(this IApplicationBuilder app
        //    , Func<HttpContext, bool> condition
        //    , Action<IApplicationBuilder> configuration)
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException(nameof(app));
        //    }

        //    if (condition == null)
        //    {
        //        throw new ArgumentNullException(nameof(condition));
        //    }

        //    if (configuration == null)
        //    {
        //        throw new ArgumentNullException(nameof(configuration));
        //    }

        //    var builder = app.New();
        //    configuration(builder);

        //    return app.Use(next => {
        //        builder.Run(next);

        //        var branch = builder.Build();

        //        return context => {
        //            if (condition(context))
        //            {
        //                return branch(context);
        //            }

        //            return next(context);
        //        };
        //    });
        //}

    }
}
