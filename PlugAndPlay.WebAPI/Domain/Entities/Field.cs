namespace PlugAndPlay.WebAPI.Domain.Entities;

public class Field
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int FieldSchemaId { get; set; }
    public string Value { get; set; }
    public int Parent { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
    public IEnumerable<Field> Children { get; set; }
}