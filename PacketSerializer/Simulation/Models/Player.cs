namespace Simulation.Models;

public readonly record struct Point(double X, double Y);

public class Player
{
    public int Id { get; init; }
    public Point Coords { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Lvl { get; init; }
    
    public override string ToString() => $"{Id} : {Name} : {Lvl} lvl : (X : {Coords.X}, Y : {Coords.Y})";
}