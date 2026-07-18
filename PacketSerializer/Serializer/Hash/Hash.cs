namespace Serializer.Hash;

public class Fnv1a32 : IHasher
{
    private const uint FnvOffsetBasis32 = 2166136261;
    private const uint FnvPrime32 = 16777619;
    
    public int Hash(byte[] data)
    {
        unchecked
        {
            uint hash = FnvOffsetBasis32;
            foreach (byte b in data)
            {
                hash ^= b;
                hash *= FnvPrime32;
            }
            return (int)hash;
        }    
    }
}