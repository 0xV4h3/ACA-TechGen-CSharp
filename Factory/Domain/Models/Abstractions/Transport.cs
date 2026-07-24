using Domain.Constants;

namespace Domain.Models.Abstractions;

public interface ITransportVehicle
{
    int Capacity { get; }
    int TicksPerTrip { get; }
    bool CanCarry(ItemType itemType);
    
    void MarkDispatched();
    void MarkIdle();
}

public interface ITransportDispatchStrategy
{
    ITransportVehicle SelectVehicle(StockOrder order, IReadOnlyList<ITransportVehicle> availableFleet);
}