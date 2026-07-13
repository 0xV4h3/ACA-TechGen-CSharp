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
            Thread.Sleep(100);

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        using var sr = new StreamReader(fs, Encoding.UTF8);

        if (mode == "2") BufferedRead(sr);
        else InstantRead(sr);
    }

    private static void InstantRead(StreamReader sr)
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

        Console.WriteLine("\nWriter disconnected.");
    }

    private static void BufferedRead(StreamReader sr)
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
}