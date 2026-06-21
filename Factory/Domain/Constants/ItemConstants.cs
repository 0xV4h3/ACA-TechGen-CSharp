namespace Domain.Constants;

public class ItemType(string value) : TypeConstant(value, Contexts.Types.Item) { }

public static class ItemTypes
{
    public static ItemType Create(string value) => new(value);
    
    public static readonly ItemType A = new("A");
    public static readonly ItemType B = new("B");
    public static readonly ItemType C = new("C");
    public static readonly ItemType Unknown = new("Unknown");
}

public class ItemState(string value) : StateConstant(value, Contexts.States.Item) { }

public static class ItemStates
{
    public static ItemState Create(string value) => new(value);
    
    public static readonly ItemState Ordered = new("Ordered");
    public static readonly ItemState Manufacturing = new("Manufacturing");
    public static readonly ItemState Staged = new("Staged");
    public static readonly ItemState Testing = new("Testing");
    public static readonly ItemState Passed = new("Passed");
    public static readonly ItemState Failed = new("Failed");
    public static readonly ItemState Stored = new("Stored");
    public static readonly ItemState Shipping = new("Shipping");
    public static readonly ItemState Delivered = new("Delivered");
}