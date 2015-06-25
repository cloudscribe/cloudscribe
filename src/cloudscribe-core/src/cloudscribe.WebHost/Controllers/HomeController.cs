using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web;

namespace cloudscribe.WebHost.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ISiteResolver siteResolver)
        {
            Site = siteResolver.Resolve();
        }

        private ISiteSettings Site;

        public IActionResult Index()
        {
            ViewBag.SiteName = Site.SiteName;

            return View();
        }

        public IActionResult About()
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
