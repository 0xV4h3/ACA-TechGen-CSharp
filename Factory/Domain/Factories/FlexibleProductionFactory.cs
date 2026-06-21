using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace Domain.Factories;

public abstract class FlexibleProductionFactory
{
    protected readonly List<ItemType> SupportedItemTypes;
    protected readonly MachineType FactoryMachineType;
    protected readonly IRegistry Registry;
    
    protected FlexibleProductionFactory(MachineType machineType, List<ItemType> supportedItemTypes, IRegistry registry)
    {
        FactoryMachineType = machineType;
        Registry = registry ?? throw new ArgumentNullException(nameof(registry));
        SupportedItemTypes = supportedItemTypes ?? throw new ArgumentNullException(nameof(supportedItemTypes));
        
        if (SupportedItemTypes.Count == 0)
            throw new ArgumentException("Factory must support at least one item type.", nameof(supportedItemTypes));
    }

    public Machine CreateMachine()
    {
        return CreateMachineInstance(FactoryMachineType, MachineStates.Idle, SupportedItemTypes, Registry);
    }
    
    public abstract Item CreateItem(int id);
    
    protected abstract Machine CreateMachineInstance(
        MachineType type, MachineState state, IEnumerable<ItemType> supportedItems, IRegistry reg);
    
    protected void ValidateTypeSupport(ItemType type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (!SupportedItemTypes.Contains(type))
        {
            throw new InvalidOperationException(
                $"This factory cannot produce '{type}'. Supported: {string.Join(", ", SupportedItemTypes)}");
        }
    }
}