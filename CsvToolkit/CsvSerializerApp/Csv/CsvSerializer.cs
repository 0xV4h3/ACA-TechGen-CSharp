using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace CsvSerializerApp.Csv;

public static class CsvSerializer
{
    private sealed record ColumnPlan(
        PropertyInfo Property,
        string Header,
        int Order,
        int DeclarationIndex);

    private static readonly ConcurrentDictionary<Type, IReadOnlyList<ColumnPlan>> PlanCache = new();

    public static string WriteAll<T>(IEnumerable<T> rows)
    {
        var plans = GetPlan(typeof(T));
        var sb = new StringBuilder();
        
        sb.AppendLine(string.Join(",", plans.Select(p => p.Header)));
        
        foreach (var row in rows)
        {
            var cells = new string[plans.Count];
            for (int i = 0; i < plans.Count; i++)
            {
                var value = plans[i].Property.GetValue(row);
                cells[i] = EscapeCell(FormatValue(value));
            }

            sb.AppendLine(string.Join(",", cells));
        }

        return sb.ToString();
    }

    public static List<T> ReadAll<T>(string csv) where T : new()
    {
        var result = new List<T>();
        if (string.IsNullOrWhiteSpace(csv))
            return result;

        var lines = ReadCsvRecords(csv).ToList();
        if (lines.Count == 0)
            return result;
        
        var headerCells = SplitCsvLine(lines[0]);
        var headerToIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < headerCells.Count; i++)
        {
            if (!headerToIndex.ContainsKey(headerCells[i]))
                headerToIndex[headerCells[i]] = i;
        }

        var plans = GetPlan(typeof(T));
        
        for (int lineIndex = 1; lineIndex < lines.Count; lineIndex++)
        {
            var cells = SplitCsvLine(lines[lineIndex]);
            if (cells.Count == 1 && cells[0].Length == 0)
                continue;

            var item = new T();

            foreach (var plan in plans)
            {
                if (!plan.Property.CanWrite)
                    continue;

                if (!headerToIndex.TryGetValue(plan.Header, out var colIndex))
                    continue;

                var raw = colIndex < cells.Count ? cells[colIndex] : string.Empty;
                var converted = ConvertCell(raw, plan.Property.PropertyType);
                plan.Property.SetValue(item, converted);
            }

            result.Add(item);
        }

        return result;
    }

    private static IReadOnlyList<ColumnPlan> GetPlan(Type type)
    {
        if (PlanCache.TryGetValue(type, out var cached))
            return cached;

        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var list = new List<ColumnPlan>(props.Length);

        for (int i = 0; i < props.Length; i++)
        {
            var p = props[i];
            if (!p.CanRead) continue;
            if (p.GetIndexParameters().Length != 0) continue;
            if (p.GetCustomAttribute<CsvIgnoreAttribute>() is not null) continue;

            var col = p.GetCustomAttribute<CsvColumnAttribute>();
            var header = col?.Header ?? p.Name;
            var order = col?.Order ?? int.MaxValue;

            list.Add(new ColumnPlan(p, header, order, i));
        }

        var plan = (IReadOnlyList<ColumnPlan>)list
            .OrderBy(x => x.Order)
            .ThenBy(x => x.DeclarationIndex)
            .ToArray();

        return PlanCache.GetOrAdd(type, plan);
    }

    private static string FormatValue(object? value)
    {
        if (value is null) return string.Empty;

        if (value is IFormattable f)
            return f.ToString(null, CultureInfo.InvariantCulture);

        return value.ToString() ?? string.Empty;
    }

    private static object? ConvertCell(string raw, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType);
        var type = underlying ?? targetType;

        if (string.IsNullOrEmpty(raw))
        {
            if (underlying != null) return null;
            if (type == typeof(string)) return "";
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        if (type == typeof(string)) return raw;
        if (type == typeof(Guid)) return Guid.Parse(raw);
        if (type.IsEnum) return Enum.Parse(type, raw, ignoreCase: true);
        if (type == typeof(DateTime))
            return DateTime.Parse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        if (type == typeof(DateTimeOffset))
            return DateTimeOffset.Parse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        if (type == typeof(bool))
            return bool.Parse(raw);

        return Convert.ChangeType(raw, type, CultureInfo.InvariantCulture);
    }

    private static string EscapeCell(string cell)
    {
        if (cell.Contains(',') || cell.Contains('"') || cell.Contains('\n') || cell.Contains('\r'))
            throw new InvalidOperationException(
                "CSV Serializer does not support quoted cells.");

        return cell;
    }
    
    private static List<string> SplitCsvLine(string line) => line.Split(',').Select(x => x.Trim()).ToList();
    
    private static IEnumerable<string> ReadCsvRecords(string csv)
    {
        using var sr = new StringReader(csv);
        string? line;
        while ((line = sr.ReadLine()) != null)
            yield return line;
    }
}