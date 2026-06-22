using Domain.Constants;
using Domain.Registries;
using Domain.Exceptions;

namespace Domain.Models;

public abstract class Machine : Entity, IMachine
{
    protected Machine(MachineType type, MachineState state, IRegistry registry) 
        : base(type, state)
    {
        if (registry == null) throw new ArgumentNullException(nameof(registry));
        
        if (!registry.IsValid(type.Value, Contexts.Types.Machine))
            throw new TypeException($"Invalid machine type '{type.Value}' for context '{Contexts.Types.Machine.Name}'", type);
    }

    protected virtual bool IsMachineReady() => State != MachineStates.Maintenance;
    protected virtual void EnsureMachineIsReady()
    {
        if (State == MachineStates.Maintenance)
            throw new InvalidOperationException("The machine is undergoing maintenance and cannot produce.");
    }
    
    protected Item RunProductionCycle(Func<Item> createItemLogic, Action beforeProduce, Action<Item> afterProduce)
    {
        EnsureMachineIsReady();

        try
        {
            ChangeState(MachineStates.Producing);
            beforeProduce();
            
            Item createdItem = createItemLogic();
            
            afterProduce(createdItem);
            return createdItem;
        }
        finally
        {
            ChangeState(MachineStates.Idle);
        }
    }
}

public abstract class SingleTypeMachine(
    MachineType type, 
    MachineState state, 
    ItemType supportedItem, 
    IRegistry registry) : Machine(type, state, registry), ISingleTypeMachine
{
    public ItemType SupportedItemType { get; } = supportedItem ?? throw new ArgumentNullException(nameof(supportedItem));
    
    public Item Produce(int id)
    {
        return RunProductionCycle(
            createItemLogic: () => CreateItem(id),
            beforeProduce: () => OnBeforeProduce(id),
            afterProduce: OnAfterProduce
        );
    }

    protected virtual void OnBeforeProduce(int id) { }
    protected virtual void OnAfterProduce(Item item) { }
    protected abstract Item CreateItem(int id);
}

public abstract class MultiTypeMachine : Machine, IMultiTypeMachine
{
    public List<ItemType> SupportedItemTypes { get; } = [];

    protected MultiTypeMachine(MachineType type, MachineState state, List<ItemType> supportedItems, IRegistry registry) 
        : base(type, state, registry)
    {
        if (supportedItems == null || supportedItems.Count == 0)
            throw new ArgumentException("MultiTypeMachine must support at least one item type.", nameof(supportedItems));
        
        if (supportedItems.Count < 2)
            throw new ArgumentException("MultiTypeMachine must support at least two item types. Use SingleTypeMachine instead.");

        SupportedItemTypes.AddRange(supportedItems);
    }
    
    public Item Produce(int id, ItemType type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (!SupportedItemTypes.Contains(type))
            throw new InvalidOperationException($"Machine cannot produce '{type}'.");

        return RunProductionCycle(
            createItemLogic: () => CreateItem(id, type),
            beforeProduce: () => OnBeforeProduce(id, type),
            afterProduce: OnAfterProduce
        );
    }

    protected virtual void OnBeforeProduce(int id, ItemType type) { }
    protected virtual void OnAfterProduce(Item item) { }
    protected abstract Item CreateItem(int id, ItemType type);
}