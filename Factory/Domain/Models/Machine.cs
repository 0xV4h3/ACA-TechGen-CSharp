using Domain.Constants;
using Domain.Registries;
using Domain.Exceptions;

namespace Domain.Models;

public abstract class Machine : Entity
{
    public List<ItemType> SupportedItemTypes { get; } = [];
    
    protected Machine(MachineType type, MachineState state, List<ItemType> supportedItems, IRegistry registry) 
        : base(type, state)
    {
        if (supportedItems == null || supportedItems.Count == 0)
            throw new ArgumentException("Machine must support at least one item type.");
        
        if (!registry.IsValid(type.Value, Contexts.Types.Machine))
            throw new TypeException($"Invalid machine type '{type.Value}' for context '{Contexts.Types.Machine}'", type);

        SupportedItemTypes.AddRange(supportedItems);
    }

    public bool CanProduce(ItemType itemType) => SupportedItemTypes.Contains(itemType);
    
    public Item Produce(int id, ItemType? type = null)
    {
        var typeToProduce = type ?? SupportedItemTypes.First();

        if (!CanProduce(typeToProduce))
            throw new InvalidOperationException($"Machine cannot produce '{typeToProduce}'.");
        
        EnsureMachineIsReady();

        try
        {
            ChangeState(MachineStates.Producing);
            OnBeforeProduce(id, typeToProduce);
            
            Item createdItem = CreateItem(id, typeToProduce);
            
            OnAfterProduce(createdItem);
            
            return createdItem;
        }
        finally
        {
            ChangeState(MachineStates.Idle);
        }
    }
    
    // protected virtual bool EnsureMachineIsReady() => State != MachineStates.Maintenance;
    protected virtual void EnsureMachineIsReady()
    {
        if (State == MachineStates.Maintenance)
            throw new InvalidOperationException("The machine is undergoing maintenance and cannot produce.");
    }
    protected virtual void OnBeforeProduce(int id, ItemType type) { }
    protected virtual void OnAfterProduce(Item item) { }
    protected abstract Item CreateItem(int id, ItemType type);
}