using System.Text.Json;

namespace Domain.Configuration;

public static class Configuration
{
    private static AppConfig? _settings;
    
    public static AppConfig Settings => _settings ?? throw new InvalidOperationException("Configuration has not been initialized. Call Initialize() first.");

    public static void Initialize()
    {
        if (_settings != null) return; 

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
            
            _settings = JsonSerializer.Deserialize<AppConfig>(jsonText, options) 
                        ?? throw new InvalidOperationException("Failed to deserialize config.json file.");
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