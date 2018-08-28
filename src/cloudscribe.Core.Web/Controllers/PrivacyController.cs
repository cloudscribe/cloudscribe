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
            _currentSite = currentSite;
        }

        private readonly SiteContext _currentSite;

        public IActionResult Index()
        {
            return View(_currentSite);
        }

        [HttpPost]
        public IActionResult WithdrawCookieConsent()
        {
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if(consentFeature != null)
            {
                consentFeature.WithdrawConsent();
            }

            return this.RedirectToSiteRoot(_currentSite);
        }

    }
}
