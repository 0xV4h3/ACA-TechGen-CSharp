using Domain.Constants;

namespace FactoryCore;

public sealed class SimulationStats
{
    public Dictionary<ItemType, int> Produced { get; } = new();
    public Dictionary<ItemType, int> Enqueued { get; } = new();
    public Dictionary<ItemType, int> Passed { get; } = new();
    public Dictionary<ItemType, int> Repaired { get; } = new();
    public Dictionary<ItemType, int> Scrapped { get; } = new();
    public Dictionary<ItemType, int> MovedToStock { get; } = new();

    public void Increment(Dictionary<ItemType, int> bucket, ItemType type, int amount = 1)
    {
        bucket[type] = bucket.GetValueOrDefault(type) + amount;
    }

    public string SummaryFor(ItemType type) =>
        $"Type {type}: produced={Produced.GetValueOrDefault(type)}, enqueued={Enqueued.GetValueOrDefault(type)}, " +
        $"passed={Passed.GetValueOrDefault(type)}, repaired={Repaired.GetValueOrDefault(type)}, " +
        $"scrapped={Scrapped.GetValueOrDefault(type)}, moved-to-stock={MovedToStock.GetValueOrDefault(type)}";
}
