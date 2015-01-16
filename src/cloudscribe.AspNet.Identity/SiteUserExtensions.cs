// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2014-12-26
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using log4net;


namespace cloudscribe.AspNet.Identity
{
    public static class SiteUserExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteUserExtensions));

        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this SiteUser user, UserManager<SiteUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            
            
            // Add custom user claims here
            Claim displayNameClaim = new Claim("DisplayName", user.DisplayName);
            if (!userIdentity.HasClaim(displayNameClaim.Type, displayNameClaim.Value))
            {
                userIdentity.AddClaim(displayNameClaim);
            }

            if (userIdentity.HasClaim(ClaimTypes.Role, "Admins"))
            {
                // if the user is an admin in the server admin site
                // add this additional role that allows site creation and
                // management of other sites
                //if(manager is SiteUserManager)
                //{
                //    SiteUserManager siteManager = manager as SiteUserManager;
                //    if(siteManager.Store is cloudscribe.AspNet.Identity.UserStore<SiteUser>)
                //    {

                //    }

                //}
                // 2015-01-16 this was suddenly broken this morning
                // it now is null. only things that changed were making some async methods really become async
                // apparently we lost the context somehow from that
                // TODO: maybe we can implement this another way with an actionfilter
                // for now I'm just adding it to all admins in the esle
                if(HttpContext.Current != null)
                {
                    IOwinContext owin = HttpContext.Current.GetOwinContext();
                    if (owin != null)
                    {
                        ISiteContext site = owin.Get<ISiteContext>();
                        if (site.SiteSettings.IsServerAdminSite)
                        {
                            Claim serverAdminRoleClaim = new Claim(ClaimTypes.Role, "ServerAdmins");
                            userIdentity.AddClaim(serverAdminRoleClaim);

                        }

                    }
                    else
                    {
                        log.Error("owincontext was null, failed to check if ServerAdmin claim should be granted");
                    }

                }
                else
                {
                    log.Error("httpcontext was null, failed to check if ServerAdmin claim should be granted");
                    //TODO: this should not be here, its a temporary hack 2015-01-16
                    Claim serverAdminRoleClaim = new Claim(ClaimTypes.Role, "ServerAdmins");
                    userIdentity.AddClaim(serverAdminRoleClaim);
                }
                
                

            }

            // the per site claims are not needed since we now have separate cookies per folder tenant
            //if(AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            //{
            //    if(!AppSettings.UseRelatedSiteMode)
            //    {
            //        // user needs a claim for the site which will be checked in MultiTenantClaimsIdentity
            //        ISiteContext site = HttpContext.Current.GetOwinContext().Get<ISiteContext>();
            //        // user could have accounts in multiple sites so we get all of them if he does
            //        List<ISiteUser> userlist = site.UserRepository.GetCrossSiteUserListByEmail(user.Email);

            //        foreach(ISiteUser u in userlist)
            //        {
            //            Claim siteClaim = new Claim("SiteMember", u.SiteGuid.ToString());
            //            if (!userIdentity.HasClaim(siteClaim.Type, siteClaim.Value))
            //            {
            //                userIdentity.AddClaim(siteClaim);
            //            }

            //        }

                    

            //    }
            //}
            

            
            return userIdentity;
        }
    }
}
