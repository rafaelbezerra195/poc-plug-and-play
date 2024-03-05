using System.Text.Json.Nodes;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestBuilder
{
    public static Request BuildRequest(int requestSchemaId, JsonObject body)
    {
        return new Request()
        {
            Type = (string)body["type"],
            RequestSchemaId = requestSchemaId,
            Requester = (string)body["requester"],
            DocumentNumber = (string)body["documentnumber"],
            Status = (string)body["status"],
            Currency = (string)body["currency"],
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
    }

    public static List<Field> BuildFields(List<FieldSchema> fieldSchemas, JsonObject body)
    {
        List<Field> fields = new List<Field>();
        foreach (var fieldSchema in fieldSchemas)
        {
            fields.Add(new Field()
            {
                FieldSchemaId = fieldSchema.Id,
                Value = body[fieldSchema.Name]?.AsValue().ToString(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });
        }

        return fields;
    }
}