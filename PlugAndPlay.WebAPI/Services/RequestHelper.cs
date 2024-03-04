using System.Text.Json.Nodes;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestHelper
{
    public static object FindField(JsonObject body, string fieldName)
    {
        if (body[fieldName] != null)
        {
            return body[fieldName]; 
        }

        List<KeyValuePair<string, JsonNode>> nodeFields = body.Where(field => field.Value is JsonObject).ToList();
        foreach (var field in nodeFields)
        {
            object result = FindField((JsonObject)field.Value, fieldName);
            if (result != null)
            {
                return result;
            }
        }
        
        return null;
    }
}