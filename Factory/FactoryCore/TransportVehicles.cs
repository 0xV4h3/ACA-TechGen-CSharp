using Domain.Constants;
using Domain.Models;
namespace FactoryCore;

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