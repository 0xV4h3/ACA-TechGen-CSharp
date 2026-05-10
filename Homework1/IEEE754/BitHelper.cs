namespace IEEE754;

public static class BitHelper
{
    public static string DecimalToBinary(int number, int length)
    {
        char[] bits = new char[length];
        for (int i = length - 1; i >= 0; --i)
        {
            bits[i] = (char)('0' + (number & 1));
            number >>= 1;
        }
        return new string(bits);
    }
    public static int BinaryToDecimal(string bits)
    {
        int val = 0;
        foreach (char c in bits)
            val = (val << 1) | (c - '0');
        return val;
    }
    public static string DecimalToBinary(uint number, int length)
    {
        char[] bits = new char[length];
        for (int i = length - 1; i >= 0; --i)
        {
            bits[i] = (char)('0' + ((number >> i) & 1));
        }
        return new string(bits);
    }
    public static uint BinaryToDecimalUnsigned(string bits)
    {
        uint val = 0;
        foreach (char c in bits)
            val = (val << 1) | (uint)(c - '0');
        return val;
    }
    public static string DecimalToBinary(ulong number, int length)
    {
        char[] bits = new char[length];
        for (int i = length - 1; i >= 0; --i)
        {
            bits[i] = (char)('0' + ((number >> i) & 1UL));
        }
        return new string(bits);
    }
    public static ulong BinaryToDecimalUnsigned64(string bits)
    {
        ulong val = 0;
        foreach (char c in bits)
            val = (val << 1) | (uint)(c - '0');
        return val;
    }
}