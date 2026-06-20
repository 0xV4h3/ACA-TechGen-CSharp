namespace Domain.Constants;

public class ItemTypes : TypeConstant
{
    private ItemTypes(string value) : base(value, Contexts.Types.Item) { }

    public static ItemTypes Create(string value) => new(value);

    public static readonly ItemTypes A = new("A");
    public static readonly ItemTypes B = new("B");
    public static readonly ItemTypes C = new("C");
    public static readonly ItemTypes Unknown = new("Unknown");

    public static IEnumerable<ItemTypes> All => [A, B, C, Unknown];
}

public class ItemStates : StateConstant
{
    private ItemStates(string value) : base(value, Contexts.States.Item) { }

    public static ItemStates Create(string value) => new(value);

    public static readonly ItemStates Ordered = new("Ordered");
    public static readonly ItemStates Manufacturing = new("Manufacturing");
    public static readonly ItemStates Staged = new("Staged");
    public static readonly ItemStates Testing = new("Testing");
    public static readonly ItemStates Passed = new("Passed");
    public static readonly ItemStates Failed = new("Failed");
    public static readonly ItemStates Stored = new("Stored");
    public static readonly ItemStates Shipping = new("Shipping");
    public static readonly ItemStates Delivered = new("Delivered");

    public static IEnumerable<ItemStates> All => 
    [
        Ordered, Manufacturing, Staged, Testing, 
        Passed, Failed, Stored, Shipping, Delivered
    ];
}