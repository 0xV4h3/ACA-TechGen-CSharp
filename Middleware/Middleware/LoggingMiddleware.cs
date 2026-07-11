using System.Diagnostics;

namespace Middleware;

public class LoggingMiddleware<TData> : IMiddleware<TData>
{
    public void Invoke(RequestContext<TData> context, RequestDelegate<TData> next)
    {
        Console.WriteLine($"[Logger] Processing started. Data: {context.RequestData}");
        var sw = Stopwatch.StartNew();

        next(context);

        sw.Stop();
        Console.WriteLine($"[Logger] Processing finished. Time: {sw.ElapsedMilliseconds} ms. Status: {context.ResponseMessage}");
    }
}