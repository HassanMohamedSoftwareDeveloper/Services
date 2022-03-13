using Microsoft.AspNetCore.Mvc;

namespace Services.CommandService.Controllers;
[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {

    }
    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound Post # Command Service");
        return Ok("Inbound test of from Platforms Controller");
    }

}
