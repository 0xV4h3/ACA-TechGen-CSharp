namespace Domain.Constants;

public class TransportStates : StateConstant
{
    private TransportStates(string value) : base(value, Contexts.States.Transport) { }

    public static TransportStates Create(string value) => new(value);

    public static readonly TransportStates Idle = new("Idle");
    public static readonly TransportStates Loading = new("Loading");
    public static readonly TransportStates Shipping = new("Shipping");
    public static readonly TransportStates Arrived = new("Arrived");
    public static readonly TransportStates Unloading = new("Unloading");
    public static readonly TransportStates Returning = new("Returning");
    public static readonly TransportStates Maintenance = new("Maintenance");

    public static IEnumerable<TransportStates> All => 
    [
        Idle, Loading, Shipping, Arrived, Unloading, Returning, Maintenance
    ];
}