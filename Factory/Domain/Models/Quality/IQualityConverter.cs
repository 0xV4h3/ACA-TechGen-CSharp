using Domain.Constants;

namespace Domain.Models.Quality;

public interface IQualityConverter<out TQuality> where TQuality : QualityConstant
{
    TQuality Convert(int percentage);
}

public record QualityRange<TQuality>(int Min, int Max, TQuality Quality) 
    where TQuality : QualityConstant;

public class RangeQualityConverter<TQuality> : IQualityConverter<TQuality> 
    where TQuality : QualityConstant
{
    private readonly List<QualityRange<TQuality>> _ranges;
    
    internal RangeQualityConverter(IEnumerable<QualityRange<TQuality>> ranges)
    {
        _ranges = ranges?.ToList() ?? throw new ArgumentNullException(nameof(ranges));
        ValidateRanges();
    }

    public TQuality Convert(int percentage)
    {
        var match = _ranges.FirstOrDefault(r => percentage >= r.Min && percentage <= r.Max);
        return match?.Quality ?? _ranges.First().Quality;
    }

    private void ValidateRanges()
    {
        if (!_ranges.Any())
            throw new InvalidOperationException("Ranges collection cannot be empty.");

        for (int i = 0; i < _ranges.Count; i++)
        {
            for (int j = i + 1; j < _ranges.Count; j++)
            {
                if (_ranges[i].Min <= _ranges[j].Max && _ranges[j].Min <= _ranges[i].Max)
                {
                    throw new InvalidOperationException(
                        $"Quality ranges overlap: [{_ranges[i].Min}-{_ranges[i].Max}] and [{_ranges[j].Min}-{_ranges[j].Max}].");
                }
            }
        }
    }
}

