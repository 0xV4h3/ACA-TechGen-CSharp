using Domain.Factories;
using Domain.Models;
using Domain.Constants;
using Domain.Registries;

namespace FactoryCore;

public class FactoryA(IRegistry registry) : ProductionFactory([ItemTypes.A], MachineTypes.MachineA, registry)
{
    public override Machine CreateMachine() => new MachineA();

    // public QualityChecker CreateQualityChecker()
    // {
    //     return default;
    // }
}

public class FactoryB(IRegistry registry) : ProductionFactory([ItemTypes.B], MachineTypes.MachineB, registry)
{
    public override Machine CreateMachine() => new MachineB();
}

public class FactoryC(IRegistry registry) : ProductionFactory([ItemTypes.C], MachineTypes.MachineC, registry)
{
    public override Machine CreateMachine() => new MachineC();
}

public class FactoryAB(IRegistry registry) : ProductionFactory([ItemTypes.A, ItemTypes.B], MachineTypes.Create("MachineAB"), registry)
{
    public override Machine CreateMachine() => new MachineAB();
}