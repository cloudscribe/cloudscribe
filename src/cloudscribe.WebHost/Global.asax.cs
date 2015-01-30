using cloudscribe.Configuration;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.Identity;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using cloudscribe.Core.Web.DeviceDetection;


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

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CoreViewEngine());

            // the idea is if you want to override a view file foo.cshtml 
            // make a copy foo.phone.cshtml and/or foo.tablet.cshtml
            // based on the following display modes which will be selected 
            // when the condition is true
            // however note that the logic for IsSmartPhone and IsTablet may not correctly detect all devices
            // it should work for the iphone, android, blackberry, windows phone
            // and ipad and android tablets

            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("phone")
            {
                ContextCondition = Context =>
                                Context.IsSmartPhone()
            });

            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("tablet")
            {
                ContextCondition = Context =>
                                Context.IsTablet()
            });


            //MvcSiteMapProvider.SiteMaps.Loader = new MvcSiteMapProvider.Loader.SiteMapLoader();

            //MvcSiteMapProvider.DI.Composer.Compose();

        }

        // Application_BeginRequest seems to be considered an ugly method to use
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

        //protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        //{
           

           
        //}

        
    }
}
