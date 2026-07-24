using Domain.Constants;
using Domain.Models.Abstractions;

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

public sealed class Truck() : TransportVehicle(TransportTypes.Truck, capacity: 6, ticksPerTrip: 2)
{
    public override bool CanCarry(ItemType itemType) => true;
}

public sealed class Ship() : TransportVehicle(TransportTypes.Ship, capacity: 40, ticksPerTrip: 10)
{
    public override bool CanCarry(ItemType itemType) => true;
}

public sealed class CargoPlane() : TransportVehicle(TransportTypes.Plane, capacity: 20, ticksPerTrip: 4)
{
    public override bool CanCarry(ItemType itemType) => true;
}

public sealed class Motorcycle() : TransportVehicle(TransportTypes.Motorcycle, capacity: 2, ticksPerTrip: 1)
{
    public override bool CanCarry(ItemType itemType) => true;
}
