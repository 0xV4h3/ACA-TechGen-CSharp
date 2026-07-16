namespace InboxScanner;

class Program
{
    static void Main(string[] args)
    {
        string dir = Path.Combine(Environment.CurrentDirectory, "inbox");
        Directory.CreateDirectory(dir);

        int count = 0;

        foreach (string path in Directory.EnumerateFiles(dir))
        {
            string fileName = Path.GetFileName(path);
            long size = new FileInfo(path).Length;
            Console.WriteLine($"{fileName} | {size} bytes");
            count++;
        }

        Console.WriteLine($"Total files: {count}");
    }
}