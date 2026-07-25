namespace Domain.Configuration;

public class SimulationSettings
{
    public int TotalSimulationTicks { get; init; }
    public int StartItemId { get; init; }
    public int OrderLineCapacity { get; init; }
    public int RandomSeed { get; init; }

    public void Validate()
    {
        if (TotalSimulationTicks < -1 || TotalSimulationTicks == 0) 
            throw new ArgumentException("TotalSimulationTicks must be -1 or greater than 0.");
        if (StartItemId < 0) 
            throw new ArgumentException("StartItemId cannot be negative.");
        if (OrderLineCapacity <= 0) 
            throw new ArgumentException("OrderLineCapacity must be greater than 0.");
    }
}