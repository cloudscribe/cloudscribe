using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Tenant2SpaPolymer.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        //[Authorize(Policy = "SecureApiPolicy")]
        //[Authorize(Policy = "OtherPolicy")]
        [HttpGet]
        public IActionResult Get()
        {

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [Route("api/identity/get1")]
        [Authorize(Policy = "SecureApiPolicy")]
        [HttpGet]
        public IActionResult GetWithPolicy1()
        {

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [Route("api/identity/get2")]
        [Authorize(Policy = "OtherPolicy")]
        [HttpGet]
        public IActionResult GetWithPolicy2()
        {

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }

}
