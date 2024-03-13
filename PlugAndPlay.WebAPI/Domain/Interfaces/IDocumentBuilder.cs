using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface IDocumentBuilder
{
    Document BuildDocument(RequestSchema requestSchema, Request request);
}