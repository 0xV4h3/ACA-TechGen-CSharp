using Domain.Constants;
using Domain.Registries;

namespace Domain.Models;

public abstract class Item(int id, ItemType type, ItemState state) : Entity(type, state)
{
    public int Id { get; init; } = id;
}
