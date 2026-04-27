namespace IEEE754;

public readonly struct DoubleBits
{
    private readonly int _sign;
    private readonly int _exponent;
    private readonly ulong _mantissa;

    private DoubleBits(int sign, int exponent, ulong mantissa)
    {
        _sign = sign;
        _exponent = exponent;
        _mantissa = mantissa;
    }
    
    public static DoubleBits FromDouble(double value)
    {
        byte[] bytes = new byte[8];
        Buffer.BlockCopy(new double[] { value }, 0, bytes, 0, 8);
        ulong bits = 0;
        for (int i = 0; i < 8; i++)
            bits |= ((ulong)bytes[i]) << (8 * i);
        int sign = (int)((bits >> 63) & 1);
        int exponent = (int)((bits >> 52) & 0x7FF);
        ulong mantissa = bits & 0xFFFFFFFFFFFFFL;
        return new DoubleBits(sign, exponent, mantissa);
    }
    
    public double ToDouble()
    {
        ulong bits = ((ulong)_sign << 63) | ((ulong)_exponent << 52) | (_mantissa & 0xFFFFFFFFFFFFFL);
        byte[] bytes = new byte[8];
        for (int i = 0; i < 8; i++)
            bytes[i] = (byte)((bits >> (8 * i)) & 0xFF);

        double[] arr = new double[1];
        Buffer.BlockCopy(bytes, 0, arr, 0, 8);
        return arr[0];
    }

    public string ToBitString(bool pretty = false)
    {
        string sSign = BitHelper.DecimalToBinary(_sign, 1);
        string sExp = BitHelper.DecimalToBinary(_exponent, 11);
        string sMan = BitHelper.DecimalToBinary(_mantissa, 52);
        return pretty
            ? $"{sSign} | {sExp} | {sMan}"
            : sSign + sExp + sMan;
    }

    public static DoubleBits FromBitString(string bits)
    {
        string cleaned = bits.Replace(" ", "").Replace("|", "");
        if (cleaned.Length != 64) throw new ArgumentException("Must be 64 bits.");
        int sign = cleaned[0] - '0';
        int exponent = BitHelper.BinaryToDecimal(cleaned.Substring(1, 11));
        ulong mantissa = BitHelper.BinaryToDecimalUnsigned64(cleaned.Substring(12, 52));
        return new DoubleBits(sign, exponent, mantissa);
    }
}