using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface IRequestRepository
{
    int UpsertRequest(Request request);

    void UpsertFields(List<Field> fields);
}