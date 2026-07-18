using System.Text;
using Serializer;
using Simulation.Models;

namespace Simulation.Utils;

public static class PlayerExtension
{
    public static Packet ToPacket(this Player player)
    {
        byte[] bytes = player.ToByte();
        return PacketSerializer.ToPacket(bytes);
    }
    
    public static byte[] ToByte(this Player player)
    {
        if (player == null) return Array.Empty<byte>();

        using (var stream = new MemoryStream())
        using (var writer = new BinaryWriter(stream, Encoding.UTF8))
        {
            writer.Write(player.Id);
            writer.Write(player.Name);
            writer.Write(player.Lvl);
            writer.Write(player.Coords.X);
            writer.Write(player.Coords.Y);
            
            return stream.ToArray();
        }
    }
}

public static class PlayersExtension
{
    public static IReadOnlyList<Packet> ToPackets(this IEnumerable<Player> players)
    {
        List<Packet> packets = [];

        foreach (var player in players)
            packets.Add(player.ToPacket());

        return packets;
    }
}