using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.IdentityServerIntegration.Configuration
{
    internal class ConfigureInternalCookieOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        private readonly IdentityServerOptions _idsrv;

        public ConfigureInternalCookieOptions(IdentityServerOptions idsrv)
        {
            _idsrv = idsrv;
        }

        public void Configure(CookieAuthenticationOptions options)
        {
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            if (name == IdentityServerConstants.DefaultCookieAuthenticationScheme)
            {
                options.SlidingExpiration = _idsrv.Authentication.CookieSlidingExpiration;
                options.ExpireTimeSpan = _idsrv.Authentication.CookieLifetime;
                options.Cookie.Name = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = ExtractLocalUrl(_idsrv.UserInteraction.LoginUrl);
                options.LogoutPath = ExtractLocalUrl(_idsrv.UserInteraction.LogoutUrl);
                options.ReturnUrlParameter = _idsrv.UserInteraction.LoginReturnUrlParameter;
            }

            if (name == IdentityServerConstants.ExternalCookieAuthenticationScheme)
            {
                options.Cookie.Name = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            }
        }

        private static string ExtractLocalUrl(string url)
        {
            if (url.IsLocalUrl())
            {
                if (url.StartsWith("~/"))
                {
                    url = url.Substring(1);
                }

                return url;
            }

            return null;
        }
    }

    internal class PostConfigureInternalCookieOptions : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        private readonly IdentityServerOptions _idsrv;
        private readonly IOptions<Microsoft.AspNetCore.Authentication.AuthenticationOptions> _authOptions;

        public PostConfigureInternalCookieOptions(IdentityServerOptions idsrv, IOptions<Microsoft.AspNetCore.Authentication.AuthenticationOptions> authOptions)
        {
            _idsrv = idsrv;
            _authOptions = authOptions;
        }

        public void PostConfigure(string name, CookieAuthenticationOptions options)
        {
            var scheme = _authOptions.Value.DefaultAuthenticateScheme ??
                _authOptions.Value.DefaultScheme;

            if (name == scheme)
            {
                options.LoginPath = ExtractLocalUrl(_idsrv.UserInteraction.LoginUrl);
                options.LogoutPath = ExtractLocalUrl(_idsrv.UserInteraction.LogoutUrl);
                options.ReturnUrlParameter = _idsrv.UserInteraction.LoginReturnUrlParameter;
            }
        }

        private static string ExtractLocalUrl(string url)
        {
            if (url.IsLocalUrl())
            {
                if (url.StartsWith("~/"))
                {
                    url = url.Substring(1);
                }

                return url;
            }

            return null;
        }
    }
}
