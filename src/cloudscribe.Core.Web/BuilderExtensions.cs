

using cloudscribe.Core.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity;

namespace cloudscribe.Core.Web
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

            return app;
        }

    }
}
