namespace Domain.Constants;

public class StorageType : TypeConstant
{
    internal StorageType(string value) : base(value, Contexts.Types.Storage) { }
}

public static class StorageTypes
{
    public static StorageType Create(string value) => new(value);
    
    public static readonly StorageType StorageA = new("StorageA");
    public static readonly StorageType StorageB = new("StorageB");
    public static readonly StorageType StorageC = new("StorageC");
    public static readonly StorageType Unknown = new("Unknown");
}

public class StorageState : StateConstant
{
    internal StorageState(string value) : base(value, Contexts.States.Storage) { }
}

public static class StorageStates
{
    public static StorageState Create(string value) => new(value);

    public static readonly StorageState Open = new("Open");
    public static readonly StorageState LockedForInbound = new("LockedForInbound");
    public static readonly StorageState LockedForOutbound = new("LockedForOutbound");
    public static readonly StorageState Closed = new("Closed");
}