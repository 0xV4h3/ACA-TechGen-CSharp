using AuditDifferTool.Audit;

namespace AuditDifferTool.Models;

public sealed class Order
{
    public Guid Id { get; set; }

    [AuditName("Customer")]
    public string CustomerName { get; set; } = "";

    public string Status { get; set; } = "";
    public Money Total { get; set; } = new();
    public List<OrderLine> Lines { get; set; } = [];
    public List<string> Tags { get; set; } = [];

    [AuditIgnore]
    public byte[]? RowVersion { get; set; }
}