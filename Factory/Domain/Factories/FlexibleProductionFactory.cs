using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace Domain.Factories;

public abstract class FlexibleProductionFactory : IProductionFactory
{
    protected readonly List<ItemType> SupportedItemTypes;
    protected readonly MachineType FactoryMachineType;
    protected readonly IRegistry ProductionRegistry;
    
    protected FlexibleProductionFactory(MachineType machineType, List<ItemType> supportedItemTypes, IRegistry registry)
    {
        FactoryMachineType = machineType;
        ProductionRegistry = registry ?? throw new ArgumentNullException(nameof(registry));
        SupportedItemTypes = supportedItemTypes ?? throw new ArgumentNullException(nameof(supportedItemTypes));
        
        if (SupportedItemTypes.Count == 0)
            throw new ArgumentException("Factory must support at least one item type.", nameof(supportedItemTypes));
    }

    public Machine CreateMachine()
    {
        return CreateMachineInstance(FactoryMachineType, MachineStates.Idle, SupportedItemTypes, ProductionRegistry);
    }
    
    protected abstract MultiTypeMachine CreateMachineInstance(
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