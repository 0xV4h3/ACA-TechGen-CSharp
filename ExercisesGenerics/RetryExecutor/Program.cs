namespace RetryExecutor;

class Program
{
    static void Main(string[] args)
    {
        int callCount = 0;

        var result = RetryExecutor.Execute(
            operation: () =>
            {
                callCount++;
                if (callCount <= 2) throw new InvalidOperationException("Temporary failure");
                return 2004;
            },
            maxAttempts: 5,
            shouldRetry: ex => ex is InvalidOperationException
        );

        Console.WriteLine($"Success: {result.Success}");
        Console.WriteLine($"Value: {result.Value}");
        Console.WriteLine($"Attempts: {result.Attempts}");
        Console.WriteLine($"Error: {result.Error?.Message ?? "Everything is OK"}");
    }
}