namespace Simulation.Models;

internal static class Data
{
    public static List<User> Users()
    {
        return new List<User>
        {
            new User(1, "Aram", 20, "Yerevan", true, new List<string> { "dotnet", "linq", "sql" }),
            new User(2, "Mila", 17, "Gyumri", false, new List<string> { "dotnet", "web" }),
            new User(3, "Narek", 25, "Yerevan", true, new List<string> { "perf", "linq" }),
            new User(4, "Anna", 22, "Vanadzor", true, new List<string> { "design", "web" }),
            new User(5, "Davit", 19, "Gyumri", false, new List<string> { "linq", "algorithms" }),
            new User(6, "Bomb", 19, "Gyumri", true, new List<string> { "linq", "algorithms" })
        };
    }

    public static List<Order> Orders()
    {
        return new List<Order>
        {
            new Order(101, 1, 120m),
            new Order(102, 1, 230m),
            new Order(103, 3, 500m),
            new Order(104, 4, 90m),
            new Order(105, 5, 75m),
            new Order(106, 6, 330m),
            new Order(107, 6, 26m),
        };
    }
}