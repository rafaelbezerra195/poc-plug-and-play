using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface IRequestRepository
{
    int UpsertRequest(Request request);
    void UpsertFields(List<Field> fields);
    Request GetRequest(int requestSchemaId, string documentNumber);
    Request GetRequestById(int id);
    List<Field> GetFields(int requestId);
    List<FieldSchemaType> FieldSchemaType { get; }
}