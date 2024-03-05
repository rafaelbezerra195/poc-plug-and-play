using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Repositories;

public class SchemaRepository: ISchemaRepository
{
    private readonly DbConnection _connection;

    public SchemaRepository(DbContext context)
    {
        _connection = context.Database.GetDbConnection();
    }
    
    public async Task<List<FieldSchema>> getFieldSchemas(int requestId)
    {
        
        string sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_FIELD_SCHEMA] where RequestSchemaId = {requestId}";
        var results = _connection.Query<FieldSchema>(sql);

        return results.ToList();
    }

    public async Task<RequestSchema> getRequestSchema(string type, string origin)
    {
        string sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_REQUEST_SCHEMA] " +
                     $"WHERE type = '{type}' and origin = '{origin}'";
        var results = _connection.Query<RequestSchema>(sql);
        return results.FirstOrDefault();
    }
}