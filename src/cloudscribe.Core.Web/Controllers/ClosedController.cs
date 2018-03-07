using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    public class ClosedController : Controller
    {
        public ClosedController(
            SiteContext currentSite,
            IStringLocalizer<CloudscribeCore> localizer
            )
        {
            _currentSite = currentSite;
            _sr = localizer;
        }

        private readonly SiteContext _currentSite;
        private IStringLocalizer _sr;

        public IActionResult Index()
        {
            if(!_currentSite.SiteIsClosed)
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            ViewData["Title"] = _sr["Site Closed To Public Temporarily"];

            return View("Index", _currentSite.SiteIsClosedMessage);
        }

    }
}
