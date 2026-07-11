namespace Middleware;

public class RequestContext<TData>(TData data)
{
    public TData RequestData { get; set; } = data;
    public string ResponseMessage { get; set; } = "OK";
    public bool IsAborted { get; set; } = false;
}

public delegate void RequestDelegate<TData>(RequestContext<TData> context);