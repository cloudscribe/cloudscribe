

using cloudscribe.Core.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using System;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseSocialAuth(
            this IApplicationBuilder app,
            SiteSettings site,
            IdentityCookieOptions cookieOptions,
            bool shouldUseFolder)
        {
            // TODO: will this require a restart if the options are updated in the ui?
            // no just need to clear the tenant cache after updating the settings
            if (!string.IsNullOrWhiteSpace(site.GoogleClientId))
            {
                app.UseGoogleAuthentication(options =>
                {
                    options.AuthenticationScheme = "Google";
                    options.SignInScheme = cookieOptions.ExternalCookie.AuthenticationScheme;

                    options.ClientId = site.GoogleClientId;
                    options.ClientSecret = site.GoogleClientSecret;

                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-google";
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(site.FacebookAppId))
            {
                app.UseFacebookAuthentication(options =>
                {
                    options.AuthenticationScheme = "Facebook";
                    options.SignInScheme = cookieOptions.ExternalCookie.AuthenticationScheme;
                    options.AppId = site.FacebookAppId;
                    options.AppSecret = site.FacebookAppSecret;

                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-facebook";
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(site.MicrosoftClientId))
            {
                app.UseMicrosoftAccountAuthentication(options =>
                {
                    options.SignInScheme = cookieOptions.ExternalCookie.AuthenticationScheme;
                    options.ClientId = site.MicrosoftClientId;
                    options.ClientSecret = site.MicrosoftClientSecret;
                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-microsoft";
                    }
                });
            }

            //app.Use()

            //Func<HttpContext, bool> hasTwitterKeys = (HttpContext context) =>
            //{
            //    var tenant = context.GetTenant<SiteSettings>();
            //    if (tenant == null) return false;
            //    if (string.IsNullOrWhiteSpace(tenant.TwitterConsumerKey)) return false;
            //    if (string.IsNullOrWhiteSpace(tenant.TwitterConsumerSecret)) return false;

            //    return true;
            //};

            //app.UseWhen(context => hasTwitterKeys(context), appBuilder =>
            //{
            if (!string.IsNullOrWhiteSpace(site.TwitterConsumerKey))
            {
                app.UseTwitterAuthentication(options =>
                {
                    options.SignInScheme = cookieOptions.ExternalCookie.AuthenticationScheme;
                    options.ConsumerKey = site.TwitterConsumerKey;
                    options.ConsumerSecret = site.TwitterConsumerSecret;

                    if (shouldUseFolder)
                    {
                        options.CallbackPath = "/" + site.SiteFolderName + "/signin-twitter";
                    }
                });
            }

            //});



            return app;
        }

        public static IApplicationBuilder UseWhen(this IApplicationBuilder app
            , Func<HttpContext, bool> condition
            , Action<IApplicationBuilder> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var builder = app.New();
            configuration(builder);

            return app.Use(next => {
                builder.Run(next);

                var branch = builder.Build();

                return context => {
                    if (condition(context))
                    {
                        return branch(context);
                    }

                    return next(context);
                };
            });
        }

    }
}
