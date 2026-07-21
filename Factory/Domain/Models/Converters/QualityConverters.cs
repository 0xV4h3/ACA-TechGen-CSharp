using Domain.Constants;
using Domain.Registries;

namespace Domain.Models.Converters;

public static class QualityConverters
{
    public static ItemQualityRegistry ForItems { get; } = new();

    public sealed class ItemQualityRegistry : RangeConverterRegistry<ItemQuality>
    {
        public IRangeConverter<ItemQuality> Standard { get; }
        public IRangeConverter<ItemQuality> Strict { get; }
        public IRangeConverter<ItemQuality> Default => Standard;

        internal ItemQualityRegistry()
        {
            Standard = Create("Standard", [
                new(90, ItemQualities.Excellent),
                new(75, ItemQualities.Good),
                new(50, ItemQualities.Average),
                new(25, ItemQualities.Fair),
                new(0,  ItemQualities.Poor)
            ]);

            Strict = Create("Strict", [
                new(95, ItemQualities.Excellent),
                new(85, ItemQualities.Good),
                new(0,  ItemQualities.Poor)
            ]);
        }
    }
}