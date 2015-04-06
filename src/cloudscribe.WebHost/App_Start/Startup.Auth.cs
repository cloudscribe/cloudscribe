// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-04-06
// 

using cloudscribe.AspNet.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web;
using cloudscribe.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
//using Ninject;


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

            //StandardKernel ninjectKernal = GetKernel();
            //ISiteRepository siteRepo = ninjectKernal.Get<ISiteRepository>();

            ISiteRepository siteRepo = DependencyResolver.Current.GetService<ISiteRepository>();

            //http://stackoverflow.com/questions/25393234/change-owin-auth-middleware-per-request-multi-tenant-oauth-api-keys-per-tenant/26534460#26534460


            if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                try
                {
                    List<SiteFolder> allFolders = siteRepo.GetAllSiteFoldersNonAsync();
                    ConfigureFolderTenantAuth(app, allFolders);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }
            else
            {
                try
                {
                    List<ISiteHost> allHosts = siteRepo.GetAllHostsNonAsync();
                    ConfigureHostTenantAuth(app, allHosts);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }
            
            // below is the default configuration
            // in the case no host name or folder is mapped
            ConfigureDefaultTenantAuth(app);
            

            //http://brockallen.com/2013/10/24/a-primer-on-owin-cookie-authentication-middleware-for-the-asp-net-developer/

            //http://msdn.microsoft.com/en-us/library/microsoft.owin.security.cookies.cookieauthenticationoptions%28v=vs.113%29.aspx

            
        }

        private void ConfigureDefaultTenantAuth(IAppBuilder app)
        {
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
                // here for folder sites we would like to be able to set the cookie name per tenant
                // ie based on the request, but it seems not possible except in startup
                CookieName = "cloudscribe-app"

                //http://aspnet.codeplex.com/SourceControl/latest#Samples/Katana/BranchingPipelines/Startup.cs

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

            // again this requirement to set at startup instead of determine per request
            // makes it hard to use different settings per tenant in mutli site installations

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

        // something like this could be done to eliminate the need for restart when new sites created
        //private void ConfigureFolderTenantAuth2(IAppBuilder app, ISiteRepository repo)
        //{
        //    app.MapWhen(context =>
        //    {
        //        //return context.Request.Environment["owin.RequestPathBase"].ToString().ToLower() == "/" + TENANT_PATH_CLIENTS;
        //        // here we could check real time if the request is a folder mapping

        //        string siteFolderName = SiteContext.GetFirstFolderSegment(context.Request.Uri.ToString());
        //        if(siteFolderName.Length > 0)
        //        {
        //            if(repo.FolderExists(siteFolderName))
        //            {
        //                return true;
        //            }
        //        }

        //        //return Task.FromResult(false);
        //        return false;
        //    }, map =>
        //    {
        //        //map.Map()

        //    });

        //}

        private void ConfigureFolderTenantAuth(IAppBuilder app, List<SiteFolder> allFolders)
        {
            
            foreach(SiteFolder f in allFolders)
            {
                // how to do multi tenant
                //http://aspnet.codeplex.com/SourceControl/latest#Samples/Katana/BranchingPipelines/Startup.cs
                app.Map("/" + f.FolderName, site =>
                {
                    site.UseCookieAuthentication(new CookieAuthenticationOptions
                    {
                        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                        LoginPath = new PathString("/" + f.FolderName + "/Account/Login"),
                        Provider = new CookieAuthenticationProvider
                        {
                            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SiteUserManager, SiteUser>(
                                validateInterval: TimeSpan.FromMinutes(30),
                                regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                        },

                        CookieName = f.FolderName + "-app"


                    });


                    site.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

                    // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
                    site.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

                    // Enables the application to remember the second login verification factor such as phone or email.
                    // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
                    // This is similar to the RememberMe option when you log in.
                    site.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

                    // Uncomment the following lines to enable logging in with third party login providers


                    //site.UseMicrosoftAccountAuthentication(
                    //    clientId: "",
                    //    clientSecret: "");

                    //site.UseTwitterAuthentication(
                    //   consumerKey: "",
                    //   consumerSecret: "");

                    //site.UseFacebookAuthentication(
                    //   appId: "",
                    //   appSecret: "");

                    //site.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                    //{
                    //    ClientId = "",
                    //    ClientSecret = ""
                    //});

                });

            }

        }

        private void ConfigureHostTenantAuth(IAppBuilder app, List<ISiteHost> allHosts)
        {
            foreach(ISiteHost host in  allHosts)
            {
                app.MapWhen(host.IsDomain, site =>
                {
                    site.UseCookieAuthentication(new CookieAuthenticationOptions
                    {
                        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                        LoginPath = new PathString("/Account/Login"),
                        Provider = new CookieAuthenticationProvider
                        {
                            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SiteUserManager, SiteUser>(
                                validateInterval: TimeSpan.FromMinutes(30),
                                regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                        },

                        CookieName = host.HostName + "-app"


                    });

                    site.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

                    // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
                    site.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

                    // Enables the application to remember the second login verification factor such as phone or email.
                    // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
                    // This is similar to the RememberMe option when you log in.
                    site.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

                    // Uncomment the following lines to enable logging in with third party login providers


                    //site.UseMicrosoftAccountAuthentication(
                    //    clientId: "",
                    //    clientSecret: "");

                    //site.UseTwitterAuthentication(
                    //   consumerKey: "",
                    //   consumerSecret: "");

                    //site.UseFacebookAuthentication(
                    //   appId: "",
                    //   appSecret: "");

                    //site.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                    //{
                    //    ClientId = "",
                    //    ClientSecret = ""
                    //});

                });



            }

        }


        
        
            

       
    }
}