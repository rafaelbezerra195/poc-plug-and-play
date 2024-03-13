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
    
    public async Task<List<FieldSchema>> GetFieldSchemas(int requestId)
    {
        
        var sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_FIELD_SCHEMA] where RequestSchemaId = {requestId}";
        var results = await _connection.QueryAsync<FieldSchema>(sql);

        return results.ToList();
    }

    public async Task<RequestSchema> GetRequestSchema(string type, string origin)
    {
        var sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_REQUEST_SCHEMA] " +
                  $"WHERE type = '{type}' and origin = '{origin}'";
        var results = await _connection.QueryAsync<RequestSchema>(sql);
        return results.FirstOrDefault();
    }

    public async Task<RequestSchema> GetRequestSchemaById(int id)
    {
        var sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_REQUEST_SCHEMA] WHERE Id = '{id}'";
        var results = await _connection.QueryAsync<RequestSchema>(sql);
        return results.FirstOrDefault();
    }

    public async Task<List<TabSchema>> GetTabSchemas(int requestSchemaId)
    {
        var sql = $"SELECT * FROM [PlugAndPlay].[dbo].[BMA_TAB_SCHEMA] WHERE RequestSchemaId = {requestSchemaId}";
        var results = await _connection.QueryAsync<TabSchema>(sql);
        return results.ToList();
    }
}