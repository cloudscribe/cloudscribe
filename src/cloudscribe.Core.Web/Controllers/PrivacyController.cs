using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Controllers.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class PrivacyController : Controller
    {
        public PrivacyController(SiteContext currentSite)
        {
            CurrentSite = currentSite;
        }

        protected SiteContext CurrentSite { get; private set; }

        public virtual IActionResult Index()
        {
            return View(CurrentSite);
        }

        [HttpPost]
        public virtual IActionResult WithdrawCookieConsent()
        {
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if(consentFeature != null)
            {
                consentFeature.WithdrawConsent();
            }

            return this.RedirectToSiteRoot(CurrentSite);
        }

    }
}
