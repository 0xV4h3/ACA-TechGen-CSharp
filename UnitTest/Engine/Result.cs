namespace Engine;

public sealed class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string Error { get; init; } = string.Empty;
    public List<string> Logs { get; init; } = [];

    public static Result<T> Success(T value, List<string>? logs = null) =>
        new() { IsSuccess = true, Value = value, Logs = logs ?? [] };

    public static Result<T> Failure(string error, List<string>? logs = null, T? value = default) =>
        new() { IsSuccess = false, Error = error, Value = value, Logs = logs ?? [] };
}

public sealed record TestCaseResult(
    string TypeName,
    string MethodName,
    IReadOnlyList<object?> Parameters,
    object? ExpectedResult,
    object? ActualResult,
    bool Passed,
    string Message
);

public sealed record TestRunReport(
    string AssemblyName,
    int Total,
    int Passed,
    int Failed,
    IReadOnlyList<TestCaseResult> Cases
);