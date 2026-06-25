namespace Domain.Constants;

public class CapacityState : StateConstant
{
    internal CapacityState(string value) : base(value, Contexts.States.Capacity) { }
}

public static class CapacityStates
{
    public static CapacityState Create(string value) => new(value);

    public static readonly CapacityState Empty = new("Empty");
    public static readonly CapacityState PartiallyFull = new("PartiallyFull");
    public static readonly CapacityState Full = new("Full");
    public static readonly CapacityState Overloaded = new("Overloaded");
}