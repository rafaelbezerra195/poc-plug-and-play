using System.Text.Json.Nodes;
using Microsoft.IdentityModel.Tokens;
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
        var errors = new List<string>();

        try
        {
            var request = await _schemaRepository.getRequestSchema(body.requestType, body.requestOrigin);
            if (request == null)
            {
                return new List<string>() { $"request schema type {body.requestType} and origin {body.requestOrigin} not found" };
            }
        
            var fields = await _schemaRepository.getFieldSchemas(request.Id);
            if (fields.Count == 0)
            {
                return new List<string>() { $"fields schema not found for request Schema type {body.requestType} and origin {body.requestOrigin}" };
            }
        
            errors = CheckRequiredFields(body, fields);
        }
        catch (Exception e)
        {
            throw e;
        }

        return errors;
    }

    public async Task<Request> BuildRequest(JsonObject body)
    {   
        var type = body["requestType"]?.ToString();
        var origin = body["requestOrigin"]?.ToString();

        try
        {
            var requestSchema = await _schemaRepository.getRequestSchema(type, origin);
            var fieldSchemas = await _schemaRepository.getFieldSchemas(requestSchema.Id);

            var request = RequestBuilder.BuildRequest(requestSchema.Id, body);
            request.Fields = RequestBuilder.BuildFields(fieldSchemas, body);

            return request;
        }
        catch (Exception e)
        {
            throw e;
        }
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
            var extraField = body.extraFields.FirstOrDefault(extraField => extraField.fieldGroupName == fieldGroupName);

            if (extraField == null)
            {
                errors.Add($"field {field.Name} is required");
                continue;
            };

            var result = RequestHelper.FindExtraFields(extraField, fieldName); 
            if (extraField.fields.Count > result.Count)
            {
                errors.Add($"field {field.Name} is required");
            }
        }

        return errors;
    }

    public void UpsertRequest(Request request)
    {
        var requestId = _requestRepository.UpsertRequest(request);
        request.Fields.ForEach(field => field.RequestId = requestId);
        _requestRepository.UpsertFields(request.Fields);
    }
}