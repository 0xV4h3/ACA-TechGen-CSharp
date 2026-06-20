using Domain.Constants;
using Domain.Registries;

namespace Domain.Models;

public abstract class Item : Entity
{
    public int Id { get; init; }

    protected Item(string type, string state, IRegistry registry) 
        : base(type, state, Contexts.Types.Item, Contexts.States.Item, registry) {}
}
