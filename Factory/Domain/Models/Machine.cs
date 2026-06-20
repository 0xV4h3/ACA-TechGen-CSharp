using Domain.Constants;
using Domain.Registries;

namespace Domain.Models;

public abstract class Machine : Entity
{
    protected Machine(string type, string state, IRegistry registry) 
        : base(type, state, Contexts.Types.Machine, Contexts.States.Machine, registry) {}
}