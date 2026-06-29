namespace Pair;

public class Pair<TFirst, TSecond>(TFirst first, TSecond second)
{
    public TFirst First { get; } = first;
    public TSecond Second { get; } = second;

    public Pair<TSecond, TFirst> SwapSides() => new Pair<TSecond, TFirst>(Second, First);

    public override string ToString() => $"({First}, {Second})";
}