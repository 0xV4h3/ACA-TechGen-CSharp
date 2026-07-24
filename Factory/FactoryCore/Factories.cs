using Domain.Models;
using Domain.Factories;
using Domain.Models.Quality;
using Domain.Registries;

namespace FactoryCore;

public class FactoryA(IRegistry registry, IQualityGenerator qualityGenerator) : SingleTypeProductionFactory<MachineA>(registry, qualityGenerator)
{
    public override Machine CreateMachine() => new MachineA(QualityGenerator);
}

public class FactoryB(IRegistry registry, IQualityGenerator qualityGenerator) : SingleTypeProductionFactory<MachineB>(registry, qualityGenerator)
{
    public override Machine CreateMachine() => new MachineB(QualityGenerator);
}

public class FactoryC(IRegistry registry, IQualityGenerator qualityGenerator) : SingleTypeProductionFactory<MachineC>(registry, qualityGenerator)
{
    public override Machine CreateMachine() => new MachineC(QualityGenerator);
}