using Microsoft.AspNetCore.Mvc;
using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly ISchemaService _schemaService;

    public RequestController(ISchemaService schemaService)
    {
        _schemaService = schemaService;
    }
    
    [HttpPost(Name = "Request")]
    public async Task<IActionResult> Post([FromBody] RequestJson body)
    {
        var errors = await _schemaService.RequestIsValid(body); 
        if (errors.Count != 0)
        {
            return BadRequest(errors);
        }

        var request = await _schemaService.BuildRequest(body);
        _schemaService.UpsertRequest(request);

        return Accepted(request);
    }
}