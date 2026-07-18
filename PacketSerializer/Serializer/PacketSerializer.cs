using System.Text;
using Serializer.Hash;

using Serializer;

public static class PacketSerializer
{
    private const int FormatVersion = 1;
    
    private const int MaxPayloadBytes = 100 * 1024 * 1024;
    public static IHasher Hasher { get; set; } = Hashers.Default;
    
    public static void Serialize(string path, IReadOnlyList<Packet> packets)
    {
        using FileStream fs = new(path, FileMode.Create, FileAccess.Write, FileShare.None);
        using BinaryWriter w = new(fs, Encoding.UTF8);
        
        w.Write(FormatVersion);
        w.Write(packets.Count);

        foreach (var p in packets)
        {
            w.Write(p.Version);
            w.Write(p.TimestampUnixMs);
            w.Write(p.PayloadLength);
            w.Write(p.Payload);
            w.Write(p.PayloadHash);
        }

        w.Write(CalculateChecksum(packets));
    }

    public static bool TryDeserialize(string path, out IReadOnlyList<Packet>? packets, int maxPayloadBytes = MaxPayloadBytes)
    {
        try
        {
            packets = Deserialize(path, maxPayloadBytes);
            return packets is not null;
        }
        catch (Exception)
        {
            packets = null;
            return false;
        }
    }
    
    public static IReadOnlyList<Packet>? Deserialize(string path, int maxPayloadBytes = MaxPayloadBytes)
    {
        using FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using BinaryReader r = new(fs, Encoding.UTF8);
        
        if (r.ReadInt32() != FormatVersion) return null;

        int count = r.ReadInt32();
        if (count < 0) return null;

        List<Packet> packets = new(count);

        for (int i = 0; i < count; i++)
        {
            int version = r.ReadInt32();
            long timestamp = r.ReadInt64();
            int len = r.ReadInt32();
            if (len < 0 || len > MaxPayloadBytes) return null;

            byte[] payload = r.ReadBytes(len);
            if (payload.Length != len) return null;

            int hash = r.ReadInt32();
            if (Hasher.Hash(payload) != hash) return null;

            packets.Add(new Packet
            {
                Version = version,
                TimestampUnixMs = timestamp,
                PayloadLength = len,
                Payload = payload,
                PayloadHash = hash
            });
        }

        if (fs.Position + sizeof(int) > fs.Length) return null;
        int savedChecksum = r.ReadInt32();
        return savedChecksum == CalculateChecksum(packets) ? packets : null;
    }

    public static Packet ToPacket(byte[] payload)
    {
        return new Packet
        {
            Payload = payload,
            PayloadLength = payload.Length,
            PayloadHash = Hasher.Hash(payload),
            TimestampUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Version = 1
        };
    }
    
    public static Packet ToPacket<T>(T data, Func<T, byte[]> converter)
    {
        byte[] payload = converter(data);
        return new Packet
        {
            Payload = payload,
            PayloadLength = payload.Length,
            PayloadHash = Hasher.Hash(payload),
            TimestampUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Version = 1
        };
    }

    private static int CalculateChecksum(IEnumerable<Packet> packets)
    {
        unchecked
        {
            uint checksum = 0;
            foreach (var p in packets)
            {
                checksum ^= (uint)p.PayloadHash;
                checksum = (checksum << 5) | (checksum >> 27);
                checksum ^= (uint)p.PayloadLength;
                checksum ^= (uint)p.TimestampUnixMs;
            }
            return (int)checksum;
        }
    }
}