namespace AuditDifferTool.Models;

public sealed class Money
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}