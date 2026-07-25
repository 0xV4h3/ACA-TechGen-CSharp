using Microsoft.Extensions.Configuration;

namespace Domain.Configuration;

public sealed class AppConfig
{
    private static AppConfig? _settings;
    public static AppConfig Settings => _settings ?? throw new InvalidOperationException("Configuration has not been initialized. Call Initialize() first.");

    private AppConfig() { }

    public static void Initialize(string[]? args = null)
    {
        if (_settings != null) return;

        try
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine("Configuration", "config.json");
            string fullPath = Path.Combine(basePath, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Configuration file not found at: {fullPath}");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(relativePath, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (args != null && args.Length > 0)
            {
                builder.AddCommandLine(args);
            }

            IConfiguration rootConfiguration = builder.Build();
            var configInstance = new AppConfig();
            
            rootConfiguration.Bind(configInstance);
            
            configInstance.Simulation?.Validate();
            configInstance.Storage?.Validate();
            configInstance.QualityChecker?.Validate();
            configInstance.Transport?.Validate();
            configInstance.Production?.Validate();

            _settings = configInstance;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[CONFIGURATION ERROR]: {ex.InnerException?.Message ?? ex.Message}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
    
    public SimulationSettings Simulation { get; init; } = new();
    public StorageSettings Storage { get; init; } = new();
    public QualityCheckerSettings QualityChecker { get; init; } = new();
    public TransportSettings Transport { get; init; } = new();
    public ProductionSettings Production { get; init; } = new();
}