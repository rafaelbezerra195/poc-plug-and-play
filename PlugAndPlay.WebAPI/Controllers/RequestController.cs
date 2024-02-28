using System.Text.Json.Nodes;
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
    public async Task<IActionResult> Post([FromBody] JsonObject body)
    {
        return Ok(body["DocumentType"]);
    }
}