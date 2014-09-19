// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2014-09-08
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;


namespace cloudscribe.AspNet.Identity
{
    public static class SiteUserExtensions
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this SiteUser user, UserManager<SiteUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            
            
            // Add custom user claims here

            if(AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                if(!AppSettings.UseRelatedSiteMode)
                {
                    // user needs a claim for the site which will be checked in MultiTenantClaimsIdentity
                    ISiteContext site = HttpContext.Current.GetOwinContext().Get<ISiteContext>();
                    // user could have accounts in multiple sites so we get all of them if he does
                    List<ISiteUser> userlist = site.UserRepository.GetCrossSiteUserListByEmail(user.Email);

                    foreach(ISiteUser u in userlist)
                    {
                        Claim siteClaim = new Claim("SiteMember", u.SiteGuid.ToString());
                        if (!userIdentity.HasClaim(siteClaim.Type, siteClaim.Value))
                        {
                            userIdentity.AddClaim(siteClaim);
                        }

                    }

                    

                }
            }
            

            
            return userIdentity;
        }
    }
}
