using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class MachineA(IRegistry registry) : Machine(MachineTypes.MachineA, MachineStates.Idle, [ItemTypes.A], registry) { }
public class MachineB(IRegistry registry) : Machine(MachineTypes.MachineB, MachineStates.Idle, [ItemTypes.B], registry) { }
public class MachineC(IRegistry registry) : Machine(MachineTypes.MachineC, MachineStates.Idle, [ItemTypes.C], registry) { }