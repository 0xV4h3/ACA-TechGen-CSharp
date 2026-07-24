using Domain.Constants;
using Domain.Models;
using Domain.Models.Quality;
using Domain.Registries;
using Domain.Utils;

namespace Domain.Factories;

public abstract class MultiTypeProductionFactory<TMachine> : IMultiTypeProductionFactory
    where TMachine : MultiTypeMachine, new()
{
    private static readonly TMachine Template = new();
    
    public IQualityGenerator QualityGenerator { get; }
    public MachineType SupportedMachineType { get; }
    public ICollection<ItemType> SupportedItemTypes { get; } = [];
    
    protected MultiTypeProductionFactory(IRegistry registry, IQualityGenerator? qualityGenerator = null)
    {
        registry.ValidateType(Template.Type);
        SupportedMachineType = Template.Type;
        
        foreach (var itemType in Template.SupportedItemTypes)
        {
            registry.ValidateType(itemType);
            SupportedItemTypes.Add(itemType);
        }
        
        QualityGenerator = qualityGenerator ?? new DefaultQualityGenerator();
    }

    public virtual Machine CreateMachine() => new TMachine();
}