namespace Book;

class Program
{
    static void Main(string[] args)
    {
        var csharpTips = new List<string>
        {
            "Tip 1: Use 'string.Equals' with 'OrdinalIgnoreCase' for fast case-insensitive comparisons.",
            "Tip 2: Prefer 'StringBuilder' over standard string concatenation inside loops.",
            "Tip 3: Always use 'using' statements or declarations for proper resource cleanup.",
            "Tip 4: Use 'ValueTask' instead of 'Task' for async methods that complete synchronously.",
            "Tip 5: Leverage pattern matching expressions to write cleaner conditional code.",
            "Tip 6: Use 'Array.Empty<T>()' instead of creating new empty arrays to save memory.",
            "Tip 7: Apply the 'readonly' modifier to struct fields to prevent defensive copying."
        };

        Book book = new Book(csharpTips);

        book.Step = 2;
        
        foreach (var page in book)
            Console.WriteLine($"[Page {book.CurrentPageNumber}]: {page}");

        Console.WriteLine("\n--------------------------------------------------\n");

        book.Step = -3;
        
        foreach (var page in book)
            Console.WriteLine($"[Page {book.CurrentPageNumber}]: {page}");
    }
}