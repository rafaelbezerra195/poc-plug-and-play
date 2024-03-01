using System.Text.Json.Nodes;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PlugAndPlay.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly ILogger<RequestController> _logger;
    private readonly DbContext _context;
    
    public RequestController(ILogger<RequestController> logger, DbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost(Name = "Request")]
    public async Task<IActionResult> Post([FromBody] JsonObject body)
    {
        var connection = _context.Database.GetDbConnection();
        var results = connection.Query("SELECT * FROM BMA_APPROVAL_RULES");
        
        return Ok(body["DocumentType"]);
    }
}