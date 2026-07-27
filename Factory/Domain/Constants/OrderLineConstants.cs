namespace Domain.Constants;

public class OrderLineType : TypeConstant
{
    internal OrderLineType(string value) : base(value, Contexts.OrderLine) { }
}

public static class OrderLineTypes
{
    public static OrderLineType Create(string value) => new(value);

    public static readonly OrderLineType Standard = new("Standard");
    public static readonly OrderLineType OrderLineA = new("OrderLineA");
    public static readonly OrderLineType OrderLineB = new("OrderLineB");
    public static readonly OrderLineType OrderLineC = new("OrderLineC");
    public static readonly OrderLineType Unknown = new("Unknown");
}

public class OrderLineState : StateConstant
{
    internal OrderLineState(string value) : base(value, Contexts.OrderLine) { }
}

public static class OrderLineStates
{
    public static OrderLineState Create(string value) => new(value);

    public static readonly OrderLineState Active = new("Active");
    public static readonly OrderLineState Paused = new("Paused");
    public static readonly OrderLineState Blocked = new("Blocked");
    public static readonly OrderLineState Disabled = new("Disabled");
}

public class OrderLineCapacity : CapacityConstant
{
    internal OrderLineCapacity(string value) : base(value, Contexts.OrderLine) { }
}

public static class OrderLineCapacities
{
    public static OrderLineCapacity Create(string value) => new(value);

    public static readonly OrderLineCapacity Empty = new("Empty");
    public static readonly OrderLineCapacity Low = new("Low");
    public static readonly OrderLineCapacity Moderate = new("Moderate");
    public static readonly OrderLineCapacity High = new("High");
    public static readonly OrderLineCapacity Full = new("Full");
}