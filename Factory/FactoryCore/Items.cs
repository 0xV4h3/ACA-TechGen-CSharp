using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class ItemA(int id, IRegistry registry) : Item(id, ItemTypes.A, ItemStates.Ordered, registry) { }
public class ItemB(int id, IRegistry registry) : Item(id, ItemTypes.B, ItemStates.Ordered, registry) { }
public class ItemC(int id, IRegistry registry) : Item(id, ItemTypes.C, ItemStates.Ordered, registry) { }