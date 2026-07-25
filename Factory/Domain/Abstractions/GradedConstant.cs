namespace Domain.Abstractions;

public sealed class GradedConstant<TConstant> where TConstant : Constant
{
    private readonly IRangeConverter<TConstant> _converter;
    public int Percentage { get; private set; }
    public TConstant Constant { get; private set; }

    public GradedConstant(IRangeConverter<TConstant> converter, int initialPercentage = 100)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        (Percentage, Constant) = ValidateAndConvert(initialPercentage);
    }

    public void Change(int newPercentage)
    {
        (Percentage, Constant) = ValidateAndConvert(newPercentage);
    }

    private (int percent, TConstant constant) ValidateAndConvert(int percent)
    {
        if (percent is < 0 or > 100) 
            throw new ArgumentOutOfRangeException(nameof(percent));
        
        return (percent, _converter.Convert(percent));
    }
}