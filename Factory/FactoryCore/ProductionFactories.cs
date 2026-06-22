using Domain.Factories;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class FactoryA(IRegistry registry) : IProductionFactory
{
    public Machine CreateMachine() => new MachineA(registry);
}

public class FactoryB(IRegistry registry) : IProductionFactory
{
    public Machine CreateMachine() => new MachineB(registry);
}

public class FactoryC(IRegistry registry) : IProductionFactory
{
    public Machine CreateMachine() => new MachineC(registry);
}