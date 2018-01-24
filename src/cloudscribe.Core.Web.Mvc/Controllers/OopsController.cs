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


            var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusFeature != null)
            {
                var originalPath = statusFeature.OriginalPath;

                if (statusCode == 404)
                {
                    var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                    log.LogWarning($"handled 404 for url: {originalPath} from ipaddress {ipAddress}");

                }

                if (originalPath.Contains("/api/"))
                {
                    var errorMessage = string.Empty;
                    switch (statusCode)
                    {
                        case 400:
                            errorMessage = "Bad Request";
                            break;
                        case 401:
                            errorMessage = "Unauthorized";
                            break;
                        case 403:
                            errorMessage = "Forbidden";
                            break;
                        case 404:
                            errorMessage = "Page Not Found";
                            break;
                        case 500:
                        default:
                            errorMessage = "Unexpected Error";
                            break;
                    }

                    var jsonResult = new JsonResult(new { error = errorMessage });
                    jsonResult.StatusCode = statusCode;
                    return jsonResult;
                }


            }
            

            
            

            return View(statusCode);
        }

    }
}
