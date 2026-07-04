namespace TTLCache;

public class Cache<T>
{
    private readonly Dictionary<string, TimedValue<T>> _cache = new(StringComparer.OrdinalIgnoreCase);

    public void Add(string key, T value, int seconds)
    {
        _cache[key] = new TimedValue<T>(value, DateTime.Now, DateTime.Now + TimeSpan.FromSeconds(seconds));
    }
    
    public bool TryAdd(string key, T value, int seconds)
    {
        var timedValue = new TimedValue<T>(value, DateTime.Now, DateTime.Now + TimeSpan.FromSeconds(seconds));
        return _cache.TryAdd(key, timedValue);
    }
    
    public bool TryGet(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var timedValue))
        {
            if (!timedValue.HasExpired())
            {
                value = timedValue.Value;
                return true;
            }
        }
        value = default;
        return false;
    }

    public void Get(string key, out T? value)
    {
        var timedValue = _cache[key];
        if (timedValue.HasExpired())
            throw new Exception();
        
        value = timedValue.Value;
    }
}