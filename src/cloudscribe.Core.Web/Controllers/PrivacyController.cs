using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class PrivacyController : Controller
    {
        public PrivacyController(SiteContext currentSite)
        {
            _currentSite = currentSite;
        }

        private readonly SiteContext _currentSite;

        public IActionResult Index()
        {
            return View(_currentSite);
        }

    }
}
