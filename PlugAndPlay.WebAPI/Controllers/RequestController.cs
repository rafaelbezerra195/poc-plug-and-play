using System.Text.Json.Nodes;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlugAndPlay.WebAPI.Domain;
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
    public async Task<IActionResult> Post([FromBody] JsonObject body)
    {
        List<string> errors = await _schemaService.RequestIsValid(body); 
        if (errors.Any())
        {
            return BadRequest(errors);
        }

        Request request = await _schemaService.BuildRequest(body);
        //await _schemaService.UpsertRequest(request);

        return Accepted(request);
    }
}