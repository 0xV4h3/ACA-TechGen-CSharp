using Domain.Utils;

namespace Domain.Models;

public class Storage : Entity<StorageType, StorageState>
{
    private readonly Dictionary<ItemType, Bin<StorageCapacity>> _bins = new();

    public Storage(
        StorageType type,
        IRegistry registry,
        IEnumerable<ItemType> supportedTypes,
        int capacityPerType,
        IRangeConverter<StorageCapacity> converter)
        : base(type, StorageStates.Open)
    {
        registry.Validate(type, StorageStates.Open);

        foreach (var itemType in supportedTypes)
        {
            registry.ValidateType(itemType);
            _bins[itemType] = new Bin<StorageCapacity>(capacityPerType, converter);
        }
    }

    public GradedConstant<StorageCapacity> FillFor(ItemType type) => BinFor(type).Fill;
    public int CountFor(ItemType type) => BinFor(type).Count;
    public int TotalCount => _bins.Values.Sum(b => b.Count);

    public bool TryStore(Item item)
    {
        EnsureOpenForInbound();
        return BinFor(item.Type).TryAdd(item);
    }

    public int MoveToTransport(ItemType type, int maxItems, Action<Item> onMoved)
    {
        EnsureOpenForOutbound();
        var bin = BinFor(type);

        int moved = 0;
        while (moved < maxItems && bin.TryRemove(out var item))
        {
            onMoved(item!);
            moved++;
        }
        return moved;
    }

    private Bin<StorageCapacity> BinFor(ItemType type) =>
        _bins.TryGetValue(type, out var bin)
            ? bin
            : throw new InvalidOperationException($"Storage '{Type}' does not support item type '{type}'.");

    private void EnsureOpenForInbound()
    {
        if (State == StorageStates.Closed || State == StorageStates.LockedForInbound)
            throw new InvalidOperationException("Storage is not accepting inbound items.");
    }

    private void EnsureOpenForOutbound()
    {
        if (State == StorageStates.Closed || State == StorageStates.LockedForOutbound)
            throw new InvalidOperationException("Storage is not accepting outbound transfers.");
    }
}
