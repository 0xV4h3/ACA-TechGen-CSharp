namespace Domain.Constants;

public class StorageStates : StateConstant
{
    private StorageStates(string value) : base(value, Contexts.States.Storage) { }

    public static StorageStates Create(string value) => new(value);

    public static readonly StorageStates Open = new("Open");
    public static readonly StorageStates LockedForInbound = new("LockedForInbound");
    public static readonly StorageStates LockedForOutbound = new("LockedForOutbound");
    public static readonly StorageStates Closed = new("Closed");

    public static IEnumerable<StorageStates> All => [Open, LockedForInbound, LockedForOutbound, Closed];
}