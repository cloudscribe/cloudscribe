using cloudscribe.Core.Web.Mvc.Components;
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
            IDecideErrorResponseType responseTypeDecider,
            ILogger<OopsController> logger
            )
        {
            _sr = localizer;
            _log = logger;
            _responseTypeDecider = responseTypeDecider;
        }

        private IStringLocalizer _sr;
        private ILogger _log;
        private IDecideErrorResponseType _responseTypeDecider;

        public IActionResult Error(int statusCode = 500)
        {
            ViewData["Title"] = _sr["Oops!"];


            var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusFeature != null)
            {
                var originalPath = statusFeature.OriginalPath;

                if (statusCode == 404)
                {
                    var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                    _log.LogWarning($"handled 404 for url: {originalPath} from ipaddress {ipAddress}");

                }

                var shouldReturnJson = _responseTypeDecider.ShouldReturnJson(originalPath, statusCode);

                if (shouldReturnJson)
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
                       
                        default:

                            if (statusCode >= 500)
                            {
                                errorMessage = "Unexpected Error";
                            }

                            break;
                    }

                    var jsonResult = new JsonResult(new { error = errorMessage })
                    {
                        StatusCode = statusCode
                    };
                    return jsonResult;
                }


            }
            

            
            

            return View(statusCode);
        }

    }
}
