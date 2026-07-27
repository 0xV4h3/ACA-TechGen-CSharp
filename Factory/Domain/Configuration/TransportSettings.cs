namespace Domain.Configuration;

public class TransportSettings
{
    public int ArrivalIntervalTicks { get; init; }
    public int CapacityPerArrival { get; init; }

    public void Validate()
    {
        if (ArrivalIntervalTicks <= 0) throw new ArgumentException("ArrivalIntervalTicks must be greater than 0.");
        if (CapacityPerArrival <= 0) throw new ArgumentException("CapacityPerArrival must be greater than 0.");
    }
}