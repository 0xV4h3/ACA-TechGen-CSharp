namespace Domain.Constants;

public class MachineTypes : TypeConstant
{
    private MachineTypes(string value) : base(value, Contexts.Types.Machine) { }

    public static MachineTypes Create(string value) => new(value);

    public static readonly MachineTypes MachineA = new("MachineA");
    public static readonly MachineTypes MachineB = new("MachineB");
    public static readonly MachineTypes MachineC = new("MachineC");
    public static readonly MachineTypes Unknown = new("Unknown");

    public static IEnumerable<MachineTypes> All => [MachineA, MachineB, MachineC, Unknown];
}

public class MachineStates : StateConstant
{
    private MachineStates(string value) : base(value, Contexts.States.Machine) { }

    public static MachineStates Create(string value) => new(value);

    public static readonly MachineStates Idle = new("Idle");
    public static readonly MachineStates Producing = new("Producing");
    public static readonly MachineStates Maintenance = new("Maintenance");

    public static IEnumerable<MachineStates> All => [Idle, Producing, Maintenance];
}