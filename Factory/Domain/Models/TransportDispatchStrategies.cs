namespace Domain.Models;

public sealed class CheapestSufficientVehicleStrategy : ITransportDispatchStrategy
{
    public ITransportVehicle SelectVehicle(StockOrder order, IReadOnlyList<ITransportVehicle> availableFleet)
    {
        var candidates = availableFleet.Where(v => v.CanCarry(order.ItemType)).ToList();
        if (candidates.Count == 0)
            throw new InvalidOperationException($"No available vehicle can carry item type '{order.ItemType}'.");

        return candidates
            .Where(v => v.Capacity >= order.Remaining)
            .OrderBy(v => v.Capacity)
            .FirstOrDefault()
            ?? candidates.OrderByDescending(v => v.Capacity).First();
    }
}

public sealed class FastestVehicleStrategy : ITransportDispatchStrategy
{
    public ITransportVehicle SelectVehicle(StockOrder order, IReadOnlyList<ITransportVehicle> availableFleet)
    {
        var candidates = availableFleet.Where(v => v.CanCarry(order.ItemType)).ToList();
        if (candidates.Count == 0)
            throw new InvalidOperationException($"No available vehicle can carry item type '{order.ItemType}'.");

        return candidates.OrderBy(v => v.TicksPerTrip).First();
    }
}
