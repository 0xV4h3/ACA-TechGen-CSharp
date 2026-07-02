namespace Domain.Models.Abstractions;

public interface IQualitative
{
    int QualityPercentage { get; }

    void ChangeQuality(int newPercentage)
    {
        if (newPercentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(newPercentage), "Quality must be between 0 and 100%.");
        }
        
        UpdatePercentageInternal(newPercentage);
    }
    
    protected void UpdatePercentageInternal(int percentage);
}
