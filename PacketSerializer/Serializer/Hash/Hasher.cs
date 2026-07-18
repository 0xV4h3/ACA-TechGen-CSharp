namespace Serializer.Hash;

public static class Hashers
{
    public static IHasher Default => new Fnv1a32();
}