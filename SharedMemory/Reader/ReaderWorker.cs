using Domain.Config;
using System.Text;

namespace Reader;

public static class ReaderWorker
{
    public static void Start(string mode, string filePath)
    {
        Console.Clear();
        Console.WriteLine("=== READER APPLICATION ===");
        Console.WriteLine($"Mode: {mode}");
        Console.WriteLine($"File: {filePath}\n");

        while (!File.Exists(filePath))
        {
            Thread.Sleep(100);

            var metadata = SharedConfig.ReadMetadata();
            if (metadata.Mode == null) return;
        }

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            fs.Seek(0, SeekOrigin.End);

            using var sr = new StreamReader(fs, Encoding.UTF8);

            if (mode == "2")
                BufferedRead(sr, filePath);
            else
                InstantRead(sr, filePath);
        }

        TryDelete(filePath);
        TryDelete(SharedConfig.MetadataConfigPath);

        Console.WriteLine("\nApplication finished. Press any key to exit...");
    }

    private static void InstantRead(StreamReader sr, string filePath)
    {
        bool shutdown = false;

        while (!shutdown)
        {
            string? line = sr.ReadLine();

            if (line == null)
            {
                Thread.Sleep(100);

                if (!File.Exists(filePath))
                {
                    break;
                }
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

        Console.WriteLine("\nWriter disconnected.");
    }

    private static void BufferedRead(StreamReader sr, string filePath)
    {
        List<string> buffer = new();
        bool shutdown = false;

        while (!shutdown)
        {
            string? line = sr.ReadLine();

            if (line == null)
            {
                Thread.Sleep(100);

                if (!File.Exists(filePath))
                {
                    break;
                }
                continue;
            }

            if (line == "[SHUTDOWN]")
            {
                shutdown = true;
            }
            else if (line == "[FLUSH]")
            {
                foreach (string message in buffer)
                {
                    Console.WriteLine(message);
                }
                buffer.Clear();
            }
            else
            {
                buffer.Add(line);
            }
        }

        Console.WriteLine("\nWriter disconnected.");
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