namespace Domain.Converters;

public record RangeStep<TResult>(int MinPercentage, TResult Value);

public class SteppedRangeConverter<TResult> : IRangeConverter<TResult>
{
    private readonly List<RangeStep<TResult>> _steps;

    public SteppedRangeConverter(IEnumerable<RangeStep<TResult>> steps)
    {
        var list = steps?.ToList() ?? throw new ArgumentNullException(nameof(steps));
        Validate(list);
        _steps = list.OrderByDescending(s => s.MinPercentage).ToList();
    }

    public TResult Convert(int percentage)
    {
        return _steps.First(s => percentage >= s.MinPercentage).Value;
    }

    private static void Validate(List<RangeStep<TResult>> steps)
    {
        if (steps.Count == 0)
            throw new InvalidOperationException("Steps collection cannot be empty.");

        var mins = steps.Select(s => s.MinPercentage).ToList();

        if (mins.Any(m => m is < 0 or > 100))
            throw new InvalidOperationException("MinPercentage must be between 0 and 100.");

        if (mins.Distinct().Count() != mins.Count)
            throw new InvalidOperationException("Duplicate MinPercentage thresholds are not allowed.");

        if (mins.Min() != 0)
            throw new InvalidOperationException(
                "A step with MinPercentage = 0 is required to cover the full 0-100 range without gaps.");
    }
}