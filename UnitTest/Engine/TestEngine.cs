using System.Reflection;
using System.Globalization;

namespace Engine;

public static class TestEngine
{
    public static Result<TestRunReport> RunTests(Assembly? assembly = null)
    {
        var logs = new List<string>();
        var cases = new List<TestCaseResult>();

        try
        {
            Assembly targetAssembly = assembly ?? Assembly.GetCallingAssembly();
            logs.Add($"Testing assembly: {targetAssembly.FullName}");

            var types = targetAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToArray();

            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    var testAttributes = method.GetCustomAttributes<TestAttribute>().ToArray();
                    if (testAttributes.Length == 0) continue;

                    foreach (var testAttribute in testAttributes)
                    {
                        var caseResult = ExecuteCase(type, method, testAttribute, logs);
                        cases.Add(caseResult);
                    }
                }
            }

            int total = cases.Count;
            int passed = cases.Count(x => x.Passed);
            int failed = total - passed;

            var report = new TestRunReport(
                targetAssembly.GetName().Name ?? "UnknownAssembly",
                total,
                passed,
                failed,
                cases
            );

            logs.Add($"Total: {total}, Passed: {passed}, Failed: {failed}");

            return Result<TestRunReport>.Success(report, logs);
        }
        catch (Exception ex)
        {
            logs.Add($"Unhandled exception: {ex.GetType().Name}: {ex.Message}");
            return Result<TestRunReport>.Failure($"Test execution failed: {ex.Message}", logs);
        }
    }

    private static TestCaseResult ExecuteCase(Type type, MethodInfo method, TestAttribute testAttribute, List<string> logs)
    {
        string typeName = type.FullName ?? type.Name;
        string methodName = method.Name;
        object?[] rawParams = testAttribute.Parameters;
        object? expected = testAttribute.ExpectedResult;

        try
        {
            var methodParams = method.GetParameters();

            if (methodParams.Length != rawParams.Length)
            {
                string message = $"Parameter count mismatch: expected {methodParams.Length}, got {rawParams.Length}";
                logs.Add($"{typeName}.{methodName}: {message}");
                return new TestCaseResult(typeName, methodName, rawParams, expected, null, false, message);
            }

            object?[] convertedParams = new object?[rawParams.Length];
            for (int i = 0; i < methodParams.Length; i++)
            {
                var convertResult = TryConvert(rawParams[i], methodParams[i].ParameterType);
                if (!convertResult.IsSuccess)
                {
                    string message = $"Parameter conversion failed at index {i}: {convertResult.Error}";
                    logs.Add($"{typeName}.{methodName}: {message}");
                    return new TestCaseResult(typeName, methodName, rawParams, expected, null, false, message);
                }

                convertedParams[i] = convertResult.Value;
            }

            object? instance = method.IsStatic ? null : Activator.CreateInstance(type);
            if (!method.IsStatic && instance is null)
            {
                string message = "Instance creation failed.";
                logs.Add($"{typeName}.{methodName}: {message}");
                return new TestCaseResult(typeName, methodName, rawParams, expected, null, false, message);
            }

            object? actual = method.Invoke(instance, convertedParams);

            var expectedConvert = TryConvert(expected, method.ReturnType);
            if (!expectedConvert.IsSuccess)
            {
                string message = $"Expected result conversion failed: {expectedConvert.Error}";
                logs.Add($"{typeName}.{methodName}: {message}");
                return new TestCaseResult(typeName, methodName, rawParams, expected, actual, false, message);
            }

            bool passed = AreEqual(actual, expectedConvert.Value);
            string status = passed ? "Passed" : "Failed";
            string details = $"{status}. Expected: {FormatValue(expectedConvert.Value)}, Actual: {FormatValue(actual)}";
            logs.Add($"{typeName}.{methodName}: {details}");

            return new TestCaseResult(typeName, methodName, rawParams, expectedConvert.Value, actual, passed, details);
        }
        catch (TargetInvocationException ex)
        {
            string message = $"Method threw exception: {ex.InnerException?.GetType().Name}: {ex.InnerException?.Message}";
            logs.Add($"{typeName}.{methodName}: {message}");
            return new TestCaseResult(typeName, methodName, rawParams, expected, null, false, message);
        }
        catch (Exception ex)
        {
            string message = $"Execution error: {ex.GetType().Name}: {ex.Message}";
            logs.Add($"{typeName}.{methodName}: {message}");
            return new TestCaseResult(typeName, methodName, rawParams, expected, null, false, message);
        }
    }

    private static Result<object?> TryConvert(object? value, Type targetType)
    {
        if (targetType == typeof(void))
            return Result<object?>.Failure("Void type is not supported for comparison.");

        if (value is null)
        {
            if (!targetType.IsValueType || Nullable.GetUnderlyingType(targetType) is not null)
                return Result<object?>.Success(null);

            return Result<object?>.Failure($"Null cannot be converted to non-nullable type {targetType.Name}.");
        }

        Type effectiveTarget = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (effectiveTarget.IsInstanceOfType(value))
            return Result<object?>.Success(value);

        try
        {
            if (effectiveTarget.IsEnum)
            {
                if (value is string s)
                    return Result<object?>.Success(Enum.Parse(effectiveTarget, s, true));

                object enumValue = Enum.ToObject(effectiveTarget, Convert.ChangeType(value, Enum.GetUnderlyingType(effectiveTarget), CultureInfo.InvariantCulture));
                return Result<object?>.Success(enumValue);
            }

            object converted = Convert.ChangeType(value, effectiveTarget, CultureInfo.InvariantCulture);
            return Result<object?>.Success(converted);
        }
        catch (Exception ex)
        {
            return Result<object?>.Failure($"Cannot convert '{value}' ({value.GetType().Name}) to {effectiveTarget.Name}: {ex.Message}");
        }
    }

    private static bool AreEqual(object? actual, object? expected)
    {
        if (actual is null && expected is null) return true;
        if (actual is null || expected is null) return false;
        return Equals(actual, expected);
    }

    private static string FormatValue(object? value) => value is null ? "null" : value.ToString() ?? "null";
}