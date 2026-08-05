using System.Reflection;
namespace Engine;

public static class TestEngine
{
    public static void RunTest(out ICollection<string> resultList)
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        Console.WriteLine($"Testing calling assembly: {assembly.FullName}");
        
        resultList = [];
        var types = assembly.GetTypes().Where(t => t.IsClass);
        foreach (var type in types)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<TestAttribute>() is null)
                    continue;
                var param = method.GetCustomAttribute<TestAttribute>()?.Parameters;
                var expectedResult = method.GetCustomAttribute<TestAttribute>()?.ExpectedResult;
                if (param is null || expectedResult is null)
                    continue;

                object instance = Activator.CreateInstance(type)!;
                var resultObj = method.Invoke(instance, new object[] { param});
                
                if(resultObj is int result)
                    if (result == expectedResult)
                        resultList.Add($"Test of {type.FullName} method {method.Name}: Parameters {param} - Result {result} - Expected Result {expectedResult} : Succesfull");
                    else
                        resultList.Add($"Test of {type.FullName} method {method.Name}: Parameters {param} - Result {result} - Expected Result {expectedResult} : Failed");
            }
        }
    }
}