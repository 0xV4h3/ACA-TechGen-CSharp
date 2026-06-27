namespace Domain.Configuration;

public class QualityCheckerSettings
{
    private int _minTicksPerItem;
    private int _maxTicksPerItem;
    private int _passPercentage;

    public int MinTicksPerItem
    {
        get => _minTicksPerItem;
        set
        {
            if (value <= 0)
                throw new ArgumentException("MinTicksPerItem must be greater than 0.");
            _minTicksPerItem = value;
        }
    }

    public int MaxTicksPerItem
    {
        get => _maxTicksPerItem;
        set
        {
            if (value < MinTicksPerItem)
                throw new ArgumentException("MaxTicksPerItem cannot be less than MinTicksPerItem.");
            _maxTicksPerItem = value;
        }
    }

    public int PassPercentage
    {
        get => _passPercentage;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("PassPercentage must be between 0 and 100.");
            _passPercentage = value;
        }
    }
}