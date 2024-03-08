using System.Text.Json.Nodes;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestBuilder
{
    public static Request BuildRequest(int requestSchemaId, JsonObject body)
    {
        return new Request()
        {
            Type = RequestHelper.FindField(body,"requestType").AsValue().ToString(),
            RequestSchemaId = requestSchemaId,
            Requester = RequestHelper.FindField(body,"requester").AsValue().ToString(),
            DocumentNumber = RequestHelper.FindField(body,"requestKey").AsValue().ToString(),
            Status = RequestHelper.FindField(body,"status").AsValue().ToString(),
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            InternalUniqueKey = String.Empty
        };
    }

    public static List<Field> BuildFields(List<FieldSchema> fieldSchemas, JsonObject body)
    {
        List<Field> fields = new List<Field>();
        foreach (var fieldSchema in fieldSchemas)
        {
            var value = RequestHelper.FindField(body, fieldSchema.Name);
            if (value == null) continue;
                 
            fields.Add(new Field()
            {
                FieldSchemaId = fieldSchema.Id,
                Value = value.ToString(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });
        }

        return fields;
    }
}