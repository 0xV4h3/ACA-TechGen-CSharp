using System.Runtime.CompilerServices;

namespace AuditDifferTool.Utils;

internal sealed class ReferencePairComparer : IEqualityComparer<ReferencePair>
{
    public static readonly ReferencePairComparer Instance = new();

    public bool Equals(ReferencePair x, ReferencePair y) =>
        ReferenceEquals(x.Left, y.Left) && ReferenceEquals(x.Right, y.Right);

    public int GetHashCode(ReferencePair obj)
    {
        var h1 = RuntimeHelpers.GetHashCode(obj.Left);
        var h2 = RuntimeHelpers.GetHashCode(obj.Right);
        return HashCode.Combine(h1, h2);
    }
}