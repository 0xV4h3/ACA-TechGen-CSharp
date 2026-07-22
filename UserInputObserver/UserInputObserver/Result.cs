namespace UserInputObserver;

public class Result<T>
{
    public bool Success { get; init; }
    public T? Value { get; init; }
    public string? ErrorMessage { get; init; }

    private Result() { }

    public static Result<T> Ok(T value) => new() 
    { 
        Success = true, 
        Value = value 
    };

    public static Result<T> Fail(string errorMessage) => new() 
    { 
        Success = false, 
        ErrorMessage = errorMessage 
    };
}
