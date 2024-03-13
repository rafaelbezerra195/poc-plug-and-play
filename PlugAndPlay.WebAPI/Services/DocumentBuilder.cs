using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Services;

public class DocumentBuilder : IDocumentBuilder
{
    const string GROUP_TYPE_NAME = "complex";
    private readonly IRequestRepository _requestRepository;

    public DocumentBuilder(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public Document BuildDocument(RequestSchema requestSchema, Request request)
    {
        const string TAB_REQUEST = "request";
        var tabRequestId = requestSchema.Tabs.FirstOrDefault(tab => tab.Name == TAB_REQUEST).Id;
        
        return new Document()
        {
            Id = request.Id,
            Supplier = request.Requester,
            Display = requestSchema.Display,
            Status = request.Status,
            Header = BuildTabFields(tabRequestId, requestSchema, request),
            Tabs = BuildTabs(requestSchema, request)
        };
    }
    
    private List<DocumentTab> BuildTabs(RequestSchema requestSchema, Request request)
    {
        const string TAB_REQUEST = "request";
        return requestSchema.Tabs.Where(tabSchema => tabSchema.Name != TAB_REQUEST)
                                .Select(tabSchema => new DocumentTab()
                                {
                                    Name = tabSchema.Display, 
                                    Fields = BuildTabFields(tabSchema.Id, requestSchema, request)
                                }).ToList();
    }

    private List<DocumentField> BuildTabFields(int tabId, RequestSchema requestSchema, Request request)
    {
        var fieldSchemasFromTab = requestSchema.Tabs.FirstOrDefault(tab => tab.Id == tabId)?.Fields;
        var fieldSchemas =
            fieldSchemasFromTab.Where(fieldSchema => fieldSchema.FieldSchemaTypesId != GetFieldSchemaComplexType());
        var groupFieldSchemas = 
            fieldSchemasFromTab.Where(fieldSchema => fieldSchema.FieldSchemaTypesId == GetFieldSchemaComplexType());

        var fields =
            request.Fields.Where(field => fieldSchemas.Any(fieldSchema => fieldSchema.Id == field.FieldSchemaId)).ToList();
        
        var fieldGroups = 
            request.Fields.Where(field => groupFieldSchemas.Any(fieldSchema => fieldSchema.Id == field.FieldSchemaId)).ToList();
        
        fieldGroups.ForEach(fieldGroup => fieldGroup.Children = fields.ToList().Where(field => field.Parent == fieldGroup.Id));

        if (fieldGroups.Count == 0)
        {
            return fields.Select(field =>
            {
                var fieldSchema = fieldSchemas.FirstOrDefault(fieldSchema => fieldSchema.Id == field.FieldSchemaId);
                return BuildDocumentField(fieldSchema, field);
            }).ToList();
        }

        return fieldGroups.Select(fieldGroup =>
        {
            var fieldGroupSchema = groupFieldSchemas.FirstOrDefault(fieldSchema => fieldSchema.Id == fieldGroup.FieldSchemaId);
            var docGroupField = BuildDocumentField(fieldGroupSchema, fieldGroup);
            
            foreach (var field in fieldGroup.Children)
            {
                var fieldSchema = fieldSchemas.FirstOrDefault(fieldSchema => fieldSchema.Id == field.FieldSchemaId);
                docGroupField.Fields.Add(BuildDocumentField(fieldSchema, field));
            }
                
            return docGroupField;
        }).ToList();
    }

    private DocumentField BuildDocumentField(FieldSchema fieldSchema, Field field)
    {
        return new DocumentField()
        {
            Name = fieldSchema.Display,
            Type = GetFieldSchemaTypeName(fieldSchema.FieldSchemaTypesId),
            Value = field.Value,
            Fields = new List<DocumentField>()
        };
    }

    private string GetFieldSchemaTypeName(int id)
    {
        return _requestRepository.FieldSchemaType.FirstOrDefault(fieldSchema => fieldSchema.Id == id).Name;
    }

    private int GetFieldSchemaComplexType()
    {
        return _requestRepository.FieldSchemaType.FirstOrDefault(type => type.Name == GROUP_TYPE_NAME).Id;
    }
}