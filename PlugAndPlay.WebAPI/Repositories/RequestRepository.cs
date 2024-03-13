using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PlugAndPlay.WebAPI.Domain.Entities;
using PlugAndPlay.WebAPI.Domain.Interfaces;

namespace PlugAndPlay.WebAPI.Repositories;

public class RequestRepository: IRequestRepository
{
    private readonly DbConnection _connection;
    public List<FieldSchemaType> FieldSchemaType { get;} 

    public RequestRepository(DbContext context)
    {
        _connection = context.Database.GetDbConnection();
        FieldSchemaType = GetFieldSchemaType();
    }

    private List<FieldSchemaType> GetFieldSchemaType()
    {
        const string sql = @"SELECT * from [PlugAndPlay].[dbo].[BMA_FIELD_SCHEMA_TYPES]";
        return _connection.Query<FieldSchemaType>(sql).ToList();
    }

    public int UpsertRequest(Request request)
    {
        request.Id = GetRequest(request.RequestSchemaId, request.DocumentNumber).Id;
        if (request.Id == 0) return InsertRequest(request);
        
        UpdateRequest(request);
        return request.Id;
    }

    public Request GetRequest(int requestSchemaId, string documentNumber)
    {
        const string sql = @"SELECT * from [PlugAndPlay].[dbo].[BMA_REQUEST] where RequestSchemaId = @RequestSchemaId and DocumentNumber = @DocumentNumber";
        var result = _connection.Query<Request>(sql, new {RequestSchemaId = requestSchemaId, DocumentNumber = documentNumber});
        return result.FirstOrDefault();
    }
    
    public Request GetRequestById(int id)
    {
        const string sql = @"SELECT * from [PlugAndPlay].[dbo].[BMA_REQUEST] where Id = @Id";
        var result = _connection.Query<Request>(sql, new {Id = id});
        return result.FirstOrDefault();
    }

    public List<Field> GetFields(int requestId)
    {
        const string sql = @"SELECT * from [PlugAndPlay].[dbo].[BMA_FIELDS] where RequestId = @RequestId";
        var result = _connection.Query<Field>(sql, new {RequestId = requestId});
        return result.ToList();
    }

    private int InsertRequest(Request request)
    {
        const string sql = """
                           INSERT INTO [PlugAndPlay].[dbo].[BMA_REQUEST] ( RequestSchemaId, Requester, DocumentNumber, Status, InternalUniqueKey, CreateDate, UpdateDate)
                           OUTPUT INSERTED.ID
                           VALUES ( @RequestSchemaId, @Requester, @DocumentNumber, @Status, @InternalUniqueKey, @CreateDate, @UpdateDate);
                           """;
        request.Id = _connection.ExecuteScalar<int>(sql, request); 
        return request.Id;
    }
    
    private void UpdateRequest(Request request)
    {
        const string sql = """
                           UPDATE [PlugAndPlay].[dbo].[BMA_REQUEST]
                                                  SET Requester = @Requester,
                                                      Status = @Status,
                                                      InternalUniqueKey = @InternalUniqueKey,
                                                      UpdateDate = @UpdateDate
                                                  WHERE Id = @Id
                           """;
        _connection.Execute(sql, request);
    }

    public void UpsertFields(List<Field> fields)
    {
        DeleteFieldsByRequest(fields.FirstOrDefault()!.RequestId);
        InsertFields(fields);
    }

    private void DeleteFieldsByRequest(int requestId)
    {
        const string sql = @"DELETE FROM [PlugAndPlay].[dbo].[BMA_FIELDS] WHERE RequestId = @RequestId";
        _connection.Execute(sql, new {RequestId = requestId});
    }

    private void InsertFields(List<Field> fields)
    {
        const string sql = """
                           INSERT INTO [PlugAndPlay].[dbo].[BMA_FIELDS] (RequestId, FieldSchemaId, Value, CreateDate, UpdateDate, Parent)
                           OUTPUT INSERTED.ID
                           VALUES (@RequestId, @FieldSchemaId, @Value, @CreateDate, @UpdateDate, @Parent)
                           """;

        foreach (var field in fields)
        {
            field.Id = _connection.ExecuteScalar<int>(sql, field);

            if (field.Children == null) continue;
    
            field.Children.ToList().ForEach(fieldChild => fieldChild.Parent = field.Id);
            _connection.Execute(sql, field.Children);
        }            
    }
}