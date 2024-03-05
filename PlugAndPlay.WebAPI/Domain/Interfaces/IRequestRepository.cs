using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface IRequestRepository
{
    Task<int> UpsertRequest(Request request);

    Task UpsertFields(List<Field> fields);
}