namespace AuditDifferTool.Models;

public sealed class Node
{
    public string Name { get; set; } = "";
    public Node? Next { get; set; }
}