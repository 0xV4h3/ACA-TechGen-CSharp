using Domain.Constants;
using Domain.Models;
using Domain.Models.Converters;

namespace FactoryCore;

public class ItemA(int id, int qualityPercentage) : Item(id, ItemTypes.A, ItemStates.Ordered, QualityConverters.ForItems.Default, qualityPercentage) { }
public class ItemB(int id, int qualityPercentage) : Item(id, ItemTypes.B, ItemStates.Ordered, QualityConverters.ForItems.Default, qualityPercentage) { }
public class ItemC(int id, int qualityPercentage) : Item(id, ItemTypes.C, ItemStates.Ordered, QualityConverters.ForItems.Default, qualityPercentage) { }