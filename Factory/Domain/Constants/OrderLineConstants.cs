namespace Domain.Constants;

public class OrderLineType : TypeConstant
{
    internal OrderLineType(string value) : base(value, Contexts.Types.OrderLine) { }
}

public static class OrderLineTypes
{
    public static OrderLineType Create(string value) => new(value);

    public static readonly OrderLineType OrderLineA = new("OrderLineA");
    public static readonly OrderLineType OrderLineB = new("OrderLineB");
    public static readonly OrderLineType OrderLineC = new("OrderLineC");
    public static readonly OrderLineType Unknown = new("Unknown");
}

public class OrderLineState : StateConstant
{
    internal OrderLineState(string value) : base(value, Contexts.States.OrderLine) { }
}

public static class OrderLineStates
{
    public static OrderLineState Create(string value) => new(value);

    public static readonly OrderLineState Active = new("Active");
    public static readonly OrderLineState Paused = new("Paused");
    public static readonly OrderLineState Blocked = new("Blocked");
    public static readonly OrderLineState Disabled = new("Disabled");
}