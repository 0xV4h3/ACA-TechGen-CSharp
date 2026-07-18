using Simulation.Models;
using Simulation.Utils;

namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        var players = new List<Player>
        {
            new() { Id = 1, Name = "Noob",   Lvl = 0,   Coords = new Point(0, 0) },
            new() { Id = 2, Name = "Pro",    Lvl = 30,  Coords = new Point(10, 5) },
            new() { Id = 3, Name = "Legend", Lvl = 100, Coords = new Point(40, 20) },
        };

        string path = Path.Combine(Environment.CurrentDirectory, $"players_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bin");

        var packets = players.ToPackets();
        PacketSerializer.Serialize(path, packets);

        var restoredPackets = PacketSerializer.Deserialize(path);
        if (restoredPackets is null)
        {
            Console.WriteLine("Check failed.");
            return;
        }

        var restoredPlayers = restoredPackets.ToPlayers();
        Console.WriteLine($"Restored players: {restoredPlayers.Count}");

        foreach (var player in restoredPlayers)
            Console.WriteLine(player);
    }
}