using System.Reflection;
using System.Collections.Concurrent;

namespace AuditDifferTool.Audit;

public static class AuditDiffer
{
    private static readonly ConcurrentDictionary<Type, PropertyMetadata[]> PropertyCache = new();

    public static IReadOnlyList<AuditChange> Diff(object? before, object? after, string rootPath = "Order")
    {
        var changes = new List<AuditChange>();
        
        return changes;
    }
}

record PropertyMetadata(PropertyInfo Property, string AuditPathName, Func<object, object?> Getter);