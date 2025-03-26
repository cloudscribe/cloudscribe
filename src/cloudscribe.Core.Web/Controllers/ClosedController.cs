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
            CurrentSite = currentSite;
            StringLocalizer = localizer;
        }

        protected SiteContext CurrentSite { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; }

        public virtual IActionResult Index()
        {
            if(!CurrentSite.SiteIsClosed)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            ViewData["Title"] = StringLocalizer["Site Closed To Public Temporarily"];

            return View("Index", CurrentSite.SiteIsClosedMessage);
        }

    }
}
