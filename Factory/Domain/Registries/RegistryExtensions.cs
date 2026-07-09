using Domain.Constants;
using Domain.Exceptions;

namespace Domain.Registries;

public static class RegistryExtensions
{
    public static void EnsureRegistry(this IRegistry registry)
    {
        if (registry == null) throw new ArgumentNullException(nameof(registry));
    }
    
    public static void Validate(this IRegistry registry, TypeConstant type, StateConstant state)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (state == null) throw new ArgumentNullException(nameof(state));
        
        if (type.Context != state.Context)
        {
            throw new ArgumentException(
                $"Domain context mismatch! Attempted to validate Type from context '{type.Context.Name}' " +
                $"together with State from context '{state.Context.Name}'.");
        }

        ValidateType(registry, type);
        ValidateState(registry, state);
    }

    public static void ValidateType(this IRegistry registry, TypeConstant type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        
        if (!registry.IsValid(type.Value, type.Context, type.Kind))
        {
            throw new TypeException($"Invalid type '{type.Value}' for context '{type.Context.Name}'.", type);
        }
    }
    
    public static void ValidateState(this IRegistry registry, StateConstant state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        
        if (!registry.IsValid(state.Value, state.Context, state.Kind))
        {
            throw new StateException($"Invalid state '{state.Value}' for context '{state.Context.Name}'.", state);
        }
    }
}
