using Microsoft.AspNetCore.Mvc;

namespace adapter.api.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet()]
    public IActionResult Get()
        => Ok(new { status = "Live!"});
}
