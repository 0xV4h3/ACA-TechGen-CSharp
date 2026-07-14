using Domain.Config;

namespace Reader;

class Program
{
    static void Main(string[] args)
    {
        string? mode = null;
        string? filePath = null;

        Console.WriteLine("Waiting for Writer...");

        while (string.IsNullOrEmpty(mode) || string.IsNullOrEmpty(filePath))
        {
            (mode, filePath) = SharedConfig.ReadMetadata();

            if (string.IsNullOrEmpty(mode))
            {
                Thread.Sleep(200);
            }
        }

        ReaderWorker.Start(mode, filePath);
    }
}