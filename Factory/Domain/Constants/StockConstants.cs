namespace Domain.Constants;

public class StockStates : StateConstant
{
    private StockStates(string value) : base(value, Contexts.States.Stock) { }

    public static StockStates Create(string value) => new(value);

    public static readonly StockStates Normal = new("Normal");
    public static readonly StockStates LowStock = new("LowStock");
    public static readonly StockStates OutOfStock = new("OutOfStock");
    public static readonly StockStates Overstock = new("Overstock");
    public static readonly StockStates Restricted = new("Restricted");

    public static IEnumerable<StockStates> All => [Normal, LowStock, OutOfStock, Overstock, Restricted];
}