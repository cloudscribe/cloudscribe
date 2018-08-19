using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class PrivacyPolicyViewModel
    {
        [Display(Name = "SiteId")]
        public Guid SiteId { get; set; } = Guid.Empty;

        [Display(Name = "Require Cookie Consent?")]
        public bool RequireCookieConsent { get; set; }

        [Display(Name = "Cookie Policy Summary")]
        [MaxLength(255,ErrorMessage = "Cookie policy summary has a maximum length of 255 characters.")]
        public string CookiePolicySummary { get; set; } = "To ensure you get the best experience, this website uses cookies.";

        [Display(Name = "Privacy Policy")]
        public string PrivacyPolicy { get; set; }

    }
}
