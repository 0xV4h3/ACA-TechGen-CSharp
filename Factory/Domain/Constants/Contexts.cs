namespace Domain.Constants;

public static class Contexts
{
    public static readonly Context Item = new("Item");
    public static readonly Context Machine = new("Machine");
    public static readonly Context OrderLine = new("OrderLine");
    public static readonly Context QualityChecker = new("QualityChecker");
    public static readonly Context Stock = new("Stock");
    public static readonly Context Transport = new("Transport");
    public static readonly Context Storage = new("Storage");
    
    public static IEnumerable<Context> All => 
    [
        Item, Machine, OrderLine, QualityChecker,
        Stock, Transport, Storage
    ];
}