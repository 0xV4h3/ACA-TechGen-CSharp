namespace Domain.Constants;

public class StockType : TypeConstant
{
    internal StockType(string value) : base(value, Contexts.Types.Stock) { }
}

public static class StockTypes
{
    public static StockType Create(string value) => new(value);
    
    public static readonly StockType StockA = new("StockA");
    public static readonly StockType StockB = new("StockB");
    public static readonly StockType StockC = new("StockC");
    public static readonly StockType Unknown = new("Unknown");
}

public class StockState : StateConstant
{
    internal StockState(string value) : base(value, Contexts.States.Stock) { }
}

public static class StockStates
{
    public static StockState Create(string value) => new(value);

    public static readonly StockState Normal = new("Normal");
    public static readonly StockState LowStock = new("LowStock");
    public static readonly StockState OutOfStock = new("OutOfStock");
    public static readonly StockState Overstock = new("Overstock");
    public static readonly StockState Restricted = new("Restricted");
}