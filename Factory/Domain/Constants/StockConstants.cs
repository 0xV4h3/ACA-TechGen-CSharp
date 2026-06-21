namespace Domain.Constants;

public class StockState(string value) : StateConstant(value, Contexts.States.Stock) { }

public static class StockStates
{
    public static StockState Create(string value) => new(value);

    public static readonly StockState Normal = new("Normal");
    public static readonly StockState LowStock = new("LowStock");
    public static readonly StockState OutOfStock = new("OutOfStock");
    public static readonly StockState Overstock = new("Overstock");
    public static readonly StockState Restricted = new("Restricted");
}