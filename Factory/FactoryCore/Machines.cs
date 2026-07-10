using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class MachineA() : SingleTypeMachine(MachineTypes.MachineA, MachineStates.Idle, ItemTypes.A)
{
    protected override Item CreateItem(int id) => new ItemA(id);
}
public class MachineB() : SingleTypeMachine(MachineTypes.MachineB, MachineStates.Idle, ItemTypes.B)
{
    protected override Item CreateItem(int id) => new ItemB(id);
}
public class MachineC() : SingleTypeMachine(MachineTypes.MachineC, MachineStates.Idle, ItemTypes.C)
{
    protected override Item CreateItem(int id) => new ItemC(id);
}

public class MachineAB() 
    : MultiTypeMachine(MachineTypes.Create("MachineAB"), MachineStates.Idle, [ItemTypes.A, ItemTypes.B])
{
    protected override Item CreateItem(int id, ItemType type)
    {
        if (type == ItemTypes.A) return new ItemA(id);
        if (type == ItemTypes.B) return new ItemB(id);

        throw new InvalidOperationException($"MachineAB cannot construct item for type '{type}'.");
    }
}
