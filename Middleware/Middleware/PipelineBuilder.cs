namespace Middleware;

public class PipelineBuilder<TData>
{
    private readonly List<Func<RequestDelegate<TData>, RequestDelegate<TData>>> _components = new();
    
    public PipelineBuilder<TData> Use(IMiddleware<TData> middleware)
    {
        _components.Add(next => context =>
        {
            if (context.IsAborted) return;
            middleware.Invoke(context, next);
        });
        
        return this;
    }
    
    public PipelineBuilder<TData> Use(Action<RequestContext<TData>, RequestDelegate<TData>> inlineMiddleware)
    {
        _components.Add(next => context =>
        {
            if (context.IsAborted) return;
            inlineMiddleware(context, next);
        });
        
        return this;
    }
    
    public RequestDelegate<TData> Build()
    {
        RequestDelegate<TData> current = context => 
        {
            Console.WriteLine("[End of chain] Request reached endpoint.");
        };
        
        for (int i = _components.Count - 1; i >= 0; i--)
            current = _components[i](current);

        return current;
    }
}