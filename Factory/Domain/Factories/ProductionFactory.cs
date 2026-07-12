using Domain.Constants;
using Domain.Models;
using Domain.Registries;
using Domain.Utils;

namespace Domain.Factories;

public abstract class ProductionFactory : IProductionFactory
{
    public ICollection<ItemType> SupportedItemTypes { get; } = [];
    public MachineType SupportedMachineType { get; }
    
    public ProductionFactory(IEnumerable<ItemType> itemTypes, MachineType machineType, IRegistry registry)
    {
        registry.ValidateType(machineType);
        SupportedMachineType = machineType;
        
        foreach (var itemType in itemTypes)
        {
            registry.ValidateType(itemType);
            SupportedItemTypes.Add(itemType);
        }
    }

    public abstract Machine CreateMachine();
}