using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Web.Common.Controllers
{
    public class ToastAlertsController : Controller
    {
        [HttpGet]
        public IActionResult GetAlerts()
        {
            var alerts = TempData.GetAlerts();

            return Ok(alerts);
        }

    }
}
