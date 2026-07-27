namespace Domain.Models;

public abstract class Item : Entity<ItemType, ItemState>
{
    public int Id { get; init; }
    public GradedConstant<ItemQuality> Quality { get; }

    protected Item(int id, ItemType type, ItemState state,
        IRangeConverter<ItemQuality> qualityConverter, int initialPercentage = 100)
        : base(type, state)
    {
        Id = id;
        Quality = new GradedConstant<ItemQuality>(qualityConverter, initialPercentage);
    }
}
