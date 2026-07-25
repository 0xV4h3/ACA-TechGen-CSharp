namespace Domain.Models;

public sealed class Bin<TCapacityConstant> where TCapacityConstant : CapacityConstant
{
    private readonly Queue<Item> _items = new();

    public int CapacityLimit { get; }
    public GradedConstant<TCapacityConstant> Fill { get; }
    public int Count => _items.Count;
    public bool IsFull => Count >= CapacityLimit;

    public Bin(int capacityLimit, IRangeConverter<TCapacityConstant> converter)
    {
        if (capacityLimit <= 0)
            throw new ArgumentException("Capacity must be greater than 0.", nameof(capacityLimit));

        CapacityLimit = capacityLimit;
        Fill = new GradedConstant<TCapacityConstant>(converter, 0);
    }

    public bool TryAdd(Item item)
    {
        if (IsFull) return false;
        _items.Enqueue(item);
        UpdateFill();
        return true;
    }

    public bool TryRemove(out Item? item)
    {
        if (!_items.TryDequeue(out item)) return false;
        UpdateFill();
        return true;
    }

    private void UpdateFill()
    {
        int percentage = Count * 100 / CapacityLimit;
        Fill.Change(percentage);
    }
}