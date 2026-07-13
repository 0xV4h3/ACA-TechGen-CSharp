using System.Text;
using Domain.Config;

namespace Writer;

public static class WriterWorker
{
    public static void Start(string mode, string filePath)
    {
        try
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using var sw = new StreamWriter(fs, Encoding.UTF8);

            Console.Clear();
            Console.WriteLine("=== WRITER APPLICATION ===");
            Console.WriteLine($"Mode: {mode} | File: {filePath}\n");
            Console.WriteLine("Type messages below. Type 'exit' to quit.\n");

            if (mode == "2")
            {
                Console.WriteLine("Type 'flush' or '--flush' to deliver buffered messages.\n");
                BufferedDisplay(sw);
            }
            else
                InstantDisplay(sw);
        }
        finally
        {
            TryDelete(filePath);
            TryDelete(SharedConfig.MetadataConfigPath);
        }
    }

    private static void InstantDisplay(StreamWriter sw)
    {
        bool isExit = false;

        while (!isExit)
        {
            Console.Write("Write: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;

            if (Exit(input))
            {
                sw.WriteLine("[SHUTDOWN]");
                sw.Flush();
                isExit = true;
            }
            else
            {
                sw.WriteLine(input);
                sw.Flush();
            }
        }
    }
    private static void BufferedDisplay(StreamWriter sw)
    {
        bool isExit = false;

        while (!isExit)
        {
            Console.Write("Write: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;

            if (Exit(input))
            {
                sw.WriteLine("[SHUTDOWN]");
                sw.Flush();
                isExit = true;
            }
            else if (Flush(input))
            {
                sw.WriteLine("[FLUSH]");
                sw.Flush();
            }
            else
            {
                sw.WriteLine(input);
                sw.Flush();
            }
        }
    }

    private static bool Exit(string input) => input.Equals("exit", StringComparison.OrdinalIgnoreCase);

    private static bool Flush(string input) => input.Equals("flush", StringComparison.OrdinalIgnoreCase) ||
                                               input.Equals("-f", StringComparison.OrdinalIgnoreCase);
    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }
}