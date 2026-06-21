using Domain.Constants;
using Domain.Models;
using Domain.Registries;

namespace Domain.Factories;

public interface IProductionFactory
{
    Item CreateItem(int id);
    Machine CreateMachine();
}