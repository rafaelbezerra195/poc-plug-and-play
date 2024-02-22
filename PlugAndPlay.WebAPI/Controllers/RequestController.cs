using Microsoft.AspNetCore.Mvc;

namespace PlugAndPlay.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly ILogger<RequestController> _logger;

    public RequestController(ILogger<RequestController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost(Name = "Request")]
    public IActionResult Post()
    {
        return Ok();
    }
}