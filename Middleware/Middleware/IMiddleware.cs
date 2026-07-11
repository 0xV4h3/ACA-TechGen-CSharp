namespace Middleware;

public interface IMiddleware<TData>
{
    void Invoke(RequestContext<TData> context, RequestDelegate<TData> next);
}