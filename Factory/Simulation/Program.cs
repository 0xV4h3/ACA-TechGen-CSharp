using Domain.Configuration;
using Domain.Registries;
using FactoryCore;

namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        PrintWelcome();

        Configuration.Initialize();
        IRegistry registry = new Registry();

        var simulation = new FactoryPipelineSimulation(registry);
        simulation.Run();

        static void PrintWelcome()
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("          FACTORY PROCESSING & LOGISTICS SIMULATION           ");
            Console.WriteLine("==============================================================");
            Console.WriteLine("Flow: Machines -> Order Line -> Quality Checker -> Storage -> Transport -> Stock");
            Console.WriteLine();
        }
    }
}
