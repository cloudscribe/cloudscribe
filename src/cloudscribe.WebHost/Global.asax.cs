using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using cloudscribe.Configuration;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.Identity;

// this was needed in mojoportal is it needed in mvc
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace cloudscribe.WebHost
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var log4NetPath = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4NetPath));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // this seems to be considered an ugly method to use
        // http://stackoverflow.com/questions/11726848/asp-net-mvc-4-intercept-all-incoming-requests
        // not sure if we can do the same in an mvc filter
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //http://stackoverflow.com/questions/1340643/how-to-enable-ip-address-logging-with-log4net
            log4net.ThreadContext.Properties["ip"] = Utils.GetIP4Address();
            log4net.ThreadContext.Properties["culture"] = CultureInfo.CurrentCulture.ToString();
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                if (AppSettings.LogFullUrls)
                {
                    log4net.ThreadContext.Properties["url"] = HttpContext.Current.Request.Url.ToString();
                }
                else
                {
                    log4net.ThreadContext.Properties["url"] = HttpContext.Current.Request.RawUrl;
                }
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            // this is needed because even though our MultiTenantClaimsIdentityFactory
            // creates a MultiTenantClaimsIdentity, when the owin middleware deserializes
            // it from the authentication cookie and puts it on the request it creates 
            // a normal ClaimsIdentity, so we have to correct that here
            // this is only needed in folder sites not using related sites mode
            // otherwise logging into any site would be logging into all sites since
            // they share a common authentication cookie
            if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                if (!AppSettings.UseRelatedSiteMode)
                {

                    if (HttpContext.Current.User != null)
                    {
                        MultiTenantClaimsIdentity id = MultiTenantClaimsIdentity.FromClaimsIdentity(
                            (ClaimsIdentity)HttpContext.Current.User.Identity);

                        ClaimsPrincipal claimsPricipal = new ClaimsPrincipal(id);
                        HttpContext.Current.User = claimsPricipal;
                    }
                }
            }
        }

        
    }
}
