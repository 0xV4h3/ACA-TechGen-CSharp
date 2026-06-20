using System.Text.Json;

namespace Domain.Configuration;

public static class Configuration
{
    public static AppConfig Settings { get; private set; } = new();

    public static void Initialize()
    {
        try
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "config.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file not found at: {filePath}");
            }
            
            string jsonText = File.ReadAllText(filePath);
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var appConfig = JsonSerializer.Deserialize<AppConfig>(jsonText, options);

            if (appConfig == null)
            {
                throw new InvalidOperationException("Failed to deserialize config.json file.");
            }

            Settings = appConfig;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[CONFIGURATION ERROR]: {ex.InnerException?.Message ?? ex.Message}");
            Console.ResetColor();
            
            Environment.Exit(1); 
        }
    }
}