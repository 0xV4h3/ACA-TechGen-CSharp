using Domain.Constants;
using Domain.Registries;
using Domain.Exceptions;

namespace Domain.Models;

public abstract class Entity
{
    public TypeConstant Type { get; init; }
    public StateConstant State { get; private set; }

    protected Entity(TypeConstant type, StateConstant state, IRegistry registry)
    {
        if (!registry.IsValid(type.Value, type.Context))
            throw new TypeException($"Invalid type '{type.Value}' for context '{type.Context}'", type);

        if (!registry.IsValid(state.Value, state.Context))
            throw new StateException($"Invalid state '{state.Value}' for context '{state.Context}'", state);

        Type = type;
        State = state;
    }

    public void ChangeState(StateConstant newState, IRegistry registry)
    {
        if (!registry.IsValid(newState.Value, newState.Context))
            throw new StateException($"Invalid state transition from '{State.Value}' to '{newState.Value}'", newState);

        State = newState;
    }
}