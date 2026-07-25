using Domain.Quality;

namespace Domain.Factories;

public interface IProductionFactory
{
    IQualityGenerator QualityGenerator { get; }
    MachineType SupportedMachineType { get; }
    Machine CreateMachine();
}

public interface ISingleTypeProductionFactory : IProductionFactory
{
    ItemType SupportedItemType { get; }
}

public interface IMultiTypeProductionFactory : IProductionFactory
{
    ICollection<ItemType> SupportedItemTypes { get; }
}