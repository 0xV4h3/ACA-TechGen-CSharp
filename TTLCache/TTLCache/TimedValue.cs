namespace TTLCache;

public class TimedValue<T>(T value, DateTime creationTime, DateTime expirationTime)
{
    public T Value { get; init; } = value;
    public DateTime CreationTime { get; init; } = creationTime;
    public DateTime ExpirationTime { get; init; } = expirationTime;

    public bool HasExpired() => DateTime.Now >= ExpirationTime;

    public override string ToString() => $"({Value} : {CreationTime} - {ExpirationTime})";
}