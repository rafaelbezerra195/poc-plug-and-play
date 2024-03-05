using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Repositories;

public class RequestRepository: IRequestRepository
{
    private readonly DbConnection _connection;

    public RequestRepository(DbContext context)
    {
        _connection = context.Database.GetDbConnection();
    }
    
    public async Task<int> UpsertRequest(Request request)
    {
        if (request.Id == 0) return await InsertRequest(request);
        await UpdateRequest(request);
        return request.Id;
    }

    private async Task<int> InsertRequest(Request request)
    {
        string sql = """
                     INSERT INTO [PlugAndPlay].[dbo].[REQUEST] (Type, RequestSchemaId, Requester, DocumentNumber, Status, Currency, InternalUniqueKey, CreateDate, UpdateDate)
                                                VALUES (@Type, @RequestSchemaId, @Requester, @DocumentNumber, @Status, @Currency, @InternalUniqueKey, @CreateDate, @UpdateDate);
                                                SELECT CAST(SCOPE_IDENTITY() as int)
                     """;
        request.Id = await _connection.ExecuteScalarAsync<int>(sql, request);
        return request.Id;
    }
    
    private async Task UpdateRequest(Request request)
    {
        const string sql = """
                           UPDATE [PlugAndPlay].[dbo].[REQUEST]
                                                  SET Type = @Type,
                                                      RequestSchemaId = @RequestSchemaId,
                                                      Requester = @Requester,
                                                      DocumentNumber = @DocumentNumber,
                                                      Status = @Status,
                                                      Currency = @Currency,
                                                      InternalUniqueKey = @InternalUniqueKey,
                                                      UpdateDate = @UpdateDate
                                                  WHERE Id = @Id
                           """;
        await _connection.ExecuteAsync(sql, request);
    }

    public async Task UpsertFields(List<Field> fields)
    {
        await DeleteFieldsByRequest(fields.FirstOrDefault()!.RequestId);
        await InsertFields(fields);
    }

    private async Task DeleteFieldsByRequest(int requestId)
    {
        string sql = @"DELETE FROM [PlugAndPlay].[dbo].[REQUEST] WHERE RequestId = @requestId";
        await _connection.ExecuteAsync(sql, requestId);
    }

    private async Task InsertFields(List<Field> fields)
    {
        string sql = @"INSERT INTO Fields (RequestId, FieldSchemaId, Value, CreateDate, UpdateDate)
                           VALUES (@RequestId, @FieldSchemaId, @Value, @CreateDate, @UpdateDate)";
        await _connection.ExecuteAsync(sql, fields);
    }
}