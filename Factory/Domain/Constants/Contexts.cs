namespace Domain.Constants;

public static class Contexts
{
    public static class Types
    {
        public static readonly TypeContext Item = new("ItemType");
        public static readonly TypeContext Machine = new("MachineType");
        
        public static IEnumerable<TypeContext> All => [Item, Machine];
    }

    public static class States
    {
        public static readonly StateContext Item = new("ItemState");
        public static readonly StateContext Machine = new("MachineState");
        public static readonly StateContext Capacity = new("CapacityState");
        public static readonly StateContext OrderLine = new("OrderLineState");
        public static readonly StateContext Stock = new("StockState");
        public static readonly StateContext Storage = new("StorageState");
        public static readonly StateContext Transport = new("TransportState");
        
        public static IEnumerable<StateContext> All => 
        [
            Item, Machine, Capacity, OrderLine,
            Stock, Storage, Transport
        ];
    }
}