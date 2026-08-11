using System.Reflection;
using System.Collections;
using System.Collections.Concurrent;
using AuditDifferTool.Utils;

namespace AuditDifferTool.Audit;

internal record PropertyMetadata(PropertyInfo Property, string AuditPathName, Func<object, object?> Getter);

public static class AuditDiffer
{
    private static readonly ConcurrentDictionary<Type, PropertyMetadata[]> PropertyCache = new();

    public static IReadOnlyList<AuditChange> Diff(object? before, object? after, string rootPath)
    {
        var changes = new List<AuditChange>();
        var visitedPairs = new HashSet<ReferencePair>(ReferencePairComparer.Instance);

        DiffInternal(before, after, rootPath, changes, visitedPairs);
        return changes;
    }

    private static void DiffInternal(
        object? before,
        object? after,
        string path,
        List<AuditChange> changes,
        HashSet<ReferencePair> visitedPairs)
    {
        if (ReferenceEquals(before, after))
            return;
        
        if (before is null || after is null)
        {
            if (!AreEqualScalars(before, after))
            {
                changes.Add(new AuditChange
                {
                    Path = path,
                    Old = FormatValue(before),
                    New = FormatValue(after)
                });
            }
            return;
        }

        var beforeType = before.GetType();
        var afterType = after.GetType();
        
        if (beforeType != afterType)
        {
            changes.Add(new AuditChange
            {
                Path = path,
                Old = FormatValue(before),
                New = FormatValue(after)
            });
            return;
        }
        
        if (IsSimpleType(beforeType))
        {
            if (!AreEqualScalars(before, after))
            {
                changes.Add(new AuditChange
                {
                    Path = path,
                    Old = FormatValue(before),
                    New = FormatValue(after)
                });
            }
            return;
        }
        
        if (!beforeType.IsValueType)
        {
            var pair = new ReferencePair(before, after);
            if (!visitedPairs.Add(pair))
                return;
        }
        
        if (IsCollectionType(beforeType))
        {
            DiffCollections((IEnumerable)before, (IEnumerable)after, path, changes, visitedPairs);
            return;
        }
        
        var props = GetProperties(beforeType);
        foreach (var p in props)
        {
            var bVal = p.Getter(before);
            var aVal = p.Getter(after);

            var childPath = $"{path}.{p.AuditPathName}";
            DiffInternal(bVal, aVal, childPath, changes, visitedPairs);
        }
    }

    private static void DiffCollections(
        IEnumerable beforeEnumerable,
        IEnumerable afterEnumerable,
        string path,
        List<AuditChange> changes,
        HashSet<ReferencePair> visitedPairs)
    {
        var beforeList = beforeEnumerable.Cast<object?>().ToList();
        var afterList = afterEnumerable.Cast<object?>().ToList();

        var max = Math.Max(beforeList.Count, afterList.Count);
        for (int i = 0; i < max; i++)
        {
            var b = i < beforeList.Count ? beforeList[i] : null;
            var a = i < afterList.Count ? afterList[i] : null;

            var itemPath = $"{path}[{i}]";
            
            if (b is null && a is null)
                continue;

            var knownType = b?.GetType() ?? a?.GetType();
            if (knownType is null)
                continue;
            
            if (IsSimpleType(knownType))
            {
                if (!AreEqualScalars(b, a))
                {
                    changes.Add(new AuditChange
                    {
                        Path = itemPath,
                        Old = FormatValue(b),
                        New = FormatValue(a)
                    });
                }
                continue;
            }
            
            DiffInternal(b, a, itemPath, changes, visitedPairs);
        }
    }

    private static PropertyMetadata[] GetProperties(Type t)
    {
        if (PropertyCache.TryGetValue(t, out var cached))
            return cached;

        var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var result = new List<PropertyMetadata>(props.Length);

        foreach (var p in props)
        {
            if (!p.CanRead) continue;
            if (p.GetIndexParameters().Length != 0) continue;
            if (p.GetCustomAttribute<AuditIgnoreAttribute>() is not null) continue;

            var auditName = p.GetCustomAttribute<AuditNameAttribute>()?.Name ?? p.Name;

            result.Add(new PropertyMetadata(
                p,
                auditName,
                obj => p.GetValue(obj)));
        }

        var arr = result.ToArray();
        PropertyCache[t] = arr;
        return arr;
    }

    private static bool IsCollectionType(Type t) =>
        t != typeof(string) && typeof(IEnumerable).IsAssignableFrom(t);

    private static bool IsSimpleType(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;

        if (t.IsEnum) return true;

        return t.IsPrimitive
               || t == typeof(string)
               || t == typeof(decimal)
               || t == typeof(DateTime)
               || t == typeof(DateTimeOffset)
               || t == typeof(TimeSpan)
               || t == typeof(Guid);
    }

    private static bool AreEqualScalars(object? left, object? right) =>
        Equals(left, right);

    private static string? FormatValue(object? value)
    {
        if (value is null) return "(missing)";
        if (value is string s) return s;
        if (value is DateTime dt) return dt.ToString("O");
        if (value is DateTimeOffset dto) return dto.ToString("O");
        if (value is IEnumerable e && value is not string)
        {
            var items = e.Cast<object?>().Select(FormatValue);
            return "[" + string.Join(",", items) + "]";
        }
        return Convert.ToString(value);
    }
}