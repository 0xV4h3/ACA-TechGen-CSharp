namespace Middleware;

class Program
{
    static void Main(string[] args)
    {
        var builder = new PipelineBuilder<string>();
        
        builder.Use(new LoggingMiddleware<string>());
        builder.Use(new ValidationMiddleware());
        
        builder.Use((context, next) =>
        {
            Console.WriteLine($"[ToUpper] Modifying data: {context.RequestData.ToUpper()}");
            next(context);
        });
        
        RequestDelegate<string> pipeline = builder.Build();
        
        Console.WriteLine("=== Test 1 ===");
        var goodContext = new RequestContext<string>("Vahe");
        pipeline(goodContext);

        Console.WriteLine("\n" + new string('-', 40) + "\n");
        
        Console.WriteLine("=== Test 2 ===");
        var badContext = new RequestContext<string>("   ");
        pipeline(badContext);
    }
}