using System.Text;
using Domain.Config;

namespace Reader;

public static class ReaderWorker
{
    public static void Start(string filePath)
    {
        Console.Clear();
        Console.WriteLine("=== READER APPLICATION ===");
        Console.WriteLine($"File: {filePath}\n");

        while (!File.Exists(filePath))
            Thread.Sleep(100);

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var sr = new StreamReader(fs, Encoding.UTF8))
        {
            fs.Position = fs.Length;
            ReadLoop(sr);
        }

        Console.WriteLine("\nWriter disconnected.");

        TryDelete(filePath);
        TryDelete(SharedConfig.MetadataConfigPath);
    }

    private static void ReadLoop(StreamReader sr)
    {
        bool shutdown = false;

        while (!shutdown)
        {
            string? line = sr.ReadLine();

            if (line == null)
            {
                Thread.Sleep(100);
                continue;
            }

            if (line == "[SHUTDOWN]")
            {
                shutdown = true;
            }
            else
            {
                Console.WriteLine(line);
            }
        }
    }

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