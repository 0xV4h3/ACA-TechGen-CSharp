using Domain.Constants;
using Domain.Exceptions;

namespace Domain.Models.Abstractions;

public abstract class Entity(TypeConstant type, StateConstant state) : IEntity
{
    public TypeConstant Type { get; init; } = type ?? throw new ArgumentNullException(nameof(type));
    public StateConstant State { get; protected set; } = state ?? throw new ArgumentNullException(nameof(state));

    protected void ChangeState(StateConstant newState)
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