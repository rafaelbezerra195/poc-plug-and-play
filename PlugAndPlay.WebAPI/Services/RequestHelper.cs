using System.Text.Json;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestHelper
{
    public static List<JsonElement> FindExtraFields(FieldGroup fieldGroup, string fieldSchemaName)
    {
        var record = fieldGroup.records.Where(field =>
        {
            field.TryGetProperty(fieldSchemaName, out JsonElement prop);
            return prop.ValueKind != JsonValueKind.Undefined;
        }).Select(field => field.GetProperty(fieldSchemaName));
        
        return record.ToList();
    }
    
    public static string GetFieldGroupName(string fieldName)
    {
        return !fieldName.Contains('.') ? "root" : fieldName.Split('.')[0];
    }
    
    public static string GetFieldName(string fieldName)
    {
        return !fieldName.Contains('.') ? fieldName : fieldName.Split('.')[1];
    }

    public static string? GetFieldValue(JsonElement record, string fieldSchemaName)
    {
        record.TryGetProperty(fieldSchemaName, out var prop);
        return prop.ValueKind == JsonValueKind.Undefined ? null : record.GetProperty(fieldSchemaName).ToString();
    }
}