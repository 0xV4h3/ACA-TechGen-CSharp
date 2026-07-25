namespace Domain.Abstractions;

public sealed class GradedConstant<TConstant> where TConstant : Constant
{
    private readonly IRangeConverter<TConstant> _converter;
    public int Percentage { get; private set; }
    public TConstant Constant { get; private set; }

    public GradedConstant(IRangeConverter<TConstant> converter, int initialPercentage = 100)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        Change(initialPercentage);
    }

    public void Change(int newPercentage)
    {
        if (newPercentage is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(newPercentage), "Must be between 0 and 100%.");
        Percentage = newPercentage;
        Constant = _converter.Convert(newPercentage);
    }
}