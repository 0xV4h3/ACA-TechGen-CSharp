using System.Text;
using System.Text.Json;
using Serializer;
using Simulation.Models;

namespace Simulation.Utils;

public static class PlayerPacket
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static Packet PlayerToPacket(Player player)
    {
        return PacketSerializer.ToPacket(
            player, 
            p => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(p, JsonOptions))
        );
    }

    public static Player? PacketToPlayer(Packet packet)
    {
        if (packet.PayloadLength != packet.Payload.Length) 
            return null;

        string json = Encoding.UTF8.GetString(packet.Payload);
        return JsonSerializer.Deserialize<Player>(json, JsonOptions);
    }
    
    public static List<Packet> PlayersToPackets(IEnumerable<Player> players)
    {
        List<Packet> result = new List<Packet>();
        
        foreach (var player in players)
            result.Add(PlayerToPacket(player));
        
        return result;
    }

    public static List<Player> PacketsToPlayers(IEnumerable<Packet> packets)
    {
        List<Player> result = [];
        
        foreach (var packet in packets)
        {
            var player = PacketToPlayer(packet);
            if (player is not null) 
                result.Add(player);
        }
        
        return result;
    }
}