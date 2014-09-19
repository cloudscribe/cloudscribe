using cloudscribe.AspNet.Identity;
using cloudscribe.Core.Models;
//using cloudscribe.Core.Repositories.Caching;
using cloudscribe.Core.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;


namespace cloudscribe.WebHost
{
    public partial class Startup
    {
        
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // legacy entity framework code from the vs project template
            // Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            
           
           // refactored this because it really is not needed for evey request
            // and it depends on sitesettings which also is not needed for every request
            //app.CreatePerOwinContext<SiteUserManager>(SiteUserManager.Create);
            //app.CreatePerOwinContext<SiteSignInManager>(SiteSignInManager.Create);
            // sitecontext is now a container for the above and lazy loads them as needed
            // see Startup.cs

            //http://brockallen.com/2013/10/24/a-primer-on-owin-cookie-authentication-middleware-for-the-asp-net-developer/

            //http://msdn.microsoft.com/en-us/library/microsoft.owin.security.cookies.cookieauthenticationoptions%28v=vs.113%29.aspx

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SiteUserManager, SiteUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
                // here for folder sites we would like to be able to set this per site
                // ie based on the request, but it seems not possible except in startup
                CookieName = "cloudscribe-app"

                //http://leastprivilege.com/2012/10/08/custom-claims-principals-in-net-4-5/
                // maybe we could add a per site claim
                // or a custom claimprincipal where we can override IsAuthenticated
                // based on something in addition to the auth cookie
                //http://msdn.microsoft.com/en-us/library/system.security.claims.claimsprincipal%28v=vs.110%29.aspx
                //http://msdn.microsoft.com/en-us/library/system.security.principal.iidentity%28v=vs.110%29.aspx
                // or custom IIdentity
                //http://msdn.microsoft.com/en-us/library/system.security.claims.claimsidentity%28v=vs.110%29.aspx

                //http://stackoverflow.com/questions/19763807/how-to-set-a-custom-claimsprincipal-in-mvc-5
            });

            //http://msdn.microsoft.com/en-us/library/microsoft.owin.security.cookies.cookieauthenticationprovider%28v=vs.113%29.aspx

            //http://stackoverflow.com/questions/25393234/change-owin-auth-middleware-per-request-multi-tenant-oauth-api-keys-per-tenant
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

            

       
    }
}