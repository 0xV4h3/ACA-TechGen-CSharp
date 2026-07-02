using Domain.Constants;
using Domain.Models;
using Domain.Models.Quality;

namespace FactoryCore;

public class ItemA(int id) : Item(id, ItemTypes.A, ItemStates.Ordered, QualityConverters.ForItems.Default) { }
public class ItemB(int id) : Item(id, ItemTypes.B, ItemStates.Ordered, QualityConverters.ForItems.Default) { }
public class ItemC(int id) : Item(id, ItemTypes.C, ItemStates.Ordered, QualityConverters.ForItems.Default) { }