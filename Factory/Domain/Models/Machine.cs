using Domain.Constants;
using Domain.Registries;

namespace Domain.Models;

public abstract class Machine : Entity
{
    public List<ItemType> SupportedItemTypes { get; } = [];

    protected Machine(MachineType type, MachineState state, IEnumerable<ItemType> supportedItems, IRegistry registry) 
        : base(type, state, registry)
    {
        SupportedItemTypes.AddRange(supportedItems);
    }

    public bool CanProduce(ItemType itemType) => SupportedItemTypes.Contains(itemType);
}