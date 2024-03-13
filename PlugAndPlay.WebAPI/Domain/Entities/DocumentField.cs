namespace PlugAndPlay.WebAPI.Domain.Entities;

public class DocumentField
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public List<DocumentField> Fields { get; set; }
}