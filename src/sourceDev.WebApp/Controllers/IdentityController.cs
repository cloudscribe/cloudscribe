using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace sourceDev.WebApp.Controllers
{
    [Route("api/[controller]")]

    public class IdentityController : ControllerBase
    {
        private const string AuthSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme;

        //[Authorize(Policy ="AdminPolicy", AuthenticationSchemes = AuthSchemes)]
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
