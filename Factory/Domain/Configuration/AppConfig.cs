namespace Domain.Configuration;

public class AppConfig
{
    public SimulationSettings Simulation { get; set; } = new();
    public StorageSettings Storage { get; set; } = new();
    public QualityCheckSettings QualityCheck { get; set; } = new();
    public TransportSettings Transport { get; set; } = new();
    public ProductionSettings Production { get; set; } = new();
}