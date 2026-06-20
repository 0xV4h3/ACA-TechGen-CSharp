namespace Domain.Constants;

public static class Contexts
{
    public static class Types
    {
        public const string Item = "ItemType";
        public const string Machine = "MachineType";
    }
    
    public static class States
    {
        public const string Item = "ItemState";
        public const string Machine = "MachineState";
        public const string Capacity = "CapacityState";
        public const string OrderLine = "OrderLineState";
        public const string Stock = "StockState";
        public const string Storage = "StorageState";
        public const string Transport = "TransportState";
    }
}