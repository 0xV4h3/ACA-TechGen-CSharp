namespace Simulation.Models;

public sealed class Order(int id, int userId, decimal total)
{
    public int Id { get; private set; } = id;
    public int UserId { get; private set; } = userId;
    public decimal Total { get; private set; } = total;
}