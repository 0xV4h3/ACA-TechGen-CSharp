namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        ConcurrentQueueSimulation.Run();
        BlockingCollectionSimulation.Run();
    }
}