using PlugAndPlay.WebAPI.Domain.Entities;

namespace PlugAndPlay.WebAPI.Domain.Interfaces;

public interface IDocumentService
{
    Task<Document> GetDocumentDetail(int id);
}