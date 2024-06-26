using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Services;

public class SchemaService : ISchemaService
{
    private readonly ISchemaRepository _schemaRepository;
    private readonly IRequestRepository _requestRepository;

    public SchemaService(ISchemaRepository schemaRepository, IRequestRepository requestRepository)
    {
        _schemaRepository = schemaRepository;
        _requestRepository = requestRepository;
    }
    public async Task<List<string>> RequestIsValid(RequestJson body)
    {
        var request = await _schemaRepository.GetRequestSchema(body.requestType, body.requestOrigin);
        if (request == null)
        {
            return new List<string>() { $"request schema type {body.requestType} and origin {body.requestOrigin} not found" };
        }
    
        var fields = await _schemaRepository.GetFieldSchemas(request.Id);
        if (fields.Count == 0)
        {
            return new List<string>() { $"fields schema not found for request Schema type {body.requestType} and origin {body.requestOrigin}" };
        }

        return CheckRequiredFields(body, fields);
    }
    
    private List<string> CheckRequiredFields(RequestJson body, List<FieldSchema> fields)
    {
        var errors = new List<string>();
        var requiredFields = fields.Where(field => field.Required).ToList();
        if (requiredFields.Count == 0) return errors;
        
        foreach (var field in requiredFields)
        {
            var fieldGroupName = RequestHelper.GetFieldGroupName(field.Name);
            var fieldName = RequestHelper.GetFieldName(field.Name);
            var fieldGroup = body.fieldGroups.FirstOrDefault(fieldGroup => fieldGroup.fieldGroupName == fieldGroupName);

            if (fieldGroup == null)
            {
                errors.Add($"field {field.Name} is required");
                continue;
            }

            var result = RequestHelper.FindExtraFields(fieldGroup, fieldName); 
            if (fieldGroup.records.Count > result.Count)
            {
                errors.Add($"field {field.Name} is required");
            }
        }

        return errors;
    }

    public async Task<Request> BuildRequest(RequestJson body)
    {   
        var requestSchema = await _schemaRepository.GetRequestSchema(body.requestType, body.requestOrigin);
        var fieldSchemas = await _schemaRepository.GetFieldSchemas(requestSchema.Id);

        var request = RequestBuilder.BuildRequest(requestSchema.Id, body);
        request.Fields = RequestBuilder.BuildFields(fieldSchemas, body.fieldGroups);

        return request;
    }
    
    public void UpsertRequest(Request request)
    {
        var requestId = _requestRepository.UpsertRequest(request);
        request.Fields.ForEach(field =>
        { 
            field.RequestId = requestId;
            field.Children.ToList().ForEach(childField => childField.RequestId = requestId);
        });
        _requestRepository.UpsertFields(request.Fields);
    }
}