namespace Simulation.Models;

public class Order(int id, List<Product> products)
{
    public int Id { get; set; } = id;
    public List<Product> Products { get; set; } = products;
}