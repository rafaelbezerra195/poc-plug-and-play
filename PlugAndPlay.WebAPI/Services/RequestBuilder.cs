using System.Text.Json;
using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Services;

public static class RequestBuilder
{
    public static Request BuildRequest(int requestSchemaId, RequestJson body)
    {
        return new Request()
        {
            RequestSchemaId = requestSchemaId,
            Requester = body.requestRequester,
            DocumentNumber = body.requestKey,
            Status = body.requestStatus,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            InternalUniqueKey = string.Empty
        };
    }

    public static List<Field> BuildFields(List<FieldSchema> fieldSchemas, List<FieldGroup> fieldGroups)
    {
        var fields = new List<Field>();
        var fieldGroupNames = fieldSchemas.Select(fieldSchema => RequestHelper.GetFieldGroupName(fieldSchema.Name))
                                                    .Distinct().ToList();

        foreach (var fieldGroupName in fieldGroupNames)
        {
            var fieldSchemasByFieldGroup = fieldSchemas.Where(fieldSchema => RequestHelper.GetFieldGroupName(fieldSchema.Name) == fieldGroupName);
            var fieldGroup = fieldGroups.FirstOrDefault(fieldGroup => fieldGroup.fieldGroupName == fieldGroupName);
            var needComplexField = fieldGroup.records.Count > 1;
            
            foreach (var record in fieldGroup.records)
            {
                var fieldsFromRecord = GetFieldsFromRecord(fieldSchemasByFieldGroup, record);

                if (!needComplexField)
                {
                    fields.AddRange(fieldsFromRecord);
                    continue;
                }

                var complexFieldTypeId = GetFieldSchemaComplexType(fieldSchemasByFieldGroup);
                var complexField = BuildComplexField(complexFieldTypeId, fieldsFromRecord);
                fields.Add(complexField);
            }
        }

        return fields;
    }
    
    private static Field BuildComplexField(int complexFieldTypeId, IEnumerable<Field> children)
    {
        return new Field()
        {
            FieldSchemaId = complexFieldTypeId,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            Children = children,
            Value = string.Empty
        };
    }
    
    private static IEnumerable<Field> GetFieldsFromRecord(IEnumerable<FieldSchema> fieldSchemas, JsonElement record)
    {
        return fieldSchemas.Where(fieldSchema => RequestHelper.GetFieldValue(record, RequestHelper.GetFieldName(fieldSchema.Name)) != null).Select(
            fieldSchema => new Field()
            {
                FieldSchemaId = fieldSchema.Id,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Value = RequestHelper.GetFieldValue(record, RequestHelper.GetFieldName(fieldSchema.Name))
            }).ToList();
    }

    private static int GetFieldSchemaComplexType(IEnumerable<FieldSchema> fieldSchemas)
    {
        return fieldSchemas.FirstOrDefault(fieldSchema => fieldSchema.Parent != 0).Parent;
    }
}