using Serializer;
using Simulation.Models;

namespace Simulation.Utils;

public static class PacketExtension
{
    public static Player? ToPlayer(this Packet packet)
    {
        if (packet.PayloadLength != packet.Payload.Length) 
            return null;

        return packet.Payload.ToPlayer();
    }
}

public static class PacketsExtension
{
    public static IReadOnlyList<Player> ToPlayers(this IEnumerable<Packet> packets)
    {
        List<Player> players = [];
        
        foreach (var packet in packets)
        {
            var player = packet.ToPlayer();
            if (player is not null) 
                players.Add(player);
        }
        
        return players;
    }
}