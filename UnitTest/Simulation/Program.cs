using Engine;
using System.Reflection;

namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        var result = TestEngine.RunTests(Assembly.GetExecutingAssembly());

        foreach (var log in result.Logs)
            Console.WriteLine(log);

        if (!result.IsSuccess || result.Value is null)
        {
            Console.WriteLine($"Run failed: {result.Error}");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Assembly: {result.Value.AssemblyName}");
        Console.WriteLine($"Total: {result.Value.Total}");
        Console.WriteLine($"Passed: {result.Value.Passed}");
        Console.WriteLine($"Failed: {result.Value.Failed}");

        Console.WriteLine();
        foreach (var testCase in result.Value.Cases)
        {
            Console.WriteLine($"{testCase.TypeName}.{testCase.MethodName} -> {(testCase.Passed ? "PASS" : "FAIL")} | {testCase.Message}");
        }
    }
}