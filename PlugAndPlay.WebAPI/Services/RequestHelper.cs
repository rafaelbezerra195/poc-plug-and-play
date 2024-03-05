using System.Text.Json.Nodes;

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
            JsonValue result = FindField(field.Value.AsObject(), fieldName);
            if (result != null)
            {
                return result;
            }
        }
        
        return null;
    }
}