namespace PlugAndPlay.WebAPI.Domain.Entities;

public class RequestSchema
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Origin { get; set; }
    public string Name { get; set; }
    public string Display { get; set; }
    public bool Active { get; set; }
    public bool AllowComments { get; set; }
    public int ApprovalRuleId { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
}