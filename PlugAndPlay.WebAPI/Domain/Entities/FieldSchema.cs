namespace PlugAndPlay.WebAPI.Domain.Entities;

public class FieldSchema
{
    public int Id { get; set; }
    public int RequestSchemaId { get; set; }
    public string Name { get; set; }
    public int TabSchemaId { get; set; }
    public int FieldSchemaTypesId { get; set; }
    public bool Required { get; set; }
    public string Display { get; set; }
    public bool Visible { get; set; }
    public int Order { get; set; }
    public int Parent { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}