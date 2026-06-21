namespace Domain.Constants;

public class StorageState(string value) : StateConstant(value, Contexts.States.Storage) { }

public static class StorageStates
{
    public static StorageState Create(string value) => new(value);

    public static readonly StorageState Open = new("Open");
    public static readonly StorageState LockedForInbound = new("LockedForInbound");
    public static readonly StorageState LockedForOutbound = new("LockedForOutbound");
    public static readonly StorageState Closed = new("Closed");
}