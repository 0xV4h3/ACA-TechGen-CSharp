using Domain.Models;

namespace Domain.Factories;

public interface IProductionFactory
{
    Machine CreateMachine();
}