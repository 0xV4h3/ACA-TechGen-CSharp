namespace Domain.Converters;

public interface IRangeConverter<out TResult>
{
    TResult Convert(int percentage);
}