namespace CollectionOps;

class Program
{
    static void Main(string[] args)
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };

        var evens = CollectionOps.Filter(numbers, n => n % 2 == 0);
        var labels = CollectionOps.Project(evens, n => $"N{n}");

        Console.WriteLine(string.Join(", ", labels));
    }
}