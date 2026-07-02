namespace Domain.Models.Quality;

public enum QualityRoute
{
    Passed,
    Repair,
    Scrap
}

public record QualityThreshold(int MinPercentage, QualityRoute Route);
