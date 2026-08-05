using Engine;

namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        TestEngine.RunTest(out var resultList);

        foreach (var result in resultList)
            Console.WriteLine(result);
    }
}