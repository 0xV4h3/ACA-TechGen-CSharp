namespace Domain.Registries;

public abstract class RangeConverterRegistry<TResult>
{
    private readonly Dictionary<string, IRangeConverter<TResult>> _converters = new(StringComparer.OrdinalIgnoreCase);

    public IRangeConverter<TResult> Create(string name, IEnumerable<RangeStep<TResult>> steps)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Converter name cannot be empty.", nameof(name));

        var converter = new SteppedRangeConverter<TResult>(steps);

        if (!_converters.TryAdd(name, converter))
            throw new InvalidOperationException($"Converter with name '{name}' is already registered.");

        return converter;
    }

    public IRangeConverter<TResult> GetByName(string name)
    {
        if (_converters.TryGetValue(name, out var converter)) return converter;
        throw new KeyNotFoundException($"Converter '{name}' was not found.");
    }

    public bool TryGetByName(string name, out IRangeConverter<TResult>? converter)
    {
        if (string.IsNullOrWhiteSpace(name)) { converter = null; return false; }
        return _converters.TryGetValue(name, out converter);
    }

    public bool IsValid(string? name) => !string.IsNullOrWhiteSpace(name) && _converters.ContainsKey(name);

    public IEnumerable<IRangeConverter<TResult>> All => _converters.Values;
    public IEnumerable<string> AllNames => _converters.Keys;
}