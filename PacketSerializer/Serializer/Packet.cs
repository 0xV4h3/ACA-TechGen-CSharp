namespace Serializer;

public class Packet
{
    public required byte[] Payload { get; init; }
    public required int PayloadLength { get; init; }
    public required int PayloadHash { get; init; }
    public required long TimestampUnixMs { get; init; }
    public int Version { get; init; } = 1;
}