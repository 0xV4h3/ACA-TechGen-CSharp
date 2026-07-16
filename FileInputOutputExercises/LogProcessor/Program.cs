using System.Text;

namespace LogProcessor;

class Program
{
    static void Main(string[] args)
    {
        string dir = Path.Combine(Environment.CurrentDirectory, "service.log");

        string[] lines =
        [
            "2026-07-10 10:00:00 INFO Logging service started",
            "2026-07-10 10:01:10 ERROR Unable to write log in line:4",
            "2026-07-10 10:02:20 INFO Пользователь Vahe вошел в систему",
            "2026-07-10 10:03:30 ERROR Failed to send запрос",
            "2026-07-10 10:04:40 INFO Cleanup completed"
        ];

        using (var writer = new StreamWriter(dir, false, Encoding.UTF8))
        {
            foreach (var line in lines)
                writer.WriteLine(line);
        }

        int errorCount = 0;

        using (var reader = new StreamReader(dir, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("ERROR"))
                    errorCount++;
            }
        }

        Console.WriteLine($"ERROR lines: {errorCount}");
    }
}