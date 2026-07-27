namespace Domain.Models;

public sealed class TransportHub
{
    private readonly List<ITransportVehicle> _fleet;
    private readonly ITransportDispatchStrategy _strategy;
    private readonly Dictionary<ITransportVehicle, int> _busyUntilTick = new();

    public TransportHub(IEnumerable<ITransportVehicle> fleet, ITransportDispatchStrategy strategy)
    {
        _fleet = fleet?.ToList() ?? throw new ArgumentNullException(nameof(fleet));
        if (_fleet.Count == 0) throw new ArgumentException("Fleet cannot be empty.", nameof(fleet));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public int Deliver(StockOrder order, Storage source, int maxItemsThisRun, int currentTick)
    {
        if (order.IsFulfilled) return 0;

        ReleaseReturnedVehicles(currentTick);

        var available = _fleet.Where(v => !_busyUntilTick.ContainsKey(v)).ToList();
        if (available.Count == 0) return 0;

        var vehicle = _strategy.SelectVehicle(order, available);
        int toMove = Math.Min(maxItemsThisRun, Math.Min(vehicle.Capacity, order.Remaining));
        if (toMove <= 0) return 0;

        int moved = source.MoveToTransport(order.ItemType, toMove, item => order.Destination.TryStore(item));
        if (moved <= 0) return 0;

        order.RegisterDelivery(moved);
        vehicle.MarkDispatched();
        _busyUntilTick[vehicle] = currentTick + vehicle.TicksPerTrip * 2;

        return moved;
    }

    private void ReleaseReturnedVehicles(int currentTick)
    {
        foreach (var (vehicle, until) in _busyUntilTick.ToList())
        {
            if (currentTick < until) continue;
            vehicle.MarkIdle();
            _busyUntilTick.Remove(vehicle);
        }
    }
}
