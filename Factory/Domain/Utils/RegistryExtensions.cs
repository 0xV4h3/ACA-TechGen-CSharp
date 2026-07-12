using Domain.Constants;
using Domain.Exceptions;
using Domain.Registries;

namespace Domain.Utils;

public static class RegistryExtensions
{
    public static void EnsureRegistry(this IRegistry registry)
    {
        if (registry == null) throw new ArgumentNullException(nameof(registry));
    }
    
    public static void Validate(this IRegistry registry, TypeConstant type, StateConstant state)
    {
        registry.EnsureRegistry();
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (state == null) throw new ArgumentNullException(nameof(state));
        
        if (type.Context != state.Context)
        {
            throw new ArgumentException(
                $"Domain context mismatch! Attempted to validate Type from context '{type.Context.Name}' " +
                $"together with State from context '{state.Context.Name}'.");
        }

        ValidateTypeInternal(registry, type);
        ValidateStateInternal(registry, state);
    }

    public static void ValidateType(this IRegistry registry, TypeConstant type)
    {
        registry.EnsureRegistry();
        ValidateTypeInternal(registry, type);
    }
    private static void ValidateTypeInternal(this IRegistry registry, TypeConstant type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        
        if (!registry.IsValid(type.Value, type.Context, type.Kind))
        {
            throw new TypeException($"Invalid type '{type.Value}' of kind '{type.Kind}' for context '{type.Context.Name}'.", type); 
        }
    }
    
    public static void ValidateState(this IRegistry registry, StateConstant state)
    {
        registry.EnsureRegistry();
        ValidateStateInternal(registry, state);
    }
    private static void ValidateStateInternal(this IRegistry registry, StateConstant state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        
        if (!registry.IsValid(state.Value, state.Context, state.Kind))
        {
            throw new StateException($"Invalid state '{state.Value}' of kind '{state.Kind}' for context '{state.Context.Name}'.", state);
        }
    }
}
