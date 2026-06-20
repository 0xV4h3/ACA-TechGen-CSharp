namespace Domain.Configuration;

public class ProductionSettings
{
    public MachineSettings MachineA { get; set; } = new();
    public MachineSettings MachineB { get; set; } = new();
    public MachineSettings MachineC { get; set; } = new();
}