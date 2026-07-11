namespace Domain.Config;

public static class SharedConfig
{
    public static readonly string DefaultExchangePath = Path.Combine(Path.GetTempPath(), "shared_memory.txt");
    public static readonly string MetadataConfigPath = Path.Combine(Path.GetTempPath(), "app_config.txt");

    public static (string? Mode, string? FilePath) ParseArguments(string[] args)
    {
        string? mode = null;
        string filePath = DefaultExchangePath;

        for (int i = 0; i < args.Length; i++)
        {
            if ((args[i] == "--mode" || args[i] == "-m") && i + 1 < args.Length)
                mode = args[++i];
            else if ((args[i] == "--path" || args[i] == "-p") && i + 1 < args.Length)
                filePath = args[++i];
        }

        if (!EnsureDirectoryExists(filePath))
        {
            Console.WriteLine($"[Warning] Unable to use path '{filePath}'. Falling back to default Temp path.");
            filePath = DefaultExchangePath;
            EnsureDirectoryExists(filePath);
        }

        return (mode, filePath);
    }

    public static void SaveMetadata(string mode, string filePath)
    {
        string targetConfigPath = MetadataConfigPath;
        if (!EnsureDirectoryExists(targetConfigPath))
        {
            Console.WriteLine("[Warning] Target configuration directory is inaccessible. Saving config to application folder.");
            targetConfigPath = "app_config.txt";
        }
        
        try
        {
            using (var sw = new StreamWriter(targetConfigPath, false))
            {
                sw.WriteLine($"MODE={mode}");
                sw.WriteLine($"PATH={filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Failed to save metadata to '{targetConfigPath}'. Details: {ex.Message}");
        }
    }

    public static (string? Mode, string? FilePath) ReadMetadata()
    {
        string targetConfigPath = MetadataConfigPath;
        if (!File.Exists(targetConfigPath))
            targetConfigPath = "app_config.txt";

        if (!File.Exists(targetConfigPath))
            return (null, null);

        string mode = "1";
        string filePath = DefaultExchangePath;

        try
        {
            string[] lines = File.ReadAllLines(targetConfigPath);
            foreach (var line in lines)
            {
                if (line.StartsWith("MODE=")) mode = line.Substring(5);
                if (line.StartsWith("PATH=")) filePath = line.Substring(5);
            }
        }
        catch (IOException)
        {
            return (null, null);
        }

        return (mode, filePath);
    }

    private static bool EnsureDirectoryExists(string filePath)
    {
        try
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return true;
        }
        catch (Exception)
        {
            return false; 
        }
    }
}