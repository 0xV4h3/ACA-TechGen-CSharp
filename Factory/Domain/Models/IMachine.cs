using Domain.Constants;

namespace Domain.Models;

public interface IMachine { }

public interface ISingleTypeMachine : IMachine
{
    ItemType SupportedItemType { get; }
    Item Produce(int id);
}

public interface IMultiTypeMachine : IMachine
{
    List<ItemType> SupportedItemTypes { get; }
    Item Produce(int id, ItemType type);
}