namespace Domain.Ticks;

public sealed class TickLog(int maxLines)
{
    private readonly string[] _lines = new string[maxLines];
    private int _count = 0;

    public void Reset() => _count = 0;

    public void Add(string message)
    {
        if (_count >= _lines.Length) return;
        _lines[_count] = message;
        _count++;
    }

    public void Print()
    {
        if (_count == 0)
        {
            Console.WriteLine("No activity this tick.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine($"{i + 1}. {_lines[i]}");
        }
    }
}
