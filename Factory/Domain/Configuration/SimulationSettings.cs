namespace Domain.Configuration;

public class SimulationSettings
{
    private int _totalSimulationTicks;
    private int _startItemId;
    private int _orderLineCapacity;

    public int TotalSimulationTicks
    {
        get => _totalSimulationTicks;
        set
        {
            if (value < -1 || value == 0)
                throw new ArgumentException("TotalSimulationTicks must be -1 or greater than 0.");
            _totalSimulationTicks = value;
        }
    }

    public int StartItemId
    {
        get => _startItemId;
        set
        {
            if (value < 0)
                throw new ArgumentException("StartItemId cannot be negative.");
            _startItemId = value;
        }
    }

    public int OrderLineCapacity
    {
        get => _orderLineCapacity;
        set
        {
            if (value <= 0)
                throw new ArgumentException("OrderLineCapacity must be greater than 0.");
            _orderLineCapacity = value;
        }
    }

    public int RandomSeed { get; set; }
}