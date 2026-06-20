namespace Domain.Configuration;

public class MachineSettings
{
    private int _intervalTicks;
    private int _totalItemsToProduce;

    public int IntervalTicks
    {
        get => _intervalTicks;
        set
        {
            if (value <= 0)
                throw new ArgumentException("IntervalTicks must be greater than 0.");
            _intervalTicks = value;
        }
    }

    public int TotalItemsToProduce
    {
        get => _totalItemsToProduce;
        set
        {
            if (value < 0)
                throw new ArgumentException("TotalItemsToProduce cannot be negative.");
            _totalItemsToProduce = value;
        }
    }
}