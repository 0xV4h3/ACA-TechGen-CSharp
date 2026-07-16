namespace DailyReportArchiver;

class Program
{
    static void Main(string[] args)
    {
        string dir = Path.Combine(Environment.CurrentDirectory, "reports");
        Directory.CreateDirectory(dir);

        string filePath = Path.Combine(dir, "daily-report.txt");

        string report =
            "Daily report\n" +
            "Completed: Daily Report Archiver\n" +
            "In Process: other 3 exercises\n" +
            "Next step: Inbox Scanner";

        File.WriteAllText(filePath, report);

        string readBack = File.ReadAllText(filePath);

        if (readBack == report)
            Console.WriteLine("Saved OK");
        else
            Console.WriteLine("Saved ERROR");

        Console.WriteLine($"File: {filePath}");
    }
}