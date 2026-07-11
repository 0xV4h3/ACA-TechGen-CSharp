using System.Text;
using Domain.Config;

namespace Writer;

public static class WriterWorker
{
    public static void Start(string mode, string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);

        try
        {
            using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var sw = new StreamWriter(fs, Encoding.UTF8);
            
            Console.Clear();
            Console.WriteLine("=== WRITER APPLICATION ===");
            Console.WriteLine($"Mode: {mode} | File: {filePath}\n");
            Console.WriteLine("Type messages below. Type 'exit' to quit.\n");
            
            
        }
        finally
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            if (File.Exists(SharedConfig.MetadataConfigPath)) File.Delete(SharedConfig.MetadataConfigPath);
        }
    }

    private static void InstantDisplay(FileStream fs, StreamWriter sw)
    {
        
    }
    private static void BufferedDisplay(FileStream fs, StreamWriter sw)
    {
        
    }
    
    private static bool Exit(string input) => input.Equals("exit", StringComparison.OrdinalIgnoreCase);

    private static bool Flush(string input) => input.Equals("flush", StringComparison.OrdinalIgnoreCase) || 
                                               input.Equals("--flush", StringComparison.OrdinalIgnoreCase);
}