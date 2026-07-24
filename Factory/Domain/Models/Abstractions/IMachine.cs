using Domain.Constants;

namespace Domain.Models.Abstractions;

public interface IMachine { }

public interface ISingleTypeMachine : IMachine
{
    ItemType SupportedItemType { get; }
    Item Produce(int id, int? qualityPercentage);
}

public interface IMultiTypeMachine : IMachine
{
    List<ItemType> SupportedItemTypes { get; }
    Item Produce(int id, ItemType type, int? qualityPercentage);
}