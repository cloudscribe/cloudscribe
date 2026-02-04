using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Controllers.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;

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

        /// <summary>
        /// Allow user to see banner again (clear dismissal)
        /// </summary>
        [HttpPost]
        public virtual IActionResult ShowCookieBanner()
        {
            // Remove dismissal cookie so banner shows again
            Response.Cookies.Delete("cookieconsent_dismissed");
            
            return this.RedirectToSiteRoot(CurrentSite);
        }

        /// <summary>
        /// Reset all cookie preferences (consent + dismissal)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult ResetCookiePreferences()
        {
            // Clear consent cookie first
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if(consentFeature != null)
            {
                consentFeature.WithdrawConsent();
            }
            
            // Delete the dismiss cookie with matching options
            Response.Cookies.Delete("cookieconsent_dismissed", new Microsoft.AspNetCore.Http.CookieOptions
            {
                Path = "/",
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                HttpOnly = false,
                Secure = Request.IsHttps  // This is crucial for HTTPS
            });
            
            // Now we can safely redirect since we know the delete works
            return this.RedirectToSiteRoot(CurrentSite);
        }

    }
}
