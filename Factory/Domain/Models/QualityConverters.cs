using Domain.Constants;
using Domain.Registries;

namespace Domain.Models;

public static class QualityConverters
{
    public static ItemQualityConverterRegistry ForItems { get; } = new();
    
    public sealed class ItemQualityConverterRegistry : QualityConverterRegistry<ItemQuality>
    {
        public IQualityConverter<ItemQuality> Standard { get; }
        public IQualityConverter<ItemQuality> Strict { get; }
        public IQualityConverter<ItemQuality> Default => Standard;

        internal ItemQualityConverterRegistry()
        {
            Standard = Create("Standard", [
                new(90, 100, ItemQualities.Excellent),
                new(75, 89,  ItemQualities.Good),
                new(50, 74,  ItemQualities.Average),
                new(25, 49,  ItemQualities.Fair),
                new(0,  24,  ItemQualities.Poor)
            ]);
            
            Strict = Create("Strict", [
                new(95, 100, ItemQualities.Excellent),
                new(85, 94,  ItemQualities.Good),
                new(0,  84,  ItemQualities.Poor)
            ]);
        }
    }
}