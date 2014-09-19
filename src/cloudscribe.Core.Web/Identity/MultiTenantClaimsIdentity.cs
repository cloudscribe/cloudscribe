// Author:					Joe Audette
// Created:				    2014-09-08
// Last Modified:		    2014-09-08
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace cloudscribe.Core.Web.Identity
{
    public class MultiTenantClaimsIdentity : ClaimsIdentity
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MultiTenantClaimsIdentity));

        public MultiTenantClaimsIdentity(IIdentity identity):base(identity)
        { }

        

        public override bool IsAuthenticated
        {
            get
            {
                //log.Info("IsAuthenticated called");

                if (base.IsAuthenticated)
                {
                    if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                    {
                        if (!AppSettings.UseRelatedSiteMode)
                        {
                            // user needs a claim for the site which will be checked in MultiTenantClaimsIdentity
                            ISiteContext site = HttpContext.Current.GetOwinContext().Get<ISiteContext>();
                            Claim siteClaim = new Claim("SiteMember", site.SiteSettings.SiteGuid.ToString());
                            return base.HasClaim(siteClaim.Type, siteClaim.Value);
                        }
                    }
                }

                return base.IsAuthenticated;
            }
        }

        public static MultiTenantClaimsIdentity FromClaimsIdentity(ClaimsIdentity identity)
        {
            MultiTenantClaimsIdentity ci2 = new MultiTenantClaimsIdentity(identity);

            ci2.BootstrapContext = identity.BootstrapContext;
            ci2.Label = identity.Label;

            foreach (Claim c in identity.Claims)
            {
                if (!ci2.HasClaim(c.Type, c.Value))
                {
                    ci2.AddClaim(c);
                }
            }

            return ci2;

        }
    }
}
