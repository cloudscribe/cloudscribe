using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity.Owin;


namespace cloudscribe.WebHost.Controllers
{
    public class HomeController : Controller
    {

        //private ISiteContext site;

        //public HomeController(ISiteContext siteContext)
        //{
        //    site = siteContext;
        //}

        public ISiteContext Site
        {
            get { return HttpContext.GetOwinContext().Get<ISiteContext>(); }
        }

        public ActionResult Index()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}