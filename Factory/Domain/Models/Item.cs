using Domain.Constants;
using Domain.Registries;

namespace Domain.Models;

public abstract class Item : Entity
{
    public int Id { get; init; }
    protected Item(int id, ItemType type, ItemState state, IRegistry registry) : base(type, state, registry)
    {
        Id = id;
    }
}
