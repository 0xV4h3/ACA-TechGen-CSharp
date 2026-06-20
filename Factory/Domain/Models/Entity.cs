using Domain.Registries;
using Domain.Exceptions;

namespace Domain.Models;

public abstract class Entity
{
    public string Type { get; init; }
    public string State { get; private set; }
    
    protected Entity(string type, string state, string typeContext, string stateContext, IRegistry registry)
    {
        if (!registry.IsValid(type, typeContext))
            throw new TypeException($"Invalid type '{type}' for context '{typeContext}'", type);
            
        if (!registry.IsValid(state, stateContext))
            throw new StateException($"Invalid state '{state}' for context '{stateContext}'", state);

        Type = type;
        State = state;
    }

    public void ChangeState(string newState, string stateContext, IRegistry registry)
    {
        if (!registry.IsValid(newState, stateContext))
            throw new StateException($"Invalid state transition: {newState}", newState);

        State = newState;
    }
}