namespace Domain.Config;

public static class SharedConfig
{
    public static readonly string DefaultExchangePath = "shared_memory.txt";
    public static readonly string MetadataConfigPath = "app_config.txt";

    public static (string? Mode, string FilePath) ParseArguments(string[] args)
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

        return (mode, filePath);
    }

    public static void SaveMetadata(string mode, string filePath)
    {
        using var sw = new StreamWriter(MetadataConfigPath, false);
        sw.WriteLine($"MODE={mode}");
        sw.WriteLine($"PATH={filePath}");
    }

    public static (string? Mode, string? FilePath) ReadMetadata()
    {
        if (!File.Exists(MetadataConfigPath))
            return (null, null);

        string mode = "1";
        string filePath = DefaultExchangePath;

        try
        {
            string[] lines = File.ReadAllLines(MetadataConfigPath);
            foreach (var line in lines)
            {
                if (line.StartsWith("MODE=")) mode = line.Substring(5);
                if (line.StartsWith("PATH=")) filePath = line.Substring(5);
            }
        }
        catch
        {
            return (null, null);
        }

        return (mode, filePath);
    }
}