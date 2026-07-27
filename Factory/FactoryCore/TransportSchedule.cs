using Domain.Ticks;

namespace FactoryCore;

public sealed class TransportSchedule : ITickable
{
    private readonly TransportHub _hub;
    private readonly Storage _source;
    private readonly List<StockOrder> _orders;
    private readonly int _arrivalIntervalTicks;
    private readonly int _capacityPerArrival;
    private readonly SimulationStats _stats;
    private readonly Action<string> _log;

    public TransportSchedule(
        TransportHub hub,
        Storage source,
        List<StockOrder> orders,
        int arrivalIntervalTicks,
        int capacityPerArrival,
        SimulationStats stats,
        Action<string> log)
    {
        _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _orders = orders ?? throw new ArgumentNullException(nameof(orders));
        _arrivalIntervalTicks = arrivalIntervalTicks;
        _capacityPerArrival = capacityPerArrival;
        _stats = stats ?? throw new ArgumentNullException(nameof(stats));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public bool HasPendingOrders => _orders.Any(o => !o.IsFulfilled);

    public void Tick(int tickNumber)
    {
        if (tickNumber % _arrivalIntervalTicks != 0) return;

        int budget = _capacityPerArrival;
        _log($"Transport arrived (budget {_capacityPerArrival}).");

        foreach (var order in _orders.Where(o => !o.IsFulfilled))
        {
            if (budget <= 0) break;

            int moved = _hub.Deliver(order, _source, budget, tickNumber);
            if (moved <= 0) continue;

            budget -= moved;
            _stats.Increment(_stats.MovedToStock, order.ItemType, moved);
            _log($"Delivered {moved}x {order.ItemType} to '{order.Destination.Name}' " +
                 $"({order.Delivered}/{order.Quantity}).");
        }
    }
}
