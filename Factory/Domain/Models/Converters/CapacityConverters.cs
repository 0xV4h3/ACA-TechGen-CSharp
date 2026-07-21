using Domain.Constants;
using Domain.Registries;

namespace Domain.Models.Converters;

public static class CapacityConverters
{
    public static StockCapacityRegistry ForStock { get; } = new();

    public sealed class StockCapacityRegistry : RangeConverterRegistry<StockCapacity>
    {
        public IRangeConverter<StockCapacity> Standard { get; }
        public IRangeConverter<StockCapacity> Default => Standard;

        internal StockCapacityRegistry()
        {
            Standard = Create("Standard", [
                new(90, StockCapacities.Full),
                new(75, StockCapacities.High),
                new(60, StockCapacities.AboveAverage),
                new(40, StockCapacities.Moderate),
                new(25, StockCapacities.BelowAverage),
                new(1, StockCapacities.Low),
                new(0,  StockCapacities.Empty)
            ]);
        }
    }
}