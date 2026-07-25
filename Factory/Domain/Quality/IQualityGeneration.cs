namespace Domain.Quality;

public interface IQualityGenerator
{
    int GenerateQuality();
}

public class DefaultQualityGenerator : IQualityGenerator
{
    public int GenerateQuality() => 100; 
}

public class RandomQuality : IQualityGenerator
{
    private readonly Random _random = new();

    public int GenerateQuality() => _random.Next(0, 101);
}

public class NormalDistributionQuality(double mean = 70.0, double stdDev = 10.0) : IQualityGenerator
{
    private readonly Random _random = new();
    private readonly double _mean = mean;
    private readonly double _stdDev = stdDev;
    
    public int GenerateQuality()
    {
        double u1 = 1.0 - _random.NextDouble();
        double u2 = 1.0 - _random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        
        double randNormal = _mean + _stdDev * randStdNormal;
        return Math.Clamp((int)Math.Round(randNormal), 0, 100);
    }
}
