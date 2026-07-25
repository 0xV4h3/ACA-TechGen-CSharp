namespace Domain.Configuration;

public class ProductionSettings
{
    public MachineSettings MachineA { get; init; } = new();
    public MachineSettings MachineB { get; init; } = new();
    public MachineSettings MachineC { get; init; } = new();

    public void Validate()
    {
        MachineA?.Validate();
        MachineB?.Validate();
        MachineC?.Validate();
    }
}