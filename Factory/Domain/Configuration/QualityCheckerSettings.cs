namespace Domain.Configuration;

public class QualityCheckerSettings
{
    public int MinTicksPerItem { get; init; }
    public int MaxTicksPerItem { get; init; }
    public int PassPercentage { get; init; }

    public void Validate()
    {
        if (MinTicksPerItem <= 0) throw new ArgumentException("MinTicksPerItem must be greater than 0.");
        if (MaxTicksPerItem < MinTicksPerItem) throw new ArgumentException("MaxTicksPerItem cannot be less than MinTicksPerItem.");
        if (PassPercentage < 0 || PassPercentage > 100) throw new ArgumentException("PassPercentage must be between 0 and 100.");
    }
}