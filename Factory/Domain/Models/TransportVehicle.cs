namespace Domain.Models;

public abstract class TransportVehicle : Entity<TransportType, TransportState>, ITransportVehicle
{
    public int Capacity { get; }
    public int TicksPerTrip { get; }

    protected TransportVehicle(TransportType type, int capacity, int ticksPerTrip)
        : base(type, TransportStates.Idle)
    {
        Capacity = capacity > 0 ? capacity : throw new ArgumentException("Capacity must be greater than 0.", nameof(capacity));
        TicksPerTrip = ticksPerTrip > 0 ? ticksPerTrip : throw new ArgumentException("TicksPerTrip must be greater than 0.", nameof(ticksPerTrip));
    }

    public abstract bool CanCarry(ItemType itemType);

    public void MarkDispatched() => ChangeState(TransportStates.Shipping);
    public void MarkIdle() => ChangeState(TransportStates.Idle);
}