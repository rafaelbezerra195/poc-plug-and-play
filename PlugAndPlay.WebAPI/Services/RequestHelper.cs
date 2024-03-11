using System.Text.Json;
using System.Text.Json.Nodes;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestHelper
{

    public static List<JsonElement> FindExtraFields(FieldGroup fieldGroup, string fieldSchemaName)
    {
        try
        {
            var record = fieldGroup.records.Where(field =>
            {
                field.TryGetProperty(fieldSchemaName, out JsonElement prop);
                return prop.ValueKind != JsonValueKind.Undefined;
            }).Select(field => field.GetProperty(fieldSchemaName));
            
            return record.ToList();
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