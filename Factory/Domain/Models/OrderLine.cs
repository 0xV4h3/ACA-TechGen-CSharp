using Domain.Utils;

namespace Domain.Models;

public class OrderLine : Entity<OrderLineType, OrderLineState>
{
    private readonly Bin<OrderLineCapacity> _bin;

    public OrderLine(
        OrderLineType type,
        IRegistry registry,
        int capacity,
        IRangeConverter<OrderLineCapacity> converter)
        : base(type, OrderLineStates.Active)
    {
        registry.Validate(type, OrderLineStates.Active);
        _bin = new Bin<OrderLineCapacity>(capacity, converter);
    }

    public GradedConstant<OrderLineCapacity> Fill => _bin.Fill;
    public int Count => _bin.Count;
    public int CapacityLimit => _bin.CapacityLimit;

    public bool TryEnqueue(Item item)
    {
        EnsureActive();
        bool added = _bin.TryAdd(item);
        UpdateState();
        return added;
    }

    public bool TryDequeue(out Item? item)
    {
        bool removed = _bin.TryRemove(out item);
        UpdateState();
        return removed;
    }

    private void EnsureActive()
    {
        if (State == OrderLineStates.Disabled)
            throw new InvalidOperationException("Order line is disabled.");
    }

    private void UpdateState()
    {
        if (State == OrderLineStates.Disabled) return;
        ChangeState(_bin.IsFull ? OrderLineStates.Blocked : OrderLineStates.Active);
    }
}