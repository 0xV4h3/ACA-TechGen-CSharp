namespace GenericSpecifications.Models;

public class Product(string name, string category, decimal price, int stock)
{
    public string Name { get; } = name;
    public string Category { get; } = category;
    public decimal Price { get; } = price;
    public int Stock { get; } = stock;
}