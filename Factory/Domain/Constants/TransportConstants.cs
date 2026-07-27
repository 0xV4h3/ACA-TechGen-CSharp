namespace Domain.Constants;

public class TransportType : TypeConstant
{
    internal TransportType(string value) : base(value, Contexts.Transport) { }
}

public static class TransportTypes
{
    public static TransportType Create(string value) => new(value);
    
    public static readonly TransportType Truck = new("Truck");
    public static readonly TransportType Ship = new("Ship");
    public static readonly TransportType Plane = new("Plane");
    public static readonly TransportType Motorcycle = new("Motorcycle");
    public static readonly TransportType Unknown = new("Unknown");
}

public class TransportState : StateConstant
{
    internal TransportState(string value) : base(value, Contexts.Transport) { }
}

public static class TransportStates
{
    public static TransportState Create(string value) => new(value);

    public static readonly TransportState Idle = new("Idle");
    public static readonly TransportState Loading = new("Loading");
    public static readonly TransportState Shipping = new("Shipping");
    public static readonly TransportState Arrived = new("Arrived");
    public static readonly TransportState Unloading = new("Unloading");
    public static readonly TransportState Returning = new("Returning");
    public static readonly TransportState Maintenance = new("Maintenance");
}