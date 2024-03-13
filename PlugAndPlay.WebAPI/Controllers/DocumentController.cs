using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController: ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var result = await _documentService.GetDocumentDetail(id);
        return Ok(result);
    }
}