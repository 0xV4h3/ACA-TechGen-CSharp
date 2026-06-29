namespace Pair;

class Program
{
    static void Main(string[] args)
    {
        var p = new Pair<int, string>(7, "seven");
        var swapped = p.SwapSides();

        Console.WriteLine(p);
        Console.WriteLine(swapped);
    }
}