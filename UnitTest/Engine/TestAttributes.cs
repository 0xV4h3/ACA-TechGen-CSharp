namespace Engine;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestAttribute(int parameters, int expectedResult) : Attribute
{
    public int Parameters { get; } = parameters;
    public int ExpectedResult { get; } = expectedResult;
}