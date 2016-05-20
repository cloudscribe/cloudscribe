

using cloudscribe.Core.Models;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseSocialAuth(
            this IApplicationBuilder app,
            SiteSettings site,
            CookieAuthenticationOptions externalCookieOptions,
            bool shouldUseFolder)
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

            //});



            return app;
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
