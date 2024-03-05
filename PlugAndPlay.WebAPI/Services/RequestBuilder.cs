using System.Text.Json.Nodes;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestBuilder
{
    public static Request BuildRequest(int requestSchemaId, JsonObject body)
    {
        return new Request()
        {
            Type = RequestHelper.FindField(body,"type").AsValue().ToString(),
            RequestSchemaId = requestSchemaId,
            Requester = RequestHelper.FindField(body,"requester").AsValue().ToString(),
            DocumentNumber = RequestHelper.FindField(body,"documentNumber").AsValue().ToString(),
            Status = RequestHelper.FindField(body,"status").AsValue().ToString(),
            Currency = RequestHelper.FindField(body,"currency").AsValue().ToString(),
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
    }

    public static List<Field> BuildFields(List<FieldSchema> fieldSchemas, JsonObject body)
    {
        List<Field> fields = new List<Field>();
        foreach (var fieldSchema in fieldSchemas)
        {
            string value = String.Empty;
            try
            {
                value = RequestHelper.FindField(body, fieldSchema.Name).AsValue().ToString();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            fields.Add(new Field()
            {
                FieldSchemaId = fieldSchema.Id,
                Value = value,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });
        }

        return fields;
    }
}