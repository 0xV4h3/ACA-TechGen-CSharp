using Domain.Constants;
using Domain.Models.Quality;
using Domain.Models.Abstractions;

namespace Domain.Models;

public abstract class Machine(
    MachineType type, 
    MachineState state, 
    IQualityGenerator? qualityGenerator) : Entity<MachineType, MachineState>(type, state), IMachine
{
    protected readonly IQualityGenerator QualityGenerator = qualityGenerator ?? new DefaultQualityGenerator();
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
    IQualityGenerator? qualityGenerator = null) : Machine(type, state, qualityGenerator), ISingleTypeMachine
{
    public ItemType SupportedItemType { get; } = supportedItem ?? throw new ArgumentNullException(nameof(supportedItem));
    
    public Item Produce(int id, int? qualityPercentage = null)
    {
        int targetQuality = qualityPercentage ?? QualityGenerator.GenerateQuality();
        
        return RunProductionCycle(
            createItemLogic: () => CreateItem(id, targetQuality),
            beforeProduce: () => OnBeforeProduce(id, targetQuality),
            afterProduce: OnAfterProduce
        );
    }

    protected virtual void OnBeforeProduce(int id, int qualityPercentage) { }
    protected virtual void OnAfterProduce(Item item) { }
    protected abstract Item CreateItem(int id, int qualityPercentage);
}

public abstract class MultiTypeMachine : Machine, IMultiTypeMachine
{
    public List<ItemType> SupportedItemTypes { get; } = [];

    protected MultiTypeMachine(
        MachineType type,
        MachineState state,
        List<ItemType> supportedItems,
        IQualityGenerator? qualityGenerator = null) : base(type, state, qualityGenerator)
    {
        if (supportedItems == null || supportedItems.Count == 0)
            throw new ArgumentException("MultiTypeMachine must support at least one item type.", nameof(supportedItems));
        
        if (supportedItems.Count < 2)
            throw new ArgumentException("MultiTypeMachine must support at least two item types. Use SingleTypeMachine instead.");

        SupportedItemTypes.AddRange(supportedItems);
    }
    
    public Item Produce(int id, ItemType type, int? qualityPercentage = null)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (!SupportedItemTypes.Contains(type))
            throw new InvalidOperationException($"Machine cannot produce '{type}'.");
        
        int targetQuality = qualityPercentage ?? QualityGenerator.GenerateQuality();

        return RunProductionCycle(
            createItemLogic: () => CreateItem(id, type, targetQuality),
            beforeProduce: () => OnBeforeProduce(id, type, targetQuality),
            afterProduce: OnAfterProduce
        );
    }

    protected virtual void OnBeforeProduce(int id, ItemType type, int qualityPercentage) { }
    protected virtual void OnAfterProduce(Item item) { }
    protected abstract Item CreateItem(int id, ItemType type, int qualityPercentage);
}