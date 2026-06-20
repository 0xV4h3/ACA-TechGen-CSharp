using Domain.Configuration;

namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        Configuration.Initialize();
    }
}

// using Domain.Configuration;
// using Domain.Constants;
// using Domain.Registries;
//
// var builder = WebApplication.CreateBuilder(args);
//
// var registry = new Registry();
//
// foreach (var state in ItemStates.All) registry.Register(state.Value, state.Context);
// foreach (var type in ItemTypes.All) registry.Register(type.Value, type.Context);
//
// builder.Services.AddSingleton<IRegistry>(registry);
