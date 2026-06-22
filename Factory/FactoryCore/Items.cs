using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace FactoryCore;

public class ItemA(int id) : Item(id, ItemTypes.A, ItemStates.Ordered) { }
public class ItemB(int id) : Item(id, ItemTypes.B, ItemStates.Ordered) { }
public class ItemC(int id) : Item(id, ItemTypes.C, ItemStates.Ordered) { }