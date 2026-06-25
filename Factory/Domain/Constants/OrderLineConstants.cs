namespace Domain.Constants;

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