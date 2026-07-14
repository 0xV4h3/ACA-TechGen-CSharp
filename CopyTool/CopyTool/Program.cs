namespace CopyTool;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: CopyTool.exe <source> <destination> [bufferSize]");
            return;
        }
        
        string sourcePath = args[0];
        string destinationPath = args[1];
        
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine("Error: Source file not found.");
            return;
        }

        int bufferSize = 65536;
        
        if (args.Length >= 3)
        {
            if (!int.TryParse(args[2], out bufferSize) || bufferSize <= 0)
            {
                bufferSize = 65536;
                Console.WriteLine($"Invalid buffer size. Using default: {bufferSize} bytes");
            }
        }
        
        destinationPath = GetUniqueDestinationPath(sourcePath, destinationPath);

        try
        {
            Console.WriteLine("Copying file...");
            
            FileInfo fileInfo = new FileInfo(sourcePath);
            ulong totalBytes = (ulong)fileInfo.Length;
            ulong updateInterval = (ulong)bufferSize * 50;
            
            using var progressBar = new ConsoleProgressBar(
                totalTicks: totalBytes,
                updateIntervalTicks: updateInterval, 
                isByteMode: true
            );
            
            CopyTool.Copy(sourcePath, destinationPath, bufferSize, progressBar);
            Console.WriteLine("Copying completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Copy error: {ex.Message}");
        }
    }

    private static string GetUniqueDestinationPath(string sourcePath, string destinationPath)
    {
        string sourceFileName = Path.GetFileNameWithoutExtension(sourcePath);
        string sourceExtension = Path.GetExtension(sourcePath);
        
        bool endsWithSlash =
            destinationPath.EndsWith(Path.DirectorySeparatorChar) ||
            destinationPath.EndsWith(Path.AltDirectorySeparatorChar);

        destinationPath = Path.GetFullPath(destinationPath);

        bool isDirectoryTarget = endsWithSlash || Directory.Exists(destinationPath);

        string targetDirectory;
        string baseName;
        string extension;

        if (isDirectoryTarget)
        {
            targetDirectory = destinationPath;
            baseName = $"{sourceFileName}_copy";
            extension = sourceExtension;
        }
        else
        {
            targetDirectory = Path.GetDirectoryName(destinationPath) ?? Directory.GetCurrentDirectory();
            baseName = Path.GetFileNameWithoutExtension(destinationPath);
            extension = Path.GetExtension(destinationPath);

            if (string.IsNullOrWhiteSpace(extension))
                extension = sourceExtension;
        }

        Directory.CreateDirectory(targetDirectory);

        string candidate = Path.Combine(targetDirectory, $"{baseName}{extension}");
        int counter = 1;

        while (File.Exists(candidate))
        {
            candidate = Path.Combine(targetDirectory, $"{baseName}({counter}){extension}");
            counter++;
        }

        return candidate;
    }
}