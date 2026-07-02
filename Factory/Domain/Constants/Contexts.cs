namespace Domain.Constants;

public static class Contexts
{
    public static class Types
    {
        public static readonly TypeContext Item = new("ItemType");
        public static readonly TypeContext Machine = new("MachineType");
        public static readonly TypeContext OrderLine = new("OrderLineType");
        public static readonly TypeContext QualityChecker = new("QualityCheckerType");
        public static readonly TypeContext Stock = new("StockType");
        public static readonly TypeContext Transport = new("TransportType");
        public static readonly TypeContext Storage = new("StorageType");
        
        public static IEnumerable<TypeContext> All => 
        [
            Item, Machine, OrderLine, QualityChecker,
            Stock, Transport, Storage
        ];
    }

    public static class States
    {
        public static readonly StateContext Item = new("ItemState");
        public static readonly StateContext Machine = new("MachineState");
        public static readonly StateContext OrderLine = new("OrderLineState");
        public static readonly StateContext QualityChecker = new("QualityCheckerState");
        public static readonly StateContext Stock = new("StockState");
        public static readonly StateContext Transport = new("TransportState");
        public static readonly StateContext Storage = new("StorageState");
        public static readonly StateContext Capacity = new("CapacityState");
        
        public static IEnumerable<StateContext> All => 
        [
            Item, Machine, OrderLine, QualityChecker,
            Stock, Transport, Storage, Capacity
        ];
    }

    public static class Qualities
    {
        public static readonly QualityContext Item = new("ItemQuality");
        
        public static IEnumerable<QualityContext> All => 
        [
            Item
        ];
    }
}