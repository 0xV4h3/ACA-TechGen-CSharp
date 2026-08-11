namespace AuditDifferTool.Utils;

internal readonly struct ReferencePair(object left, object right)
{
    public object Left { get; } = left;
    public object Right { get; } = right;
}