using Domain.Ticks;

namespace FactoryCore;

public sealed class MachineProductionSchedule : ITickable
{
    private readonly SingleTypeMachine _machine;
    private readonly OrderLine _orderLine;
    private readonly int _intervalTicks;
    private readonly int _totalItemsToProduce;
    private readonly Func<int> _nextItemId;
    private readonly SimulationStats _stats;
    private readonly Action<string> _log;

    private int _producedCount;
    private Item? _pendingItem;

    public MachineProductionSchedule(
        SingleTypeMachine machine,
        OrderLine orderLine,
        int intervalTicks,
        int totalItemsToProduce,
        Func<int> nextItemId,
        SimulationStats stats,
        Action<string> log)
    {
        _machine = machine ?? throw new ArgumentNullException(nameof(machine));
        _orderLine = orderLine ?? throw new ArgumentNullException(nameof(orderLine));
        _intervalTicks = intervalTicks;
        _totalItemsToProduce = totalItemsToProduce;
        _nextItemId = nextItemId ?? throw new ArgumentNullException(nameof(nextItemId));
        _stats = stats ?? throw new ArgumentNullException(nameof(stats));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public bool IsProductionComplete => _producedCount >= _totalItemsToProduce && _pendingItem is null;

    public void Tick(int tickNumber)
    {
        if (_pendingItem is not null)
        {
            if (_orderLine.TryEnqueue(_pendingItem))
            {
                _stats.Increment(_stats.Enqueued, _machine.SupportedItemType);
                _log($"Buffered item #{_pendingItem.Id} entered Order Line after waiting.");
                _pendingItem = null;
            }
            else
            {
                _log($"Order Line FULL -> item #{_pendingItem.Id} still waiting.");
                return;
            }
        }

        if (_producedCount >= _totalItemsToProduce) return;
        if (tickNumber % _intervalTicks != 0) return;

        var item = _machine.Produce(_nextItemId());
        _producedCount++;
        _stats.Increment(_stats.Produced, _machine.SupportedItemType);
        _log($"Machine {_machine.Type} produced item #{item.Id}.");

        if (_orderLine.TryEnqueue(item))
        {
            _stats.Increment(_stats.Enqueued, _machine.SupportedItemType);
            _log($"Item #{item.Id} entered Order Line.");
        }
        else
        {
            _pendingItem = item;
            _log($"Order Line FULL -> item #{item.Id} waiting for free space.");
        }
    }
}
