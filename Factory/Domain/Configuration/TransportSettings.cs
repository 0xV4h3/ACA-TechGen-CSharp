namespace Domain.Configuration;

public class TransportSettings
{
    private int _arrivalIntervalTicks;
    private int _capacityPerArrival;

    public int ArrivalIntervalTicks
    {
        get => _arrivalIntervalTicks;
        set
        {
            if (value <= 0)
                throw new ArgumentException("ArrivalIntervalTicks must be greater than 0.");
            _arrivalIntervalTicks = value;
        }
    }

    public int CapacityPerArrival
    {
        get => _capacityPerArrival;
        set
        {
            if (value <= 0)
                throw new ArgumentException("CapacityPerArrival must be greater than 0.");
            _capacityPerArrival = value;
        }
    }
}