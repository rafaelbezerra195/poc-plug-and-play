using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Services;

public class DocumentService: IDocumentService
{
    private readonly IRequestRepository _requestRepository;
    private readonly ISchemaRepository _schemaRepository;
    private readonly IDocumentBuilder _documentBuilder;

    public DocumentService(IRequestRepository requestRepository, ISchemaRepository schemaRepository, IDocumentBuilder documentBuilder)
    {
        _requestRepository = requestRepository;
        _schemaRepository = schemaRepository;
        _documentBuilder = documentBuilder;
    }

    public async Task<Document> GetDocumentDetail(int Id)
    {
        var request = _requestRepository.GetRequestById(Id);
        request.Fields = _requestRepository.GetFields(request.Id);
        
        var requestSchema = await _schemaRepository.GetRequestSchemaById(request.RequestSchemaId);
        requestSchema.Tabs = await _schemaRepository.GetTabSchemas(requestSchema.Id);
        var fieldSchemas = await _schemaRepository.GetFieldSchemas(requestSchema.Id);
        requestSchema.Tabs.ForEach(tabSchema =>
        {
            tabSchema.Fields = fieldSchemas.Where(fieldSchema => fieldSchema.TabSchemaId == tabSchema.Id).ToList();
        });

        return _documentBuilder.BuildDocument(requestSchema, request);
    }
}