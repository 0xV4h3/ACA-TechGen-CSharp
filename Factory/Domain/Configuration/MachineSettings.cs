namespace Domain.Configuration;

public class MachineSettings
{
    public int IntervalTicks { get; init; }
    public int TotalItemsToProduce { get; init; }

    public void Validate()
    {
        if (IntervalTicks <= 0) throw new ArgumentException("IntervalTicks must be greater than 0.");
        if (TotalItemsToProduce < 0) throw new ArgumentException("TotalItemsToProduce cannot be negative.");
    }
}