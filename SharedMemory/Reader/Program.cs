using Domain.Config;

namespace Reader;

class Program
{
    static void Main(string[] args)
    {
        string? filePath = null;

        Console.WriteLine("Waiting for Writer...");

        while (string.IsNullOrEmpty(filePath))
        {
            (_, filePath) = SharedConfig.ReadMetadata();

            if (string.IsNullOrEmpty(filePath))
                Thread.Sleep(200);
        }

        ReaderWorker.Start(filePath);
    }
}