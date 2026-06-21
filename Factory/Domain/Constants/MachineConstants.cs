namespace Domain.Constants;

public class MachineType(string value) : TypeConstant(value, Contexts.Types.Machine) { }

public static class MachineTypes
{
    public static MachineType Create(string value) => new(value);

    public static readonly MachineType MachineA = new("MachineA");
    public static readonly MachineType MachineB = new("MachineB");
    public static readonly MachineType MachineC = new("MachineC");
    public static readonly MachineType Unknown = new("Unknown");
}

public class MachineState(string value) : StateConstant(value, Contexts.States.Machine) { }

public static class MachineStates
{
    public static MachineState Create(string value) => new(value);

    public static readonly MachineState Idle = new("Idle");
    public static readonly MachineState Producing = new("Producing");
    public static readonly MachineState Maintenance = new("Maintenance");
}