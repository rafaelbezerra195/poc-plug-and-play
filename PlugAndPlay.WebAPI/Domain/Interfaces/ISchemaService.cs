using System.Text.Json.Nodes;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface ISchemaService
{
    List<string> RequestIsValid(JsonObject body);
}