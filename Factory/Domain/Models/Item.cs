using Domain.Constants;
using Domain.Models.Quality;

namespace Domain.Models;

public abstract class Item : Entity, IQualitative
{
    private readonly IQualityConverter<ItemQuality> _qualityConverter;
    public string QualityConverterName { get; private set; }

    public int Id { get; init; }
    public int QualityPercentage { get; private set; }
    public ItemQuality Quality { get; private set; }

    protected Item(
        int id, 
        ItemType type, 
        ItemState state, 
        IQualityConverter<ItemQuality> qualityConverter,
        int initialPercentage = 100) 
        : base(type, state)
    {
        Id = id;
        _qualityConverter = qualityConverter ?? throw new ArgumentNullException(nameof(qualityConverter));
        QualityConverterName = QualityConverters.ForItems.AllNames
            .FirstOrDefault(name => QualityConverters.ForItems.GetByName(name) == qualityConverter) ?? "Standard";

        ((IQualitative)this).ChangeQuality(initialPercentage); 
    }
    
    void IQualitative.UpdatePercentageInternal(int percentage)
    {
        QualityPercentage = percentage;
        Quality = _qualityConverter.Convert(percentage);
    }
}
