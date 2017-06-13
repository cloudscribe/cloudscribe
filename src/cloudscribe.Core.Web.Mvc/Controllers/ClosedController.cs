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
            this.currentSite = currentSite;
            sr = localizer;
        }

        private readonly SiteContext currentSite;
        private IStringLocalizer sr;

        public IActionResult Index()
        {
            if(!currentSite.SiteIsClosed)
            {
                return this.RedirectToSiteRoot(currentSite);
            }

            ViewData["Title"] = sr["Site Closed To Public Temporarily"];

            return View("Index", currentSite.SiteIsClosedMessage);
        }

    }
}
