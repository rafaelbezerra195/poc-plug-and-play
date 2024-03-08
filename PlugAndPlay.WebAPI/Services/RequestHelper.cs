using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestHelper
{
    public static JsonValue FindField(JsonObject body, string fieldName)
    {
        if (body[fieldName] != null)
        {
            return body[fieldName].AsValue(); 
        }
        
        List<KeyValuePair<string, JsonNode>> nodeFields = body.AsObject().Where(field => field.Value is JsonObject).ToList();
        foreach (var field in nodeFields)
        {
            var result = FindField(field.Value.AsObject(), fieldName);
            if (result != null)
            {
                return result;
            }
        }
        
        return null;
    }

    public static List<JsonElement> FindExtraFields(ExtraField extraField, string fieldSchemaName)
    {
        try
        {
            var fields = extraField.fields.Where(field =>
            {
                field.TryGetProperty(fieldSchemaName, out JsonElement prop);
                return prop.ValueKind != JsonValueKind.Undefined;
            }).Select(field => field.GetProperty(fieldSchemaName));
            
            return fields.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    public static string GetFieldGroupName(string fieldName)
    {
        return !fieldName.Contains('.') ? "root" : fieldName.Split('.')[0];
    }
    
    public static string GetFieldName(string fieldName)
    {
        return !fieldName.Contains('.') ? fieldName : fieldName.Split('.')[1];
    }
}