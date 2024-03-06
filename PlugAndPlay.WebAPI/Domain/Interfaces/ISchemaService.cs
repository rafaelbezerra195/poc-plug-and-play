using System.Text.Json.Nodes;
using Azure.Core;
using PlugAndPlay.WebAPI.Domain.Entities;
using Request = PlugAndPlay.WebAPI.Domain.Entities.Request;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface ISchemaService
{
    Task<List<string>> RequestIsValid(JsonObject body);
    Task<Request> BuildRequest(JsonObject body);
    void UpsertRequest(Request request);
}