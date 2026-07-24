using Domain.Constants;
using Domain.Models.Abstractions;
using Domain.Models.Converters;
using Domain.Registries;
using Domain.Utils;

namespace Domain.Models;

public class StockLocation : Entity<StockType, StockState>
{
    private readonly Dictionary<ItemType, Bin<StockCapacity>> _bins = new();

    public string Name { get; }

    public StockLocation(
        StockType type,
        string name,
        IRegistry registry,
        IEnumerable<ItemType> supportedTypes,
        int capacityPerType,
        IRangeConverter<StockCapacity> converter)
        : base(type, StockStates.Normal)
    {
        registry.Validate(type, StockStates.Normal);
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
        if (added) UpdateState(item.Type);
        return added;
    }

    private void UpdateState(ItemType type)
    {
        var grade = FillFor(type).Constant;

        if (grade == StockCapacities.Overloaded) ChangeState(StockStates.Overstock);
        else if (grade == StockCapacities.Empty) ChangeState(StockStates.OutOfStock);
        else if (grade == StockCapacities.Low) ChangeState(StockStates.LowStock);
        else ChangeState(StockStates.Normal);
    }

    private Bin<StockCapacity> BinFor(ItemType type) =>
        _bins.TryGetValue(type, out var bin)
            ? bin
            : throw new InvalidOperationException($"Stock location '{Name}' does not support item type '{type}'.");
}