using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface ISchemaRepository
{
    Task<List<FieldSchema>> GetFieldSchemas(int requestId);

    Task<RequestSchema> GetRequestSchema(string type, string origin);
}