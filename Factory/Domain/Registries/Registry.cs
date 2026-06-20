namespace Domain.Registries;

public class Registry : IRegistry
{
    private readonly Dictionary<string, HashSet<string>> _storage = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string value, string context)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(context))
            throw new ArgumentException("Value and Context cannot be empty.");

        if (!_storage.TryGetValue(context, out var set))
        {
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _storage[context] = set;
        }

        set.Add(value);
    }

    public bool IsValid(string value, string context)
    {
        if (value == null || context == null) return false;

        return _storage.TryGetValue(context, out var set) && set.Contains(value);
    }

    public IEnumerable<string> GetAll(string context)
    {
        if (context == null) return Enumerable.Empty<string>();

        return _storage.TryGetValue(context, out var set) ? set.ToList() : Enumerable.Empty<string>();
    }
}