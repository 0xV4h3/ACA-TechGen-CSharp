namespace RetryExecutor;

public sealed class Result<T>
{
    public bool Success { get; init; }
    public T? Value { get; init; }
    public Exception? Error { get; init; }
    public int Attempts { get; init; }
}