namespace Domain.Constants;

public class CapacityStates : StateConstant
{
    private CapacityStates(string value) : base(value, Contexts.States.Capacity) { }

    public static CapacityStates Create(string value) => new(value);

    public static readonly CapacityStates Empty = new("Empty");
    public static readonly CapacityStates PartiallyFull = new("PartiallyFull");
    public static readonly CapacityStates Full = new("Full");
    public static readonly CapacityStates Overloaded = new("Overloaded");

    public static IEnumerable<CapacityStates> All => [Empty, PartiallyFull, Full, Overloaded];
}

