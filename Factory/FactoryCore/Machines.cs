using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class MachineA(IRegistry registry) : Machine(MachineTypes.MachineA, MachineStates.Idle, [ItemTypes.A], registry)
{
    protected override Item CreateItem(int id, ItemType type) => new ItemA(id);
}
public class MachineB(IRegistry registry) : Machine(MachineTypes.MachineB, MachineStates.Idle, [ItemTypes.B], registry)
{
    protected override Item CreateItem(int id, ItemType type) => new ItemA(id);
}
public class MachineC(IRegistry registry) : Machine(MachineTypes.MachineC, MachineStates.Idle, [ItemTypes.C], registry)
{
    protected override Item CreateItem(int id, ItemType type) => new ItemA(id);
}

public class MachineAB(IRegistry registry) 
    : Machine(MachineTypes.Create("MachineAB"), MachineStates.Idle, [ItemTypes.A, ItemTypes.B], registry)
{
    protected override Item CreateItem(int id, ItemType type)
    {
        if (type == ItemTypes.A) return new ItemA(id);
        if (type == ItemTypes.B) return new ItemB(id);

        throw new InvalidOperationException($"MachineAB cannot construct item for type '{type}'.");
    }
}
