namespace PlugAndPlay.WebAPI.Domain.Entities;

public class TabSchema
{
    public int Id { get; set; }
    public int RequestSchemaId { get; set; }
    public string Name { get; set; }
    public string Display { get; set; }
    public int Order { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public List<FieldSchema> Fields { get; set; }
}