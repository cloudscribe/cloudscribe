using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    public class OopsController : Controller
    {
        public OopsController(
            IStringLocalizer<CloudscribeCore> localizer,
            ILogger<OopsController> logger
            )
        {
            sr = localizer;
            log = logger;
        }

        private IStringLocalizer sr;
        private ILogger log;

        public IActionResult Error(int statusCode = 500)
        {
            ViewData["Title"] = sr["Oops!"];

            if (statusCode == 404)
            {
                var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                if (statusFeature != null)
                {
                    var originalPath = statusFeature.OriginalPath;
                    var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                    log.LogWarning($"handled 404 for url: {originalPath} from ipaddress {ipAddress}");
                }

            }
            return View(statusCode);
        }

    }
}
