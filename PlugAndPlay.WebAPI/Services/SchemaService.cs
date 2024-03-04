using System.Text.Json.Nodes;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Services;

public class SchemaService : ISchemaService
{
    private readonly DbContext _context;

    public SchemaService(DbContext context)
    {
        _context = context;
    }
    public List<string> RequestIsValid(JsonObject body)
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

            RequestSchema request = getRequestSchema(type, origin);
            if (request == null)
            {
                return new List<string>() { $"request schema type {type} and origin {origin} not found" };
            }
        
            List<FieldSchema> fields = getFieldsSchema(request.Id);
            if (fields.Count == 0)
            {
                return new List<string>() { $"fields schema not found for request Schema type {type} and origin {origin}" };
            }
        
            errors = checkRequiredFields(body, fields);
        }
        catch (Exception e)
        {
            throw e;
        }

        return errors;
    }

    private List<string> checkRequiredFields(JsonObject body, List<FieldSchema> fields)
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


    private List<FieldSchema> getFieldsSchema(int requestId)
    {
        var connection = _context.Database.GetDbConnection();
        string sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_FIELD_SCHEMA] where RequestSchemaId = {requestId}";
        var results = connection.Query<FieldSchema>(sql);

        return results.ToList();
    }

    public RequestSchema getRequestSchema(string type, string origin)
    {
        var connection = _context.Database.GetDbConnection();
        string sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_REQUEST_SCHEMA] " +
                     $"WHERE type = '{type}' and origin = '{origin}'";
        var results = connection.Query<RequestSchema>(sql);
        return results.FirstOrDefault();
    }
}