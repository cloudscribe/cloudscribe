using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class PolicyController : Controller
    {
        public PolicyController(SiteContext currentSite)
        {
            _currentSite = currentSite;
        }

        private readonly SiteContext _currentSite;

        public IActionResult Privacy()
        {
            return View(_currentSite);
        }

    }
}
