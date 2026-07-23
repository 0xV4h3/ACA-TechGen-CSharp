using Domain.Constants;
using Domain.Registries;

namespace Domain.Models.Converters;

public static class CapacityConverters
{
    public static OrderLineCapacityRegistry ForOrderLine { get; } = new();
    public static StorageCapacityRegistry ForStorage { get; } = new();
    public static StockCapacityRegistry ForStock { get; } = new();

    public sealed class OrderLineCapacityRegistry : RangeConverterRegistry<OrderLineCapacity>
    {
        public IRangeConverter<OrderLineCapacity> Standard { get; }
        public IRangeConverter<OrderLineCapacity> Default => Standard;

        internal OrderLineCapacityRegistry()
        {
            Standard = Create("Standard", [
                new(90, OrderLineCapacities.Full),
                new(60, OrderLineCapacities.High),
                new(30, OrderLineCapacities.Moderate),
                new(1,  OrderLineCapacities.Low),
                new(0,  OrderLineCapacities.Empty)
            ]);
        }
    }

    public sealed class StorageCapacityRegistry : RangeConverterRegistry<StorageCapacity>
    {
        public IRangeConverter<StorageCapacity> Standard { get; }
        public IRangeConverter<StorageCapacity> Default => Standard;

        internal StorageCapacityRegistry()
        {
            Standard = Create("Standard", [
                new(90, StorageCapacities.Full),
                new(60, StorageCapacities.High),
                new(30, StorageCapacities.Moderate),
                new(1,  StorageCapacities.Low),
                new(0,  StorageCapacities.Empty)
            ]);
        }
    }
    
    public sealed class StockCapacityRegistry : RangeConverterRegistry<StockCapacity>
    {
        public IRangeConverter<StockCapacity> Standard { get; }
        public IRangeConverter<StockCapacity> Default => Standard;

        internal StockCapacityRegistry()
        {
            Standard = Create("Standard", [
                new(95, StockCapacities.Overloaded),
                new(85, StockCapacities.Full),
                new(65, StockCapacities.High),
                new(45, StockCapacities.AboveAverage),
                new(30, StockCapacities.Moderate),
                new(15, StockCapacities.BelowAverage),
                new(1,  StockCapacities.Low),
                new(0,  StockCapacities.Empty)
            ]);
        }
    }
}
