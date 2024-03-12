namespace PlugAndPlay.WebAPI.Domain.Entities;

public class Request
{
    public int Id { get; set; }
    public int RequestSchemaId { get; set; }
    public string Requester { get; set; }
    public string DocumentNumber { get; set; }
    public string Status { get; set; }
    public string InternalUniqueKey { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
    public List<Field> Fields { get; set; }
}