using Domain.Constants;
using Domain.Exceptions;

namespace Domain.Registries;

public class Registry : IRegistry
{
    private readonly Dictionary<string, Context> _contexts = new(StringComparer.OrdinalIgnoreCase);

    public Registry()
    {
        AutoDiscoverContexts();
    }

    private void AutoDiscoverContexts()
    {
        foreach (var typeContext in Contexts.Types.All)
        {
            RegisterContext(typeContext);
        }
        foreach (var stateContext in Contexts.States.All)
        {
            RegisterContext(stateContext);
        }
    }

    public bool RegisterContext(Context context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        
        return _contexts.TryAdd(context.Name, context);
    }

    public bool Register(Constant constant, bool autoRegisterContext = false)
    {
        if (constant == null) throw new ArgumentNullException(nameof(constant));

        var contextName = constant.Context.Name;

        if (!_contexts.TryGetValue(contextName, out var registeredContext))
        {
            if (autoRegisterContext)
            {
                registeredContext = constant.Context;
                RegisterContext(registeredContext);
            }
            else
            {
                throw new ContextException(
                    $"Context '{constant.Context.Name}' is not registered. Set autoRegisterContext to true to register it automatically.", constant.Context);
            }
        }
        
        if (registeredContext.IsValid(constant.Value))
            return false;

        registeredContext.AddConstant(constant);
        return true;
    }

    public bool IsValid(string value, Context context)
    {
        return _contexts.TryGetValue(context.Name, out var registeredContext) && registeredContext.IsValid(value);
    }

    public IEnumerable<Constant> GetAll(Context context)
    {
        return _contexts.TryGetValue(context.Name, out var registeredContext) ? registeredContext.GetAll() : [];
    }

    public IEnumerable<string> GetAllString(Context context)
    {
        return _contexts.TryGetValue(context.Name, out var registeredContext) ? registeredContext.GetAllString() : [];
    }
}