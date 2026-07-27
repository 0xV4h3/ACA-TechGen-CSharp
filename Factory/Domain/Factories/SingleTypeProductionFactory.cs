using Domain.Quality;
using Domain.Utils;

namespace Domain.Factories;

public abstract class SingleTypeProductionFactory<TMachine> : ISingleTypeProductionFactory
    where TMachine : SingleTypeMachine, new()
{
    private static readonly TMachine Template = new();

    public IQualityGenerator QualityGenerator { get; }
    public MachineType SupportedMachineType { get; }
    public ItemType SupportedItemType { get; }

    protected SingleTypeProductionFactory(IRegistry registry, IQualityGenerator? qualityGenerator = null)
    {
        registry.ValidateType(Template.Type);
        registry.ValidateType(Template.SupportedItemType);

        SupportedMachineType = Template.Type;
        SupportedItemType = Template.SupportedItemType;
        QualityGenerator = qualityGenerator ?? new DefaultQualityGenerator();
    }

    public virtual Machine CreateMachine() => new TMachine(); 
}