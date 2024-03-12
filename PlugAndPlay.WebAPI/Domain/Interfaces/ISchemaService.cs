using PlugAndPlay.WebAPI.Domain.Entities;
using Request = PlugAndPlay.WebAPI.Domain.Entities.Request;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface ISchemaService
{
    Task<List<string>> RequestIsValid(RequestJson body);
    Task<Request> BuildRequest(RequestJson body);
    void UpsertRequest(Request request);
}