using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace example.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger)
        {
            log = logger;
        }

        private ILogger log;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            // was just testing the error log here
            //throw new ArgumentException("just for the sake of argument");
            return View();
        }

        public IActionResult Error(int statusCode)
        {
            if(statusCode == 404)
            {
                var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                if(statusFeature != null)
                {
                    log.LogWarning("handled 404 for url: {OriginalPath}", statusFeature.OriginalPath);
                }
                
            }
            return View(statusCode);
        }
    }
}
