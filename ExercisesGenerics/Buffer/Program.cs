namespace Buffer;

class Program
{
    static void Main(string[] args)
    {
        var comparer = new IntDescendingComparer();
        var buffer = new TopNBuffer<int>(3, comparer);
        
        var values = new[] { 5, 1, 9, 3, 7, 2 };

        foreach (var v in values)
            buffer.Add(v);
        
        Console.WriteLine(string.Join(", ", buffer.Snapshot())); 
    }
}