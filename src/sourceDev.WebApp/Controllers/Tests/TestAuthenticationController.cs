using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/testauth")]
[Authorize(AuthenticationSchemes = "Test")]
public class TestAuthenticationController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { Message = "Authenticated to test controller" });
}