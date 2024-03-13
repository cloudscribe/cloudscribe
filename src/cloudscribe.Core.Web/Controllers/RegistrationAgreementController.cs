using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class RegistrationAgreementController : Controller
    {
        public RegistrationAgreementController(SiteContext currentSite)
        {
            CurrentSite = currentSite;
        }

        protected SiteContext CurrentSite { get; private set; }

        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult Index()
        {
            return View(CurrentSite);
        }
    }
}
