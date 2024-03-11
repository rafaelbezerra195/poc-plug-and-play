using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly ILogger<RequestController> _logger;
    private readonly ISchemaService _schemaService;

    public RequestController(ILogger<RequestController> logger, ISchemaService schemaService)
    {
        _logger = logger;
        _schemaService = schemaService;
    }
    
    [HttpPost(Name = "Request")]
    public async Task<IActionResult> Post([FromBody] RequestJson body)
    {
        List<string> errors = await _schemaService.RequestIsValid(body); 
        if (errors.Count != 0)
        {
            return BadRequest(errors);
        }

        // Request request = await _schemaService.BuildRequest(body);
        //_schemaService.UpsertRequest(request);

        return Accepted(new Request());
    }
}