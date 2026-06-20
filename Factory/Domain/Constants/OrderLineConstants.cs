namespace Domain.Constants;

public class OrderLineStates : StateConstant
{
    private OrderLineStates(string value) : base(value, Contexts.States.OrderLine) { }

    public static OrderLineStates Create(string value) => new(value);

    public static readonly OrderLineStates Active = new("Active");
    public static readonly OrderLineStates Paused = new("Paused");
    public static readonly OrderLineStates Blocked = new("Blocked");
    public static readonly OrderLineStates Disabled = new("Disabled");

    public static IEnumerable<OrderLineStates> All => [Active, Paused, Blocked, Disabled];
}