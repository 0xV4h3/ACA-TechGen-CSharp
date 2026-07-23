namespace Domain.Models.Converters;

public record Range<TResult>(int MinPercentage, int MaxPercentage, TResult Value);

public class RangeConverter<TResult> : IRangeConverter<TResult>
{
    private readonly List<Range<TResult>> _ranges;

    internal RangeConverter(IEnumerable<Range<TResult>> ranges)
    {
        _ranges = ranges?.ToList() ?? throw new ArgumentNullException(nameof(ranges));
        Validate();
    }

    public TResult Convert(int percentage)
    {
        var match = _ranges.FirstOrDefault(r => percentage >= r.MinPercentage && percentage <= r.MaxPercentage);
        return match is not null ? match.Value : _ranges.First().Value;
    }

    private void Validate()
    {
        if (!_ranges.Any())
            throw new InvalidOperationException("Ranges collection cannot be empty.");

        for (int i = 0; i < _ranges.Count; i++)
        {
            for (int j = i + 1; j < _ranges.Count; j++)
            {
                if (_ranges[i].MinPercentage <= _ranges[j].MaxPercentage && _ranges[j].MinPercentage <= _ranges[i].MaxPercentage)
                {
                    throw new InvalidOperationException(
                        $"Ranges overlap: [{_ranges[i].MinPercentage}-{_ranges[i].MaxPercentage}] and [{_ranges[j].MinPercentage}-{_ranges[j].MaxPercentage}].");
                }
            }
        }
    }
}