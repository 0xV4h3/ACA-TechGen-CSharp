namespace AuditDifferTool.Audit;

public sealed class AuditChange
{
    public string Path { get; init; } = "";
    public string? Old { get; init; }
    public string? New { get; init; }
}