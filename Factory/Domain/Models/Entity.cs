using Domain.Constants;
using Domain.Exceptions;

namespace Domain.Models;

public abstract class Entity
{
    public TypeConstant Type { get; init; }
    public StateConstant State { get; private set; }

    protected Entity(TypeConstant type, StateConstant state)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        State = state ?? throw new ArgumentNullException(nameof(state));
    }

    public void ChangeState(StateConstant newState)
    {
        if (newState == null) throw new ArgumentNullException(nameof(newState));
        
        if (!string.Equals(State.Context.Name, newState.Context.Name, StringComparison.OrdinalIgnoreCase))
        {
            throw new StateException(
                $"Cannot change state to '{newState.Value}' because its context '{newState.Context.Name}' " +
                $"does not match the entity state context '{State.Context.Name}'.", newState);
        }
        
        if (string.Equals(State.Value, newState.Value, StringComparison.OrdinalIgnoreCase))
            return;

        State = newState;
    }
}