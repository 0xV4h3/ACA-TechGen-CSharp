namespace Simulation.Models;

public interface IActive
{
    public bool Active { get; }
}
public class User(int id, string name, int age, string region, bool active, List<string> tags) : IActive
{
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public int Age { get; private set; } = age;
    public string Region { get; private set; } = region;
    public bool Active { get; private set; } = active;
    public List<string> Tags { get; private set; } = tags;
}