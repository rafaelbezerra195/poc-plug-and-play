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
    public async Task<List<string>> RequestIsValid(JsonObject body)
    {
        List<string> errors = new List<string>();

        string type = body["requestType"]?.ToString();
        string origin = body["requestOrigin"]?.ToString();
        
        try
        {
            errors = CheckStandardFields(body);
            if (errors.Any()) return errors;

            RequestSchema request = await _schemaRepository.getRequestSchema(type, origin);
            if (request == null)
            {
                return new List<string>() { $"request schema type {type} and origin {origin} not found" };
            }
        
            List<FieldSchema> fields = await _schemaRepository.getFieldSchemas(request.Id);
            if (fields.Count == 0)
            {
                return new List<string>() { $"fields schema not found for request Schema type {type} and origin {origin}" };
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
        string type = body["requestType"]?.ToString();
        string origin = body["requestOrigin"]?.ToString();

        try
        {
            RequestSchema requestSchema = await _schemaRepository.getRequestSchema(type, origin);
            List<FieldSchema> fieldSchemas = await _schemaRepository.getFieldSchemas(requestSchema.Id);

            Request request = RequestBuilder.BuildRequest(requestSchema.Id, body);
            request.Fields = RequestBuilder.BuildFields(fieldSchemas, body);

            return request;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private List<string> CheckStandardFields(JsonObject body)
    {
        string[] standardFields = new string[] { "requestType", "requestKey", "requestOrigin", "status", "requester", "currency" };
        List<string> errors = new List<string>();
        
        foreach (var fieldName in standardFields)
        {
            if (body[fieldName] == null)
            {
                errors.Add($"field {fieldName} is required");
            }
        }

        return errors;
    }

    private List<string> CheckRequiredFields(JsonObject body, List<FieldSchema> fields)
    {
        List<string> errors = new List<string>();
        List<FieldSchema> requiredFields = fields.Where(field => field.Required).ToList();
        if (requiredFields.Count == 0) return errors;
        
        foreach (var field in requiredFields)
        {
            if (RequestHelper.FindField(body, field.Name) == null) 
            {
                errors.Add($"field {field.Name} is required");
            }
        }

        return errors;
    }

    public void UpsertRequest(Request request)
    {
        int requestId = _requestRepository.UpsertRequest(request);
        request.Fields.ForEach(field => field.RequestId = requestId);
        _requestRepository.UpsertFields(request.Fields);
    }
}