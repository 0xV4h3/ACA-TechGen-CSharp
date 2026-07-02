using Domain.Constants;
using Domain.Models.Quality;

namespace Domain.Registries;

public abstract class QualityConverterRegistry<TQuality> where TQuality : QualityConstant
{
    private readonly Dictionary<string, IQualityConverter<TQuality>> _converters = new(StringComparer.OrdinalIgnoreCase);
    
    public IQualityConverter<TQuality> Create(string name, IEnumerable<QualityRange<TQuality>> ranges)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Converter name cannot be empty.", nameof(name));
        
        var converter = new RangeQualityConverter<TQuality>(ranges);

        if (!_converters.TryAdd(name, converter))
            throw new InvalidOperationException($"Converter with name '{name}' is already registered in this context.");

        return converter;
    }
    
    public IQualityConverter<TQuality> GetByName(string name)
    {
        if (_converters.TryGetValue(name, out var converter))
            return converter;
        throw new KeyNotFoundException($"Quality converter '{name}' was not found in this context.");
    }
    
    public bool TryGetByName(string name, out IQualityConverter<TQuality>? converter)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            converter = null;
            return false;
        }

        return _converters.TryGetValue(name, out converter);
    }
    
    public bool IsValid(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return _converters.ContainsKey(name);
    }
    
    public IEnumerable<IQualityConverter<TQuality>> All => _converters.Values;
    public IEnumerable<string> AllNames => _converters.Keys;
}
