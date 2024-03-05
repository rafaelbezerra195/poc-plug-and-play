using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface ISchemaRepository
{
    Task<List<FieldSchema>> getFieldSchemas(int requestId);

    Task<RequestSchema> getRequestSchema(string type, string origin);

    Task<int> UpsertRequest(Request request);

    Task UpsertFields(List<Field> fields);
}