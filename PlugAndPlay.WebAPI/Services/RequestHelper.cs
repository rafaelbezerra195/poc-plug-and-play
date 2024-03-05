using System.Text.Json.Nodes;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestHelper
{
    public static JsonNode FindField(JsonNode body, string fieldName)
    {
        if (body[fieldName] != null)
        {
            return body[fieldName]; 
        }

        try
        {
            List<KeyValuePair<string, JsonNode>> nodeFields = body.AsObject().Where(field => field.Value is JsonObject).ToList();
            foreach (var field in nodeFields)
            {
                JsonNode result = FindField(field.Value, fieldName);
                if (result != null)
                {
                    return result;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return null;
    }
}