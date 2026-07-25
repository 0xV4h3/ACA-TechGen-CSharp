namespace Domain.Abstractions;

public abstract class Entity<TType, TState>(TType type, TState state)
    where TType : TypeConstant
    where TState : StateConstant
{
    public TType Type { get; init; } = type ?? throw new ArgumentNullException(nameof(type));
    public TState State { get; protected set; } = state ?? throw new ArgumentNullException(nameof(state));
    
    protected void ChangeState(TState newState)
    {
        if (newState is null) throw new ArgumentNullException(nameof(newState));

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