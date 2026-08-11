using Domain.Config;

namespace Writer;

class Program
{
    static void Main(string[] args)
    {
        var (mode, filePath) = SharedConfig.ParseArguments(args);
        
        if (string.IsNullOrEmpty(mode))
            mode = ModeSelection();
        
        SharedConfig.SaveMetadata(mode, filePath);
        
        WriterWorker.Start(mode, filePath);
    }

    private static string ModeSelection()
    {
        Console.WriteLine("Select Working Mode (1 or 2):");
        Console.WriteLine("1 - Instant Display");
        Console.WriteLine("2 - Buffered Display");
        Console.Write("Choice: ");

        string? choice = Console.ReadLine();
        return choice == "2" ? "2" : "1";
    }
}