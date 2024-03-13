namespace PlugAndPlay.WebAPI.Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public string Supplier { get; set; }
    public string Display { get; set; }
    public string Status { get; set; }
    public List<DocumentField> Header { get; set; }
    public List<DocumentTab> Tabs { get; set; }
}

