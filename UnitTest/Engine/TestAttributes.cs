namespace Engine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class TestAttribute(object? expectedResult, params object?[] parameters) : Attribute
{
    public object? ExpectedResult { get; } = expectedResult;
    public object?[] Parameters { get; } = parameters;
}