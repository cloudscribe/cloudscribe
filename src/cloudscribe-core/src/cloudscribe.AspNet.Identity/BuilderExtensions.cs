

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Builder;
//using Microsoft.AspNet.Identity;

//namespace cloudscribe.AspNet.Identity
//{
//    public static class BuilderExtensions
//    {
//        public static IApplicationBuilder UseCloudscribeIdentity(this IApplicationBuilder app)
//        {
//            if (app == null)
//            {
//                throw new ArgumentNullException(nameof(app));
//            }
//            //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Cookies/CookieAppBuilderExtensions.cs
//            app.UseCookieAuthentication(null, IdentityOptions.ExternalCookieAuthenticationScheme);
//            app.UseCookieAuthentication(null, IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme);
//            app.UseCookieAuthentication(null, IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme);
//            app.UseCookieAuthentication(null, IdentityOptions.ApplicationCookieAuthenticationScheme);
//            return app;
//        }

//    }
//}
