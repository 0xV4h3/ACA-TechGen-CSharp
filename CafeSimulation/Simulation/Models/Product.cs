namespace Simulation.Models;

public class Product(string name, int preparationTimeMs)
{
    public string Name { get; set; } = name;
    public int PreparationTimeMs { get; set; } = preparationTimeMs;
}