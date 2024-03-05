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

        string type = body["type"]?.ToString();
        string origin = body["origin"]?.ToString();
        
        try
        {
            if (type.IsNullOrEmpty() || origin.IsNullOrEmpty())
            {
                return new List<string>() { "Fields type and origin is required." };
            }

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
    {   string type = body["type"]?.ToString();
        string origin = body["origin"]?.ToString();

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

        return new Request();
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
    
    public async Task UpsertRequest(Request request)
    {
        int requestId = await _requestRepository.UpsertRequest(request);
        request.Fields.ForEach(field => field.RequestId = requestId);
        await _requestRepository.UpsertFields(request.Fields);
    }
}