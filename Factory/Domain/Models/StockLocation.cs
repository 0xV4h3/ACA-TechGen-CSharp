using Domain.Utils;

namespace Domain.Models;

public class StockLocation : Entity<StockType, StockState>
{
    private readonly Dictionary<ItemType, Bin<StockCapacity>> _bins = new();
    
    private static readonly StockState[] SeverityOrder =
    [
        StockStates.OutOfStock,
        StockStates.Overstock,
        StockStates.LowStock,
        StockStates.Normal
    ];

    public string Name { get; }

    public StockLocation(
        StockType type,
        string name,
        IRegistry registry,
        IEnumerable<ItemType> supportedTypes,
        int capacityPerType,
        IRangeConverter<StockCapacity> converter)
        : base(type, StockStates.OutOfStock)
    {
        registry.Validate(type, StockStates.OutOfStock);
        Name = string.IsNullOrWhiteSpace(name) ? type.Value : name;

        foreach (var itemType in supportedTypes)
        {
            registry.ValidateType(itemType);
            _bins[itemType] = new Bin<StockCapacity>(capacityPerType, converter);
        }
    }

    public GradedConstant<StockCapacity> FillFor(ItemType type) => BinFor(type).Fill;
    public int CountFor(ItemType type) => BinFor(type).Count;

    public bool TryStore(Item item)
    {
        bool added = BinFor(item.Type).TryAdd(item);
        if (added) UpdateState();
        return added;
    }

    private void UpdateState()
    {
        if (State == StockStates.Restricted) return;

        var signals = _bins.Values.Select(bin => GradeToState(bin.Fill.Constant)).ToHashSet();
        var worst = SeverityOrder.First(signals.Contains);
        ChangeState(worst);
    }

    private static StockState GradeToState(StockCapacity grade)
    {
        if (grade == StockCapacities.Overloaded) return StockStates.Overstock;
        if (grade == StockCapacities.Empty) return StockStates.OutOfStock;
        if (grade == StockCapacities.Low) return StockStates.LowStock;
        return StockStates.Normal;
    }

    private Bin<StockCapacity> BinFor(ItemType type) =>
        _bins.TryGetValue(type, out var bin)
            ? bin
            : throw new InvalidOperationException($"Stock location '{Name}' does not support item type '{type}'.");
}