namespace Domain.Constants;

public class ItemType : TypeConstant
{
    internal ItemType(string value) : base(value, Contexts.Types.Item) { }
}

public static class ItemTypes
{
    public static ItemType Create(string value) => new(value);
    
    public static readonly ItemType A = new("A");
    public static readonly ItemType B = new("B");
    public static readonly ItemType C = new("C");
    public static readonly ItemType Unknown = new("Unknown");
}

public class ItemState : StateConstant
{
    internal ItemState(string value) : base(value, Contexts.States.Item) { }
}

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

public class ItemQuality : QualityConstant
{
    internal ItemQuality(string value) : base(value, Contexts.Qualities.Item) { }
}

public static class ItemQualities
{
    public static ItemQuality Create(string value) => new(value);
    
    public static readonly ItemQuality Excellent = new("Excellent");
    public static readonly ItemQuality Good = new("Good");
    public static readonly ItemQuality Average = new("Average");
    public static readonly ItemQuality Fair = new("Fair");
    public static readonly ItemQuality Poor = new("Poor");
}