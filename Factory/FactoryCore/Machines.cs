using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class MachineA(IRegistry registry) : SingleTypeMachine(MachineTypes.MachineA, MachineStates.Idle, ItemTypes.A, registry)
{
    protected override Item CreateItem(int id) => new ItemA(id);
}
public class MachineB(IRegistry registry) : SingleTypeMachine(MachineTypes.MachineB, MachineStates.Idle, ItemTypes.B, registry)
{
    protected override Item CreateItem(int id) => new ItemB(id);
}
public class MachineC(IRegistry registry) : SingleTypeMachine(MachineTypes.MachineC, MachineStates.Idle, ItemTypes.C, registry)
{
    protected override Item CreateItem(int id) => new ItemC(id);
}

public class MachineAB(IRegistry registry) 
    : MultiTypeMachine(MachineTypes.Create("MachineAB"), MachineStates.Idle, [ItemTypes.A, ItemTypes.B], registry)
{
    protected override Item CreateItem(int id, ItemType type)
    {
        if (type == ItemTypes.A) return new ItemA(id);
        if (type == ItemTypes.B) return new ItemB(id);

        throw new InvalidOperationException($"MachineAB cannot construct item for type '{type}'.");
    }
}
